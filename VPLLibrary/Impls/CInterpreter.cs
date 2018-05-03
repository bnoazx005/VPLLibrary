﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CInterpreter
    /// 
    /// This is main class within the library. It provides
    /// a functionality of evaluating correct AST, which
    /// describes operations over interger arrays
    /// </summary>

    public class CInterpreter: IInterpreter, IVisitor<Object>
    {
        protected IEnvironment mEnvironment;

        public CInterpreter()
        {
            mEnvironment = new CEnvironment();
        }

        /// <summary>
        /// The method evaluates (executes) a program, which is represented as a syntax tree of nodes.
        /// An input parameters could be integer arrays or integer values (represented as an array with single value).
        /// </summary>
        /// <param name="program">A program is represented as an AST</param>
        /// <param name="inputData">Program's input data</param>
        /// <returns>An evaluated array of some data</returns>

        public int[] Eval(IASTNode program, int[][] inputData)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program", "The argument cannot equal to null");
            }

            if (inputData == null)
            {
                throw new ArgumentNullException("inputData", "The argument cannot equal to null");
            }

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

                mEnvironment.Assign(assigment.Id, assignedValue);

                return assignedValue;
            }

            if (rightOp.Type == E_NODE_TYPE.NT_IDENTIFIER)
            {
                string id = (rightOp as IIdentifierASTNode).Name;

                int[] assignedValue = mEnvironment.Get(id);

                mEnvironment.Assign(assigment.Id, assignedValue);

                return assignedValue;
            }

            // common case
            Object value = assigment.Expression.Accept(this);

            mEnvironment.Assign(assigment.Id, (int[])value);

            return value;
        }

        public Object VisitIdentifierNode(IIdentifierASTNode identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier", "The argument cannot equal to null");
            }

            return identifier.Name;
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
            ParameterExpression x = Expression.Parameter(typeof(int), "x");
            ParameterExpression y = Expression.Parameter(typeof(int), "y");

            Expression lambdaBody = null;

            switch (binaryLambdaFunc.OpType)
            {
                case E_OPERATION_TYPE.OT_ADD:
                    lambdaBody = Expression.Add(x, y);
                    break;
                case E_OPERATION_TYPE.OT_SUB:
                    lambdaBody = Expression.Subtract(x, y);
                    break;
                case E_OPERATION_TYPE.OT_MUL:
                    lambdaBody = Expression.Multiply(x, y);
                    break;
                case E_OPERATION_TYPE.OT_DIV:
                    lambdaBody = Expression.Divide(x, y);
                    break;
                case E_OPERATION_TYPE.OT_MOD:
                    lambdaBody = Expression.Modulo(x, y);
                    break;
                //case E_OPERATION_TYPE.OT_POW:
                //    lambdaBody = Expression.Power(x, y);
                //    break;
                default:
                    lambdaBody = Expression.Empty();
                    break;
            }

            return Expression.Lambda(lambdaBody, x, y).Compile();
        }

        public Object VisitLambdaPredicateNode(ILambdaPredicateASTNode lambdaPredicate)
        {
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
                    lambdaBody = Expression.Equal(Expression.Modulo(x, value), Expression.Constant(lambdaPredicate.SecondOperand.Value[0]));
                    break;
            }

            return Expression.Lambda(lambdaBody, x).Compile();
        }
    }
}
