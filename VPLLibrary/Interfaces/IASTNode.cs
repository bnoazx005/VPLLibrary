using System.Collections.Generic;
using VPLLibrary.Impls;


namespace VPLLibrary.Interfaces
{
    /// <summary>
    /// The enumeration represents all types of AST nodes
    /// </summary>

    public enum E_NODE_TYPE
    {
        NT_PROGRAM,
        NT_ASSIGMENT,
        NT_IDENTIFIER,
        NT_EXPRESSION,
        NT_VALUE,
        NT_CALL,
        NT_BINARY_LAMBDA_FUNC,
        NT_LAMBDA_PREDICATE,
        NT_UNARY_LAMBDA_FUNC,
        NT_DEFAULT
    }


    /// <summary>
    /// The enumeration contains all possible mathematical operations
    /// </summary>

    public enum E_OPERATION_TYPE
    {
        OT_ADD,
        OT_SUB,
        OT_MUL,
        OT_DIV,
        OT_MOD,
        OT_POW,
    }


    /// <summary>
    /// The enumeration contains all possible comparison operations
    /// </summary>

    public enum E_LOGIC_OP_TYPE
    {
        LOT_LT,         // <
        LOT_LE,         // <=
        LOT_GT,         // >
        LOT_GE,         // >=
        LOT_EQ,         // ==
        LOT_NEQ,        // !=
        LOT_MOD         // % (modulo comparison)
    }


    /// <summary>
    /// interface IASTNode
    /// 
    /// The interface represents a single AST node's functionality
    /// </summary>

    public interface IASTNode
    {
        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>        
        /// /// <returns>A value of T type</returns>

        T Accept<T>(IVisitor<T> interpreter);

        /// <summary>
        /// The readonly property returns a type of a node
        /// </summary>

        E_NODE_TYPE Type { get; }

        /// <summary>
        /// The readonly property returns left child of a node
        /// </summary>

        IASTNode Left { get; set; }

        /// <summary>
        /// The readonly property returns right child of a node
        /// </summary>

        IASTNode Right { get; set; }
    }


    /// <summary>
    /// interface IProgramASTNode
    /// 
    /// The interface represents a program node within an AST
    /// </summary>

    public interface IProgramASTNode
    {
        /// <summary>
        /// The readonly property returns a list of commands
        /// </summary>

        IList<IASTNode> Operations { get; }
    }


    /// <summary>
    /// interface IAssigmentASTNode
    /// 
    /// The interface represents an assigment within an AST
    /// </summary>

    public interface IAssigmentASTNode
    {
        /// <summary>
        /// The readonly property returns an identifier
        /// </summary>

        string Id { get; }

        /// <summary>
        /// The readonly property returns an expression
        /// </summary>

        IASTNode Expression { get; }
    }



    /// <summary>
    /// interface IIdentifierASTNode
    /// 
    /// The interface represents an identifier's node within an AST
    /// </summary>

    public interface IIdentifierASTNode
    {
        /// <summary>
        /// The readonly property returns an identifier
        /// </summary>

        string Name { get; }
    }



    /// <summary>
    /// interface IValueASTNode
    /// 
    /// The interface represents an value's node within an AST
    /// </summary>

    public interface IValueASTNode
    {
        /// <summary>
        /// The readonly property returns a value
        /// </summary>

        int[] Value { get; }
    }


    /// <summary
    /// interface ICallASTNode
    /// 
    /// The interface represents a intrinsic function's call within an AST
    /// </summary>
    /// 

    public interface ICallASTNode
    {
        /// <summary>
        /// The readonly property returns a name of a function
        /// </summary>

        E_INTRINSIC_FUNC_TYPE IntrinsicType { get; }

        /// <summary>
        /// The readonly property returns arguments of a function
        /// </summary>

        IList<IASTNode> Args { get; }
    }


    /// <summary>
    /// interface IBinaryLambdaFuncASTNode
    /// 
    /// The interface represents a binary lambda functor within an AST
    /// </summary>

    public interface IBinaryLambdaFuncASTNode
    {
        /// <summary>
        /// The readonly property returns a type of mathematical operation
        /// that the lambda executes
        /// </summary>

        E_OPERATION_TYPE OpType { get; }
    }


    /// <summary>
    /// interface ILambdaPredicateASTNode
    /// 
    /// The interface represents a lambda predicate within an AST
    /// </summary>

    public interface ILambdaPredicateASTNode
    {
        /// <summary>
        /// The readonly property returns a type of boolean operation
        /// that the lambda executes
        /// </summary>

        E_LOGIC_OP_TYPE LOPType { get; }

        /// <summary>
        /// The readonly property returns a value of first operand
        /// </summary>

        IValueASTNode FirstOperand { get; }

        /// <summary>
        /// The readonly property returns a value of second operand
        /// </summary>

        IValueASTNode SecondOperand { get; }
    }


    /// <summary>
    /// interface IUnaryLambdaFuncASTNode
    /// 
    /// The interface represents an unary lambda functor within an AST
    /// </summary>

    public interface IUnaryLambdaFuncASTNode
    {
        /// <summary>
        /// The readonly property returns a type of mathematical operation
        /// that the lambda executes
        /// </summary>

        E_OPERATION_TYPE OpType { get; }

        /// <summary>
        /// The readonly property returns a body's node of a functor
        /// </summary>

        IASTNode Body { get; }
    }
}
