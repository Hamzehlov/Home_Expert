using System;
using System.Globalization;

namespace Home_Expert.Helpers
{
    public static class StringExtensions
    {
        public static (string firstPart, string restPart) SplitFirstThree(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return ("", "");

            if (input.Length <= 3)
                return (input, "");

            return (input.Substring(0, 3), input.Substring(3));
        }
    }

}
