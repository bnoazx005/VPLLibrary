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

            IASTNode program = new CProgramASTNode();

            int[][] input = new int[1][];

            Assert.Throws<ArgumentNullException>(() => { interpreter.Eval(program, null); });
            Assert.Throws<ArgumentNullException>(() => { interpreter.Eval(null, input); });
        }
        
        [Test]
        public void TestEval_EmptyProgram_ReturnsEmptyArray()
        {
            IInterpreter interpreter = new CInterpreter();

            IASTNode program = new CProgramASTNode();

            int[][] input = new int[1][];

            var result = interpreter.Eval(program, input);
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int[]>(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void VisitIdentifierNode_NullArgument_ThrowsNullArgumentException()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Assert.Throws<ArgumentNullException>(() => { interpreter.VisitIdentifierNode(null); });
        }

        [Test]
        public void VisitIdentifierNode_CorrectArgument_ReturnsName()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            IIdentifierASTNode identifierNode = new CIdentifierASTNode("TestId");

            var result = interpreter.VisitIdentifierNode(identifierNode);

            Assert.AreEqual(result, identifierNode.Name);
        }

        [Test]
        public void VisitValueNode_NullArgument_ThrowsNullArgumentException()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Assert.Throws<ArgumentNullException>(() => { interpreter.VisitValueNode(null); });
        }

        [Test]
        public void VisitValueNode_CorrectArgument_ReturnsName()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            IValueASTNode valueNode = new CValueASTNode(new int[] { 1, 2 });

            var result = interpreter.VisitValueNode(valueNode);

            Assert.AreEqual(result, valueNode.Value);
        }
    }
}
