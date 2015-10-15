using System;
using NUnit.Framework;
using Shouldly;
using TFSToolset.UI.Views.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TFSToolset.Tests
{
    [TestFixture]
    public class TfsHelperFunctionsTests
    {
        #region SetUp

        private TfsHelperFunctions _tfsHelperFunctions = new TfsHelperFunctions("https://jac21.visualstudio.com/DefaultCollection/", "TestProject");

        [TestFixtureSetUp]
        public void Init()
        {
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
            _tfsHelperFunctions.TfsUrl.ShouldBe("https://jac21.visualstudio.com/DefaultCollection/");
        }

        #endregion
     
        [TearDown]
        public void TestTearDown()
        {
            
        }
    }
}
