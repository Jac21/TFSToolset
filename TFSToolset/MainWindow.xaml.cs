﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSToolset
{
    public class VisualTreeHelperExtensions
    {
        /// <summary>
        /// Helper function to programmatically find an ancestor in the visual tree
        /// Example usage: var grid = VisualTreeHelperExtensions.FindAncestor<Grid>(this);</Grid>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public T FindAncestor<T>(DependencyObject dependencyObject) where T : class
        {
            DependencyObject target = dependencyObject;
            do
            {
                target = VisualTreeHelper.GetParent(target);
            } while (target is T);

            return target as T;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //fields
        TfsHelperFunctions tfsHelperFunctions;// = new TfsHelperFunctions("https://jac21.visualstudio.com/DefaultCollection/", "TestProject");
        VisualTreeHelperExtensions visualTreeHelperExtensions = new VisualTreeHelperExtensions();

        //////////////////////////////////////////////////////////////////////////////
        /// GotFocus methods on Text Boxes to clear default text when clicked by user
        private void NewQueryFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            NewQueryFolderTextBox.Text = string.Empty;
        }

        private void OldTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            OldTextBox.Text = string.Empty;
        }

        private void NewTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            NewTextBox.Text = string.Empty;
        }

        private void MoveQueryOldFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            MoveQueryOldFolderTextBox.Text = string.Empty;
        }

        private void MoveQueryNewFolderTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            MoveQueryNewFolderTextBox.Text = string.Empty;
        }

        private void TFSURLTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TFSURLTextBox.Text = string.Empty;
        }

        private void ProjectNameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ProjectNameTextBox.Text = string.Empty;
        }

        //////////////////////////////////////////////////////////////////////////////
        /// Button click methods
        private void NewQueryAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Add folder
                tfsHelperFunctions.AddNewFolder(NewQueryFolderTextBox.Text);

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

        private void ReplaceTextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Replace necessary text after iterating throughout all queries
                foreach (QueryDefinition queryDefinition in tfsHelperFunctions.GetAllTeamQueries())
                {
                    if (queryDefinition.QueryText.Contains(OldTextBox.Text))
                    {
                        queryDefinition.QueryText = queryDefinition.QueryText.Replace(OldTextBox.Text, NewTextBox.Text);
                    }
                }

                tfsHelperFunctions.SaveHierarchy();

                this.ShowMessageAsync("Success",
                    "\"" + OldTextBox.Text + "\"" + " replaced with \"" + NewTextBox.Text + "\"");
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    this.ShowMessageAsync("Error", "Please enter a valid string to replace");
                }
            }
        }

        private void MoveQueryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Add folder
                QueryFolder myNewFolder = tfsHelperFunctions.AddNewFolder(MoveQueryOldFolderTextBox.Text);

                QueryFolder myOldFolder = tfsHelperFunctions.AddNewFolder(NewQueryFolderTextBox.Text);

                //// Test query if needed
                //tfsHelperFunctions.AddNewQuery("Test Query Sprint " + newSprintNumber,
                //    "SELECT [System.Title], [System.State] FROM WorkItems WHERE [System.AssignedTo] = @me " +
                //    "AND [System.WorkItemType] = 'Task'", myOldFolder);

                //Copy queries from previous folder
                tfsHelperFunctions.CopyPreviousQueryFolderContent(myOldFolder, myNewFolder);

                //success message
                this.ShowMessageAsync("Success", "Queries moved from " + MoveQueryOldFolderTextBox.Text + " to " +
                                MoveQueryNewFolderTextBox.Text + "!");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    this.ShowMessageAsync("Error", "Cannot add a folder that already exists");
                }
                else if (ex is NullReferenceException)
                {
                    this.ShowMessageAsync("Error", "Please connect to a TFS project before copying queries between folders");
                }
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //construct class with user input URL and project name
                tfsHelperFunctions = new TfsHelperFunctions(TFSURLTextBox.Text, ProjectNameTextBox.Text);

                //success message
                string fullPath = TFSURLTextBox.Text + ProjectNameTextBox.Text;
                this.ShowMessageAsync("Success", "Connected to " + fullPath);
            }
            catch (Exception ex)
            {
                if (ex is UriFormatException)
                {
                    this.ShowMessageAsync("Error", "Please enter a valid TFS URL/Project combination");
                }
            }
        }
    }
}
