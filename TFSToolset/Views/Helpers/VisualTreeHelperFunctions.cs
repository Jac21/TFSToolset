using System.Windows;
using System.Windows.Media;

namespace TFSToolset.UI.Views.Helpers
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
}