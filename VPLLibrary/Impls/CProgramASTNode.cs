using System.Collections.Generic;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CProgramASTNode
    /// 
    /// The class represents the following BNF rule
    /// program ::= | assigment |
    /// assigment program  
    /// </summary>

    public class CProgramASTNode : CBaseASTNode, IProgramASTNode
    {
        public CProgramASTNode() :
            base(E_NODE_TYPE.NT_PROGRAM)
        {
        }

        public CProgramASTNode(IList<IASTNode> commands):
            base(E_NODE_TYPE.NT_PROGRAM)
        {
            mChildren.AddRange(commands);

            foreach (IASTNode operatorNode in mChildren)
            {
                operatorNode.Parent = this;
            }
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitProgramNode(this);
        }

        /// <summary>
        /// The readonly property returns a list of commands
        /// </summary>

        public IList<IASTNode> Operations
        {
            get
            {
                return mChildren;
            }
        }
    }
}
