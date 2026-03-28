using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Base64
{
    public static class Base64Utility
    {
        public static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public static byte[] Base64UrlDecode(string input)
        {
            string padded = input
                .Replace("-", "+")
                .Replace("_", "/");

            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }

            return Convert.FromBase64String(padded);
        }

        public static long ExtractExp(string json)
        {
            var expStr = json.Split("\"exp\":")[1].Split(',')[0].Replace("}", "");
            return long.Parse(expStr);
        }
    }
}
