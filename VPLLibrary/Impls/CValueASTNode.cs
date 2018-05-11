using System;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// class CValueASTNode
    /// 
    /// The class represents a node, which contains value
    /// </summary>

    public class CValueASTNode : CBaseASTNode, IValueASTNode
    {
        protected int[] mValue;

        public CValueASTNode(int[] value, E_NODE_ATTRIBUTES attributes = E_NODE_ATTRIBUTES.NA_DEFAULT) :
            base(E_NODE_TYPE.NT_VALUE)
        {
            mValue = value;

            mAttributes = attributes | E_NODE_ATTRIBUTES.NA_IS_LEAF_NODE;
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public override T Accept<T>(IVisitor<T> interpreter)
        {
            return interpreter.VisitValueNode(this);
        }

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public override object Clone()
        {
            int[] valueCopy = new int[mValue.Length];

            Array.Copy(mValue, valueCopy, mValue.Length);

            return new CValueASTNode(valueCopy);
        }

        /// <summary>
        /// The readonly property returns a value
        /// </summary>

        public int[] Value
        {
            get
            {
                return mValue;
            }
        }
    }
}
