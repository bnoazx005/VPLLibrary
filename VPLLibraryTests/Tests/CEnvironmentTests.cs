using NUnit.Framework;
using VPLLibrary.Impls;
using VPLLibrary.Interfaces;


namespace VPLLibraryTests.Tests
{
    [TestFixture]
    public class CEnvironmentTests
    {
        [Test]
        public void TestDefine_TryDefineAlreadyDefinedVariable_ThrowsException()
        {
            IEnvironment env = new CEnvironment();

            string id = "Test";

            int[] value = new[] { 0, 1 };

            Assert.DoesNotThrow(() => { env.Define(id, value); });

            Assert.Throws<CRuntimeError>(() => { env.Define(id, value); });
        }

        [Test]
        public void TestAssign_TryAssignUndeclaredVariable_NewVariableWillBeCreated()
        {
            IEnvironment env = new CEnvironment();

            string id = "Test";

            int[] value = new[] { 0, 1 };

            Assert.DoesNotThrow(() => { env.Assign(id, value); });

            Assert.IsTrue(env.Exists(id));
        }

        [Test]
        public void TestGet_TryGetValueOfUndeclaredVariable_ThrowsException()
        {
            IEnvironment env = new CEnvironment();

            string id = "Test";
            
            Assert.Throws<CRuntimeError>(() => { env.Get(id); });
        }
    }
}
