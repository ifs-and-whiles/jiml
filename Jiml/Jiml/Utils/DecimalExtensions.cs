using System;

namespace Jiml.Utils
{
    public static class DecimalExtensions
    {
        public static bool IsInteger(this decimal decValue, out int intValue)
        {
            if (Math.Abs(decValue % 1) == 0)
            {
                intValue = (int) decValue;
                return true;
            }

            intValue = 0;
            return false;
        }
    }
}