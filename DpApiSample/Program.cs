namespace DpApiSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var el1 = new Credential {Login = "login1", Password = "pass1", Id = 1};
            var el2 = new Credential {Login = "login2", Password = "pass2", Id = 2};
            var el3 = new Credential {Login = "login3", Password = "pass3", Id = 3};

            var credentialsStorage = CredentialsStorage.Open("C:\\credentials.json");
            credentialsStorage.SetCredential(el1);
            credentialsStorage.SetCredential(el2);
            credentialsStorage.SetCredential(el3);
            credentialsStorage.Save();

            var first = credentialsStorage.GetCredentialById(1);
        }
    }
}