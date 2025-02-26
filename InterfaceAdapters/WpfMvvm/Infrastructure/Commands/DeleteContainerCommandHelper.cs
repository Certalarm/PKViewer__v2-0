using System.Collections.Generic;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class DeleteContainerCommandHelper
    {
        internal static (IList<string> errorPaths, IList<string> Empties) SplitByErrorsAndEmpties(
            IList<string> containerPaths, IList<string> errorsOrEmpties)
                {
                    IList<string> errorContinerPaths = [];
                    IList<string> empties = [];
                    for (int i = 0; i < errorsOrEmpties.Count; i++)
                        if (!string.IsNullOrWhiteSpace(errorsOrEmpties[i]))
                            errorContinerPaths.Add(containerPaths[i]);
                        else
                            empties.Add(containerPaths[i]);
                    return (errorContinerPaths, empties);
                }

        internal static KeyValuePair<string, IList<string>> NullPair() =>
            new(null, null);
    }
}
