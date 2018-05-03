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

        public CIdentifierASTNode(string id) :
            base(E_NODE_TYPE.NT_IDENTIFIER)
        {
            mName = id;
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
