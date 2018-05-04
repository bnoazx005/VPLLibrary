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

        protected IList<IASTNode>         mThenBranch;

        protected IList<IASTNode>         mElseBranch;

        public CIfThenElseASTNode(IIdentifierASTNode var, ILambdaPredicateASTNode pred, 
                                  IList<IASTNode> thenBranch, IList<IASTNode> elseBranch):
            base(E_NODE_TYPE.NT_IF_THEN_ELSE)
        {
            mVariable = var;

            mPredicate = pred;

            mThenBranch = thenBranch;

            mElseBranch = elseBranch;
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
                return mVariable;
            }
        }

        /// <summary>
        /// The readonly property returns a predicate's node to test a variable
        /// </summary>

        public ILambdaPredicateASTNode Predicate
        {
            get
            {
                return mPredicate;
            }
        }

        /// <summary>
        /// The readonly property returns a node that will be executed if a predicate returns true
        /// </summary>

        public IList<IASTNode> ThenBranch
        {
            get
            {
                return mThenBranch;
            }
        }

        /// <summary>
        /// The readonly property returns a node that will be executed if a predicate returns false
        /// </summary>

        public IList<IASTNode> ElseBranch
        {
            get
            {
                return mElseBranch;
            }
        }
    }
}
