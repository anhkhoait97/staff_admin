using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VnvcStaffAdmin.Application.Helpers
{
    public static class StringHelper
    {
        public static string FormatPhoneNumber(string phoneNumber)
        {
            try
            {
                PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                PhoneNumber phoneParsed = phoneUtil.Parse(phoneNumber, "VN");

                return phoneParsed.CountryCode == 84 ? $"0{phoneParsed.NationalNumber}" : phoneNumber;
            }
            catch (Exception ex)
            {
                return phoneNumber;
            }
        }
    }
}
