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
        
        protected IValueASTNode   mFirstOpValue;

        protected IValueASTNode   mSecondOpValue; //needed when modulo comparison is used (stores 1 or 0)

        public CLambdaPredicateASTNode(E_LOGIC_OP_TYPE type, IValueASTNode firstOp, IValueASTNode secondOp) :
            base(E_NODE_TYPE.NT_LAMBDA_PREDICATE)
        {
            mLogicOpType = type;

            mFirstOpValue = firstOp;
            
            mSecondOpValue = secondOp;
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
                return mFirstOpValue;
            }
        }

        /// <summary>
        /// The readonly property returns a value of second operand
        /// </summary>

        public IValueASTNode SecondOperand
        {
            get
            {
                return mSecondOpValue;
            }
        }
    }
}
