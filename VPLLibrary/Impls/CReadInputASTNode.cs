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
            mAttributes = E_NODE_ATTRIBUTES.NA_IS_LEAF_NODE;
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

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            return new CReadInputASTNode(mType == E_NODE_TYPE.NT_READ_INT_ARRAY ? true : false);
        }
    }
}
