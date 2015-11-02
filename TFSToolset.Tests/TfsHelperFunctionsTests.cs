using System;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
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

        //[SetUp]
        //public void TestInit()
        //{
        //}

        #endregion

        #region Tests

        [Test]
        public void ConnectivityTest()
        {
            // arrange

            // act

            // assert
            _tfsHelperFunctions.TfsUrl.ShouldBe(null);
        }

        [Test]
        public void FolderAddTest()
        {
            // arrange
            Random random = new Random();
            int randomNumber = random.Next(0, 100);

            string folder = "TestFolder" + randomNumber;

            // act
            _tfsHelperFunctions.AddNewFolder(folder);

            // assert
            //_tfsHelperFunctions.QueryHierarchy.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void QueryTest()
        {
            // arrange
            string queryTitle = "TestTitle";
            string queryCommand = "SELECT [System.Title], [System.State] FROM WorkItems WHERE [System.AssignedTo] = @me";
            QueryFolder testQueryFolder = new QueryFolder("TestQueryFolder");

            // act
            _tfsHelperFunctions.AddNewQuery(queryTitle, queryCommand, testQueryFolder);

            // assert

        }

        [Test]
        public void CopyTest()
        {
            // arrange

            // act

            // assert
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
