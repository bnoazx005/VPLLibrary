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

        protected IASTNode    mLeftChild;

        protected IASTNode    mRightChild;

        public CBaseASTNode()
        {
            mType = E_NODE_TYPE.NT_DEFAULT;

            mLeftChild = mRightChild = null;
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
        /// The readonly property returns left child of a node
        /// </summary>

        public IASTNode Left
        {
            get
            {
                return mLeftChild;
            }

            set
            {
                mLeftChild = value;
            }
        }

        /// <summary>
        /// The readonly property returns right child of a node
        /// </summary>

        public IASTNode Right
        {
            get
            {
                return mRightChild;
            }

            set
            {
                mRightChild = value;
            }
        }
    }
}
