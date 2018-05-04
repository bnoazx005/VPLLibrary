using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CReadInputASTNode
    /// 
    /// The class represents read input command
    /// 
    /// operation ::= identifier <- int | identifier <- [int]
    /// </summary>

    public class CReadInputASTNode: CBaseASTNode, IReadInputASTNode
    {
        public CReadInputASTNode(bool isArray):
            base(isArray ? E_NODE_TYPE.NT_READ_INT_ARRAY : E_NODE_TYPE.NT_READ_INT)
        {
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitReadInputNode(this);
        }
    }
}
