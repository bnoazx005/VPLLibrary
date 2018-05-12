using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CBinaryLambdaFuncASTNode
    /// 
    /// The class represents the following BNF rule
    /// binary-lambda-func ::= ( op )
    /// </summary>

    public class CBinaryLambdaFuncASTNode : CBaseASTNode, IBinaryLambdaFuncASTNode
    {
        protected E_OPERATION_TYPE mOpType;

        public CBinaryLambdaFuncASTNode(E_OPERATION_TYPE type) :
            base(E_NODE_TYPE.NT_BINARY_LAMBDA_FUNC)
        {
            mOpType = type;

            mAttributes = E_NODE_ATTRIBUTES.NA_IS_LEAF_NODE;
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitBinaryLambdaFuncNode(this);
        }

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            return new CBinaryLambdaFuncASTNode(mOpType);
        }

        /// <summary>
        /// The readonly property returns a type of mathematical operation
        /// that the lambda executes
        /// </summary>

        public E_OPERATION_TYPE OpType
        {
            get
            {
                return mOpType;
            }
        }
    }
}
