using BG.Infrastructure.Process.NCommon.Data;
using NCommon.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using BG.Infrastructure.Process.NCommon.Extensions;
using BG.Extensions;
using BG.Infrastructure.Common.Cryptography;

namespace BG.Domain.Infrastructure.Tests
{
    [Category("CryptographyTest")]
    public class CryptographyHelperTests 
    {
        [Test]
        public void Encrypt_message()
        {
            var key = "scstbbf";
            var message = "http://crmsrv.aetp.ru";

            var secretStr= CryptographyHelper.EncryptStringAES(message, key);
            var originalStr = CryptographyHelper.DecryptStringAES(secretStr, key);
            Assert.IsTrue(message == originalStr, $"Строки не совпадают {message} - {originalStr}");
        }


    }
}
