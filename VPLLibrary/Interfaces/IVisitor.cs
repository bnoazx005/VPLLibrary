namespace VPLLibrary.Interfaces
{
    public interface IVisitor<T>
    {
        T visitProgramNode(IASTNode program);

        T visitAssigmentNode(IAssigmentASTNode assigment);

        T visitIdentifierNode(IIdentifierASTNode identifier);

        T visitValueNode(IValueASTNode value);
    }
}
