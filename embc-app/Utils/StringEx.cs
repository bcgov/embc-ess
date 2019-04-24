using System;
using System.Text;

namespace Gov.Jag.Embc.Public.Utils
{
    public static class StringEx
    {
        public static string Base64Encode(this string s)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string s)
        {
            var base64EncodedBytes = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
