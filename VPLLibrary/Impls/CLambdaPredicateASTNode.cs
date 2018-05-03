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

        public CLambdaPredicateASTNode(E_LOGIC_OP_TYPE type) :
            base(E_NODE_TYPE.NT_LAMBDA_PREDICATE)
        {
            mLogicOpType = type;
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
    }
}
