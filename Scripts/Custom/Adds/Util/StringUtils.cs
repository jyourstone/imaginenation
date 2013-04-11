using System;
using System.Text;

namespace Server.Util
{
    class StringUtils
    {
        public static string GetString(String value, String defaultValue)
        {
            return value != null && value.Trim().Length > 0 ? value : defaultValue;
        }

        public static bool HasLength(string target)
        {
            return (target != null && target.Length > 0);
        }

        public static bool HasText(string target)
        {
            if (target == null)
            {
                return false;
            }
            else
            {
                return HasLength(target.Trim());
            }
        }
    }
}
