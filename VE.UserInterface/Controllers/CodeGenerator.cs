using System;
using System.Linq;

namespace VE.UserInterface.Controllers
{
    public class CodeGenerator
    {
        private static readonly Random random = new Random();

        public static string GenerateRandomCode(int length = 5)
        {
            const string chars = "0123456789";
            var randomString = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var uniqueCode = $"{randomString}";

            return uniqueCode;
        }
    }
}