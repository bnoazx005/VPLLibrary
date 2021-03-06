﻿namespace VPLLibrary.Interfaces
{
    public interface IVisitor<T>
    {
        T VisitProgramNode(IASTNode program);

        T VisitAssigmentNode(IAssigmentASTNode assigment);

        T VisitIdentifierNode(IIdentifierASTNode identifier);

        T VisitValueNode(IValueASTNode value);

        T VisitCallNode(ICallASTNode call);

        T VisitBinaryLambdaFuncNode(IBinaryLambdaFuncASTNode binaryLambdaFunc);

        T VisitLambdaPredicateNode(ILambdaPredicateASTNode lambdaPredicate);

        T VisitUnaryLambdaFuncNode(IUnaryLambdaFuncASTNode unaryLambdaFunc);

        T VisitIfThenElseNode(IIfThenElseASTNode ifStatementNode);

        T VisitReadInputNode(IReadInputASTNode readNode);
    }
}
