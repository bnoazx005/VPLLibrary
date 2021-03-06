﻿using System.Collections.Generic;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CCallASTNode
    /// 
    /// The class represents the following BNF rule
    /// program ::= | assigment |
    /// assigment program  
    /// </summary>

    public class CCallASTNode : CBaseASTNode, ICallASTNode
    {
        protected E_INTRINSIC_FUNC_TYPE mIntrinsicType;

        public CCallASTNode(E_INTRINSIC_FUNC_TYPE type, IList<IASTNode> arguments) :
            base(E_NODE_TYPE.NT_CALL)
        {
            mIntrinsicType = type;

            mChildren = arguments;

            int argumentsCount = arguments.Count;

            IASTNode currArgument = null;

            for (int i = 0; i < argumentsCount; ++i)
            {
                currArgument = mChildren[i];

                currArgument.NodeId = i;
                currArgument.Parent = this;
            }
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitCallNode(this);
        }

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            IList<IASTNode> argsList = new List<IASTNode>();

            foreach (IASTNode currArg in mChildren)
            {
                argsList.Add(currArg.Clone() as IASTNode);
            }

            return new CCallASTNode(mIntrinsicType, argsList);
        }

        public override bool Equals(IASTNode obj)
        {
            if (obj.Type != E_NODE_TYPE.NT_CALL ||
                (obj as ICallASTNode).IntrinsicType != mIntrinsicType)
            {
                return false;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// The readonly property returns a name of a function
        /// </summary>

        public E_INTRINSIC_FUNC_TYPE IntrinsicType
        {
            get
            {
                return mIntrinsicType;
            }
        }

        /// <summary>
        /// The readonly property returns arguments of a function
        /// </summary>

        public IList<IASTNode> Args
        {
            get
            {
                return mChildren;
            }
        }
    }
}
