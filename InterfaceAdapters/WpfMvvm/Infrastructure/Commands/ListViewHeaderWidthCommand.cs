using System.Windows.Controls;
using WpfMvvm.Infrastructure.Commands.Base;

namespace WpfMvvm.Infrastructure.Commands
{
    public class ListViewHeaderWidthCommand : Command
    {
        private const int MIN_COLUMN_WIDTH = 35;

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            if (IsBadParam(parameter, out var parameters))
                return;
            if (parameters[0] is not GridViewColumnHeader header || header.Role == GridViewColumnHeaderRole.Padding)
                return;
            if (IsNeedChangeWidth(header))
                header.Column.Width = MIN_COLUMN_WIDTH;
        }

        private static bool IsBadParam(object parameter, out object[] parameters)
        {
            parameters = null;
            if (parameter is not object[] innerParameters)
                return true;
            if (innerParameters.Length < 2)
                return true;
            parameters = innerParameters;
            return false;
        }

        private static bool IsNeedChangeWidth(GridViewColumnHeader header) =>
            IsFirstHeader(header) || IsHeaderWidthTooSmall(header);

        private static bool IsFirstHeader(GridViewColumnHeader header) => header.Tag == null;

        private static bool IsHeaderWidthTooSmall(GridViewColumnHeader header) =>
            header.Column.Width != double.NaN && header.Column.Width < MIN_COLUMN_WIDTH;
    }
}
