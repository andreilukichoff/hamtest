using Microsoft.AspNetCore.DataProtection;

namespace HamTestWasmHosted.Server.Services
{
    public class CipherService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "1b0Ek-^k(}-79izHJpk?u@_Wn/:UDAOV";

        public CipherService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }
    }
}