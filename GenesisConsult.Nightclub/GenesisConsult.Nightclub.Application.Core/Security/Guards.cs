using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace GenesisConsult.Nightclub.Application.Core.Security
{
    public static class Guards
    {
        public static void IsNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void IsNotNullOrEmpty(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
                throw new ArgumentNullException(argumentName);
        }

        public static void IsNotZero(int argumentValue, string argumentName)
        {
            if (argumentValue == 0)
                throw new ArgumentException($"Argument '{argumentName}' cannot be zero");
        }

        public static void IsLessThan(int maxValue, int argumentValue, string argumentName)
        {
            if (argumentValue >= maxValue)
                throw new ArgumentException($"Argument '{argumentName}' cannot exceed '{maxValue}'");
        }

        public static void IsMoreThan(int minValue, int argumentValue, string argumentName)
        {
            if (argumentValue <= minValue)
                throw new ArgumentException($"Argument '{argumentName}' cannot be lower than '{minValue}'");
        }

        public static void IsInteger(string value, string argumentName)
        {
            if (!int.TryParse(value, out _))
            {
                throw new ArgumentException($"Argument '{argumentName}' is not a valid integer");
            }
        }

        public static void IsLong(string value, string argumentName)
        {
            if (!long.TryParse(value, out _))
            {
                throw new ArgumentException($"Argument '{argumentName}' is not a valid long");
            }
        }

        public static void IsValidContactDetails(string contactDetails)
        {
            if (!IsEmail(contactDetails) && !IsPhoneNumber(contactDetails))
            {
                throw new ArgumentException($"Argument '{contactDetails}' is not a valid email or phone number. Formats expected:\nEmail: exemple@mail.be\nPhone: +32011223344");
            }
        }

        public static void IsAdult(DateTime birthdate, int age = 18)
        {
            var today = DateTime.Today;
            var memberAge = today.Year - birthdate.Year;
            if (birthdate.Date > today.AddYears(-memberAge)) memberAge--;

            if (memberAge < age)
            {
                throw new ArgumentException($"Member is not adult");
            }
        }

        public static void IsValidNationalNumber(string nationalNumber)
        {
            Regex regex = new Regex(@"^\d{3}\.\d{2}\.\d{2}-\d{3}-\d{2}$");

            if (!regex.Match(nationalNumber).Success)
            {
                throw new ArgumentException($"National number is not valid, format expected: xxx.xx.xx-xxx-xx");
            }
        }

        public static void IsDateLessThan(DateTime startDate, DateTime endDate, string argumentName, string endArgumentName)
        {
            if (DateTime.Compare(startDate, endDate) > 0)
            {
                throw new ArgumentException($"Start date '{argumentName}' is later than {endArgumentName}.");
            }
        }

        private static bool IsEmail(string email)
        {
            try
            {
                _ = new MailAddress(email);
                return true;
            } catch
            {
                return false;
            }
        }

        private static bool IsPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex(@"^([\+]?(?:00)?[0-9]{1,3}[\s.-]?[0-9]{1,12})$");

            return regex.Match(phoneNumber).Success;
        }
    }
}
