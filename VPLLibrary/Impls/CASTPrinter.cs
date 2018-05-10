using System;
using System.Text;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CASTPrinter
    /// 
    /// The class provides functionality of converting ASTs to their string views
    /// </summary>

    public class CASTPrinter : IVisitor<string>
    {
        public string Print(IASTNode tree)
        {
            if (tree == null)
            {
                throw new ArgumentNullException("tree", "The argument cannot equal to null");
            }

            return tree.Accept(this);
        }

        public string VisitAssigmentNode(IAssigmentASTNode assigment)
        {
            StringBuilder assigmentStr = new StringBuilder();

            assigmentStr.AppendFormat("{0} <- {1}", assigment.Id, assigment.Expression.Accept(this));

            return assigmentStr.ToString();
        }

        public string VisitBinaryLambdaFuncNode(IBinaryLambdaFuncASTNode binaryLambdaFunc)
        {
            return string.Format("({0})", _constructOperationStr(binaryLambdaFunc.OpType));
        }

        public string VisitCallNode(ICallASTNode call)
        {
            StringBuilder argsStr = new StringBuilder();

            foreach (IASTNode arg in call.Args)
            {
                argsStr.AppendFormat("{0} ", arg.Accept(this));
            }

            StringBuilder callStr = new StringBuilder();

            switch (call.IntrinsicType)
            {
                case E_INTRINSIC_FUNC_TYPE.IFT_CONCAT:
                    callStr.Append("CONCAT");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_FILTER:
                    callStr.Append("FILTER");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_GET:
                    callStr.Append("GET");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_HEAD:
                    callStr.Append("HEAD");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_INDEXOF:
                    callStr.Append("INDEXOF");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_LAST:
                    callStr.Append("LAST");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_LEN:
                    callStr.Append("LEN");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_MAP:
                    callStr.Append("MAP");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_MAX:
                    callStr.Append("MAX");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_MIN:
                    callStr.Append("MIN");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_REVERSE:
                    callStr.Append("REVERSE");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_SLICE:
                    callStr.Append("SLICE");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_SORT:
                    callStr.Append("SORT");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_SUM:
                    callStr.Append("SUM");
                    break;

                case E_INTRINSIC_FUNC_TYPE.IFT_VECOP:
                    callStr.Append("VECOP");
                    break;
            }

            callStr.AppendFormat(" {0}", argsStr);

            return callStr.ToString();
        }

        public string VisitIdentifierNode(IIdentifierASTNode identifier)
        {
            return identifier.Name;
        }

        public string VisitIfThenElseNode(IIfThenElseASTNode ifStatementNode)
        {
            StringBuilder branchingStr = new StringBuilder();

            branchingStr.AppendFormat("IF {0} {1} THEN {2} ELSE {3}",
                                      (ifStatementNode.Variable as IASTNode).Accept(this),
                                      (ifStatementNode.Predicate as IASTNode).Accept(this),
                                      ifStatementNode.ThenBranch.Accept(this),
                                      ifStatementNode.ElseBranch.Accept(this));

            return branchingStr.ToString();
        }

        public string VisitLambdaPredicateNode(ILambdaPredicateASTNode lambdaPredicate)
        {
            string firstOperandStr = (lambdaPredicate.FirstOperand as IASTNode).Accept(this);

            switch (lambdaPredicate.LOPType)
            {
                case E_LOGIC_OP_TYPE.LOT_GT:
                    return string.Format("({0} {1})", ">", firstOperandStr);

                case E_LOGIC_OP_TYPE.LOT_GE:
                    return string.Format("({0} {1})", ">=", firstOperandStr);

                case E_LOGIC_OP_TYPE.LOT_LT:
                    return string.Format("({0} {1})", "<", firstOperandStr);

                case E_LOGIC_OP_TYPE.LOT_LE:
                    return string.Format("({0} {1})", "<=", firstOperandStr);

                case E_LOGIC_OP_TYPE.LOT_EQ:
                    return string.Format("({0} {1})", "==", firstOperandStr);

                case E_LOGIC_OP_TYPE.LOT_NEQ:
                    return string.Format("({0} {1})", "!=", firstOperandStr);

                case E_LOGIC_OP_TYPE.LOT_MOD:
                    return string.Format("(% {0} == {1})", firstOperandStr, (lambdaPredicate.SecondOperand as IASTNode).Accept(this));
            }

            return string.Empty;
        }

        public string VisitProgramNode(IASTNode program)
        {
            IProgramASTNode programTree = program as IProgramASTNode;

            var operators = programTree.Operations;

            StringBuilder programStr = new StringBuilder();

            foreach(var opNode in operators)
            {
                if (opNode == null)
                {
                    throw new NullReferenceException("The tree's node cannot equal to null");
                }

                programStr.AppendLine(opNode.Accept(this));
            }

            return programStr.ToString();
        }

        public string VisitReadInputNode(IReadInputASTNode readNode)
        {
            return "[int]";
        }

        public string VisitUnaryLambdaFuncNode(IUnaryLambdaFuncASTNode unaryLambdaFunc)
        {
            StringBuilder lambdaStr = new StringBuilder();

            lambdaStr.AppendFormat("({0}{1})",
                                   _constructOperationStr(unaryLambdaFunc.OpType),
                                   unaryLambdaFunc.Body.Accept(this));

            return lambdaStr.ToString();
        }

        public string VisitValueNode(IValueASTNode value)
        {
            StringBuilder arrayStr = new StringBuilder("[");

            int[] values = value.Value;

            int valuesCount = values.Length;

            if (valuesCount == 0)
            {
                return "NULL";
            }

            if (valuesCount == 1)
            {
                return values[0].ToString();
            }

            for (int i = 0; i < valuesCount; ++i)
            {
                if (i < valuesCount - 1)
                {
                    arrayStr.AppendFormat("{0}, ", values[i]);

                    continue;
                }

                arrayStr.AppendFormat("{0}]", values[i]);
            }

            return arrayStr.ToString();
        }

        protected string _constructOperationStr(E_OPERATION_TYPE type)
        {
            switch (type)
            {
                case E_OPERATION_TYPE.OT_ADD:
                    return "+";

                case E_OPERATION_TYPE.OT_SUB:
                    return "-";

                case E_OPERATION_TYPE.OT_MUL:
                    return "*";

                case E_OPERATION_TYPE.OT_DIV:
                    return "/";

                case E_OPERATION_TYPE.OT_MOD:
                    return "%";

                case E_OPERATION_TYPE.OT_POW:
                    return "**";

                case E_OPERATION_TYPE.OT_MAX:
                    return "MAX";

                case E_OPERATION_TYPE.OT_MIN:
                    return "MIN";
            }

            return string.Empty;
        }
    }
}
