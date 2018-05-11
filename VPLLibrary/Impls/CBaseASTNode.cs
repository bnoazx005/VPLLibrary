using System;
using System.Collections.Generic;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{
    /// <summary>
    /// abstract class CBaseASTNode
    /// 
    /// The class represents a base AST node
    /// </summary>

    public abstract class CBaseASTNode: IASTNode
    {
        protected E_NODE_TYPE       mType;

        protected IASTNode          mParentNode;

        protected int               mNodeId;

        protected E_NODE_ATTRIBUTES mAttributes;

        protected IList<IASTNode>   mChildren;

        public CBaseASTNode(E_NODE_TYPE type)
        {
            mNodeId = -1;

            mType = type;

            mChildren = new List<IASTNode>();

            mAttributes = E_NODE_ATTRIBUTES.NA_DEFAULT;
        }

        /// <summary>
        /// The method accepts current node to a interpreter's reference
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns>A value of T type</returns>

        public abstract T Accept<T>(IVisitor<T> interpreter);

        /// <summary>
        /// The method creates deep clone of a node
        /// </summary>
        /// <returns>A cloned node</returns>

        public abstract object Clone();

        /// <summary>
        /// The readonly property returns a type of a node
        /// </summary>

        public E_NODE_TYPE Type
        {
            get
            {
                return mType;
            }
        }

        /// <summary>
        /// The readonly property returns a parent of a node
        /// </summary>

        public IASTNode Parent
        {
            get
            {
                return mParentNode;
            }

            set
            {
                mParentNode = value;
            }
        }

        /// <summary>
        /// The indexer provides mechanisms to iterate over node's children
        /// </summary>
        /// <param name="childId">A child's id</param>
        /// <returns>A reference to specified child</returns>

        public IASTNode this[int childId]
        {
            get
            {
                if (childId < 0 || childId >= mChildren.Count)
                {
                    throw new IndexOutOfRangeException("childId's value is out of range");
                }

                return mChildren[childId];
            }

            set
            {
                if (childId < 0 || childId >= mChildren.Count)
                {
                    throw new IndexOutOfRangeException("childId's value is out of range");
                }

                mChildren[childId] = value;
            }
        }

        /// <summary>
        /// The readonly property returns a number of children
        /// </summary>

        public int ChildrenCount
        {
            get
            {
                return mChildren.Count;
            }
        }

        /// <summary>
        /// The property provides get/set methods to change node's id
        /// </summary>

        public int NodeId
        {
            get
            {
                return mNodeId;
            }

            set
            {
                mNodeId = value;
            }
        }


        /// <summary>
        /// The property provides get/set methods to change attributes of a node
        /// </summary>

        public E_NODE_ATTRIBUTES Attributes
        {
            get
            {
                return mAttributes;
            }

            set
            {
                mAttributes = value;
            }
        }
    }
}
