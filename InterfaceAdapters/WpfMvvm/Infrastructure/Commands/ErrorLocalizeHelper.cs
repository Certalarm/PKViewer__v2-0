using System.Linq;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Commands
{
    internal static class ErrorLocalizeHelper
    {
        internal const char __errorPartDelim = '.';

        internal static bool IsError(string value)
        {
            var parts = value.Split(__errorPartDelim);
            return !value.Any(x => x == System.IO.Path.DirectorySeparatorChar)
                && __knownErrorKeys.Contains(parts[0]);
        }

        internal static (string errorKey, string errorDesc) GetLocalizeError(string errorValue)
        {
            if (string.IsNullOrWhiteSpace(errorValue))
                return EmptyResult();

            var errorParts = errorValue.Split(__errorPartDelim);

            return errorParts.Length != 2
                ? EmptyResult()
                : BuildLocalizeResult(errorParts);
        }

        private static (string, string) EmptyResult() =>
            (string.Empty, string.Empty);

        private static (string, string) BuildLocalizeResult(string[] errorParts)
        {
            var langDict = FindLangDict();
            var errorKey = __knownErrorKeys.Contains(errorParts[0])
                ? GetValue(langDict, errorParts[0])
                : errorParts[0];
            var errorDesc = __knownErrorDescs.Contains(errorParts[1])
                ? GetValue(langDict, errorParts[1])
                : errorParts[1];
            return (errorKey, errorDesc);
        }
    }
}
