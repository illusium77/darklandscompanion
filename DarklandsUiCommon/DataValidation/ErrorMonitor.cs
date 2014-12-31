using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DarklandsUiCommon.DataValidation
{
    public static class ErrorMonitor
    {
        private static ICollection<Panel> s_registeredPanels = new List<Panel>();

        public static int ErrorCount { get; set; }
        public static bool HasErrors { get { return ErrorCount != 0; } }

        public static void Register(Panel panel)
        {
            if (!s_registeredPanels.Contains(panel))
            {
                s_registeredPanels.Add(panel);

                // subscribe only for textbox validaiton errors 
                foreach (var tb in panel.Children.OfType<TextBox>())
                {
                    Validation.AddErrorHandler(tb, OnValidationError);

                    // check if we already have error
                    var prop = tb.GetBindingExpression(TextBox.TextProperty);
                    if (prop.HasError)
                    {
                        ErrorCount++;
                    }
                }
            }
        }

        public static void ShutDown()
        {
            foreach (var panel in s_registeredPanels)
            {
                foreach (var tb in panel.Children.OfType<TextBox>())
                {
                    Validation.RemoveErrorHandler(tb, OnValidationError);
                }
            }

            s_registeredPanels.Clear();
        }

        private static void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) ErrorCount += 1;
            if (e.Action == ValidationErrorEventAction.Removed) ErrorCount -= 1;
        }

    }
}
