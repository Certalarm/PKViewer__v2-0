using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System;
using WpfMvvm.Models.KeyContainerItem;
using WpfMvvm.ViewModels.Base;

namespace WpfMvvm.ViewModels.KeyContainerItem
{     
    public class KeyContainerItemVM : ViewModel
    {
        private bool _isChecked;

        public KeyContainerType Type { get; private set; }

        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public NameHolder NameHolder { get; private set; }
        public DateTime? DateNotAfterUTC { get; private set; }
        public DateTime? DateOfCopyUTC { get; private set; }
        public TimeInterval RemainingTimeUntilEndKey { get; private set; }
        public TimeInterval ElapsedTimeAfterCopy { get; private set; }
        public string PublicKey { get; private set; }
        public string Path { get; private set; }

        #region .ctors
        private KeyContainerItemVM()
        {
        }
        #endregion

        public static KeyContainerItemVM Create(KeyContainerSnapshotInfo keyInfo) =>
            new()
            {
                Type = keyInfo.Type,
                IsChecked = false,
                NameHolder = NameHolder.Create(keyInfo),
                DateNotAfterUTC = keyInfo.DateNotAfterUTC,
                DateOfCopyUTC = keyInfo.DateOfCopyUTC,
                RemainingTimeUntilEndKey = keyInfo.RemainingTimeUntilEndKey,
                ElapsedTimeAfterCopy = keyInfo.ElapsedTimeAfterCopy,
                PublicKey = keyInfo.PublicKey,
                Path = keyInfo.Path
            };
    }
}
