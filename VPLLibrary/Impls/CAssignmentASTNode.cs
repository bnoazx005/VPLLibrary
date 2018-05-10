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
        public CAssignmentASTNode(string id, IASTNode expr) :
            base(E_NODE_TYPE.NT_ASSIGMENT)
        {
            IASTNode variableNode = new CIdentifierASTNode(id);

            variableNode.Parent = this;

            mChildren.Add(variableNode);

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

        public IIdentifierASTNode Id
        {
            get
            {
                return mChildren[0] as IIdentifierASTNode;
            }
        }

        /// <summary>
        /// The readonly property returns an expression
        /// </summary>

        public IASTNode Expression
        {
            get
            {
                return mChildren[1];
            }
        }
    }
}
