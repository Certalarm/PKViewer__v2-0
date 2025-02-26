using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System;
using System.Collections.Generic;

namespace WpfMvvm.Models.CertInfoPopupModel
{
    public class CertInfoPopupVM
    {
        public string Serial { get; private set; }
        public KeyValuePair<DateTime, DateTime> Validity { get; private set; }
        public KeyValuePair<CertType, CertOwnerType> Type { get; private set; }
        public string Organization { get; private set; }
        public string OrganizationUnit { get; private set; }
        public string Owner { get; private set; }
        public string OwnerTitle { get; private set; }
        public string OwnerEmail { get; private set; }
        public string Issuer { get; private set; }

        #region .ctors
        private CertInfoPopupVM()
        {
        }
        #endregion

        public static CertInfoPopupVM Create(CertInfo certInfo) =>
            new()
            {
                Serial = certInfo.Serial,
                Validity = new((DateTime)certInfo.NotBeforeUTC, (DateTime)certInfo.NotAfterUTC),
                Type = new(certInfo.Type, certInfo.OwnerType),
                Organization = certInfo.Organization,
                OrganizationUnit = certInfo.OrganizationUnit,
                Owner = certInfo.Owner,
                OwnerTitle = certInfo.OwnerTitle,
                OwnerEmail = certInfo.OwnerEmail,
                Issuer = certInfo.Issuer,
            };
    }
}
