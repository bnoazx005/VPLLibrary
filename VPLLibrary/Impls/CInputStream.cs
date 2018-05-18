using System;
using System.Collections.Generic;
using VPLLibrary.Interfaces;


namespace VPLLibrary.Impls
{

    /// <summary>
    /// class CInputSteam
    /// 
    /// The implementation of IInputStream. It provides
    /// mechanisms of reading arrays that are stored within RAM
    /// </summary>

    public class CInputStream: IInputStream
    {
        protected IList<int[]> mInputData;

        public CInputStream(int[][] inputData)
        {
            mInputData = new List<int[]>(inputData.Length);

            foreach (int[] inputArray in inputData)
            {
                if (inputArray == null)
                {
                    continue;
                }

                int[] inputArrayCopy = new int[inputArray.Length];

                Array.Copy(inputArray, inputArrayCopy, inputArray.Length);

                mInputData.Add(inputArrayCopy);
            }
        }

        /// <summary>
        /// The method returns an input parameter, which is placed
        /// at position of specified index
        /// </summary>
        /// <param name="index">An input index, should be positive</param>
        /// <returns>An input data (an integer array)</returns>

        public int[] Read(int index)
        {
            if (index >= mInputData.Count)
            {
                return CIntrinsicsUtils.mNullArray;
            }
            
            return mInputData[index] ?? CIntrinsicsUtils.mNullArray;
        }
    }
}
