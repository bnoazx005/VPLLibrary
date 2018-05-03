using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CProgramASTNode
    /// 
    /// The class represents the following BNF rule
    /// program ::= | assigment |
    /// assigment program  
    /// </summary>

    public class CProgramASTNode: CBaseASTNode
    {
        public CProgramASTNode(IASTNode left, IASTNode right):
            base(E_NODE_TYPE.NT_PROGRAM)
        {
            mLeftChild  = left;
            mRightChild = right;
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return default(T);
        }
    }
}
