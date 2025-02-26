using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Commands.Base;
using WpfMvvm.ViewModels.MainWindow.CtrlCombobox;

namespace WpfMvvm.Infrastructure.Converters.Base
{
    public abstract class SelectionChangedConverter : Converter
    {
        protected CtrlComboboxVM _ctrlComboboxVM;
        private int _previousIndex = -1;
        protected Command _command;

        // Convert вызывается дважды из одного триггера,
        // сначала с value как CtrlComboboxVM (единожды), потом с value как
        // ComboBox.SelectedIndex (при каждом изменении SelectedIndex).
        // Сделано для того, чтобы вместо обработки события ComboBox.SelectionChanged
        // обрабатывать bool-свойство ComboBox.IsDropDownOpen, т.к. в стандартной поставке
        // wpf некорректно обрабатывается EventTrigger на ComboBox.SelectionChanged,
        // для этого нужно тащить либу Interactive.
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if(p != null && value is CtrlComboboxVM)
                ResetState();

            SelectionChangedConvert(value);
            return true;
        }

        private void SelectionChangedConvert(object value)
        {
            if (_ctrlComboboxVM == null && value is CtrlComboboxVM ctrlComboboxVM)
                _ctrlComboboxVM = ctrlComboboxVM;

            if (IsNeedExecuteCommand(value))
                ChangePreviousIndexAndExecuteCommand((int)value);
        }

        private bool IsNeedExecuteCommand(object value) =>
           _ctrlComboboxVM != null && value is int index && _previousIndex != index;

        private void ChangePreviousIndexAndExecuteCommand(int index)
        {
            ExecuteCommand(index);
            _previousIndex = index;
        }

        protected abstract void ExecuteCommand(int index);

        private void ResetState()
        {
            _ctrlComboboxVM = null;
            _previousIndex = -1;
        }
    }
}
