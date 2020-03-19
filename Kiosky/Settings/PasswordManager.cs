using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace Kiosky.Settings
{
    /// <summary>
    /// Helper class to compare and generate passwords
    /// </summary>
    public class PasswordManager
    {
        /// <summary>
        /// Compares user input to the password in settings
        /// </summary>
        /// <param name="UserPassword">The user input password</param>
        /// <param name="settings">The settings</param>
        /// <returns></returns>
        public static bool ComparePassword(string UserPassword, Settings settings)
        {
            if(settings.AdminPasswordHash == null)
            {
                return true;
            }
            else
            {
                if(settings.AdminPasswordHash.Length == 0 && UserPassword.Length == 0)
                {
                    return true;
                }else  if(UserPassword.Length > 0 && settings.AdminPasswordHash.Length > 0 && settings.AdminPasswordHash.Split('$').Count() == 6) {

                    string[] hashSegments = settings.AdminPasswordHash.Split('$');

                    //Decode the encoded has back into it's parts, we skip the first two parameters because we only use Argon2ID
                    string aParams = null;
                    string salt = null;

                    // The parameters for the encoder
                    aParams = hashSegments[3];
                    // verify we have all the needed parameters
                    if(aParams.Contains(",") && aParams.Split(',').Count() == 3)
                    {
                        //Get the parameters for the hashing
                        int DoP, iter, mem;

                        var aParamsArray = aParams.Split(',');
                        DoP = GetParam("p=", aParamsArray);
                        mem = GetParam("m=", aParamsArray);
                        iter = GetParam("t=", aParamsArray);

                        


                        // Verify we got all three settings needed
                        if(DoP > 0 && iter > 0 && mem > 0)
                        {
                            salt = hashSegments[4];

                            //Check if the passwords match
                            if (salt != null && GeneratePasswordHash(UserPassword, Convert.FromBase64String(salt), DoP, iter, mem)
                                .Equals(settings.AdminPasswordHash, StringComparison.Ordinal))

                                return true;
                            else
                                return false;


                        }

                    }

                    return false;
                }
                else
                {
                    //Throw an invalid hash error?
                    return false;
                }
            }
            
        }
        /// <summary>
        /// Use Argon2id to generate a PD for storage
        /// </summary>
        /// <param name="UserPassword">The user supplied password</param>
        /// <param name="Salt">A salt, see GenerateSalt for a salt.</param>
        /// <param name="DegreeOfParalellism">Defaults to 4</param>
        /// <param name="Iterations">Defaults to 8</param>
        /// <param name="MemorySize">KiB of memory to use, defaults to 262144 (256MiB)</param>
        /// <returns></returns>
        public static string GeneratePasswordHash(string UserPassword, byte[] Salt, int DegreeOfParalellism = 4, int Iterations = 4, int MemorySize = (1024*64))
        {
            Argon2id argon2 = new Argon2id(UTF8Encoding.UTF8.GetBytes(UserPassword));

            //Set up the password
            argon2.Salt = Salt;
            argon2.DegreeOfParallelism = DegreeOfParalellism;
            argon2.Iterations = Iterations;
            argon2.MemorySize =MemorySize; 

            var encodedSalt = Convert.ToBase64String(argon2.Salt);
            var encodedHash = Convert.ToBase64String(argon2.GetBytes(128));
            var pwHash = String.Format("$argon2id$v=19$m={0},t={1},p={2}${3}${4}", argon2.MemorySize, argon2.Iterations, argon2.DegreeOfParallelism, encodedSalt, encodedHash);
            return pwHash;
        }

        /// <summary>
        /// Generate a salt for the password
        /// </summary>
        /// <param name="size">Number of bytes for the salt</param>
        /// <returns></returns>
        public static byte[] GenerateSalt(int size)
        {
            //Use rngCSP to generate the salt
            byte[] saltBuff = new byte[size];
            using (var rngCSP = new RNGCryptoServiceProvider())
            {
                rngCSP.GetNonZeroBytes(saltBuff);
            }

            return saltBuff;
        }

        private static int GetParam(string prefix,string[] parameters)
        {
            foreach (var p in parameters)
            {
                if (p.StartsWith(prefix) && p.Length >= 3)
                {
                    int t = 0;

                    if (Int32.TryParse(p.Substring(2), out t))
                    {
                        return t;
                    }
                }
            }
            return 0;
        }
    }
}
