using System;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CInterpreter
    /// 
    /// This is main class within the library. It provides
    /// a functionality of evaluating correct AST, which
    /// describes operations over interger arrays
    /// </summary>

    public class CInterpreter: IInterpreter, IVisitor<Object>
    {
        public CInterpreter()
        {
        }

        /// <summary>
        /// The method evaluates (executes) a program, which is represented as a syntax tree of nodes.
        /// An input parameters could be integer arrays or integer values (represented as an array with single value).
        /// </summary>
        /// <param name="program">A program is represented as an AST</param>
        /// <param name="inputData">Program's input data</param>
        /// <returns>An evaluated array of some data</returns>

        public int[] Eval(IASTNode program, int[][] inputData)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program", "The argument cannot equal to null");
            }

            if (inputData == null)
            {
                throw new ArgumentNullException("inputData", "The argument cannot equal to null");
            }

            return (int[])program.Accept(this);
        }

        public Object visitProgramNode(IASTNode program)
        {
            if (program.Type != E_NODE_TYPE.NT_PROGRAM)
            {
                throw new CRuntimeError("A root of an AST should have NT_PROGRAM type");
            }

            return null;
        }

        public Object visitAssigmentNode(IAssigmentASTNode assigment)
        {
            return null;
        }

        public Object visitIdentifierNode(IIdentifierASTNode identifier)
        {
            return null;
        }

        public Object visitValueNode(IValueASTNode value)
        {
            return null;
        }
    }
}
