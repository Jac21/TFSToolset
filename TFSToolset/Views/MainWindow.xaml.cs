using System;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TFSToolset.UI.Views.Helpers;

namespace TFSToolset.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //fields
        TfsHelperFunctions _tfsHelperFunctions;// = new TfsHelperFunctions("https://jac21.visualstudio.com/DefaultCollection/", "TestProject");
        ExcelHelperFunctions _excelHelperFunctions;
        //VisualTreeHelperExtensions visualTreeHelperExtensions = new VisualTreeHelperExtensions();

        //////////////////////////////////////////////////////////////////////////////
        /// GotFocus methods on Text Boxes to clear default text when clicked by user
        
        private void NewQueryFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            NewQueryFolderTextBox.Text = string.Empty;
            NewQueryFolderTextBox.GotFocus -= NewQueryFolderTextBox_OnGotFocus;
        }

        private void MoveQueryOldFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            MoveQueryOldFolderTextBox.Text = string.Empty;
            MoveQueryOldFolderTextBox.GotFocus -= MoveQueryOldFolderTextBox_OnGotFocus;
        }

        private void MoveQueryNewFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            MoveQueryNewFolderTextBox.Text = string.Empty;
            MoveQueryNewFolderTextBox.GotFocus -= MoveQueryNewFolderTextBox_OnGotFocus;
        }

        private void ReplaceFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ReplaceFolderTextBox.Text = string.Empty;
            ReplaceFolderTextBox.GotFocus -= ReplaceFolderTextBox_OnGotFocus;
        }

        private void OldTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            OldTextBox.Text = string.Empty;
            OldTextBox.GotFocus -= OldTextBox_OnGotFocus;
        }

        private void NewTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            NewTextBox.Text = string.Empty;
            NewTextBox.GotFocus -= NewTextBox_OnGotFocus;
        }

        private void TFSURLTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TFSURLTextBox.Text = string.Empty;
            TFSURLTextBox.GotFocus -= TFSURLTextBox_OnGotFocus;
        }

        private void ProjectNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ProjectNameTextBox.Text = string.Empty;
            ProjectNameTextBox.GotFocus -= ProjectNameTextBox_OnGotFocus;
        }

        //////////////////////////////////////////////////////////////////////////////
        /// Button click methods
        private void NewQueryAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Add folder
                _tfsHelperFunctions.AddNewFolder(NewQueryFolderTextBox.Text);

                this.ShowMessageAsync("Success", "New folder " + "\"" + NewQueryFolderTextBox.Text + "\"" + " added!");
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                {
                    this.ShowMessageAsync("Error", "Please connect to a TFS project before copying queries between folders");
                }
            }
        }

        private void MoveQueryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Search for folders
                QueryFolder myOldFolder = _tfsHelperFunctions.Search(MoveQueryOldFolderTextBox.Text);

                QueryFolder myNewFolder = _tfsHelperFunctions.Search(MoveQueryNewFolderTextBox.Text);

                //// Test query if needed
                //tfsHelperFunctions.AddNewQuery("Test Query",
                //    "SELECT [System.Title], [System.State] FROM WorkItems WHERE [System.AssignedTo] = @me " +
                //    "AND [System.WorkItemType] = 'Task'", myOldFolder);

                //Copy queries from previous folder
                _tfsHelperFunctions.CopyPreviousQueryFolderContent(myOldFolder, myNewFolder);

                //success message
                this.ShowMessageAsync("Success", "Queries moved from " + MoveQueryOldFolderTextBox.Text + " to " +
                                MoveQueryNewFolderTextBox.Text + "!");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    this.ShowMessageAsync("Error", "Cannot add queries that already exist");
                }
                else if (ex is NullReferenceException)
                {
                    this.ShowMessageAsync("Error", "Please connect to a TFS project" +
                                                   " and verify the folders exist before copying queries between them");
                }
            }
        }

        private void ReplaceTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Find specified folder
                var replaceTextFolder = _tfsHelperFunctions.Search(ReplaceFolderTextBox.Text);

                // iterate through folders' queries, replace specified text and entries
                foreach (var queryItem in replaceTextFolder)
                {
                    var queryDefinition = (QueryDefinition) queryItem;
                    if (queryDefinition.QueryText.Contains(OldTextBox.Text))
                    {
                        queryDefinition.QueryText = queryDefinition.QueryText.Replace(OldTextBox.Text, NewTextBox.Text);
                    }
                }

                _tfsHelperFunctions.SaveHierarchy();

                this.ShowMessageAsync("Success",
                    "\"" + OldTextBox.Text + "\"" + " replaced with \"" + NewTextBox.Text + "\""
                    + " in " + ReplaceFolderTextBox.Text);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    this.ShowMessageAsync("Error", "Please enter a valid string to replace");
                }
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //construct class with user input URL and project name
                _tfsHelperFunctions = new TfsHelperFunctions(TFSURLTextBox.Text, ProjectNameTextBox.Text);

                //success message
                string fullPath = TFSURLTextBox.Text + ProjectNameTextBox.Text;
                this.ShowMessageAsync("Success", "Connected to " + fullPath);
                ConnectButton.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                if (ex is UriFormatException)
                {
                    this.ShowMessageAsync("Error", "Please enter a valid TFS URL/Project combination");
                }
            }
        }

        private void StackPanel(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
