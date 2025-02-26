using System.Windows;
using System.Windows.Controls;
using static WpfMvvm.Infrastructure.Commands.ListViewSortCommandHelper;

namespace WpfMvvm.Infrastructure.Selectors
{
    public class ColumnHeaderTemplateSelector : DataTemplateSelector
    {
        private GridView _gridView;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var _curHeader = RelativeParent<GridViewColumnHeader>(container);
            _gridView ??= RelativeParent<ListView>(container)?.View as GridView;
            return _gridView == null
                ? base.SelectTemplate(item, container)
                : GetHeaderTemplate(_curHeader);
        }

        private DataTemplate GetHeaderTemplate(GridViewColumnHeader curHeader) =>
            curHeader.Tag == null 
                ? __simpleToggleTemplate
                : DetectTemplateByHeaderText(curHeader);

        private DataTemplate DetectTemplateByHeaderText(GridViewColumnHeader curHeader) =>
            IsEqualsHeadersText(GetStartHeader(_gridView), curHeader)
                ? __sortedTemplate
                : __simpleTextTemplate;
    }
}
