using PKInfo.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKInfoLib.Tests.Certificate
{
    internal class TestCertWriter : ICertWriter
    {
        public string Write(byte[] certRawData, string serial)
        {
            return $"D:\\Downloads\\{serial}.cer";
        }
    }
}
