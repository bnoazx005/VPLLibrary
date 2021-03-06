﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// The enumeration contains all possible flags that specify 
    /// interpreter's work
    /// </summary>

    [Flags]
    public enum E_INTERPRETER_ATTRIBUTES
    {
        IA_IS_SAFE_DIVISION_ENABLED = 0x1,
        IA_DEFAULT                  = 0x0
    }


    /// <summary>
    /// class CInterpreter
    /// 
    /// This is main class within the library. It provides
    /// a functionality of evaluating correct AST, which
    /// describes operations over interger arrays
    /// </summary>

    public class CInterpreter: IInterpreter, IVisitor<Object>
    {
        protected IEnvironment             mEnvironment;

        protected IInputStream             mInputStream;
        
        protected E_INTERPRETER_ATTRIBUTES mAttributes;

        public CInterpreter(E_INTERPRETER_ATTRIBUTES attributes = E_INTERPRETER_ATTRIBUTES.IA_DEFAULT)
        {
            mEnvironment = new CEnvironment();

            mAttributes = attributes;
        }

        /// <summary>
        /// The method evaluates (executes) a program, which is represented as a syntax tree of nodes.
        /// An input parameters could be integer arrays or integer values (represented as an array with single value).
        /// </summary>
        /// <param name="program">A program is represented as an AST</param>
        /// <param name="inputStream">Program's input data</param>
        /// <returns>An evaluated array of some data</returns>

        public int[] Eval(IASTNode program, IInputStream inputStream)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program", "The argument cannot equal to null");
            }

            if (inputStream == null)
            {
                throw new ArgumentNullException("inputData", "The argument cannot equal to null");
            }

            mInputStream = inputStream;
            
            mEnvironment.Clear();

            return (int[])program.Accept(this);
        }

        public Object VisitProgramNode(IASTNode program)
        {
            if (program.Type != E_NODE_TYPE.NT_PROGRAM)
            {
                throw new CRuntimeError("A root of an AST should have NT_PROGRAM type");
            }

            IList<IASTNode> commands = (program as IProgramASTNode).Operations;

            foreach (IASTNode currCommand in commands)
            {
                currCommand.Accept(this);
            }

            string lastAssignedVariableId = mEnvironment.GetLastAssignedVariableId();

            // if input program is empty, just return an empty array as a result
            if (string.IsNullOrEmpty(lastAssignedVariableId))
            {
                return new int[0];
            }

            return mEnvironment.Get(lastAssignedVariableId);
        }

        public Object VisitAssigmentNode(IAssigmentASTNode assigment)
        {
            IASTNode rightOp = assigment.Expression;

            // if right operator is just an identifier or a value, then assign it immediately
            if (rightOp.Type == E_NODE_TYPE.NT_VALUE)
            {
                int[] assignedValue = (rightOp as IValueASTNode).Value;

                mEnvironment.Assign(assigment.Id.Name, assignedValue);

                return assignedValue;
            }

            if (rightOp.Type == E_NODE_TYPE.NT_IDENTIFIER)
            {
                string id = (rightOp as IIdentifierASTNode).Name;

                int[] assignedValue = mEnvironment.Get(id);

                mEnvironment.Assign(assigment.Id.Name, assignedValue);

                return assignedValue;
            }

            // common case
            Object value = assigment.Expression.Accept(this);

            mEnvironment.Assign(assigment.Id.Name, (int[])value);

            return value;
        }

        public Object VisitIdentifierNode(IIdentifierASTNode identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier", "The argument cannot equal to null");
            }

            IASTNode idNode = identifier as IASTNode;

            if ((idNode.Attributes & E_NODE_ATTRIBUTES.NA_LVALUE) == E_NODE_ATTRIBUTES.NA_LVALUE)
            {
                return identifier.Name;
            }
            
            int[] assignedValue = mEnvironment.Get(identifier.Name);

            return assignedValue;
        }

        public Object VisitValueNode(IValueASTNode value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "The argument cannot equal to null");
            }

            return value.Value;
        }

        public Object VisitCallNode(ICallASTNode call)
        {
            if (call == null)
            {
                throw new ArgumentNullException("call", "The argument cannot equal to null");
            }

            IList<IASTNode> argsList = call.Args;

            List<Object> evaluatedArgsList = new List<Object>();

            foreach(IASTNode arg in argsList)
            {
                if (arg == null)
                {
                    throw new ArgumentNullException("arg", "The argument cannot equal to null");
                }

                if (arg.Type == E_NODE_TYPE.NT_IDENTIFIER)
                {
                    evaluatedArgsList.Add(mEnvironment.Get((arg as IIdentifierASTNode).Name));

                    continue;
                }

                evaluatedArgsList.Add(arg.Accept(this));
            }

            return CIntrinsicsUtils.Eval(call.IntrinsicType, evaluatedArgsList.ToArray());
        }

        public Object VisitBinaryLambdaFuncNode(IBinaryLambdaFuncASTNode binaryLambdaFunc)
        {
            if (binaryLambdaFunc == null)
            {
                throw new ArgumentNullException("binaryLambdaFunc", "The argument cannot equal to null");
            }

            ParameterExpression x = Expression.Parameter(typeof(int), "x");
            ParameterExpression y = Expression.Parameter(typeof(int), "y");

            Expression lambdaBody = null;

            lambdaBody = _createExpressionByOpType(binaryLambdaFunc.OpType, x, y);

            return Expression.Lambda(lambdaBody, x, y).Compile();
        }

        public Object VisitLambdaPredicateNode(ILambdaPredicateASTNode lambdaPredicate)
        {
            if (lambdaPredicate == null)
            {
                throw new ArgumentNullException("lambdaPredicate", "The argument cannot equal to null");
            }

            ParameterExpression x = Expression.Parameter(typeof(int), "x");

            Expression value = Expression.Constant(lambdaPredicate.FirstOperand.Value[0]);

            Expression lambdaBody = Expression.Empty();

            switch (lambdaPredicate.LOPType)
            {
                case E_LOGIC_OP_TYPE.LOT_EQ:
                    lambdaBody = Expression.Equal(value, x);
                    break;
                case E_LOGIC_OP_TYPE.LOT_NEQ:
                    lambdaBody = Expression.NotEqual(value, x);
                    break;
                case E_LOGIC_OP_TYPE.LOT_LT:
                    lambdaBody = Expression.LessThan(x, value);
                    break;
                case E_LOGIC_OP_TYPE.LOT_LE:
                    lambdaBody = Expression.LessThanOrEqual(x, value);
                    break;
                case E_LOGIC_OP_TYPE.LOT_GT:
                    lambdaBody = Expression.GreaterThan(x, value);
                    break;
                case E_LOGIC_OP_TYPE.LOT_GE:
                    lambdaBody = Expression.GreaterThanOrEqual(x, value);
                    break;
                case E_LOGIC_OP_TYPE.LOT_MOD:

                    if ((mAttributes & E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED) == E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED)
                    {
                        lambdaBody = Expression.Equal(_createSafeModuloOperator(x, value), Expression.Constant(lambdaPredicate.SecondOperand.Value[0]));
                    }
                    else
                    {
                        lambdaBody = Expression.Equal(Expression.Modulo(x, value), Expression.Constant(lambdaPredicate.SecondOperand.Value[0]));
                    }

                    break;
            }

            return Expression.Lambda(lambdaBody, x).Compile();
        }

        public Object VisitUnaryLambdaFuncNode(IUnaryLambdaFuncASTNode unaryLambdaFunc)
        {
            if (unaryLambdaFunc == null)
            {
                throw new ArgumentNullException("unaryLambdaFunc", "The argument cannot equal to null");
            }

            ParameterExpression x = Expression.Parameter(typeof(int), "x");

            Expression lambdaBody = Expression.Empty();
            
            Expression y = (Expression)_visitUnaryLambdaFuncHelper(unaryLambdaFunc.Body, x);
            
            lambdaBody = _createExpressionByOpType(unaryLambdaFunc.OpType, x, y);
            
            return Expression.Lambda(lambdaBody.Reduce(), x).Compile();
        }

        public Object VisitIfThenElseNode(IIfThenElseASTNode ifStatementNode)
        {
            if (ifStatementNode == null)
            {
                throw new ArgumentNullException("ifStatementNode", "The argument cannot equal to null");
            }

            if (ifStatementNode.Variable == null)
            {
                throw new ArgumentNullException("ifStatementNode.Variable", "The argument cannot equal to null");
            }

            if (ifStatementNode.Predicate == null)
            {
                throw new ArgumentNullException("ifStatementNode.Predicate", "The argument cannot equal to null");
            }

            Func<int, bool> predicate = (Func<int, bool>)(ifStatementNode.Predicate as IASTNode).Accept(this);

            int[] evaluatedExpr = null;

            IASTNode variableNode = ifStatementNode.Variable;

            if ((variableNode.Type == E_NODE_TYPE.NT_IDENTIFIER) && 
                ((variableNode.Attributes & E_NODE_ATTRIBUTES.NA_LVALUE) == E_NODE_ATTRIBUTES.NA_LVALUE))
            {
                evaluatedExpr = mEnvironment.Get((string)(variableNode).Accept(this)); // variable is lvalue
            }
            else
            {
                evaluatedExpr = (int[])variableNode.Accept(this); // variable is rvalue, replace it with its value
            }
            
            bool conditionResult = evaluatedExpr.Length >= 1 ? predicate(evaluatedExpr[0]) : false;

            if (conditionResult)
            {
                if (ifStatementNode.ThenBranch == null)
                {
                    throw new ArgumentNullException("ifStatementNode.ThenBranch", "The argument cannot equal to null");
                }

                return ifStatementNode.ThenBranch.Accept(this);
            }

            if (ifStatementNode.ElseBranch == null)
            {
                throw new ArgumentNullException("ifStatementNode.ElseBranch", "The argument cannot equal to null");
            }

            return ifStatementNode.ElseBranch.Accept(this);
        }

        public Object VisitReadInputNode(IReadInputASTNode readNode)
        {
            if (readNode == null)
            {
                throw new ArgumentNullException("readNode", "The argument cannot equal to null");
            }            

            if (mInputStream == null)
            {
                throw new CRuntimeError("The data pointer is out of range");
            }

            int[] readOpParameter = (int[])readNode.Index.Accept(this);

            int readDataIndex = readOpParameter.Length >= 1 ? readOpParameter[0] : 0;

            return mInputStream.Read(readDataIndex);
        }

        protected Object _visitUnaryLambdaFuncHelper(IASTNode node, Expression parameter)
        {
            int[] valueArray = null;

            if (node.Type == E_NODE_TYPE.NT_VALUE)
            {
                valueArray = (node as IValueASTNode).Value;

                return Expression.Constant(valueArray.Length >= 1 ? valueArray[0] : 0);
            }

            IUnaryLambdaFuncASTNode funcNode = node as IUnaryLambdaFuncASTNode;

            if (funcNode == null)
            {
                throw new CRuntimeError("Invalid lambda's body");
            }

            IASTNode lambdaBody = funcNode.Body;

            Expression value = null;

            if (lambdaBody.Type == E_NODE_TYPE.NT_VALUE)
            {
                valueArray = (lambdaBody as IValueASTNode).Value;

                value = Expression.Constant(valueArray.Length >= 1 ? valueArray[0] : 0);

                return _createExpressionByOpType(funcNode.OpType, parameter, value);
            }

            if (lambdaBody.Type != E_NODE_TYPE.NT_UNARY_LAMBDA_FUNC)
            {
                throw new CRuntimeError("Invalid lambda's body");
            }

            value = (Expression)_visitUnaryLambdaFuncHelper(lambdaBody, parameter);

            return _createExpressionByOpType(funcNode.OpType, parameter, value);
        }

        protected Expression _createExpressionByOpType(E_OPERATION_TYPE type, Expression left, Expression right)
        {
            switch (type)
            {
                case E_OPERATION_TYPE.OT_ADD:
                    return Expression.Add(left, right);

                case E_OPERATION_TYPE.OT_SUB:
                    return Expression.Subtract(left, right);

                case E_OPERATION_TYPE.OT_MUL:
                    return Expression.Multiply(left, right);

                case E_OPERATION_TYPE.OT_DIV:

                    if ((mAttributes & E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED) == E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED)
                    {
                        return _createSafeDivisionOperator(left, right);
                    }

                    return Expression.Divide(left, right);

                case E_OPERATION_TYPE.OT_MOD:

                    if ((mAttributes & E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED) == E_INTERPRETER_ATTRIBUTES.IA_IS_SAFE_DIVISION_ENABLED)
                    {
                        return _createSafeModuloOperator(left, right);
                    }

                    return Expression.Modulo(left, right);

                case E_OPERATION_TYPE.OT_MAX:
                    return Expression.Call(typeof(Math).GetMethod("Max", new Type[] { typeof(Int32), typeof(Int32) }), left, right);

                case E_OPERATION_TYPE.OT_MIN:
                    return Expression.Call(typeof(Math).GetMethod("Min", new Type[] { typeof(Int32), typeof(Int32) }), left, right);
            }

            return Expression.Empty();
        }

        protected Expression _createSafeDivisionOperator(Expression first, Expression second)
        {
            return Expression.Condition(Expression.Equal(second, Expression.Constant(0)), first, Expression.Divide(first, second));
        }

        protected Expression _createSafeModuloOperator(Expression first, Expression second)
        {
            return Expression.Condition(Expression.Equal(second, Expression.Constant(0)), first, Expression.Modulo(first, second));
        }
    }
}
