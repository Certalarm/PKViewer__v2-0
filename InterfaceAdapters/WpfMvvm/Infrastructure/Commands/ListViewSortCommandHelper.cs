using System.Windows;
using System.Windows.Controls;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class ListViewSortCommandHelper
    {
        internal const int __startSortColumnIndex = 2;

        internal static readonly DataTemplate __sortedTemplate =
            Application.Current.Resources["SortedHeader"] as DataTemplate;
        internal static readonly DataTemplate __simpleTextTemplate =
            Application.Current.Resources["SimpleTextHeader"] as DataTemplate;
        internal static readonly DataTemplate __simpleToggleTemplate =
            Application.Current.Resources["SimpleToggleHeader"] as DataTemplate;

        internal static bool IsEqualsHeadersText(GridViewColumnHeader header1, GridViewColumnHeader header2) =>
            string.Equals(GetHeaderText(header1), GetHeaderText(header2), System.StringComparison.OrdinalIgnoreCase);

        internal static string GetHeaderText(GridViewColumnHeader header) =>
            header?.DataContext?.ToString() ?? string.Empty;

        internal static GridViewColumnHeader GetStartHeader(GridView gridView) =>
            GetStartColumn(gridView).Header as GridViewColumnHeader;

        internal static GridViewColumn GetStartColumn(GridView gridView) =>
            gridView?.Columns[__startSortColumnIndex];
    }
}
