using System;
using System.Collections.Generic;
using System.Linq;
using static WpfMvvm.Models.IniHandler.IniHandler;

namespace WpfMvvm.Models.Settings.Base
{
    public class BaseSettings
    {
        private const string __settingsFilename = "settings.ini";
        private static readonly StringComparison __caseFree = StringComparison.OrdinalIgnoreCase;
        private static readonly StringComparer __caseFree2 = StringComparer.OrdinalIgnoreCase;

        private static readonly Lazy<BaseSettings> _lazy = new(() => new BaseSettings(), true);
        private static readonly Lazy<IList<string>> _errors = new(() => [], true);
        private static IniSections _sections;
        private static string _oldHash;

        public static BaseSettings Settings => _lazy.Value;

        public static IList<string> Errors => _errors.Value;

        #region .ctors
        private BaseSettings()
        {
            InitSections();
            _oldHash = NewHash();
        }
        #endregion

        public void SaveToFileIfChanged()
        {
            if (!IsNeedSave())
                return;
            var error = _sections.SaveToFile(__settingsFilename);
            AddNotEmptyError(error);
        }

        public string GetValue(string section, string key)
        {
            var value = _sections.GetValueOrEmpty(section, key);
            return value.Length > 0
                ? value
                : DefaultSettings.Sections.GetValueOrEmpty(section, key);
        }

        public void SetValue(string section, string key, string value) =>
            _sections.SetValue(section, key, value);

        private static void InitSections()
        {
            _sections ??= new(__caseFree2);
            var error = _sections.LoadFromFile(__settingsFilename);
            AddNotEmptyError(error);
            if(!_sections.Any())
                _sections = DefaultSettings.Sections;
        }

        private static bool IsNeedSave() => IsOldHashEmpty() || IsDifferentHashes();

        private static bool IsOldHashEmpty() => string.IsNullOrWhiteSpace(_oldHash);

        private static bool IsDifferentHashes() => !string.Equals(_oldHash, NewHash(), __caseFree);

        private static string NewHash() => MD5Hasher.CalcHash(_sections);

        private static void AddNotEmptyError(string error)
        {
            if (error.Length > 0)
                _errors.Value.Add(error);
        }
    }
}
