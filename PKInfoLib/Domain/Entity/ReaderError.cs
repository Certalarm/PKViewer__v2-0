using PKInfo.Domain.Entity.Base;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public class ReaderError : BaseError
    {
        public IReadOnlyList<KeyMediaError> KeyMediaErrors { get; } = [];

        #region .ctors
        private ReaderError(string error, ReaderType rType, List<KeyMediaError> kmErrors) 
            : base((int)DepthLevelType.ReaderDepth, error)
        {
            SetIfNotEmpty(Txt.ReaderType, rType.ToString());
            KeyMediaErrors = kmErrors;
        }

        private ReaderError(Dictionary<string, string> content, List<KeyMediaError> kmErrors)
            :base((int)DepthLevelType.ReaderDepth, string.Empty)
        {
            Content = content;
            KeyMediaErrors = kmErrors;
        }
        #endregion

        public static ReaderError Create(Dictionary<string, string> content, List<KeyMediaError> kmErrors)
        {
            return new(content, kmErrors);
        }

        internal static ReaderError Create(ReaderInfo readerInfo)
        {
            var kmErrors = readerInfo.KeyMediasInfo
                .Select(KeyMediaError.Create)
                .Where(x => x.Content.HasKey(ErrorBody) || x.KeyContainerTypeErrors.Count > 0)
                .ToList();
            return new(readerInfo.Error, readerInfo.Type, kmErrors);
        }

#if DEBUG
        public override string ToString()
        {
            return base.ToString()
                + $"{Rn}{Tab}{Tabs()}{nameof(KeyMediaErrors)}{Colon}{Rn}"
                + string.Join(Rn, KeyMediaErrors.Select(x => x.ToString()))
                ;
        }
#endif
    }
}
