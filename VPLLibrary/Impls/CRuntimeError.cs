using System;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CRuntimeError
    /// 
    /// The class represents a runtime error, which can occur
    /// during interpretation of an AST. It may be incorrect
    /// node at a particular position, incompatible or restricted
    /// constructions an etc.
    /// </summary>

    public class CRuntimeError: Exception
    {
        public CRuntimeError(string message):
            base(message)
        {
        }
    }
}
