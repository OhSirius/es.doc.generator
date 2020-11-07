using System;

namespace BG.Infrastructure.Common
{
    #region class FastString - Быстрая работа со строками - 10.05.2007
    public class FastString
    {
        #region StartWith x5-20
        /// <summary>
        /// StartWith
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static bool StartWith(string str_in, string str_find)
        {
            int len_find = str_find.Length;
            int len_in = str_in.Length;
            if (len_in == 0 || len_find == 0 || len_in < len_find)
                return false;

            int i = 0;

            while (i != len_find)
            {
                if (str_in[i] != str_find[i])
                    return false;
                i++;
            }
            return true;
        }
        #endregion

        #region StartWithCaseUnsensitive
        /// <summary>
        /// StartWith
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static bool StartWithCaseUnsensitive(string str_in, string str_find)
        {
            int len_in = str_in.Length;
            int len_find = str_find.Length;
            if (len_in == 0 || len_find == 0 || len_in < len_find)
                return false;

            int i = 0;
            char c1, c2;

        while_begin:

            c1 = str_in[i];
            c2 = str_find[i];

            if (c1 == c2)
                goto while_next;

            if (c1 >= 'a' && c1 <= 'z')
                c1 = (char)(c1 - 32);
            else
                if (c1 > 255)
                    c1 = char.ToUpper(c1);

            if (c2 >= 'a' && c2 <= 'z')
                c2 = (char)(c2 - 32);
            else
                if (c2 > 255)
                    c2 = char.ToUpper(c2);

            if (c1 != c2)
                return false;

        while_next:
            i++;
            if (i >= len_find)
                return true;

            goto while_begin;
        }
        #endregion

        #region StartWithIgnoreSpace
        /// <summary>
        /// StartWithIgnoreSpace
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static bool StartWithIgnoreSpace(string str_in, string str_find)
        {
            int len_in = str_in.Length;
            int len_find = str_find.Length;
            if (len_in == 0 || len_find == 0 || len_in < len_find)
                return false;

            int i = 0;
            int i1 = 0;
            len_in -= len_find;
            while (i < len_in)
            {
                if (str_in[i] != ' ' && str_in[i] != '\t')
                    break;
                i++;
            }

            while (i1 < len_find)
            {
                if (str_in[i] != str_find[i1])
                    return false;
                i++;
                i1++;
            }
            return true;
        }
        #endregion

        //

        #region ContainsOf(string str_in, string str_find) x3-10
        /// <summary>
        /// ContainsOf
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static bool ContainsOf(string str_in, string str_find)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;
            if (len_find == 0)
                return false;
            if (len_in == 0)
                return false;
            int len_find_len_in = len_in - len_find;

            int fi = 0;
            int i = 0;
            char c;

            c = str_find[0];

            fi--;

        while_begin:
            fi++;
            do
            {
                if (str_in[fi] == c)
                    goto next_1;
                fi++;
            }
            while (fi != len_in);

            return false;

        next_1:
            if (fi < 0 || fi > len_find_len_in)
                return false;

            i = 1;
            while (i != len_find)
            {
                if (str_in[fi + i] != str_find[i])
                    goto while_begin;
                i++;
            }

            return true;
        }
        #endregion

        #region ContainsOfCaseUnsensitive(string str_in, string str_find) x4-10
        /// <summary>
        /// ContainsOfCaseUnsensitive
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static bool ContainsOfCaseUnsensitive(string str_in, string str_find)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;
            if (len_in == 0 || len_find == 0 || len_in < len_find)
                return false;

            int fi = 0;
            int i = 0;
            char c;
            char c1;

            c = ToUpper(str_find[0]);
            c1 = str_find[0];

            fi--;

        while_begin:
            fi++;
            do
            {
                if (str_in[fi] == c1)
                    goto next_1;
                if (ToUpper(str_in[fi]) == c)
                    goto next_1;
                fi++;
            }
            while (fi < len_in);

            return false;

        next_1:
            if (fi < 0 || fi + len_find > len_in)
                return false;

            i = 1;
            while (i < len_find)
            {
                if (ToUpper(str_in[fi + i]) != ToUpper(str_find[i]))
                    goto while_begin;
                i++;
            }

