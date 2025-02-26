using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WpfMvvm.Infrastructure.Commands.Base;
using static WpfMvvm.Infrastructure.Commands.ListViewSortCommandHelper;
using static WpfMvvm.Infrastructure.Converters.ListViewIsCheckedHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    public class ListViewSortCommand : Command
    {
        private ListView _listView;
        private ToggleButton _toggleButton;
        private GridViewColumnHeader _curHeaderClicked = null;
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public override bool CanExecute(object parameter) => parameter is DependencyObject;

        public override void Execute(object parameter)
        {
            InitCurHeaderClicked(parameter);
            if (IsBadHeader())
                return;
            InitListView();
            ChangeHeaders(parameter);
        }

        private void ChangeHeaders(object parameter)
        {
            if (IsHeaderWithEmptyContent())
                ChangeToggleButtonHeader(parameter);
            else
                ChangeSortedAndSimpleHeaders();
        }

        private void InitCurHeaderClicked(object parameter) =>
            _curHeaderClicked = RelativeParent<GridViewColumnHeader>(parameter as DependencyObject);

        private bool IsBadHeader() =>
            _curHeaderClicked == null || _curHeaderClicked.Role == GridViewColumnHeaderRole.Padding;

        private void InitListView() => _listView ??= GetListView();

        private ListView GetListView() => RelativeParent<ListView>(_curHeaderClicked);

        private bool IsHeaderWithEmptyContent() => _curHeaderClicked.Tag == null;

        private void ChangeToggleButtonHeader(object parameter)
        {
            if (!(GetToggleButton(parameter) is ToggleButton tglBtn and not null))
                return;
            InitToggleButton(tglBtn);
            _toggleButton.IsChecked = !_toggleButton.IsChecked;
        }

        private ToggleButton GetToggleButton(object parameter) =>
            RelativeChild<ToggleButton>(parameter as GridViewColumnHeader);

        private void InitToggleButton(ToggleButton toggleButton) =>
            _toggleButton ??= toggleButton;

        private void ChangeSortedAndSimpleHeaders()
        {
            var direction = GetSortDirection();
            Sort(GetSortBy(), direction);
            SetHeaderTemplatesIfAnotherHeaderClick();
            _lastHeaderClicked = _curHeaderClicked;
            _lastDirection = direction;
        }

        private ListSortDirection GetSortDirection() =>
            _lastHeaderClicked != null || IsNotSortedHeaderClick()
                ? GetSortDirectionIfAnotherHeaderClick()
                : ListSortDirection.Ascending;

        private ListSortDirection GetSortDirectionIfAnotherHeaderClick() =>
            _curHeaderClicked != _lastHeaderClicked
                ? ListSortDirection.Descending
                : ReverseDirection();

        private ListSortDirection ReverseDirection() =>
            _lastDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;

        private string GetSortBy() => _curHeaderClicked.Tag.ToString();

        private void Sort(string sortBy, ListSortDirection direction)
        {
            IsSortOrFilterCall = true;

            var collectionView = GetCollectionView(_listView);
            collectionView.SortDescriptions.RemoveAt(collectionView.SortDescriptions.Count -1);
            SortDescription sd = new(sortBy, direction);
            collectionView.SortDescriptions.Add(sd);
        }

        private void SetHeaderTemplatesIfAnotherHeaderClick()
        {
            if (_lastHeaderClicked == _curHeaderClicked)
                return;
            if (_lastHeaderClicked != null)
                SetHeaderTemplatesIfNotFirstClick();
            if (IsNotSortedHeaderClick())
                SetHeaderTemplatesIfNotSortedHeaderClick();
        }

        private void SetHeaderTemplatesIfNotFirstClick()
        {
            _curHeaderClicked.Column.HeaderTemplate = __sortedTemplate;
            _lastHeaderClicked.Column.HeaderTemplate = __simpleTextTemplate;
        }

        private bool IsNotSortedHeaderClick() =>
            !IsEqualsHeadersText(GetStartHeader(_listView.View as GridView), _curHeaderClicked);

        private void SetHeaderTemplatesIfNotSortedHeaderClick()
        {
            _curHeaderClicked.Column.HeaderTemplate = __sortedTemplate;
            GetStartColumn(_listView.View as GridView).HeaderTemplate = __simpleTextTemplate;
        }
    }
}
