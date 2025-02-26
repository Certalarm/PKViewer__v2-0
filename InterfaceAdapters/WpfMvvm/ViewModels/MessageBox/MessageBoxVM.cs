using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace WpfMvvm.ViewModels.MessageBox
{
    public class MessageBoxVM
    {
        private readonly string _caption;
        private readonly string _titleOk;
        private IList<string> _itemsOk;
        private readonly string _titleError;
        private IList<string> _itemsError;
        private readonly string _question;
        private IReadOnlyList<ButtonVM> _buttonVMs;

        public string Caption => _caption;
        public string TitleOk => _titleOk;
        public IList<string> ItemsOk => _itemsOk;
        public string TitleError => _titleError;
        public IList<string> ItemsError => _itemsError;
        public string Question => _question;
        public IReadOnlyList<ButtonVM> ButtonVMs => _buttonVMs;
        public bool HasQuestion => Question != null && Question.Length > 0;


        #region .ctors
        public MessageBoxVM(
            string caption, 
            KeyValuePair<string, IList<string>> pairOk, 
            KeyValuePair<string, IList<string>> pairError, 
            IList<ButtonVM> buttonVMs, 
            string question = default)
        {
            _caption = caption;
            _titleOk = pairOk.Key;
            _titleError = pairError.Key;
            _question = question;
            InitCollections(pairOk.Value, pairError.Value, buttonVMs);
        }
        #endregion

        private void InitCollections(IList<string> itemsOk, IList<string> itemsError, IList<ButtonVM> buttonVMs)
        {
            if (itemsOk != null)
                _itemsOk = new List<string>(itemsOk);
            if (itemsError != null)
                _itemsError = new List<string>(itemsError);
            if (buttonVMs != null)
                _buttonVMs = new List<ButtonVM>(buttonVMs);
        }

        internal void OnClosing(object s, CancelEventArgs e)
        {
            (s as Window).Hide(); 
            e.Cancel = true;
        }
    }
}
