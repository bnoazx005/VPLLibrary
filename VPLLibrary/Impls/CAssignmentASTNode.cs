using System;
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
            IASTNode variableNode = new CIdentifierASTNode(id, E_NODE_ATTRIBUTES.NA_LVALUE);

            variableNode.Parent = this;

            variableNode.NodeId = 0;

            mChildren.Add(variableNode);

            expr.Parent = this;

            expr.NodeId = 1;

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
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            return new CAssignmentASTNode((mChildren[0] as IIdentifierASTNode).Name, mChildren[1].Clone() as IASTNode);
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
