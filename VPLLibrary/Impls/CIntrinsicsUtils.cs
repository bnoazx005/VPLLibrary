using System;
using System.Collections.Generic;

namespace VPLLibrary.Impls
{
    /// <summary>
    /// The enumeration contains all possible types of intrinsic functions, which is built in
    /// within the language's defenition
    /// </summary>

    public enum E_INTRINSIC_FUNC_TYPE
    {
        IFT_HEAD,
        IFT_LAST,
        IFT_SUM,
        IFT_REVERSE,
        IFT_SORT,
        IFT_MIN,
        IFT_MAX,
        IFT_CONCAT,
        IFT_SLICE,
        IFT_INDEXOF,
        IFT_GET,
        IFT_LEN,
        IFT_MAP,
        IFT_FILTER,
        IFT_VECOP,
        //IFT_DEFAULT
    }


    /// <summary>
    /// static class CIntrinsicsUtils
    /// 
    /// The class contains implementations of all built-int functions
    /// that the language provides. 
    /// </summary>

    public static class CIntrinsicsUtils
    {
        public static int[] mNullArray = new int[0];


        public static int[] Eval(E_INTRINSIC_FUNC_TYPE type, params Object[] args)
        {
            switch (type)
            {
                case E_INTRINSIC_FUNC_TYPE.IFT_HEAD:
                    return GetHead((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_LAST:
                    return GetLast((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_SUM:
                    return GetSumOfElements((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_REVERSE:
                    return Reverse((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_SORT:
                    return Sort((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_MIN:
                    return GetMinElement((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_MAX:
                    return GetMaxElement((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_CONCAT:
                    return Concat((int[])args[0], (int[])args[1]);

                case E_INTRINSIC_FUNC_TYPE.IFT_SLICE:
                    return Slice((int[])args[0], (int[])args[1], (int[])args[2]);

                case E_INTRINSIC_FUNC_TYPE.IFT_INDEXOF:
                    return GetIndexOf((int[])args[0], (int[])args[1]);

                case E_INTRINSIC_FUNC_TYPE.IFT_GET:
                    return GetElement((int[])args[0], (int[])args[1]);

                case E_INTRINSIC_FUNC_TYPE.IFT_LEN:
                    return GetLength((int[])args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_MAP:
                    return Map((int[])args[1], (Func<int, int>)args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_FILTER:
                    return Filter((int[])args[1], (Func<int, bool>)args[0]);

                case E_INTRINSIC_FUNC_TYPE.IFT_VECOP:
                    return VecOp((int[])args[1], (int[])args[2], (Func<int, int, int>)args[0]);
            }

            return mNullArray;
        }

        public static int[] GetHead(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            return new int[] { inputArray[0] };
        }

        public static int[] GetLast(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            return new int[] { inputArray[arrayLength - 1] };
        }

        public static int[] GetSumOfElements(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return new int[] { 0 };
            }

            int sum = 0;

            for (int i = 0; i < arrayLength; ++i)
            {
                sum += inputArray[i];
            }

            return new int[] { sum };
        }

        public static int[] Reverse(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            int[] resultArray = new int[arrayLength];

            Array.Copy(inputArray, resultArray, arrayLength);

            Array.Reverse(resultArray);

            return resultArray;
        }

        public static int[] GetMinElement(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            int minElement = int.MaxValue;

            for (int i = 0; i < arrayLength; ++i)
            {
                if (minElement > inputArray[i])
                {
                    minElement = inputArray[i];
                }
            }

            return new int[] { minElement };
        }

        public static int[] GetMaxElement(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            int maxElement = int.MinValue;

            for (int i = 0; i < arrayLength; ++i)
            {
                if (maxElement < inputArray[i])
                {
                    maxElement = inputArray[i];
                }
            }

            return new int[] { maxElement };
        }

        public static int[] Sort(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            int[] resultArray = new int[arrayLength];

            Array.Copy(inputArray, resultArray, arrayLength);

            Array.Sort(resultArray);

            return resultArray;
        }


        public static int[] Concat(int[] firstArray, int[] secondArray)
        {
            if (firstArray == null ||
                secondArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int firstArrayLength = firstArray.Length;
            int secondArrayLength = secondArray.Length;
            int totalLength = firstArrayLength + secondArrayLength;

            int[] resultArray = new int[totalLength];

            Array.Copy(firstArray, resultArray, firstArrayLength);
            Array.Copy(secondArray, 0, resultArray, firstArrayLength, secondArrayLength);

            return resultArray;
        }

        public static int[] Slice(int[] inputArray, int[] firstIndex, int[] secondIndex)
        {
            if (inputArray == null ||
                firstIndex == null ||
                secondIndex == null /*||
                /*firstIndex.Length != 1 || 
                 secondIndex.Length != 1*/)
            {
                throw new CRuntimeError("Memory violation");
            }

            int inputArrayLength = inputArray.Length;

            if (inputArrayLength == 0)
            {
                return mNullArray;
            }

            int firstIndexValue  = firstIndex.Length > 0 ? GetWrappedArrayIndex(firstIndex[0], inputArrayLength) : 0; 
            int secondIndexValue = secondIndex.Length > 0 ? GetWrappedArrayIndex(secondIndex[0], inputArrayLength) : 0;

            if (firstIndexValue < 0 || secondIndexValue < 0)
            {
                throw new CRuntimeError("An array index couldn't be negative value");
            }

            if (firstIndexValue >= inputArrayLength || secondIndexValue >= inputArrayLength)
            {
                throw new CRuntimeError("An index is out of range");
            }

            if (firstIndexValue > secondIndexValue)
            {
                int temp         = firstIndexValue;
                firstIndexValue  = secondIndexValue;
                secondIndexValue = temp;
            }

            int totalLength = secondIndexValue - firstIndexValue;

            int[] resultArray = new int[totalLength];

            Array.Copy(inputArray, firstIndexValue, resultArray, 0, totalLength);

            return resultArray;
        }

        public static int[] GetIndexOf(int[] inputArray, int[] element)
        {
            if (inputArray == null ||
                element == null/* ||
                element.Length != 1*/)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return new int[] { -1 };
            }

            return new int[] { Array.FindIndex(inputArray, t => t == element[0]) };
        }

        public static int[] GetElement(int[] inputArray, int[] element)
        {
            if (inputArray == null ||
                element == null /*||
                element.Length != 1 || 
                element[0] < 0*/)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;
            
            if (arrayLength == 0)
            {
                return mNullArray;
            }

            int wrappedIndex = element.Length > 0 ? GetWrappedArrayIndex(element[0], arrayLength) : 0;

            return new int[] { inputArray[wrappedIndex] };
        }
        
        public static int[] GetLength(int[] inputArray)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return new int[] { 0 };
            }

            return new int[] { inputArray.Length };
        }

        public static int[] Map(int[] inputArray, Func<int, int> functor)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            for (int i = 0; i < arrayLength; ++i)
            {
                inputArray[i] = functor(inputArray[i]);
            }

            return inputArray;
        }

        public static int[] Filter(int[] inputArray, Func<int, bool> functor)
        {
            if (inputArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return inputArray;
            }

            List<int> resultArray = new List<int>();

            int currValue = 0;

            for (int i = 0; i < arrayLength; ++i)
            {
                currValue = inputArray[i];
                
                if (functor(currValue))
                {
                    resultArray.Add(currValue);
                }
            }

            return resultArray.ToArray();
        }

        public static int[] VecOp(int[] firstArray, int[] secondArray, Func<int, int, int> functor)
        {
            if (firstArray == null ||
                secondArray == null)
            {
                throw new CRuntimeError("Memory violation");
            }

            int firstArrayLength = firstArray.Length;
            int secondArrayLength = secondArray.Length;

            if (firstArrayLength == 0 ||
                secondArrayLength == 0)
            {
                return mNullArray;
            }

            int resultArrayLength = Math.Min(firstArrayLength, secondArrayLength);

            int[] resultArray = new int[resultArrayLength];
            
            for (int i = 0; i < resultArrayLength; ++i)
            {
                resultArray[i] = functor(firstArray[i], secondArray[i]);
            }

            return resultArray;
        }

        /// <summary>
        /// The method wraps up the specified index and makes it stay
        /// within allowed range from 0 to +inf
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>

        public static int GetWrappedArrayIndex(int index, int arrayLength)
        {
            if (index >= 0)
            {
                return index % arrayLength;
            }

            return arrayLength + index % arrayLength;
        }
    }
}
