using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BG.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns true if <paramref name="s"/> contains System.Int32
        /// </summary>
        public static bool IsInt32(string s)
        {
            int dummy;
            return Int32.TryParse(s, out dummy);
        }

        /// <summary>
        /// Returns true if <paramref name="s"/> contains System.Double
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDouble(string s)
        {
            double dummy;
            return Double.TryParse(s, out dummy);
        }

        /// <summary>
        /// Extension method that calls String.IsNullorEmpty on <paramref name="s"/>. Returns true if <paramref name="s"/> is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static string ToUpper(this string s, int startIndex, int length)
        {
            return ToUpper(s, startIndex, length, CultureInfo.CurrentCulture);
        }

        public static string ToUpper(this string s, int startIndex)
        {
            return ToUpper(s, startIndex, string.IsNullOrEmpty(s) ? 0 : s.Length);
        }

        public static string ToUpper(this string s, int startIndex, int length, CultureInfo culture)
        {
            Guard.Against<ArgumentNullException>(culture == null, "culture");
            Guard.Against<ArgumentOutOfRangeException>(startIndex <= 0 || startIndex > int.MaxValue, "startIndex");

            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            if (length > s.Length)
            {
                length = s.Length;
            }

            if (s.Length < startIndex + length)
            {
                length = s.Length - startIndex;
            }

            string prefix = s.Substring(0, startIndex);

            string postfix = s.Substring(startIndex + length);

            string upperPart = s.Substring(startIndex, length).ToUpper(culture);

            return prefix + upperPart + postfix;
        }

        public static bool Contains(this string s, string pattern, StringComparison comparison)
        {
            Guard.Against<ArgumentNullException>(s.IsNullOrEmpty(), "s");
            Guard.Against<ArgumentNullException>(pattern.IsNullOrEmpty(), "pattern");

            return s.IndexOf(pattern, comparison) != -1;
        }
    }
}
