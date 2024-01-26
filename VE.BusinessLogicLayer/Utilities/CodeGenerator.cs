using System;
using System.Linq;
namespace VE.BusinessLogicLayer.Utilities
{
    public class CodeGenerator
    {
        private static readonly Random Random = new Random();

        public static string GenerateRandomCode(int length = 5)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
}
