using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace FindXamlReferences
{
    public static class ScrollViewerHelper
    {

        public static bool GetAutoScroll(ScrollViewer obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        internal static void SetAutoScroll(ScrollViewer obj, bool value)
        {
            obj.SetValue(AutoScrollPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AutoScrollPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("AutoScroll", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false));

        public static readonly DependencyProperty AutoScrollProperty = AutoScrollPropertyKey.DependencyProperty;

        public static bool GetAutoScrollToButtom(ScrollViewer obj)
        {
            return (bool)obj.GetValue(AutoScrollToButtomProperty);
        }

        public static void SetAutoScrollToButtom(ScrollViewer obj, bool value)
        {
            obj.SetValue(AutoScrollToButtomProperty, value);
        }

        public static readonly DependencyProperty AutoScrollToButtomProperty =
            DependencyProperty.RegisterAttached("AutoScrollToButtom", typeof(bool), typeof(ScrollViewerHelper), new PropertyMetadata(false, AutoScrollToButtomPropertyChanged));

        private static void AutoScrollToButtomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;

            if (scrollViewer != null && e.NewValue is bool autoscrollToButtom )
            {
                if (autoscrollToButtom)
                {
                    scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
                    scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                }
                else
                {
                    scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
                }
                SetAutoScroll(scrollViewer, true);
            }
        }

        private static void ScrollViewer_ScrollChanged(Object sender, ScrollChangedEventArgs e)
        {//source: https://stackoverflow.com/questions/2984803/how-to-automatically-scroll-scrollviewer-only-if-the-user-did-not-change-scrol
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null)
                return;

            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    SetAutoScroll(scrollViewer,true);
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    SetAutoScroll(scrollViewer,false);
                }
            }

            // Content scroll event : auto-scroll eventually
            if (GetAutoScroll(scrollViewer) && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }
    }
}
