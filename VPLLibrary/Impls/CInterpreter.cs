using System;
using System.Collections.Generic;
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
        protected IEnvironment mEnvironment;

        public CInterpreter()
        {
            mEnvironment = new CEnvironment();
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

            mEnvironment.Clear();

            return (int[])program.Accept(this);
        }

        public Object VisitProgramNode(IASTNode program)
        {
            if (program.Type != E_NODE_TYPE.NT_PROGRAM)
            {
                throw new CRuntimeError("A root of an AST should have NT_PROGRAM type");
            }

            IList<IASTNode> commands = (program as IProgramASTNode).Operations;

            foreach (IASTNode currCommand in commands)
            {
                currCommand.Accept(this);
            }

            string lastAssignedVariableId = mEnvironment.GetLastAssignedVariableId();

            // if input program is empty, just return an empty array as a result
            if (string.IsNullOrEmpty(lastAssignedVariableId))
            {
                return new int[0];
            }

            return mEnvironment.Get(lastAssignedVariableId);
        }

        public Object VisitAssigmentNode(IAssigmentASTNode assigment)
        {
            IASTNode rightOp = assigment.Expression;

            // if right operator is just an identifier or a value, then assign it immediately
            if (rightOp.Type == E_NODE_TYPE.NT_VALUE)
            {
                int[] assignedValue = (rightOp as IValueASTNode).Value;

                mEnvironment.Assign(assigment.Id, assignedValue);

                return assignedValue;
            }

            if (rightOp.Type == E_NODE_TYPE.NT_IDENTIFIER)
            {
                string id = (rightOp as IIdentifierASTNode).Name;

                int[] assignedValue = mEnvironment.Get(id);

                mEnvironment.Assign(assigment.Id, assignedValue);

                return assignedValue;
            }

            // common case
            Object value = assigment.Expression.Accept(this);

            mEnvironment.Assign(assigment.Id, (int[])value);

            return value;
        }

        public Object VisitIdentifierNode(IIdentifierASTNode identifier)
        {
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier", "The argument cannot equal to null");
            }

            return identifier.Name;
        }

        public Object VisitValueNode(IValueASTNode value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "The argument cannot equal to null");
            }

            return value.Value;
        }
    }
}
