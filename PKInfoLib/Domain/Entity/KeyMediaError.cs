using PKInfo.Domain.Entity.Base;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public class KeyMediaError : BaseError
    {
        public IReadOnlyList<KeyContainerTypeError> KeyContainerTypeErrors { get; } = [];

        #region .ctors
        private KeyMediaError(string error, string rootPath, KeyMediaType kmType, List<KeyContainerTypeError> kctErrors) 
            : base((int)DepthLevelType.KeyMediaDepth, error)
        {
            SetIfNotEmpty(RootPath, rootPath);
            SetIfNotEmpty(Txt.KeyMediaType, kmType.ToString());
            KeyContainerTypeErrors = kctErrors;
        }

        private KeyMediaError(Dictionary<string, string> content, List<KeyContainerTypeError> kctErrors)
            : base((int)DepthLevelType.KeyMediaDepth, string.Empty)
        {
            Content = content;
            KeyContainerTypeErrors = kctErrors;
        }
        #endregion

        public static KeyMediaError Create(Dictionary<string, string> content, List<KeyContainerTypeError> kctErrors)
        {
            return new KeyMediaError(content, kctErrors);
        }

        internal static KeyMediaError Create(KeyMediaInfo kmInfo)
        {
            var kctErrors = kmInfo.ErrorsKeyContainerTypes
                .Select(x => CreateKcteError(kmInfo, x))
                .Where(HasError)
                .ToList();
            return new(kmInfo.Error, kmInfo.RootPath, kmInfo.Type, kctErrors);
        }

        private static bool HasError(KeyContainerTypeError value) =>
            value.KeyContainerErrors.Count > 0 
            || value.Content.HasKey(ErrorBody) 
            || value.Content.HasKey(ErrorDeletedBody);

        private static KeyContainerTypeError CreateKcteError(KeyMediaInfo kmInfo, ErrorKeyContainerTypeInfo error)
        {
            var oneTypeContainers = FilterByType(kmInfo.KeyContainersInfo, error.KeyContainerType);
            return KeyContainerTypeError.Create(oneTypeContainers, error);
        }

        private static IList<KeyContainerInfo> FilterByType(
            IEnumerable<KeyContainerInfo> containers,
            KeyContainerType type)
            => containers
                .Where(x => x.Type == type)
                .ToList();

#if DEBUG
        public override string ToString()
        {
            return base.ToString()
                + $"{Rn}{Tab}{Tabs()}{nameof(KeyContainerTypeErrors)}{Colon}"
                + (KeyContainerTypeErrors.Any()
                    ? $"{Rn}{string.Join(Rn + Tabs(2) + Tabs(), KeyContainerTypeErrors.Select(x => x.ToString()))}"
                    : string.Empty)
                ;
        }
#endif
    }
}
