using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;


namespace System
{
    /// <summary>
    /// This Class does simple field validations
    /// </summary>
    public static class Validator
    {

        /// <summary>
        /// Checks if a field is a correct id number format, not the complete correctness
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsID(string value)
        {
            string value2 = value.Trim();
            return IsNumberic(value2) && value2.Length == 13 && !IsDuplicateText(value2);
        }

        /// <summary>
        /// Checks if the given value is a username
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUsername(string value)
        {
            return value.Length > 0 && IsLetter(value[0]) && IsAlphaNumeric(value);
        }

 
        public static bool IsCellNumber(string value)
        {
            string value2 = value.Trim();
            string v = value2.Replace("+", "").Replace(" ", "");
            return IsNumbericPure(v) && v.Length > 9 && v.Length<13 && !IsDuplicateText(v);
        }

        public static bool IsNameOrLastname(string value)
        {
            string value2 = value.Trim();
            string v = value2.Replace(" ", "");
            return IsAlphaNumeric(v) && v.Length<200&& v.Length >= 3 && !IsDuplicateText(v);
        }



        public static bool IsAddress(string value)
        {
            string value2 = value.Trim();
            return value2.Length > 2 && !IsDuplicateText(value2);
        }


        public static bool IsEmail(string strIn)
        {

            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)", DomainMapper, RegexOptions.None);
            }
            catch
            {
                return false;
            }


            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))",
                      RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;

            domainName = idn.GetAscii(domainName);

            return match.Groups[1].Value + domainName;
        }

        public static bool IsNumberic(string value)
        {
            string value2 = value.Trim();

            if (value2.Trim().StartsWith("-"))
            {
                value2 = value2.Trim().Remove(0, 1);
            }

            foreach (char r in value2)
            {
                if (!IsNumber(r))
                {
                    return false;
                }
            }

            return value.Length != 0;
        }

        public static bool IsNumbericPure(string value)
        {
            string value2 = value.Trim();
            foreach (char r in value2)
            {
                if (!IsNumber(r))
                {
                    return false;
                }
            }

            return value.Length != 0;
        }

        public static bool IsNumbericDecimalPure(string value)
        {
            bool dot = false;
            string value2 = value.Trim();

            foreach (char r in value2)
            {
                if (!IsNumber(r))
                {
                    if (r == '.' && dot == false)
                    {
                        dot = true;
                        continue;
                    }
                    return false;
                }
            }

            return !value2.StartsWith(".") && !value2.EndsWith(".") && value.Length != 0; ;
        }

        public static bool IsNumbericDecimal(string value)
        {
            bool dot = false;
            string value2 = value.Trim();

            if (value2.Trim().StartsWith("-"))
            {
                value2 = value2.Trim().Remove(0, 1);
            }

            foreach (char r in value2)
            {
                if (!IsNumber(r))
                {
                    if (r == '.' && dot == false)
                    {
                        dot = true;
                        continue;
                    }
                    return false;
                }
            }

            return !value2.StartsWith(".") && !value2.EndsWith(".") && value.Length != 0; ;
        }

        public static bool IsAlphabetic(string value)
        {
            string value2 = value.Trim();
            foreach (char r in value2)
            {
                if (!IsLetter(r))
                {
                    return false;
                }
            }
            return value.Length != 0;
        }

        public static bool IsAlphabeticSpace(string value)
        {
            string value2 = value.Trim();
            foreach (char r in value2)
            {
                if (!IsLetter(r) && r != ' ')
                {
                    return false;
                }
            }
            return value.Length != 0;
        }

        public static bool IsAlphaNumeric(string value)
        {
            string value2 = value.Trim();
            foreach (char r in value2)
            {
                if (!IsLetter(r) && !IsNumber(r))
                {
                    return false;
                }
            }
            return value.Length != 0;
        }

        public static bool IsAlphaNumericSpace(string value)
        {
            string value2 = value.Trim();
            foreach (char r in value2)
            {
                if (!IsLetter(r) && !IsNumber(r) && r != ' ')
                {
                    return false;
                }
            }
            return value.Length != 0;
        }

        public static bool NotEmpty(string value)
        {
            return value.Trim().Length > 0;
        }

        private static bool IsNumber(char value)
        {
            return value > ('0' - 1) && value < ('9' + 1);
        }

        private static bool IsLetter(char value)
        {
            bool f = (value > ('a' - 1) && value < ('z' + 1)) || (value > ('A' - 1) && value < ('Z' + 1));
            return f;
        }

        public static bool IsMonth(string value)
        {

            string val = value.Trim();
            int len = val.Length;

            if (IsNumberic(val) && (len == 1 || len == 2))
            {
                if (value.Substring(0, 1) == "0")
                {
                    val = value.Substring(1, 1);
                }

                int v = int.Parse(val);
                if (v >= 1 && v <= 12)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsYear(string value)
        {

            string val = value.Trim();
            int len = val.Length;

            if (IsNumberic(val) && (len == 2 || len == 4))
            {

                return true;
            }

            return false;
        }

        public static bool IsDay(string value)
        {

            string val = value.Trim();
            int len = val.Length;

            if (IsNumberic(val) && (len == 1 || len == 2))
            {
                int v = int.Parse(val);
                if (v >= 1 && v <= 31)
                {
                    return true;
                }
            }
            return false;
        }

      

        public static bool IsDuplicateText(string value)
        {

            string a = value.ToLower().Trim();

            if (a.Length > 1)
            {
                string b = "".PadRight(a.Length, a[0]);


                if (b == a)
                {
                    return true;
                }
            }


            return false;
        }

    }
}
