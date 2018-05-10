using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// abstract class CBaseASTNode
    /// 
    /// The class represents a base AST node
    /// </summary>

    public abstract class CBaseASTNode: IASTNode
    {
        protected E_NODE_TYPE mType;

        protected IASTNode    mParentNode;

        public CBaseASTNode(E_NODE_TYPE type = E_NODE_TYPE.NT_DEFAULT)
        {
            mType = type;
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public abstract T Accept<T>(IVisitor<T> interpreter);

        /// <summary>
        /// The readonly property returns a type of a node
        /// </summary>

        public E_NODE_TYPE Type
        {
            get
            {
                return mType;
            }
        }

        /// <summary>
        /// The readonly property returns a parent of a node
        /// </summary>

        public IASTNode Parent
        {
            get
            {
                return mParentNode;
            }

            set
            {
                mParentNode = value;
            }
        }
    }
}
