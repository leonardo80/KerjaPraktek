using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP
{
    class Function
    {
        public static string separator(string price)
        {
            double result = Convert.ToDouble(price);
            CultureInfo culture = CultureInfo.CreateSpecificCulture("de-DE");
            return result.ToString("N0", culture);
        }
    }
}
