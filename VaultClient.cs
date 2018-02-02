using System;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;


namespace RewrapExample
{
    class VaultClient
    {
        IVaultClient client;
        string transitKeyName;
        const string keyPath = "/transit/keys/";
        
        public VaultClient(string vaultAddr, string vaultToken, string keyName)
        {
            Uri vaultUri = new Uri(vaultAddr);
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(vaultToken);
            client = VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);
            transitKeyName = keyName;
        } 
        
        // get latest transit key version
        public async Task<int> GetLatestTransitKeyVersion()
        {
            int keyVersion = -1;
            var resp = await client.ReadSecretAsync(keyPath + transitKeyName);
            if (resp.Data.ContainsKey("latest_version"))
            {
                keyVersion = (int)(long)resp.Data["latest_version"];
            }
            
            return keyVersion;
        }
        
        // rewrap endpoint, possible to upload batches of records,  but that is
        // not currently supported by the VaultSharp client.  You can specify things like
        // alternate mount point, context for derived keys, etc.  Please see the documentation:
        // https://github.com/rajanadar/VaultSharp
        public async Task ReWrapValue(string ciphertext)
        {
            await client.TransitRewrapWithLatestEncryptionKeyAsync(transitKeyName, ciphertext);
        }

        // encrypt data, required for seeding 
        public async Task<string> EncryptValue(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            string b64encoded = Convert.ToBase64String(bytes);
            var ciphertext = await client.TransitEncryptAsync(transitKeyName, b64encoded);
            return ciphertext.Data.CipherText;
        }  
    }
}