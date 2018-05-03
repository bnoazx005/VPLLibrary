using NUnit.Framework;
using System;
using VPLLibrary.Impls;
using VPLLibrary.Interfaces;


namespace VPLLibraryTests.Tests
{
    [TestFixture]
    public class CInterpreterTests
    {
        [Test]
        public void TestEval_NullArguments_ThrowsNullArgumentException()
        {
            IInterpreter interpreter = new CInterpreter();

            IASTNode program = new CProgramASTNode(null, null);

            int[][] input = new int[1][];

            Assert.Throws<ArgumentNullException>(() => { interpreter.Eval(program, null); });
            Assert.Throws<ArgumentNullException>(() => { interpreter.Eval(null, input); });
        }
        
        [Test]
        public void TestEval_EmptyProgram_ReturnsEmptyArray()
        {
            IInterpreter interpreter = new CInterpreter();

            IASTNode program = new CProgramASTNode(null, null);

            int[][] input = new int[1][];

            var result = interpreter.Eval(program, input);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int[]>(result);
            Assert.IsEmpty(result);
        }
    }
}
