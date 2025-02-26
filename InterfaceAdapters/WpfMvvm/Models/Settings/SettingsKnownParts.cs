namespace WpfMvvm.Models.Settings
{
    public static class SettingsKnownParts
    {
        #region Known Sections
        public const string SectionMain = "Main";
        #endregion

        #region Section Items
        public const string MainLang = "Lang";
        public const string MainIsDark = "IsDark";
        public const string MainIsShowPubKeyPrefix = "IsShowPubKeyPrefix";
        public const string MainIsDeletedPresent = "IsDeletedPresent";

        public const string MainLangDefault = "Ru-ru";
        public const string MainIsDarkDefault = "False";
        public const string MainIsShowPubKeyPrefixDefault = "True";
        public const string MainIsDeletedPresentDefault = "True";
        #endregion
    }
}
