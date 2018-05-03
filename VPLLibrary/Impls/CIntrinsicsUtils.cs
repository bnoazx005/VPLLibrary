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
        IFT_VECOP
    }


    /// <summary>
    /// static class CIntrinsicsUtils
    /// 
    /// The class contains implementations of all built-int functions
    /// that the language provides. 
    /// </summary>

    public static class CIntrinsicsUtils
    {
        public delegate int UnaryLambdaFunction(int x);
        
        public delegate int BinaryLambdaFunction(int x, int y);

        public delegate bool LambdaPredicate(int x);


        public static int[] Eval(E_INTRINSIC_FUNC_TYPE type, params Object[] args)
        {
            throw new NotImplementedException();
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

        public static int[] Map(int[] inputArray, UnaryLambdaFunction functor)
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

        public static int[] Filter(int[] inputArray, LambdaPredicate functor)
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

        public static int[] VecOp(int[] firstArray, int[] secondArray, BinaryLambdaFunction functor)
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
                return new int[0];
            }

            int resultArrayLength = Math.Min(firstArrayLength, secondArrayLength);

            int[] resultArray = new int[resultArrayLength];
            
            for (int i = 0; i < resultArrayLength; ++i)
            {
                resultArray[i] = functor(firstArray[i], secondArray[i]);
            }

            return resultArray;
        }
    }
}
