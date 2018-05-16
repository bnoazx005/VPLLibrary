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
        public void TestVisitLValueIdentifierNode_CorrectArgument_ReturnsName()
        {
            IVisitor<Object> interpreter = new CInterpreter();

            IIdentifierASTNode identifierNode = new CIdentifierASTNode("TestId", E_NODE_ATTRIBUTES.NA_LVALUE);

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
        [TestCase(E_OPERATION_TYPE.OT_MAX, -6, 2, 2)]
        [TestCase(E_OPERATION_TYPE.OT_MIN, -6, 2, -6)]
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

        [Test]
        public void TestReadInput_TryReadEmptyInputArray_ReturnsEmptyArray()
        {
            IInterpreter interpreter = new CInterpreter();

            int[] testValue = new int[] { 5 };

            /*program looks like
             * t <- [int]
            */
            var program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("t", new CReadInputASTNode(new CValueASTNode(new int[] { 0 })))
            });

            int[][] inputData = new int[1][];

            Assert.DoesNotThrow(() =>
            {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result, CIntrinsicsUtils.mNullArray);
            });
        }
        
        [Test]
        public void TestEval_SimpleProgramWithInputAndIntrinsics_ReturnsResult()
        {
            IInterpreter interpreter = new CInterpreter();
            
            /*program looks like
             * x <- [int]
             * y <- [int]
             * z <- VecOp (-) x y
            */
            var program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("x", new CReadInputASTNode(new CValueASTNode(new int[] { 0 }))),
                new CAssignmentASTNode("y", new CReadInputASTNode(new CValueASTNode(new int[] { 1 }))),
                new CAssignmentASTNode("z", new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_VECOP, 
                                                new List<IASTNode>()
                                                {
                                                    new CBinaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_SUB),
                                                    new CIdentifierASTNode("x"),
                                                    new CIdentifierASTNode("y")
                                                }))
            });

            int[][] inputData = new int[][]
            {
                new int[] { 4, 4, 4, 4 }, //x
                new int[] { 2, 2, 2, 2 }  //y
            };

            int[] expectedResult = new int[] { 2, 2, 2, 2 };

            Assert.DoesNotThrow(() =>
            {
                var result = interpreter.Eval(program, inputData);

                for (int i = 0; i < Math.Min(result.Length, expectedResult.Length); ++i)
                {
                    Assert.AreEqual(expectedResult[i], result[i]);
                }
            });
        }

        [Test]
        public void TestEval_TestReferencingByIdentifierInExpr_ReturnsValueStoredWithinVar()
        {
            IInterpreter interpreter = new CInterpreter();

            /*program looks like
             * x <- int
             * y <- IF x (> 0) THEN x ELSE NULL
            */
            var program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("x", new CReadInputASTNode(new CValueASTNode(new int[] { 0 }))),
                new CAssignmentASTNode("z", new CIfThenElseASTNode(new CIdentifierASTNode("x", E_NODE_ATTRIBUTES.NA_ID_SHOULD_EXIST),
                                                    new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_GT, new CValueASTNode(new int[] { 0 }), null),
                                                    new CIdentifierASTNode("x", E_NODE_ATTRIBUTES.NA_ID_SHOULD_EXIST),
                                                    new CValueASTNode(CIntrinsicsUtils.mNullArray)))
            });

            int[][] inputData = new int[][]
            {
                new int[] { 4 }, //x
            };

            int[] expectedResult = new int[] { 2, 2, 2, 2 };

            Assert.DoesNotThrow(() =>
            {
                var result = interpreter.Eval(program, inputData);

                Assert.AreEqual(result.Length, 1);
                Assert.AreEqual(inputData[0][0], result[0]);
            });
        }        
    }
}
