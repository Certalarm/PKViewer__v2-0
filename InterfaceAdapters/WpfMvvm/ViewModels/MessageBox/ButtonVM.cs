using System.Windows.Input;

namespace WpfMvvm.ViewModels.MessageBox
{
    public class ButtonVM
    {
        private readonly string _caption;
        private readonly ICommand _command;
        private readonly object _parameter;

        public string Caption => _caption;
        public ICommand Command => _command;
        public object Parameter => _parameter;

        #region .ctors
        public ButtonVM(string caption, ICommand command, object parameter = default)
        {
            _caption = caption;
            _command = command;
            _parameter = parameter;
        }
        #endregion
    }
}
