using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CValueASTNode
    /// 
    /// The class represents a node, which contains value
    /// </summary>

    public class CValueASTNode : CBaseASTNode, IValueASTNode
    {
        protected int[] mValue;

        public CValueASTNode(int[] value) :
            base(E_NODE_TYPE.NT_VALUE)
        {
            mValue = value;
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

        /// <summary>
        /// The readonly property returns a value
        /// </summary>

        public int[] Value
        {
            get
            {
                return mValue;
            }
        }
    }
}
