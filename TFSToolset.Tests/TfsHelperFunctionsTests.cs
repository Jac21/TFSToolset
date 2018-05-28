using System;
using System.Collections.Generic;
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

        private TfsHelperFunctions tfsHelperFunctions;
        private List<QueryFolder> testFoldersToDelete;

        [SetUp]
        public void Init()
        {
            tfsHelperFunctions =
                new TfsHelperFunctions("https://tfstoolset.visualstudio.com", "MyFirstProject");

            testFoldersToDelete = new List<QueryFolder>();
        }

        #endregion

        #region Tests

        [Test]
        public void ConnectivityTest()
        {
            // arrange

            // act

            // assert
            tfsHelperFunctions.TfsUrl.ShouldBe(null);
        }

        [Test]
        public void FolderAddTest()
        {
            // arrange
            Random random = new Random();
            int randomNumber = random.Next(0, 1000);

            string folder = "TestFolder" + randomNumber;

            // act
            testFoldersToDelete.Add(
                tfsHelperFunctions.AddNewFolder(folder));

            // assert
            //_tfsHelperFunctions.QueryHierarchy.Count.ShouldBeGreaterThan(0);
        }

        [Test]
        public void QueryTest()
        {
            // arrange
            string queryTitle = "TestTitle";
            string queryCommand =
                "SELECT [System.Title], [System.State] FROM WorkItems WHERE [System.AssignedTo] = @me";
            QueryFolder testQueryFolder = new QueryFolder("TestQueryFolder");

            // act
            tfsHelperFunctions.AddNewQuery(queryTitle, queryCommand, testQueryFolder);

            // assert
        }

        [Test]
        public void CopyTest()
        {
            // arrange
            QueryFolder testFolderOne = new QueryFolder("TestFolderOne");
            QueryFolder testFolderTwo = new QueryFolder("TestFolderTwo");

            string queryTitle = "TestTitle";
            string queryCommand =
                "SELECT [System.Title], [System.State] FROM WorkItems WHERE [System.AssignedTo] = @me";

            // act
            tfsHelperFunctions.AddNewQuery(queryTitle, queryCommand, testFolderOne);

            tfsHelperFunctions.CopyPreviousQueryFolderContent(testFolderOne, testFolderTwo);

            // assert
        }

        [Test]
        public void DetailsTest()
        {
            // arrange

            // act
            tfsHelperFunctions.GetStoreDetails();

            // assert
        }

        #endregion

        #region TearDown

        [TearDown]
        public void TestTearDown()
        {
            // Delete any test-made folders
            foreach (var folder in testFoldersToDelete)
            {
                folder.Delete();
            }
        }

        #endregion
    }
}