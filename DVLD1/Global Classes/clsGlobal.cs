using DVLD1_Business;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD1.Global_Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVDL1";

            string userName = "UserName";
            string passWord = "Password";
            try
            {
                // Write the value to the Registry
                Registry.SetValue(keyPath, userName, Username, RegistryValueKind.String);
                Registry.SetValue(keyPath, passWord, Password, RegistryValueKind.String);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }           
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {// Specify the Registry key and path
         // string keyPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\YourSoftware";
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVDL1";
            string userName = "userName";
            string password = "Password";

            try
            {
                // Read the value from the Registry
                string userNameValue = Registry.GetValue(keyPath, userName, null) as string;
                string passwordValue = Registry.GetValue(keyPath, password, null) as string;


                if (userNameValue != null && passwordValue != null)
                {
                    Username = userNameValue;
                    Password = passwordValue;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        static public string ComputeHash(string input)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