            return true;
        }
        #endregion

        #region IndexOf(string str_in, string str_find) x3-10
        /// <summary>
        /// IndexOf
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static int IndexOf(string str_in, string str_find)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;
            if (len_find == 0)
                return 0;
            if (len_in == 0)
                return -1;
            int len_find_len_in = len_in - len_find;

            int fi = 0;
            int i = 0;
            char c;

            c = str_find[0];

            fi--;

        while_begin:
            fi++;
            do
            {
                if (str_in[fi] == c)
                    goto next_1;
                fi++;
            }
            while (fi != len_in);

            return -1;

        next_1:
            //if(fi<0 || fi+len_find>len_in)
            if (fi < 0 || fi > len_find_len_in)
                return -1;

            i = 1;
            while (i != len_find)
            {
                if (str_in[fi + i] != str_find[i])
                    goto while_begin;
                i++;
            }

            return fi;
        }
        #endregion

        #region IndexOf(string str_in, string str_find, int begin) x3-8
        /// <summary>
        /// IndexOf
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <param name="begin"></param>
        /// <returns></returns>
        public static int IndexOf(string str_in, string str_find, int begin)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;

            if (len_in == 0
                || len_find == 0
                || len_in < len_find
                || begin >= len_in
                )
                return -1;

            int fi = 0;
            int i = 0;
            char c;

            c = str_find[0];

            fi--;
            fi += begin;
        while_begin:

            fi++;

            do
            {
                if (str_in[fi] == c)
                    goto next_1;
                fi++;
            }
            while (fi != len_in);

            return -1;

        next_1:
            if (fi < 0 || fi + len_find > len_in)
                return -1;

            i = 1;
            while (i != len_find)
            {
                if (str_in[fi + i] != str_find[i])
                    goto while_begin;
                i++;
            }

            return fi;
        }
        #endregion

        #region IndexOf(string str_in, string str_find, int begin, int len) x8-10
        /// <summary>
        /// IndexOf
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <param name="begin"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int IndexOf(string str_in, string str_find, int begin, int len)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;
            if (len_in == 0
                || len_find == 0
                || len_in < len_find
                || begin + len >= len_in
                )
                return -1;

            len_in = begin + len;

            int fi = 0;
            int i = 0;
            char c;

            c = str_find[0];

            fi--;
            fi += begin;
        while_begin:

            fi++;

            do
            {
                if (str_in[fi] == c)
                    goto next_1;
                fi++;
            }
            while (fi < len_in);

            return -1;

        next_1:
            if (fi < 0 || fi + len_find > len_in)
                return -1;

            i = 1;
            while (i < len_find)
            {
                if (str_in[fi + i] != str_find[i])
                    goto while_begin;
                i++;
            }

            return fi;
        }
        #endregion

        #region IndexOfCaseUnsensitive(string str_in, string str_find) x4-10
        /// <summary>
        /// IndexOf
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static int IndexOfCaseUnsensitive(string str_in, string str_find)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;
            if (len_in == 0 || len_find == 0 || len_in < len_find)
                return -1;

            int fi = 0;
            int i = 0;
            char c;
            char c1;

            c = ToUpper(str_find[0]);
            c1 = str_find[0];

            fi--;

        while_begin:
            fi++;
            do
            {
                if (str_in[fi] == c1)
                    goto next_1;
                if (ToUpper(str_in[fi]) == c)
                    goto next_1;
                fi++;
            }
            while (fi < len_in);

            return -1;

        next_1:
            if (fi < 0 || fi + len_find > len_in)
                return -1;

            i = 1;
            while (i < len_find)
            {
                if (ToUpper(str_in[fi + i]) != ToUpper(str_find[i]))
                    goto while_begin;
                i++;
            }

            return fi;
        }
        #endregion

        //

        #region LastIndexOf(string str_in, string str_find) x2-10
        /// <summary>
        /// LastIndexOf
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static int LastIndexOf(string str_in, string str_find)
        {
            int len_in;
            int len_find;
            len_in = str_in.Length;
            len_find = str_find.Length;

            if (len_in == 0 || len_in < len_find)
            {
                if (len_find == 0)
                    return 0;
                return -1;
            }


            if (len_find == 0)
                return len_in - 1;


            int fi = 0;
            int i = 0;
            char c;
            int r = -1;
            c = str_find[0];

            fi--;

        while_begin:
            fi++;
            do
            {
                if (str_in[fi] == c)
                    goto next_1;
                fi++;
            }
            while (fi != len_in);

            return r;

        next_1:
            if (fi < 0 || fi + len_find > len_in)
                //if(fi<0 || fi>len_find_len_in)
                return r;

            i = 1;
            while (i != len_find)
            {
                if (str_in[fi + i] != str_find[i])
                    goto while_begin;
                i++;
            }
            r = fi;
            goto while_begin;

            //return fi;
        }
        #endregion

        //

     

        #region ReplaceIfSeparated(string str_in, char c_from, char c_to)
        /// <summary>
        /// StrReplace
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="c"></param>
        /// <param name="beg"></param>
        /// <param name="len"></param>
        public static string ReplaceIfSeparated(string str_in, string str_old, string str_new, string sep_chars)
        {
            if (str_in == null || str_in.Length == 0 || str_old == null || str_old.Length == 0)
                return str_in;

            if (str_new == null)
                str_new = "";

            int index = 0;
            int index1 = 0;

            while (index >= 0)
            {
                index1 = FastString.IndexOf(str_in, str_old, index);
                if (index1 < 0)
                    break;
                index = index1 + 1;

                if (index1 > 0)
                {
                    if (sep_chars != null)
                    {
                        bool fl = false;
                        for (int i = 0; i < sep_chars.Length; i++)
                            if (str_in[index1 - 1] == sep_chars[i])
                            {
                                fl = true;
                                break;
                            }
                        if (fl == false)
                            continue;
                    }
                }
                if (index1 + str_old.Length != str_in.Length)
                {
                    if (sep_chars != null)
                    {
                        bool fl = false;
                        for (int i = 0; i < sep_chars.Length; i++)
                            if (str_in[index1 + str_old.Length] == sep_chars[i])
                            {
                                fl = true;
                                break;
                            }
                        if (fl == false)
                            continue;
                    }
                }
                str_in = str_in.Remove(index1, str_old.Length);
                str_in = str_in.Insert(index1, str_new);

            }
            return str_in;
        }
        #endregion

        //
        #region FindSubstring
        public static int FindSubstring(string input, string pattern)
        {
            Guard.Against<ArgumentException>(string.IsNullOrEmpty(input), "Input string mustn't be empty");
            Guard.Against<ArgumentException>(string.IsNullOrEmpty(pattern), "Pattern string mustn't be empty");

            int n = input.Length;
            int m = pattern.Length;
            if (0 == n) throw new ArgumentException("String mustn't be empty", "input");
            if (0 == m) throw new ArgumentException("String mustn't be empty", "pattern");

            int[] d = GetPrefixFunction(pattern);

            int i, j;
            for (i = 0, j = 0; (i < n) && (j < m); i++, j++)
                while ((j >= 0) && (pattern[j] != input[i]))
                    j = d[j];

            if (j == m)
                return i - j;
            else
                return -1;
        }
        #endregion

        #region GetPrefixFunction
        private static int[] GetPrefixFunction(string pattern)
        {
            int length = pattern.Length;
            int[] result = new int[length];

            int i = 0;
            int j = -1;
            result[0] = -1;
            while (i < length - 1)
            {
                while ((j >= 0) && (pattern[j] != pattern[i]))
                    j = result[j];
                i++;
                j++;
                if (pattern[i] == pattern[j])
                    result[i] = result[j];
                else
                    result[i] = j;
            }
            return result;
        }
        #endregion

        //

        #region IsEqualCaseUnsensitive
        /// <summary>
        /// IsEqualCaseUnsensitive
        /// </summary>
        /// <param name="str_in"></param>
        /// <param name="str_find"></param>
        /// <returns></returns>
        public static bool IsEqualCaseUnsensitive(string str_in, string str_find)
        {
            int len_in = str_in.Length;
            int len_find = str_find.Length;
            if (len_in != len_find || len_in == 0)
                return false;

            int i = 0;
            char c1, c2;

        while_begin:

            c1 = str_in[i];
            c2 = str_find[i];

            if (c1 == c2)
                goto while_next;

            if (c1 >= 'a' && c1 <= 'z')
                c1 = (char)(c1 - 32);
            else
                if (c1 > 255)
                    c1 = char.ToUpper(c1);

            if (c2 >= 'a' && c2 <= 'z')
                c2 = (char)(c2 - 32);
            else
                if (c2 > 255)
                    c2 = char.ToUpper(c2);

            if (c1 != c2)
                return false;

        while_next:
            i++;
            if (i >= len_in)
                return true;

            goto while_begin;
        }
        #endregion

        #region IsEmpty
        /// <summary>
        /// IsEmpty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {
            if (str == null || str.Length == 0)
                return true;
            return false;
        }
        #endregion

        #region IsEqualAccounts
        /// <summary>
        /// IsEqualAccounts
        /// </summary>
        /// <param name="account1"></param>
        /// <param name="account2"></param>
        /// <returns></returns>
        public static bool IsEqualAccounts(string account1, string account2)
        {
            if (account1 == null
                || account1.Length == 0
                || account2 == null
                || account2.Length == 0
                )
                return false;

            if (account1 == account2)
                return true;

            account1 = account1.ToLower();
            account2 = account2.ToLower();

            if (account1 == account2)
                return true;
            int index1;

            index1 = account1.IndexOf('@');
            if (index1 > 0)
            {
                int index2 = account1.IndexOf('.', index1);
                if (index2 >= 0)
                {
                    account1 = account1.Substring(0, index2);
                }
                account1 = (account1.Substring(index1 + 1) + @"\" + account1.Substring(0, index1));
            }

            index1 = account2.IndexOf('@');
            if (index1 > 0)
            {
                int index2 = account2.IndexOf('.', index1);
                if (index2 >= 0)
                {
                    account2 = account2.Substring(0, index2);
                }
                account2 = (account2.Substring(index1 + 1) + @"\" + account2.Substring(0, index1));
            }

            if (account1 == account2)
                return true;

            return false;
        }
        #endregion

        //

        #region ToUpper x1-2
        //System.Globalization.CultureInfo culture=System.Globalization.CultureInfo.CurrentCulture;
        static System.Globalization.TextInfo textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        /// <summary>
        /// ToUpper
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToUpper(string s)
        {
            int len = s.Length;
            char[] cl = new char[len];
            int i = 0;
            char c;
            while (i != len)
            {
                c = s[i];
                if (c >= 'a' && c <= 'z')
                {
                    cl[i] = (char)(c - 32);
                    i++;
                    continue;
                }
                //else
                if (c < 256)
                {
                    cl[i] = c;
                    i++;
                    continue;
                }
                //else
                cl[i] = textInfo.ToUpper(c);//char.ToUpper(c);
                //cl[i]=char.ToUpper(c);
                i++;
            }
            return new string(cl);
        }
        #endregion

        #region ToLower x1-2
        /// <summary>
        /// ToUpper
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToLower(string s)
        {
            int i = s.Length;
            char[] cl = new char[i];
            char c;
            while (i != 0)
            {
                i--;
                c = s[i];
                if (c >= 'A' && c <= 'Z')
                {
                    cl[i] = (char)(c + 32);
                    continue;
                }
                if (c < 256)
                {

                    cl[i] = c;
                    continue;
                }

                cl[i] = textInfo.ToLower(c);
            }
            return new string(cl);
        }
        #endregion

        //

        #region >ToUpper
        /// <summary>
        /// ToUpper
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static char ToUpper(char c)
        {
            if (c >= 'a' && c <= 'z')
                return (char)(c - 32);
            if (c < 256)
                return c;

            return textInfo.ToUpper(c);
        }
        #endregion

        #region GetStringHash
        //public static unsafe uint GetStringHash(string s)
        public static UInt64 GetStringHash(string s)
        {
            return GetStringFastHash(s);
            /* !!!!!!!!!!!!!!!!!
            unsafe
            {
                fixed (char* str = s)
                {
                    char* chPtr = str;
                    int num = 0x15051505;
                    int num2 = num;
                    int* numPtr = (int*)chPtr;
                    for (int i = s.Length; i > 0; i -= 4)
                    {
                        num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                        if (i <= 2)
                        {
                            break;
                        }
                        num2 = (((num2 << 5) + num2) + (num2 >> 0x1b)) ^ numPtr[1];
                        numPtr += 2;
                    }
                    return (uint)(num + (num2 * 0x5d588b65));
                }
            }
            */
        }
        #endregion


        #region GetStringFastHash
        /// <summary>
        /// GetStringHashLong
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public UInt64 GetStringFastHash(string str)
        {
            // !!!!!!!!!!!!!!!!!!!!
            return GetStringHashLong(str);
            /*
            UInt64 hash = 11;
            char[] arr = str.ToCharArray();
            unchecked
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    UInt64 ch = (UInt64)arr[i];
                    hash += ch;
                    //hash = hash ^ ch;
                    hash *= 37;
                }
            }
            return hash % 1000000000;*/
        }
        #endregion

        #region GetStringHashLong
        /// <summary>
        /// GetStringHashLong
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public UInt64 GetStringHashLong(string str)
        {
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(str));
            
            return BitConverter.ToUInt64(result, 0);
        }
        #endregion

        #region GetStringHashAsString
        /// <summary>
        /// GetStringHash
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string GetStringHashAsString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            System.Text.Encoding encoding = System.Text.Encoding.Unicode;
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(encoding.GetBytes(str));

            return System.Convert.ToBase64String(result);
        }
        #endregion

        #region ToDouble
        public static double ToDouble(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            try
            {
                s = s.Trim();
                //if (s.Contains(" "))
                //    s = s.Substring(0, s.IndexOf(' '));
                s = s.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("+", "").Replace("-", "");

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != '0' && s[i] != '1' && s[i] != '2' && s[i] != '3' && s[i] != '4' && s[i] != '5'
                        && s[i] != '6' && s[i] != '7' && s[i] != '8' && s[i] != '9')
                    {
                    }
                    else
                    {
                        s = s.Substring(i, s.Length - i);
                        break;
                    }
                }


                bool is_first_dot = true;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != '0' && s[i] != '1' && s[i] != '2' && s[i] != '3' && s[i] != '4' && s[i] != '5'
                        && s[i] != '6' && s[i] != '7' && s[i] != '8' && s[i] != '9')
                    {
                        if ((s[i] != ',' && s[i] != '.') || is_first_dot == false)
                        {
                            s = s.Substring(0, i);
                            break;
                        }
                        else
                            is_first_dot = false;

                    }
                }

                string num = s;
                num = num.Replace(".", ",").Replace(",", System.Globalization.CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

                return (double)Convert.ChangeType(num, typeof(double));
            }
            catch
            {
                try
                {
                    string num = s.Replace(".", ",");

                    return (double)Convert.ChangeType(num, typeof(double));
                }
                catch
                {
                    try
                    {
                        string num = s.Replace(",", ".");

                        return (double)Convert.ChangeType(num, typeof(double));
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
        }
        #endregion

        #region ToDecimal
        public static decimal ToDecimal(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;
            try
            {
                s = s.Trim();
                //if (s.Contains(" "))
                //    s = s.Substring(0, s.IndexOf(' '));
                s = s.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("+", "").Replace("-", "");

                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != '0' && s[i] != '1' && s[i] != '2' && s[i] != '3' && s[i] != '4' && s[i] != '5'
                        && s[i] != '6' && s[i] != '7' && s[i] != '8' && s[i] != '9')
                    {
                    }
                    else
                    {
                        s = s.Substring(i, s.Length - i);
                        break;
                    }
                }


                bool is_first_dot = true;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] != '0' && s[i] != '1' && s[i] != '2' && s[i] != '3' && s[i] != '4' && s[i] != '5'
                        && s[i] != '6' && s[i] != '7' && s[i] != '8' && s[i] != '9')
                    {
                        if ((s[i] != ',' && s[i] != '.') || is_first_dot == false)
                        {
                            s = s.Substring(0, i);
                            break;
                        }
                        else
                            is_first_dot = false;

                    }
                }

                string num = s;
                num = num.Replace(".", ",").Replace(",", System.Globalization.CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);

                return (decimal)Convert.ChangeType(num, typeof(decimal));
            }
            catch
            {
                try
                {
                    string num = s.Replace(".", ",");

                    return (decimal)Convert.ChangeType(num, typeof(decimal));
                }
                catch
                {
                    try
                    {
                        string num = s.Replace(",", ".");

                        return (decimal)Convert.ChangeType(num, typeof(decimal));
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
        }
        #endregion
    }
    #endregion
}




