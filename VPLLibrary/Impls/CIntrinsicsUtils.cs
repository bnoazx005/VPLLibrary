using System;

namespace VPLLibrary.Impls
{
    /// <summary>
    /// static class CIntrinsicsUtils
    /// 
    /// The class contains implementations of all built-int functions
    /// that the language provides. 
    /// </summary>

    public static class CIntrinsicsUtils
    {
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

            Array.Reverse(inputArray);

            return inputArray;
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

            Array.Sort(inputArray);

            return inputArray;
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

        public static int[] GetIndexOf(int[] inputArray, int[] element)
        {
            if (inputArray == null ||
                element == null ||
                element.Length != 1)
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
                element == null ||
                element.Length != 1 || 
                element[0] < 0)
            {
                throw new CRuntimeError("Memory violation");
            }

            int arrayLength = inputArray.Length;

            if (arrayLength == 0)
            {
                return new int[0];
            }

            return new int[] { inputArray[element[0]] };
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
    }
}
