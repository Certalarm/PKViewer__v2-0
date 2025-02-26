using PKInfo.Domain.Entity.Base;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public class KeyContainerTypeError : BaseError
    {
        public IReadOnlyList<KeyContainerError> KeyContainerErrors { get; } = [];

        #region
        private KeyContainerTypeError(string error, string errorDeleted, KeyContainerType kcType, List<KeyContainerError> kcErrors) 
            : base((int)DepthLevelType.KeyContainerTypeDepth, error)
        {
            SetIfNotEmpty(Txt.TypeKeyContainerType, kcType.ToString());
            SetIfNotEmpty(ErrorDeletedBody, errorDeleted);
            KeyContainerErrors = kcErrors;
        }
        
        private KeyContainerTypeError(Dictionary<string, string> content, List<KeyContainerError> kcErrors)
            : base((int)DepthLevelType.KeyContainerTypeDepth, string.Empty)
        {
            Content = content;
            KeyContainerErrors = kcErrors;
        }
        #endregion

        public static KeyContainerTypeError Create(Dictionary<string, string> content, List<KeyContainerError> kcErrors)
        {
            return new(content, kcErrors);
        }

        internal static KeyContainerTypeError Create(IList<KeyContainerInfo> oneKcInfos, ErrorKeyContainerTypeInfo errorKcti)
        {
            List<KeyContainerError> keyContainerErrors = oneKcInfos
                .Where(x => x.Error.Length > 0)
                .Select(KeyContainerError.Create)
                .ToList();
            return new(errorKcti.Error, errorKcti.ErrorDeleted, errorKcti.KeyContainerType, keyContainerErrors);
        }

#if DEBUG
        public override string ToString()
        {
            return base.ToString()
                + $"{Rn}{Tab}{Tabs()}{nameof(KeyContainerErrors)}{Colon}"
                + (KeyContainerErrors.Any()
                    ? $"{Rn}{string.Join(Rn + Tabs(2) + Tabs(), KeyContainerErrors.Select(x => x.ToString()))}"
                    : string.Empty)
                ;
        }
#endif
    }
}
