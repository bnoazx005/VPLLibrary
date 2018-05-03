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
    }
}
