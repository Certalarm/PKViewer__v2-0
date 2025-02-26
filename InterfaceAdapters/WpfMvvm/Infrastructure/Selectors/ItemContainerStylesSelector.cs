using System.Windows;
using System.Windows.Controls;

namespace WpfMvvm.Infrastructure.Selectors
{
    public class ItemContainerStylesSelector: StyleSelector
    {
        // Стиль Нечётных строк, задается в Xaml
        public Style EvenItemsStyle { get; set; }
        // Стиль Чётных строк, задается в Xaml
        public Style OddItemsStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var itemsControl = ListView.ItemsControlFromItemContainer(container);
            int index = GetItemIndex(itemsControl, container);
            var itemsCount = itemsControl.Items.Count;
            return (!IsOdd(index) && IsOdd(itemsCount)) || (IsOdd(index) && !IsOdd(itemsCount)) 
                ? OddItemsStyle
                : EvenItemsStyle;
        }

        private static int GetItemIndex(ItemsControl itemsControl, DependencyObject container) =>
            itemsControl.ItemContainerGenerator.IndexFromContainer(container);

        private static bool IsOdd(int value) => value % 2 == 0;
    }
}
