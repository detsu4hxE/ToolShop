using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;

namespace TestClass
{
    public class TestClass
    {
        public static bool authorization(string login, string password)
        {
            if (login == "" || password == "")
            {
                return false;
            }
            return true;
        }
        public static int intNumber(string textNumber)
        {
            Regex intNumber = new Regex(@"^\d+$");
            MatchCollection matches;
            matches = intNumber.Matches(textNumber);
            if (matches.Count > 0)
            {
                return int.Parse(textNumber);
            }
            return -1;
        }
        public static double doubleNumber(string textNumber)
        {
            Regex doubleNumber = new Regex(@"^\d+(,\d+)?$");
            MatchCollection matches;
            matches = doubleNumber.Matches(textNumber);
            if (matches.Count > 0)
            {
                return double.Parse(textNumber);
            }
            return -1.0;
        }

    }
}
