using System.Collections.Generic;
using System.Linq;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity.Base
{
    public abstract class BaseError
    {
        public int DepthLevel { get; }

        public Dictionary<string, string> Content { get; protected set; } = [];

        #region .ctors
        protected BaseError(int depthLevel, string errorBody)
        {
            DepthLevel = depthLevel;
            SetIfNotEmpty(ErrorBody, errorBody);
        }
        #endregion

        protected void SetIfNotEmpty(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            Content[key] = value;
        }

#if DEBUG
        public override string ToString()
        {
            return $"{Tabs()}{nameof(DepthLevel)}{Colon}{Space}{DepthLevel}" 
                + $"{Rn}{Tabs()}{nameof(Content)}{Colon}"
                + $"{Rn}{ContentToString()}"
                ;
        }

        protected string ContentToString() =>
            $"{Tab}{Tabs()}{string.Join(Rn + Tab + Tabs(), Content.Select(ContentItemToString))}";

        protected string Tabs(int count = default) =>
            count == default
                ? TabsByDepth()
                : new string(Tab, count);

        private string TabsByDepth() =>
            DepthLevel < 1
                ? string.Empty
                : new string(Tab, DepthLevel);

        private string ContentItemToString(KeyValuePair<string, string> item) =>
            $"{item.Key}{Colon}{Space}{item.Value}";
#endif
    }
}
