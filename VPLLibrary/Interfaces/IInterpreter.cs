namespace VPLLibrary.Interfaces
{
    /// <summary>
    /// interface IInterpreter
    /// 
    /// The class provides functionality of AST's execution for
    /// the DSL
    /// </summary>

    public interface IInterpreter
    {
        /// <summary>
        /// The method evaluates (executes) a program, which is represented as a syntax tree of nodes.
        /// An input parameters could be integer arrays or integer values (represented as an array with single value).
        /// </summary>
        /// <param name="program">A program is represented as an AST</param>
        /// <param name="inputData">Program's input data</param>
        /// <returns>An evaluated array of some data</returns>

        int[] Eval(IASTNode program, int[][] inputData);
    }
}
