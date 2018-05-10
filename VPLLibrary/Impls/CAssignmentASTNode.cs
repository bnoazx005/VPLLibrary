using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class COperationASTNode
    /// 
    /// The class represents the following BNF rule
    /// assigment ::= identifier <- int |
    /// identifier <- int |
    /// identifier <- expression
    /// </summary>

    public class CAssignmentASTNode : CBaseASTNode, IAssigmentASTNode
    {
        protected string   mIdentifier;
        
        public CAssignmentASTNode(string id, IASTNode expr) :
            base(E_NODE_TYPE.NT_ASSIGMENT)
        {
            mIdentifier = id;

            expr.Parent = this;

            mChildren.Add(expr);
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitAssigmentNode(this);
        }

        /// <summary>
        /// The readonly property returns an identifier
        /// </summary>

        public string Id
        {
            get
            {
                return mIdentifier;
            }
        }

        /// <summary>
        /// The readonly property returns an expression
        /// </summary>

        public IASTNode Expression
        {
            get
            {
                return mChildren[0];
            }
        }
    }
}
