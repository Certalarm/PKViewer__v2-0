using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using WpfMvvm.ViewModels.MessageBox;
using static WpfMvvm.ViewModels.MainWindow.Win32Helper;

namespace WpfMvvm.ViewModels.MainWindow
{
    public static class UIHelper
    {
        internal static T RelativeParent<T>(DependencyObject children) where T : DependencyObject
        {
            if (IsNotNeedNext(children, out T typedParent))
                return typedParent;
            var parent = VisualTreeHelper.GetParent(children);
            while (parent is not null and not T)
                parent = VisualTreeHelper.GetParent(parent);
            return parent as T;
        }

        internal static T RelativeChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (IsNotNeedNext(parent, out T typedChildren))
                return typedChildren;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;
                if (RelativeChild<T>(child) is T descendant and not null)
                    return descendant;
            }
            return default;
        }

        internal static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                yield break;

            foreach (var result in FindInVisualTree<T>(parent))
                yield return result;

            // Handle ContentElement case
            if (parent is ContentElement contentElement)
                foreach (var result in FindInLogicalTree<T>(contentElement))
                    yield return result;
        }

        internal static void ShowMessage(MessageBoxVM vm)
        {
            Window messageBox = GetMessageBox();
            messageBox.Closing += vm.OnClosing;
            messageBox.DataContext = vm;
            messageBox.Owner ??= GetMainWindow();
            ChangeTheme(messageBox);
            CenterOnOwner(messageBox);
            messageBox.ShowDialog();
            messageBox.Closing -= vm.OnClosing;
            messageBox.DataContext = null;
        }

        private static void CenterOnOwner(Window child)
        {
            if (child == null || child.Owner == null)
                return;
            child.UpdateLayout();
            (var left, var top) = GetLeftAndTop(child.Owner);
            child.Left = left + (child.Owner.ActualWidth - child.ActualWidth) / 2;
            child.Top = top + (child.Owner.ActualHeight - child.ActualHeight) / 2;
        }

        private static bool IsMaximized(Window window) =>
            window.WindowState == WindowState.Maximized;

        private static (double left, double top) GetLeftAndTop(Window window)
        {
            var left = 0.0;
            var top = 0.0;
            if (!IsMaximized(window))
            {
                left = window.Left;
                top = window.Top;
            }
            return (left, top);
        }

        private static void ChangeTheme(Window window)
        {
            if (window.Visibility != Visibility.Hidden)
            {
                window.Show();
                window.Hide();
            }
            SwitchWindowSystemTheme(IsDarkTheme(), window);
        }

        private static bool IsNotNeedNext<T>(DependencyObject current, out T typedRelative) where T : DependencyObject
        {
            typedRelative = null;
            if (current == null)
                return true;
            if (current is T typedCurrent)
            {
                typedRelative = typedCurrent;
                return true;
            }
            return false;
        }

        private static IEnumerable<T> FindInVisualTree<T>(DependencyObject parent) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is not null and T)
                    yield return (T)child;
                foreach (T descendant in FindVisualChildren<T>(child))
                    yield return descendant;
            }
        }

        private static IEnumerable<T> FindInLogicalTree<T>(ContentElement parent) where T : DependencyObject
        {
            var logicalChildren = LogicalTreeHelper.GetChildren(parent);
            foreach (object logicalChild in logicalChildren)
            {
                var depObj = logicalChild as DependencyObject;
                if (depObj is not null and T)
                    yield return (T)depObj;
            }
        }
    }
}
