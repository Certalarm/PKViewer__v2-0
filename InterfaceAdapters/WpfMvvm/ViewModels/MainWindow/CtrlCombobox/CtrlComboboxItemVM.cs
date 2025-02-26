namespace WpfMvvm.ViewModels.MainWindow.CtrlCombobox
{
    public class CtrlComboboxItemVM
    {
        public string Title { get; }
        public string ControlKey { get; }

        #region .ctors
        public CtrlComboboxItemVM(string title, string controlKey = default)
        {
            Title = title;
            ControlKey = controlKey;
        }
        #endregion
    }
}
