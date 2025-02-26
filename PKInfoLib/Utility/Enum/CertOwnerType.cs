namespace PKInfo.Utility.Enum
{
    public enum CertOwnerType
    {
        Unknown = 0,
        ForUL = 1, // Юр. лицо
        ForULwoFIO = 2, // Юр. лицо без ФИО
        ForDL = 3, // Должностное лицо
        ForIP = 4, // Индивидуальный предприниматель
        ForFL = 5, // Физ. лицо
    }
}
