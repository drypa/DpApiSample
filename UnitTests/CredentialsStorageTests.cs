using System;
using System.IO;
using DpApiSample;
using NUnit.Framework;

namespace UnitTests
{
    public class CredentialsStorageTests
    {
        [Test]
        public void GetCredentialById_NoFileStorageExists_NoResult()
        {
            CredentialsStorage credentialsStorage = CredentialsStorage.Open("C:\\no-file-exists.json");
            Assert.IsNull(credentialsStorage.GetCredentialById(100500));
        }

        [Test]
        public void GetCredentialById_EmptyStorage_NoResult()
        {
            var filePath = Path.Combine(Path.GetTempPath(), "credentials.json");
            try
            {
                var storage = CredentialsStorage.Open(filePath);
                storage.Save();

                var credentialsStorage = CredentialsStorage.Open(filePath);
                var credential = credentialsStorage.GetCredentialById(100500);

                Assert.IsNull(credential);
            }
            finally
            {
                File.Delete(filePath);
            }

           
        }

        [Test]
        public void GetCredentialById_NotContainsCredentialWithSuchId_NoResult()
        {
            var filePath = Path.Combine(Path.GetTempPath(), "credentials.json");
            try
            {
                var storage = CredentialsStorage.Open(filePath);
                storage.SetCredential(new Credential
                {
                    Id = 1,
                    Password = "pass",
                    Login = "login"
                });
                storage.Save();

                var credentialsStorage = CredentialsStorage.Open(filePath);
                var credential = credentialsStorage.GetCredentialById(100500);

                Assert.IsNull(credential);
            }
            finally
            {
                File.Delete(filePath);
            }

           
        }


        [Test]
        public void GetCredentialById_HasPersistedCredential_Success()
        {
            var filePath = Path.Combine(Path.GetTempPath(), "credentials.json");
            var storedCredential = new Credential
            {
                Id = 1,
                Password = "pass",
                Login = "login"
            };
            try
            {
                var storage = CredentialsStorage.Open(filePath);
               
                storage.SetCredential(storedCredential);
                storage.Save();

                //act
                var credentialsStorage = CredentialsStorage.Open(filePath);
                var credential = credentialsStorage.GetCredentialById(1);

                Assert.IsNotNull(credential);
                Assert.AreEqual(storedCredential.Password, credential.Password);
                Assert.AreEqual(storedCredential.Id, credential.Id);
                Assert.AreEqual(storedCredential.Login, credential.Login);
            }
            finally
            {
                File.Delete(filePath);
            }
        }



        [Test]
        public void Save_NoFileExists_FileCreated()
        {
            var filePath = Path.Combine(Path.GetTempPath(), "credentials.json");
            Assert.IsFalse(File.Exists(filePath));
            var storedCredential = new Credential
            {
                Id = 1,
                Password = "pass",
                Login = "login"
            };
            try
            {
                var storage = CredentialsStorage.Open(filePath);

                storage.SetCredential(storedCredential);
                storage.Save();

                Assert.IsTrue(File.Exists(filePath));
               
            }
            finally
            {
                File.Delete(filePath);
            }
        }
    }
}
