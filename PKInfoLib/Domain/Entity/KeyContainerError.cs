using PKInfo.Domain.Entity.Base;
using PKInfo.Utility.Enum;
using System.Collections.Generic;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public class KeyContainerError : BaseError
    {
        #region .ctors
        private KeyContainerError(string error, string path) 
            : base((int)DepthLevelType.KeyContainerDepth, error)
        {
            SetIfNotEmpty(Path, path);
        }
        
        private KeyContainerError(Dictionary<string, string> content)
            : base((int)DepthLevelType.KeyContainerDepth, string.Empty)
        {
            Content = content;
        }
        #endregion

        public static KeyContainerError Create(Dictionary<string, string> content)
        {
            return new KeyContainerError(content);
        }

        internal static KeyContainerError Create(KeyContainerInfo kcInfo)
        {
            var error = string.IsNullOrWhiteSpace(kcInfo.Error)
                ? string.Empty 
                : kcInfo.Error;
            var path = string.IsNullOrWhiteSpace(error)
                ? string.Empty
                : kcInfo.Path;
            return new(error, path);
        }
    }
}
