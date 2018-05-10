using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CLambdaPredicateASTNode
    /// 
    /// The class represents the following BNF rule
    /// lambda-predicate ::= ( lop integer) | 
    /// (% integer == 1) | (% integer == 0)
    /// </summary>

    public class CLambdaPredicateASTNode : CBaseASTNode, ILambdaPredicateASTNode
    {
        protected E_LOGIC_OP_TYPE mLogicOpType;

        /// <summary>
        /// The base constructor
        /// </summary>
        /// <param name="type">A logical operation's type</param>
        /// <param name="firstOp">The operand stores an integer value</param>
        /// <param name="secondOp">The operand is needed when modulo comparison is used (stores 1 or 0)</param>

        public CLambdaPredicateASTNode(E_LOGIC_OP_TYPE type, IValueASTNode firstOp, IValueASTNode secondOp) :
            base(E_NODE_TYPE.NT_LAMBDA_PREDICATE)
        {
            mLogicOpType = type;

            IASTNode firstOpNode  = firstOp as IASTNode;
            IASTNode secondOpNode = secondOp as IASTNode;

            firstOpNode.Parent  = this;

            firstOpNode.NodeId = 0;

            mChildren.Add(firstOpNode);

            if (secondOpNode != null)
            {
                secondOpNode.Parent = this;

                secondOpNode.NodeId = 1;

                mChildren.Add(secondOpNode);
            }
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitLambdaPredicateNode(this);
        }

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            if (mChildren.Count <= 1)
            {
                return new CLambdaPredicateASTNode(mLogicOpType, mChildren[0].Clone() as IValueASTNode, null);
            }

            return new CLambdaPredicateASTNode(mLogicOpType, mChildren[0].Clone() as IValueASTNode, 
                                               mChildren[1].Clone() as IValueASTNode);
        }

        /// <summary>
        /// The readonly property returns a type of boolean operation
        /// that the lambda executes
        /// </summary>

        public E_LOGIC_OP_TYPE LOPType
        {
            get
            {
                return mLogicOpType;
            }
        }

        /// <summary>
        /// The readonly property returns a value of first operand
        /// </summary>

        public IValueASTNode FirstOperand
        {
            get
            {
                return mChildren[0] as IValueASTNode;
            }
        }

        /// <summary>
        /// The readonly property returns a value of second operand
        /// </summary>

        public IValueASTNode SecondOperand
        {
            get
            {
                return mChildren[1] as IValueASTNode;
            }
        }
    }
}
