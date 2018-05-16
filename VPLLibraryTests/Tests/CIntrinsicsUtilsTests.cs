using NUnit.Framework;
using VPLLibrary.Impls;

namespace VPLLibraryTests.Tests
{
    [TestFixture]
    public class CIntrinsicsUtilsTests
    {
        [Test]
        public void TestGetHead_CorrectArrayWith4Elements_ReturnsFirstElement()
        {
            int[] testArray = new int[4] { 42, 23, -9, 12 };

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.GetHead(testArray); });

            Assert.AreEqual(resultArray.Length, 1);
            Assert.AreEqual(resultArray[0], testArray[0]);
        }

        [Test]
        public void TestGetHead_EmptyArray_ReturnsThisArray()
        {
            int[] testArray = new int[0];

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.GetHead(testArray); });

            Assert.AreSame(resultArray, testArray);
        }

        [Test]
        public void TestGetHead_PassNullArgument_ThrowsException()
        {            
            Assert.Throws<CRuntimeError>(() => { CIntrinsicsUtils.GetHead(null); });            
        }

        [Test]
        public void TestGetLast_CorrectArrayWith4Elements_ReturnsLastElement()
        {
            int[] testArray = new int[4] { 42, 23, -9, 12 };

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.GetLast(testArray); });

            Assert.AreEqual(resultArray.Length, 1);
            Assert.AreEqual(resultArray[0], testArray[testArray.Length - 1]);
        }

        [Test]
        public void TestGetLast_EmptyArray_ReturnsThisArray()
        {
            int[] testArray = new int[0];

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.GetLast(testArray); });

            Assert.AreSame(resultArray, testArray);
        }

        [Test]
        public void TestGetLast_PassNullArgument_ThrowsException()
        {
            Assert.Throws<CRuntimeError>(() => { CIntrinsicsUtils.GetLast(null); });
        }

        [Test]
        public void TestGetSumOfElements_PassCorrectArray_ReturnsSumOfElements()
        {
            int[] testArray = new int[] { 0, 1, 2, 3, 4 };

            int expectedSum = 0;

            foreach (int unit in testArray)
            {
                expectedSum += unit;
            }

            int[] sum = null;

            Assert.DoesNotThrow(() => { sum = CIntrinsicsUtils.GetSumOfElements(testArray); });

            Assert.AreEqual(sum.Length, 1);
            Assert.AreEqual(sum[0], expectedSum);
        }

        [Test]
        public void TestGetSumOfElements_PassEmptyArray_ReturnsZeroSum()
        {
            int[] testArray = CIntrinsicsUtils.mNullArray;
            
            int[] sum = null;

            Assert.DoesNotThrow(() => { sum = CIntrinsicsUtils.GetSumOfElements(testArray); });

            Assert.AreEqual(sum.Length, 1);
            Assert.AreEqual(sum[0], 0);
        }

        [Test]
        public void TestGetSumOfElements_PassNullArgument_ThrowsException()
        {
            Assert.Throws<CRuntimeError>(() => { CIntrinsicsUtils.GetSumOfElements(null); });
        }

        [Test]
        public void TestReverse_PassEmptyArray_ReturnsEmptyArray()
        {
            int[] testArray = CIntrinsicsUtils.mNullArray;

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.Reverse(testArray); });

            Assert.AreSame(resultArray, testArray);
        }

        [Test]
        public void TestReverse_PassSingleElementArray_ReturnsSingleElementArray()
        {
            int[] testArray = new int[] { 42 };

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.Reverse(testArray); });

            Assert.AreEqual(resultArray.Length, 1);
            Assert.AreEqual(testArray.Length, 1);
            Assert.AreEqual(resultArray[0], testArray[0]);
        }

        [Test]
        public void TestReverse_PassCorrectArray_ReturnsReversedArray()
        {
            int[] testArray = new int[] { 42, -9, 0, 34, 12, 9, 5, 5 };

            int[] resultArray = null;

            Assert.DoesNotThrow(() => { resultArray = CIntrinsicsUtils.Reverse(testArray); });

            for (int i = 0; i < testArray.Length; ++i)
            {
                Assert.AreEqual(testArray[testArray.Length - i - 1], resultArray[i]);
            }
        }

        [Test]
        public void TestReverse_PassNullArgument_ThrowsException()
        {
            Assert.Throws<CRuntimeError>(() => { CIntrinsicsUtils.Reverse(null); });
        }

        [Test]
        public void TestGetWrappedArrayIndex_PassNegativeValueGreatherThenArrayLength_ReturnsWrappedIndexValue()
        {
            int testArrayLength = 14;
            int testIndexValue = -30;

            int wrappedIndexValue = CIntrinsicsUtils.GetWrappedArrayIndex(testIndexValue, testArrayLength);

            Assert.IsTrue(wrappedIndexValue >= 0 && wrappedIndexValue < testArrayLength);
        }

        [Test]
        public void TestGetWrappedArrayIndex_PassValueGreatherThenArrayLength_ReturnsWrappedIndexValue()
        {
            int testArrayLength = 14;
            int testIndexValue = 15;

            int wrappedIndexValue = CIntrinsicsUtils.GetWrappedArrayIndex(testIndexValue, testArrayLength);

            Assert.IsTrue(wrappedIndexValue >= 0 && wrappedIndexValue < testArrayLength);
        }

        [Test]
        public void TestGetWrappedArrayIndex_PassNevativeValueAbsGreaterThenArrayLength_ReturnsCorrectWrappedIndex()
        {
            int testArrayLength = 1;
            int testIndexValue = -13;

            int wrappedIndexValue = CIntrinsicsUtils.GetWrappedArrayIndex(testIndexValue, testArrayLength);

            Assert.IsTrue(wrappedIndexValue >= 0 && wrappedIndexValue < testArrayLength);
        }

        [Test]
        public void TestGetIndexOf_PassEmptyArray_ReturnsElementThatEqualsToZero()
        {
            var testArray = new int[] { 4, -4, 3, 13, 0, 10, 12, 1 };

            int indexOfZero = System.Array.FindIndex(testArray, t => t == 0);

            Assert.DoesNotThrow(() =>
            {
                var result = CIntrinsicsUtils.GetIndexOf(testArray, CIntrinsicsUtils.mNullArray);

                Assert.AreEqual(indexOfZero, result[0]);
            });
        }
    }
}
