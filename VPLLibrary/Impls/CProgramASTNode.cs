using System.Collections.Generic;
using VPLLibrary.Interfaces;
using System;


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
        /// The method adds a new operator in the end of inner array
        /// </summary>
        /// <param name="assigmentOp"></param>

        public void AddOperator(IASTNode assigmentOp)
        {
            if (assigmentOp == null)
            {
                throw new ArgumentNullException("assigmentOp", "The argument cannot equal to null");
            }

            assigmentOp.Parent = this;
            assigmentOp.NodeId = mChildren.Count;

            mChildren.Add(assigmentOp);
        }

        /// <summary>
        /// The method removes specified operator
        /// </summary>
        /// <param name="index">An index of an operator</param>

        public void RemoveOperator(int index)
        {
            if (index < 0 || index >= mChildren.Count)
            {
                throw new IndexOutOfRangeException("index is out of range");
            }

            mChildren.RemoveAt(index);

            // recompute nodes' ids
            for (int i = index; i < mChildren.Count; ++i)
            {
                mChildren[i].NodeId = i;
            }
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
