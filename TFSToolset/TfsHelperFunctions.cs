using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSToolset
{
    class TfsHelperFunctions
    {
        //class fields
        private TfsTeamProjectCollection _coll;
        private WorkItemStore _store;
        public static Project _myProject;
        public QueryHierarchy _queryHierarchy;

        public string _tfsUrl;
        public string _projectName;

        /// <summary>
        /// Constructor that takes in the projects URL, grabs the work item store where
        /// the queries are found, as well as the specific project name
        /// </summary>
        /// <param name="tfsUrl"></param>
        /// <param name="projectName"></param>
        public TfsHelperFunctions(string tfsUrl, string projectName)
        {
            _coll = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(
                new Uri(tfsUrl)); //"http://dev2010:8080/tfs/TeamProjectCollection/"

            _store = new WorkItemStore(_coll);

            _myProject = _store.Projects[projectName]; //"TestProject"

            _queryHierarchy = _store.Projects[projectName].QueryHierarchy;
        }

        /// <summary>
        /// Create a new folder under My Queries
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public QueryFolder AddNewFolder(string folderName)
        {
            QueryFolder folder = new QueryFolder(folderName, GetMyQueriesFolder());
            _myProject.QueryHierarchy.Save();
            return folder;
        }

        /// <summary>
        /// Adds a query in "My Queries"
        /// </summary>
        /// <param name="queryTitle"></param>
        /// <param name="queryCommand"></param>
        /// <param name="parentFolder"></param>
        /// <returns></returns>
        public QueryDefinition AddNewQuery(string queryTitle, string queryCommand, QueryFolder parentFolder)
        {
            QueryDefinition query = new QueryDefinition(queryTitle, queryCommand, parentFolder);
            _myProject.QueryHierarchy.Save();
            return query;
        }

        /// <summary>
        /// Renames a query folder or query
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="folderQrQuery"></param>
        public void RenameQueryItem(string newName, QueryItem folderQrQuery)
        {
            folderQrQuery.Name = newName;
            _myProject.QueryHierarchy.Save();
        }

        /// <summary>
        /// Moves a query folder or query to another targeted folder
        /// </summary>
        /// <param name="queryItem"></param>
        /// <param name="targetFolder"></param>
        public void MoveQueryItem(QueryItem queryItem, QueryFolder targetFolder)
        {
            targetFolder.Add(queryItem);
            targetFolder.Project.QueryHierarchy.Save();
        }

        /// <summary>
        /// Returns the My Queries folder
        /// </summary>
        /// <returns></returns>
        public QueryFolder GetMyQueriesFolder()
        {
            foreach (var queryItem in _myProject.QueryHierarchy)
            {
                var folder = (QueryFolder)queryItem;
                if (folder.IsPersonal)
                    return folder;
            }

            throw new Exception("Cannot find the \"My Queries\" folder!");
        }

        /// <summary>
        /// Returns a QueryFolder object of the name given in the 
        /// "folderName" argument
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public QueryFolder Search(string folderName)
        {
            return (from QueryFolder folder in _queryHierarchy select Search(folder, folderName)).FirstOrDefault(result => result != null);
        }

        /// <summary>
        /// Helper function for Search, used to iterate through each 
        /// query folder and return the exact match to the folderName
        /// argument
        /// </summary>
        /// <param name="hierarchyFolder"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        private static QueryFolder Search(QueryFolder hierarchyFolder, string folderName)
        {
            foreach (QueryFolder folder in hierarchyFolder.OfType<QueryFolder>())
            {
                if (folderName.Equals(folder.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return folder;
                }

                var result = Search(folder, folderName);

                if (result == null) continue;
                return result;
            }

            return null;
        }

        /// <summary>
        /// Copies all queries from specified folder, adds them to specified folder without 
        /// affecting the former folder's queries
        /// </summary>
        /// <param name="oldFolder"></param>
        /// <param name="newFolder"></param>
        public void CopyPreviousQueryFolderContent(QueryFolder oldFolder, QueryFolder newFolder)
        {
            //List of all the old folder's queries
            List<QueryDefinition> oldQueryList = new List<QueryDefinition>();
            oldQueryList.AddRange(GetAllTeamQueries(oldFolder));

            //List for the new folder's queries, copy of the previous list
            List<QueryDefinition> newQueryList = new List<QueryDefinition>();
            newQueryList.AddRange(oldQueryList);

            //iterates through each query in copied list, adds newly constructed queries in new folder
            foreach (var queryItem in newQueryList)
            {
                QueryDefinition queryDefinition = new QueryDefinition(queryItem.Name, queryItem.QueryText);
                newFolder.Add(queryDefinition);
            }

            newFolder.Project.QueryHierarchy.Save();
        }


        /// <summary>
        /// Saves overall hierarchy after additional modifications have been made,
        /// keeps class field static instead of transplanting to Main()
        /// </summary>
        public void SaveHierarchy()
        {
            _myProject.QueryHierarchy.Save();
        }

        /// <summary>
        /// Grabs the entirety of the queries located in the specified folder
        /// </summary>
        /// <returns></returns>
        public List<QueryDefinition> GetAllTeamQueries()
        {
            List<QueryDefinition> queryList = new List<QueryDefinition>();

            var startQueryFolder = GetQueryFolder(_myProject.QueryHierarchy,
                                                  _projectName == null ? (new string[] { }) : _projectName.Split('\\'));

            queryList.AddRange(GetAllTeamQueries(startQueryFolder));

            return queryList;
        }

        /// <summary>
        /// Helper function, grabs queries from specified folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static List<QueryDefinition> GetAllTeamQueries(QueryFolder folder)
        {
            List<QueryDefinition> queryList = new List<QueryDefinition>();

            foreach (QueryItem queryItem in folder)
            {
                if (queryItem is QueryFolder)
                {
                    queryList.AddRange(GetAllTeamQueries(queryItem as QueryFolder));
                }
                else
                {
                    queryList.Add(queryItem as QueryDefinition);
                }
            }

            return queryList;
        }

        /// <summary>
        /// Helper function, grabs specified folder
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="folders"></param>
        /// <returns></returns>
        private static QueryFolder GetQueryFolder(QueryFolder folder, string[] folders)
        {
            return folders.Length == 0
                ? folder
                : GetQueryFolder((QueryFolder)folder[folders[0]], folders.Skip(1).ToArray());
        }

    }
}
