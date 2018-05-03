using System.Collections.Generic;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CEnvironment
    /// 
    /// The class represents a storage of all global variables,
    /// which is created during execution of a program
    /// </summary>

    public class CEnvironment: IEnvironment
    {
        protected IDictionary<string, int[]> mHashMap;

        protected string                     mLastAssignedVariableId;

        public CEnvironment()
        {
            mHashMap = new Dictionary<string, int[]>();

            mLastAssignedVariableId = string.Empty;
        }

        /// <summary>
        /// The method defines a new variable within inner store.
        /// If there is another one variable with the same name,
        /// the method throws an exception.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <param name="value">An initial value</param>

        public void Define(string id, int[] value)
        {
            if (mHashMap.ContainsKey(id))
            {
                throw new CRuntimeError(string.Format("The variable [{0}] has already defined", id));
            }

            mHashMap.Add(id, value);

            mLastAssignedVariableId = id;
        }

        /// <summary>
        /// The method assigns a new value into a specified variable.
        /// It defines a new variable if there is no specified variable within
        /// the environment.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <param name="value">A value</param>

        public void Assign(string id, int[] value)
        {
            if (!mHashMap.ContainsKey(id))
            {
                Define(id, value);
                
                return;
            }

            mHashMap[id] = value;

            mLastAssignedVariableId = id;
        }

        /// <summary>
        /// The method returns a value of specified variable.
        /// Throws RuntimeError exception if there is no specified.
        /// variable.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <returns>A value of specified variable</returns>

        public int[] Get(string id)
        {
            if (!mHashMap.ContainsKey(id))
            {
                throw new CRuntimeError(string.Format("Undeclared variable [{0}]", id));
            }

            return mHashMap[id];
        }

        /// <summary>
        /// The method returns an identifier of a last assigned variable
        /// </summary>
        /// <returns>An identifier of a last assigned variable</returns>

        public string GetLastAssignedVariableId()
        {
            return mLastAssignedVariableId;
        }

        /// <summary>
        /// The method returns true if a variable with specified name
        /// exists within the environment.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <returns>The method returns true if a variable with specified name
        /// exists within the environment.</returns>

        public bool Exists(string id)
        {
            return mHashMap.ContainsKey(id);
        }

        /// <summary>
        /// The method cleans up all data stored within the environment
        /// </summary>

        public void Clear()
        {
            mHashMap.Clear();
        }
    }
}
