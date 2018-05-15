using NUnit.Framework;
using System.Collections.Generic;
using VPLLibrary.Impls;
using VPLLibrary.Interfaces;


namespace VPLLibraryTests.Tests
{
    [TestFixture]
    public class CASTPrinterTests
    {
        [Test]
        public void TestPrint_PassCorrectSimpleProgram_GenerateCorrectOutputString()
        {
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

            CASTPrinter printer = new CASTPrinter();

            Assert.DoesNotThrow(() => 
            {
                string result = printer.Print(program);

                Assert.IsFalse(result.Length < 1);
            });
        }

        [Test]
        public void TestPrint_PassValue_GenerateCorrectString()
        {
            var program = new CProgramASTNode(new List<IASTNode>()
            {
                new CAssignmentASTNode("x", new CValueASTNode(new int[] { 1 })),
            });

            CASTPrinter printer = new CASTPrinter();

            Assert.DoesNotThrow(() =>
            {
                string result = printer.Print(program);
            });
        }
    }
}
