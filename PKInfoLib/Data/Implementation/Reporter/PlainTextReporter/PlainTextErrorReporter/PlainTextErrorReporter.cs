using PKInfo.Domain.Entity;
using PKInfo.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.Reporter.PlainTextReporter.PlainTextErrorReporter
{
    internal class PlainTextErrorReporter : BaseReporter
    {
        private const string __tab = "  ";

        #region .ctors
        internal PlainTextErrorReporter(RootError rootError) : base(rootError)
        {
        }
        #endregion

        protected sealed override T Report<T>() => 
            ErrorToString(__rootError, ReaderErrors) as T;

        private static IEnumerable<string> ReaderErrors(RootError rootError) =>
            Errors(rootError.ReaderErrors, MediaErrors);

        private static IEnumerable<string> MediaErrors(ReaderError readerError) =>
            Errors(readerError.KeyMediaErrors, TypeErrors);

        private static IEnumerable<string> TypeErrors(KeyMediaError mediaError) =>
            Errors(mediaError.KeyContainerTypeErrors, ContainerErrors);

        private static IEnumerable<string> ContainerErrors(KeyContainerTypeError typeError) =>
            Errors(typeError.KeyContainerErrors, (_) => null);

        private static IEnumerable<string> Errors<T>(IEnumerable<T> errors, Func<T, IEnumerable<string>> func) 
            where T: BaseError => 
            errors
                .Select(x => ErrorToString(x, func));

        private static string ErrorToString<T>(T error, Func<T, IEnumerable<string>> func) where T: BaseError
        {
            var stringContent = ContentToString(error.Content, error.DepthLevel);
            var stringItems = string.Join(Rn, func(error) ?? []);
            return ErrorPartsToString(stringContent, stringItems);
        }

        private static string ErrorPartsToString(string content, string items) =>
            (content.Length, items.Length) switch
            {
                (0, 0) => string.Empty,
                (0, _) => items,
                (_, 0) => content,
                (_, _) => $"{content}{Rn}{items}"
            };

        private static string ContentToString(Dictionary<string, string> content, int depthLevel) =>
            ContentItemsToString(ContentItemsWoEmptyValues(content, depthLevel), depthLevel);

        private static IEnumerable<string> ContentItemsWoEmptyValues(Dictionary<string, string> content, int depthLevel) =>
            content
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                .Select(x => ContentItemToString(x, depthLevel));

        private static string ContentItemsToString(IEnumerable<string> content, int depthLevel) =>
            content.Any()
                ? $"{Tabs(depthLevel)}{string.Join(Rn + Tabs(depthLevel), content)}"
                : string.Empty;

        private static string ContentItemToString(KeyValuePair<string, string> item, int depthLevel) =>
            string.IsNullOrWhiteSpace(item.Value)
                ? string.Empty
                : MayBeErrorContentItemToString(item, depthLevel);

        private static string MayBeErrorContentItemToString(KeyValuePair<string, string> item, int depthLevel) =>
            IsErrorValue(item)    
                ? $"{item.Key}{Colon}{Rn}{ErrorItemValueToString(item.Value, depthLevel)}"
                : $"{item.Key}{Colon}{Space}{item.Value}";

        private static bool IsErrorValue(KeyValuePair<string, string> item) =>
            item.Value.Contains($"{Colon}{Space}");

        private static string ErrorItemValueToString(string value, int depthLevel)
        {
            var parts = value.Split([ Colon ], StringSplitOptions.RemoveEmptyEntries);
            var tabs = $"{__tab}{Tabs(depthLevel)}";
            return parts.Length < 2
                ? $"{tabs}{value}"
                : $"{tabs}{parts[0]}{Colon}{Rn}{tabs}{parts[1].Trim()}";
        }

        private static string Tabs(int count) =>
            count < 1
                ? string.Empty
                : string.Concat(Enumerable.Repeat(__tab, count));
    }
}
