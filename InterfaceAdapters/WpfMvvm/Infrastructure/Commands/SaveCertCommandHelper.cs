using System.Collections.Generic;
using System.Linq;
using static WpfMvvm.Infrastructure.Commands.ErrorLocalizeHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class SaveCertCommandHelper
    {
        internal static (IList<string> errorPaths, IList<string> okPaths) SplitByErrorsAndPaths(
            IList<string> containerPaths, IList<string> errorsOrPaths)
        {
            IList<string> errorContinerPaths = [];
            IList<string> okContainerPaths = [];
            for (int i = 0; i < errorsOrPaths.Count; i++)
                if (IsError(errorsOrPaths[i]))
                    errorContinerPaths.Add(containerPaths[i]);
                else
                    okContainerPaths.Add(containerPaths[i]);
            return (errorContinerPaths, okContainerPaths);
        }

        internal static bool HasErrors(IList<string> errorsOrPaths) =>
            errorsOrPaths
                .Any(IsError);

        internal static bool HasPaths(IList<string> errorsOrPaths) =>
            errorsOrPaths
                .Any(x => !IsError(x));

        internal static IList<string> GetErrors(IList<string> errorsOrPaths) =>
            errorsOrPaths
                .Where(IsError)
                .ToList();

        internal static IList<string> GetPaths(IList<string> errorsOrPaths) =>
            errorsOrPaths
                .Where(x => !IsError(x))
                .ToList();

        internal static KeyValuePair<string, IList<string>> NullPair() =>
            new (null, null);
    }
}
