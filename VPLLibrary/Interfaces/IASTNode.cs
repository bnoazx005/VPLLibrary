﻿using System.Collections.Generic;


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
        NT_DEFAULT
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
}
