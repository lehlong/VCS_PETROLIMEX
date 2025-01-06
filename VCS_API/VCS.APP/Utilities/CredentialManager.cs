using CredentialManagement;
using DMS.BUSINESS.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCS.APP.Utilities
{
    public static class CredentialManager
    {
        private const string APPLICATION_NAME = "VCSApplication";

        public static void SaveCredentials(LoginCredentials credentials)
        {
            if (credentials.RememberMe)
            {
                var cred = new Credential
                {
                    Target = APPLICATION_NAME,
                    Username = credentials.Username,
                    Password = EncryptPassword(credentials.Password),
                    PersistanceType = PersistanceType.LocalComputer
                };
                cred.Save();
            }
            else
            {
                DeleteCredentials();
            }
        }

        public static LoginCredentials GetSavedCredentials()
        {
            var cred = new Credential { Target = APPLICATION_NAME };

            if (!cred.Load())
                return null;

            return new LoginCredentials
            {
                Username = cred.Username,
                Password = DecryptPassword(cred.Password),
                RememberMe = true
            };
        }

        public static void DeleteCredentials()
        {
            var cred = new Credential { Target = APPLICATION_NAME };
            cred.Delete();
        }

        private static string EncryptPassword(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        private static string DecryptPassword(string encryptedPassword)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedPassword));
        }
    }

}
