using NUnit.Framework;
using Shouldly;
using TFSToolset.UI.Views.Helpers;

namespace TFSToolset.Tests
{
    [TestFixture]
    public class TfsHelperFunctionsTests
    {
        #region SetUp

        private TfsHelperFunctions _tfsHelperFunctions;

        [TestFixtureSetUp]
        public void Init()
        {
            _tfsHelperFunctions = new TfsHelperFunctions("https://jac21.visualstudio.com/DefaultCollection/", "TestProject");
        }

        [SetUp]
        public void TestInit()
        {
        }

        #endregion

        #region Tests

        [Test]
        public void ConnectivityTest()
        {
            _tfsHelperFunctions.TfsUrl.ShouldBe(null);
        }

        [Test]
        public void QueryTest()
        {
            
        }

        [Test]
        public void CopyTest()
        {
            
        }

        #endregion

        #region TearDown

        [TearDown]
        public void TestTearDown()
        {
            
        }

        #endregion
    }
}
