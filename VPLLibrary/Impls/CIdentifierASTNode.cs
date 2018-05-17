using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CIdentifierASTNode
    /// 
    /// The class represents a node, which contains identifier
    /// </summary>

    public class CIdentifierASTNode : CBaseASTNode, IIdentifierASTNode
    {
        protected string mName;

        public CIdentifierASTNode(string id, E_NODE_ATTRIBUTES attributes = E_NODE_ATTRIBUTES.NA_DEFAULT) :
            base(E_NODE_TYPE.NT_IDENTIFIER)
        {
            mName = id;

            mAttributes = attributes | E_NODE_ATTRIBUTES.NA_IS_LEAF_NODE;
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitIdentifierNode(this);
        }

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            return new CIdentifierASTNode(mName);
        }

        public override bool Equals(IASTNode obj)
        {
            if (obj.Type != E_NODE_TYPE.NT_IDENTIFIER ||
                obj.Attributes != mAttributes ||
                (obj as IIdentifierASTNode).Name != mName)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The readonly property returns an identifier
        /// </summary>

        public string Name
        {
            get
            {
                return mName;
            }
        }
    }
}
