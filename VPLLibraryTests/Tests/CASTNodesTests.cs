using NUnit.Framework;
using System.Collections.Generic;
using VPLLibrary.Impls;
using VPLLibrary.Interfaces;


namespace VPLLibraryTests.Tests
{
    [TestFixture]
    public class CASTNodesTests
    {
        [Test]
        public void TestEquals_CheckUpEmptyAndSameTrees_ShouldReturnsTrue()
        {
            IASTNode firstSubtree = new CProgramASTNode();

            IASTNode secondSubtree = new CProgramASTNode();

            Assert.IsTrue(firstSubtree.Equals(secondSubtree));
        }

        [Test]
        public void TestEquals_CheckUpEmptyTreesWithDiffrentChildren_ShouldReturnsFalse()
        {
            IASTNode firstSubtree = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("x", new CReadInputASTNode(new CValueASTNode(new int[] { 0 })))
            });

            IASTNode secondSubtree = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("y", new CReadInputASTNode(new CValueASTNode(new int[] { 0 })))
            });

            Assert.IsFalse(firstSubtree.Equals(secondSubtree));
        }

        [Test]
        public void TestEquals_CheckUpDifferentValueNodes_ReturnsFalse()
        {
            IASTNode firstValueNode  = new CValueASTNode(new int[] { 0, 1, 2 });
            IASTNode secondValueNode = new CValueASTNode(new int[] { 2, 1, 0 });

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpSameValueNodes_ReturnsTrue()
        {
            IASTNode firstValueNode = new CValueASTNode(new int[] { 0, 1, 2 });
            IASTNode secondValueNode = new CValueASTNode(new int[] { 0, 1, 2 });

            Assert.IsTrue(firstValueNode.Equals(secondValueNode));
            Assert.IsTrue(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpSameReadInputNodes_ReturnsTrue()
        {
            IASTNode firstValueNode = new CReadInputASTNode(new CValueASTNode(new int[] { 0 }));
            IASTNode secondValueNode = new CReadInputASTNode(new CValueASTNode(new int[] { 0 }));

            Assert.IsTrue(firstValueNode.Equals(secondValueNode));
            Assert.IsTrue(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpDifferentReadInputNodes_ReturnsFalse()
        {
            IASTNode firstValueNode = new CReadInputASTNode(new CValueASTNode(new int[] { 1 }));
            IASTNode secondValueNode = new CReadInputASTNode(new CValueASTNode(new int[] { 0 }));

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpSameLambdaPredicatesNodes_ReturnsTrue()
        {
            IASTNode firstValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 0 }), null);
            IASTNode secondValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 0 }), null);

            Assert.IsTrue(firstValueNode.Equals(secondValueNode));
            Assert.IsTrue(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpDifferentLambdaPredicatesNodes_ReturnsFalse()
        {
            IASTNode firstValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 0 }), null);
            IASTNode secondValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_NEQ, new CValueASTNode(new int[] { 0 }), null);

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));

            firstValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 0 }), null);
            secondValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 1 }), null);

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));

            firstValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 0 }), new CValueASTNode(new int[] { 1 }));
            secondValueNode = new CLambdaPredicateASTNode(E_LOGIC_OP_TYPE.LOT_EQ, new CValueASTNode(new int[] { 0 }), null);

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpSameBinaryLambdaFuncNodes_ReturnsTrue()
        {
            IASTNode firstValueNode = new CBinaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD);
            IASTNode secondValueNode = new CBinaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD);

            Assert.IsTrue(firstValueNode.Equals(secondValueNode));
            Assert.IsTrue(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpDifferentBinaryLambdaFuncNodes_ReturnsFalse()
        {
            IASTNode firstValueNode = new CBinaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV);
            IASTNode secondValueNode = new CBinaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD);

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpSameUnaryLambdaFuncNodes_ReturnsTrue()
        {
            IASTNode firstValueNode = new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV, 
                                                new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD, new CValueASTNode(new int[] { 0 })));

            IASTNode secondValueNode = new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV, 
                                                new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD, new CValueASTNode(new int[] { 0 })));

            Assert.IsTrue(firstValueNode.Equals(secondValueNode));
            Assert.IsTrue(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpDifferentUnaryLambdaFuncNodes_ReturnsFalse()
        {
            IASTNode firstValueNode = new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV,
                                                new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD, new CValueASTNode(new int[] { 2 })));

            IASTNode secondValueNode = new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV,
                                                new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD, new CValueASTNode(new int[] { 0 })));

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));

            firstValueNode = new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV,
                                                    new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_MAX, new CValueASTNode(new int[] { 0 })));

            secondValueNode = new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_DIV,
                                                    new CUnaryLambdaFuncASTNode(E_OPERATION_TYPE.OT_ADD, new CValueASTNode(new int[] { 0 })));

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpSameFuncCallNodes_ReturnsTrue()
        {
            IASTNode firstValueNode = new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_CONCAT, new List<IASTNode>());
            IASTNode secondValueNode = new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_CONCAT, new List<IASTNode>());

            Assert.IsTrue(firstValueNode.Equals(secondValueNode));
            Assert.IsTrue(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestEquals_CheckUpDifferentFuncCallNodes_ReturnsFalse()
        {
            IASTNode firstValueNode = new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_CONCAT, new List<IASTNode>());
            IASTNode secondValueNode = new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_FILTER, new List<IASTNode>());

            Assert.IsFalse(firstValueNode.Equals(secondValueNode));
            Assert.IsFalse(secondValueNode.Equals(firstValueNode));
        }

        [Test]
        public void TestGetDepth_ComputeDepthOfTree_ReturnsTreeDepth()
        {
            IASTNode program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("x", new CReadInputASTNode(new CValueASTNode(new int[] {2 }))),
                new CAssignmentASTNode("y", new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_GET,
                                                    new List<IASTNode>()
                                                    {
                                                        new CReadInputASTNode(new CValueASTNode(new int[] {1 })),
                                                        new CCallASTNode(E_INTRINSIC_FUNC_TYPE.IFT_LEN,
                                                                new List<IASTNode>()
                                                                {
                                                                    new CValueASTNode(new int[] {0,2 })
                                                                })
                                                    }))
            });

            int expectedDepth = 4;

            Assert.AreEqual(expectedDepth, program.Depth);
        }
    }
}
