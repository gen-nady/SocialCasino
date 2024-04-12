using System.Globalization;
using System.Text.RegularExpressions;

namespace _Project.Scripts.Helpers
{
    public class ParseConverter
    {
        public static decimal DecimalParse(string decString)
        {
            if (string.IsNullOrEmpty(decString))
                return 0;
            try
            {
                var culture = Regex.IsMatch(decString, @"^(:?[\d,]+\.)*\d+$")
                    ? new CultureInfo("en-US")
                    : new CultureInfo("de-DE");
                return decimal.Parse(decString, culture);
            }
            catch
            {
                return decimal.Parse(decString, new CultureInfo("en-US"));
            }
        }
    }
}