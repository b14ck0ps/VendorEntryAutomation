using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VE.UserInterface.Controllers
{
    public class CodeGenerator
    {
        private static readonly Random random = new Random();
        public static string GenerateRandomCode(int length = 5)
        {
            const string chars = "0123456789";
            string randomString = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            string uniqueCode = $"{randomString}";

            return uniqueCode;
        }
    }
}