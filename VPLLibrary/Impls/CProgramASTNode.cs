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
            mChildren = commands;

            int operatorsCount = commands.Count;

            IASTNode currOperator = null;

            for (int i = 0; i < operatorsCount; ++i)
            {
                currOperator = mChildren[i];

                currOperator.NodeId = i;
                currOperator.Parent = this;
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
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            IList<IASTNode> operatorsList = new List<IASTNode>();

            foreach (IASTNode currOperator in mChildren)
            {
                operatorsList.Add(currOperator.Clone() as IASTNode);
            }

            return new CProgramASTNode(operatorsList);
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
