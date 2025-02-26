using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvm.Infrastructure.Commands;
using WpfMvvm.ViewModels.Base;

namespace WpfMvvm.ViewModels.MainWindow.PkvPopup
{
    public class PkvPopupVM: ViewModel
    {
        private bool _isButtonVisible;
        private string _title;
        private string _buttonTitle;
        private LambdaCommand _pressButtonCommand;

        public bool IsButtonVisible
        {
            get => _isButtonVisible;
            set => Set(ref _isButtonVisible, value);
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string ButtonTitle
        {
            get => _buttonTitle;
            set => Set(ref _buttonTitle, value);
        }

        public LambdaCommand PressButtonCommand
        {
            get => _pressButtonCommand;
            set => Set(ref _pressButtonCommand, value);
        }

        public void Init(string title, string buttonTitle = default, Action<object> PressButtonAction = default)
        {
            _title = title;
            if (buttonTitle is null || PressButtonAction is null)
                return;
            _buttonTitle = buttonTitle;
            _isButtonVisible = true;
            _pressButtonCommand = new LambdaCommand(PressButtonAction);
        }
    }
}
