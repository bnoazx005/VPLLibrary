using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CUnaryLambdaFuncASTNode
    /// 
    /// The class represents the following BNF rule
    /// unary-lambda-func ::= ( op body)
    /// body ::= integer | unary-lambda-func
    /// </summary>

    public class CUnaryLambdaFuncASTNode : CBaseASTNode, IUnaryLambdaFuncASTNode
    {
        protected E_OPERATION_TYPE mOpType;
        
        public CUnaryLambdaFuncASTNode(E_OPERATION_TYPE type, IASTNode body) :
            base(E_NODE_TYPE.NT_UNARY_LAMBDA_FUNC)
        {
            mOpType = type;
            
            body.Parent = this;

            body.NodeId = 0;

            mChildren.Add(body);
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitUnaryLambdaFuncNode(this);
        }

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            return new CUnaryLambdaFuncASTNode(mOpType, mChildren[0].Clone() as IASTNode);
        }

        public override bool Equals(IASTNode obj)
        {
            if (obj.Type != E_NODE_TYPE.NT_UNARY_LAMBDA_FUNC ||
                (obj as IUnaryLambdaFuncASTNode).OpType != mOpType)
            {
                return false;
            }

            return base.Equals(obj);
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

        /// <summary>
        /// The readonly property returns a body's node of a functor
        /// </summary>

        public IASTNode Body
        {
            get
            {
                return mChildren[0];
            }
        }
    }
}
