using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DarklandsUiCommon.Contracts;

namespace DarklandsUiCommon.ViewServices
{
    public static class ErrorMonitor
    {
        //private static int _erkrorCount;
        private static readonly ICollection<DependencyObject> ValidationObjects = new List<DependencyObject>();

        public static bool HasErrors
        {
            get { return ValidationObjects.Any(p => !IsValid(p)); }
        }

        private static bool IsValid(DependencyObject obj)
        {
            if (obj is DataGrid)
            {
                var grid = obj as DataGrid;

                if (grid.Tag == null || (int) grid.Tag == 0)
                {
                    return true;
                }

                return false;
            }

            return !Validation.GetHasError(obj)
                   && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }

        public static void Register(DependencyObject panel)
        {
            if (ValidationObjects.Contains(panel)) return;

            ValidationObjects.Add(panel);

            // LogicalTreeHelper.GetChildren does not return DataGrid's children
            if (panel is DataGrid)
            {
                Validation.AddErrorHandler(panel, OnValidationError);
            }
        }

        private static void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null || grid.Items.Count == 0)
            {
                return;
            }

            // Tag conveys number of invalid items
            grid.Tag = grid.Items.OfType<IValidableObject>().Count(i => !i.IsValid);
        }

        public static void ShutDown()
        {
            foreach (var o in ValidationObjects.OfType<DataGrid>())
            {
                Validation.RemoveErrorHandler(o, OnValidationError);
            }

            ValidationObjects.Clear();
        }
    }
}