using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace UTE_UWP_.Helpers
{
    public class EncryptorsDecryptors
    {
            public static string Base64Encode(string text)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).TrimEnd('=').Replace('+', '-')
                    .Replace('/', '_');
            }

            public static string Base64Decode(string text)
            {
                text = text.Replace('_', '/').Replace('-', '+');
                switch (text.Length % 4)
                {
                    case 2:
                        text += "==";
                        break;
                    case 3:
                        text += "=";
                        break;
                }
                return Encoding.UTF8.GetString(Convert.FromBase64String(text));
            }

        public static string SHA1Encrypt(string text)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(text));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }


    }
    }
