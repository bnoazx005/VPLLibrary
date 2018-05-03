namespace VPLLibrary.Interfaces
{
    /// <summary>
    /// interface IEnvironment
    /// 
    /// The interface describes a functionality of
    /// a interpreter's execution environment, that
    /// stores variables and its states
    /// </summary>

    public interface IEnvironment
    {
        /// <summary>
        /// The method defines a new variable within inner store.
        /// If there is another one variable with the same name,
        /// the method throws an exception.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <param name="value">An initial value</param>

        void Define(string id, int[] value);

        /// <summary>
        /// The method assigns a new value into a specified variable.
        /// It defines a new variable if there is no specified variable within
        /// the environment.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <param name="value">A value</param>

        void Assign(string id, int[] value);

        /// <summary>
        /// The method returns a value of specified variable.
        /// Throws RuntimeError exception if there is no specified.
        /// variable.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <returns>A value of specified variable</returns>

        int[] Get(string id);

        /// <summary>
        /// The method returns an identifier of a last assigned variable
        /// </summary>
        /// <returns>An identifier of a last assigned variable</returns>

        string GetLastAssignedVariableId();

        /// <summary>
        /// The method returns true if a variable with specified name
        /// exists within the environment.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <returns>The method returns true if a variable with specified name
        /// exists within the environment.</returns>

        bool Exists(string id);

        /// <summary>
        /// The method cleans up all data stored within the environment
        /// </summary>

        void Clear();
    }
}
