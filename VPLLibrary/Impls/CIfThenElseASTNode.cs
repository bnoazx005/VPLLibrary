using System.Collections.Generic;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CIfThenElseASTNode
    /// </summary>

    public class CIfThenElseASTNode: CBaseASTNode, IIfThenElseASTNode
    {
        protected IIdentifierASTNode      mVariable;

        protected ILambdaPredicateASTNode mPredicate;

        protected IASTNode                mThenBranch;

        protected IASTNode                mElseBranch;

        public CIfThenElseASTNode(IIdentifierASTNode var, ILambdaPredicateASTNode pred, 
                                  IASTNode thenBranch, IASTNode elseBranch):
            base(E_NODE_TYPE.NT_IF_THEN_ELSE)
        {
            IASTNode variableNode  = var as IASTNode;
            IASTNode predicateNode = pred as IASTNode;

            variableNode.Parent  = this;
            predicateNode.Parent = this;
            thenBranch.Parent    = this;
            elseBranch.Parent    = this;

            variableNode.NodeId  = 0;
            predicateNode.NodeId = 1;
            thenBranch.NodeId    = 2;
            elseBranch.NodeId    = 3;

            mChildren.Add(variableNode);
            mChildren.Add(predicateNode);
            mChildren.Add(thenBranch);
            mChildren.Add(elseBranch);
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitIfThenElseNode(this);
        }

        /// <summary>
        /// The readonly property returns a variable under checking
        /// </summary>

        public IIdentifierASTNode Variable
        {
            get
            {
                return mChildren[0] as IIdentifierASTNode;
            }
        }

        /// <summary>
        /// The readonly property returns a predicate's node to test a variable
        /// </summary>

        public ILambdaPredicateASTNode Predicate
        {
            get
            {
                return mChildren[1] as ILambdaPredicateASTNode;
            }
        }

        /// <summary>
        /// The readonly property returns a node that will be executed if a predicate returns true
        /// </summary>

        public IASTNode ThenBranch
        {
            get
            {
                return mChildren[2];
            }
        }

        /// <summary>
        /// The readonly property returns a node that will be executed if a predicate returns false
        /// </summary>

        public IASTNode ElseBranch
        {
            get
            {
                return mChildren[3];
            }
        }
    }
}
