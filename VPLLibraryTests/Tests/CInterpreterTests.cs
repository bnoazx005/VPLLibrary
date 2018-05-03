using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void TestVisitIdentifierNode_NullArgument_ThrowsNullArgumentException()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Assert.Throws<ArgumentNullException>(() => { interpreter.VisitIdentifierNode(null); });
        }

        [Test]
        public void TestVisitIdentifierNode_CorrectArgument_ReturnsName()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            IIdentifierASTNode identifierNode = new CIdentifierASTNode("TestId");

            var result = interpreter.VisitIdentifierNode(identifierNode);

            Assert.AreEqual(result, identifierNode.Name);
        }

        [Test]
        public void TestVisitValueNode_NullArgument_ThrowsNullArgumentException()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Assert.Throws<ArgumentNullException>(() => { interpreter.VisitValueNode(null); });
        }

        [Test]
        public void TestVisitValueNode_CorrectArgument_ReturnsName()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            IValueASTNode valueNode = new CValueASTNode(new int[] { 1, 2 });

            var result = interpreter.VisitValueNode(valueNode);

            Assert.AreEqual(result, valueNode.Value);
        }

        [Test]
        public void TestVisitAssigmentNode_SingleAssigmentOfValue_ReturnsValue()
        {
            IInterpreter interpreter = new CInterpreter();

            int[][] inputData = new int[3][];

            int[] expectedValue = new int[] { 42 };

            // simple program that should assign 42 to a variable t
            IASTNode program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("t", new CValueASTNode(expectedValue))
            });

            Assert.DoesNotThrow(() => {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result, expectedValue);
            });
        }

        [Test]
        public void TestVisitAssigmentNode_NestedAssigmentOfValue_ReturnsValue()
        {
            IInterpreter interpreter = new CInterpreter();

            int[][] inputData = new int[3][];

            int[] expectedValue = new int[] { 42 };

            // simple program that should assign 42 to a variable t
            // now program looks like t <- v <- 42
            IASTNode program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("t",
                            new CAssignmentASTNode("v", new CValueASTNode(expectedValue)))
            });

            Assert.DoesNotThrow(() => {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result, expectedValue);
            });
        }

        [Test]
        public void TestVisitAssigmentNode_SequentialAssigmentOfValueViaReference_ReturnsValue()
        {
            IInterpreter interpreter = new CInterpreter();

            int[][] inputData = new int[3][];

            int[] expectedValue = new int[] { 42 };

            // simple program that should assign 42 to a variable t
            // now program looks like t <- 42 \n v <- t
            IASTNode program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("t", new CValueASTNode(expectedValue)),
                new CAssignmentASTNode("v", new CIdentifierASTNode("t"))
            });

            Assert.DoesNotThrow(() => {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result, expectedValue);
            });
        }
    }
}
