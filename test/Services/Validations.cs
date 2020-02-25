using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdFundingTest.Services
{
    public class Validations
    {
        public static bool EmailIsValid(string email)
        {
            return email.Contains("@") ? true : false;
        }
        public static bool VatNumberIsValid(string vatnumber)
        {
            foreach (char x in vatnumber)
                if (char.IsLetter(x))
                {
                    return false;
                }
            if (vatnumber.Length != 9)
            {
                return false;
            }
            return true;
        }
    }
}
