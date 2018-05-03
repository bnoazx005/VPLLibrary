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

        public CEnvironment()
        {
            mHashMap = new Dictionary<string, int[]>();
        }

        /// <summary>
        /// The method defines a new variable within inner store.
        /// If there is another one variable with the same name,
        /// the method return false.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <param name="value">An initial value</param>
        /// <returns>Returns false if insertion cannot be completed, true in other cases</returns>

        public void Define(string id, int[] value)
        {
            if (mHashMap.ContainsKey(id))
            {
                throw new CRuntimeError(string.Format("The variable [{0}] has already defined", id));
            }

            mHashMap.Add(id, value);
        }

        /// <summary>
        /// The method assigns a new value into a specified variable.
        /// It returns false if there is no specified variable within
        /// the environment.
        /// </summary>
        /// <param name="id">An identifier</param>
        /// <param name="value">A value</param>
        /// <returns>Returns false if there is no specified variable and true in other cases</returns>

        public void Assign(string id, int[] value)
        {
            if (!mHashMap.ContainsKey(id))
            {
                throw new CRuntimeError(string.Format("Undeclared variable [{0}]", id));
            }

            mHashMap[id] = value;
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
    }
}
