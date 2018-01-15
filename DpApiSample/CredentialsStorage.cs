using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace DpApiSample
{
    public class CredentialsStorage
    {
        private readonly byte[] _entropy = new byte[16];
        private string _filePath;
        private Dictionary<int, string> _enctryptedCredentials = new Dictionary<int, string>();

        private CredentialsStorage()
        {
        }

        public static CredentialsStorage Open(string filePath)
        {
            var result = new CredentialsStorage
            {
                _filePath = filePath
            };
            if (!File.Exists(filePath))
            {
                return result;
            }
            var text = File.ReadAllText(filePath);
            result._enctryptedCredentials = JsonConvert.DeserializeObject<Dictionary<int, string>>(text);

            return result;
        }

        public Credential GetCredentialById(int id)
        {
            if (_enctryptedCredentials.ContainsKey(id))
            {
                return Dectypt<Credential>(_enctryptedCredentials[id]);
            }

            return null;
        }

        public void SetCredential(Credential credential)
        {
            _enctryptedCredentials[credential.Id] = Encrypt(credential);
        }

        public void Save()
        {
            var fileContent = JsonConvert.SerializeObject(_enctryptedCredentials);
            File.WriteAllText(_filePath, fileContent);
        }


        private string Encrypt<TMessage>(TMessage message)
        {
            string serializeObject = JsonConvert.SerializeObject(message);
            byte[] messageAsBytes = Encoding.UTF8.GetBytes(serializeObject);
            byte[] encrypted = ProtectedData.Protect(messageAsBytes, _entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }
        private TMessage Dectypt<TMessage>(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decrypted = ProtectedData.Unprotect(encryptedBytes, _entropy, DataProtectionScope.CurrentUser);
            var decryptedString = Encoding.UTF8.GetString(decrypted);
            var result = JsonConvert.DeserializeObject<TMessage>(decryptedString);
            return result;
        }
    }
}