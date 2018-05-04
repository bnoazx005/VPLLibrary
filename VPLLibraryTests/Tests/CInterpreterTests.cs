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

        [Test]
        public void TestVisitCallNode_SimpleProgram_ReturnsValue()
        {
            IInterpreter interpreter = new CInterpreter();

            int[][] inputData = new int[3][];

            int[] testArray = new int[] { 5, 4, -3, 1 };

            int sum = 0;

            foreach (int unit in testArray)
            {
                sum += unit;
            }

            int[] expectedValue = new int[] { testArray[0], sum };

            // simple program that returns a sum of length and a first element of an array
            // program's code looks like this 
            // a <- [5, 4, -3, 1]
            // t <- head a
            // s <- sum a
            // r <- concat t s
            IASTNode program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("a", new CValueASTNode(testArray)),
                new CAssignmentASTNode("t", new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_HEAD, new List<IASTNode>() { new CIdentifierASTNode("a") })),
                new CAssignmentASTNode("s", new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_SUM, new List<IASTNode>() { new CIdentifierASTNode("a") })),
                new CAssignmentASTNode("r", new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_CONCAT, new List<IASTNode>() { new CIdentifierASTNode("t"), new CIdentifierASTNode("s") }))
            });

            Assert.DoesNotThrow(() => {
                var result = interpreter.Eval(program, inputData);

                for (int i = 0; i < result.Length; ++i)
                {
                    Assert.AreEqual(expectedValue[i], result[i]);
                }
            });
        }

        [TestCase(E_OPERATION_TYPE.OT_ADD, 2, 2, 4)]
        [TestCase(E_OPERATION_TYPE.OT_SUB, 2, 2, 0)]
        [TestCase(E_OPERATION_TYPE.OT_MUL, 25, 2, 50)]
        [TestCase(E_OPERATION_TYPE.OT_DIV, 50, 2, 25)]
        [TestCase(E_OPERATION_TYPE.OT_MOD, 25, 2, 1)]
        //[TestCase(E_OPERATION_TYPE.OT_POW, 3, 2, 9)] works only for double, need to find some trick to implement it
        public void TestVisitBinaryLambdaFuncNode_CheckLambdaFunctorGeneration_ReturnsLambdaFunctor(E_OPERATION_TYPE type, int x, int y, int res)
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Func<int, int, int> lambdaFunctor = null;

            Assert.DoesNotThrow(() => {
                lambdaFunctor = (Func<int, int, int>)interpreter.VisitBinaryLambdaFuncNode(new CBinaryLambdaFuncASTNode(type));                
            });

            Assert.AreEqual(res, lambdaFunctor(x, y));
        }

        [TestCase(E_LOGIC_OP_TYPE.LOT_EQ, 2, 2, true)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_EQ, 2, 4, false)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_NEQ, 2, 2, false)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_NEQ, 2, 4, true)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_LE, 2, 2, true)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_LE, 4, 2, false)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_LT, 2, 4, true)]
        [TestCase(E_LOGIC_OP_TYPE.LOT_LT, 4, 2, false)]
        public void TestVisitLambdaPredicateNode_CheckLambdaPredicateGeneration_ReturnsLambdaPredicate(E_LOGIC_OP_TYPE type, int x, int value, bool res)
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Func<int, bool> lambdaPredicate = null;

            Assert.DoesNotThrow(() => {
                lambdaPredicate = (Func<int, bool>)interpreter.VisitLambdaPredicateNode(
                                                new CLambdaPredicateASTNode(type, new CValueASTNode(new int[] { value }), null));
            });

            Assert.AreEqual(res, lambdaPredicate(x));
        }

        [TestCase(4, 2, 1, false)]
        [TestCase(7, 2, 0, false)]
        [TestCase(7, 2, 1, true)]
        public void TestVisitLambdaPredicateNode_CheckModuloComparisonLambdaPredicate_ReturnsLambdaPredicate(int x, int value, int secondOp, bool res)
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Func<int, bool> lambdaPredicate = null;

            Assert.DoesNotThrow(() => {
                lambdaPredicate = (Func<int, bool>)interpreter.VisitLambdaPredicateNode(
                                                new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_MOD, new CValueASTNode(new int[] { value }), 
                                                            new CValueASTNode(new int[] { secondOp })));
            });

            Assert.AreEqual(res, lambdaPredicate(x));
        }

        [TestCase(E_OPERATION_TYPE.OT_ADD, 2, 4, 6)]
        [TestCase(E_OPERATION_TYPE.OT_MUL, 2, 4, 8)]
        public void TestVisitUnaryLambdaFuncNode_CheckSimpleLambdaPredicateGeneration_ReturnsLambdaPredicate(E_OPERATION_TYPE type, int x, int value, int res)
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Func<int, int> lambdaFunctor = null;

            Assert.DoesNotThrow(() => {
                lambdaFunctor = (Func<int, int>)interpreter.VisitUnaryLambdaFuncNode(
                                                new CUnaryLambdaFuncASTNode(type, new CValueASTNode(new int[] { value })));
            });

            Assert.AreEqual(res, lambdaFunctor(x));
        }

        [Test]
        public void TestVisitUnaryLambdaFuncNode_CheckNestedLambdaPredicateGeneration_ReturnsLambdaPredicate()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            Func<int, int> lambdaFunctor = null;

            Func<int, int> expectedLambdaFunctor = (int x) => { return x + x * (x - 2); };

            int inputParameter = 5;

            Assert.DoesNotThrow(() => {
                // the interpreter should infer x + x * (x - 2) expression
                lambdaFunctor = (Func<int, int>)interpreter.VisitUnaryLambdaFuncNode(
                                                new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD,
                                                        new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_MUL,
                                                                new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_SUB, new CValueASTNode(new int[] { 2 })))));
            });

            Assert.AreEqual(expectedLambdaFunctor(inputParameter), lambdaFunctor(inputParameter));
        }

        [Test]
        public void TestIfStatement_TrueCondition_ReturnsEvaluatedExpressionOfChosenBranch()
        {
            IInterpreter interpreter = new CInterpreter();

            int[] testValue = new int[] { 5 };

            /*program looks like
             * t <- 5
             * r <- if t (== 5) then 5 else -1
            */
            var program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("t", new CValueASTNode(testValue)),
                new CAssignmentASTNode("r", new CIfThenElseASTNode(new CIdentifierASTNode("t"), 
                                new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(testValue), null),
                                new CValueASTNode(testValue),
                                new CValueASTNode(new int[] {-1 })))
            });

            int[][] inputData = new int[3][];


            Assert.DoesNotThrow(() =>
            {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result.Length, 1);
                Assert.AreEqual(result[0], testValue[0]);
            });
        }

        [Test]
        public void TestIfStatement_FalseCondition_ReturnsEvaluatedExpressionOfChosenBranch()
        {
            IInterpreter interpreter = new CInterpreter();

            int[] testValue = new int[] { 5 };

            /*program looks like
             * t <- 5
             * r <- if t (!= 5) then 5 else -1
            */
            var program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("t", new CValueASTNode(testValue)),
                new CAssignmentASTNode("r", new CIfThenElseASTNode(new CIdentifierASTNode("t"),
                                                new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_NEQ, new CValueASTNode(testValue), null),
                                                new CValueASTNode(testValue),
                                                new CValueASTNode(new int[] {-1 })))
            });

            int[][] inputData = new int[3][];


            Assert.DoesNotThrow(() =>
            {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result.Length, 1);
                Assert.AreEqual(result[0], -1);
            });
        }
    }
}
