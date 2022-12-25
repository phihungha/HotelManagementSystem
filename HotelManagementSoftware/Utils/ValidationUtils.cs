using PhoneNumbers;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HotelManagementSoftware.Utils
{
    public static class ValidationUtils
    {
        /// <summary>
        /// Get international format phone number from a recognizable phone number.
        /// </summary>
        /// <param name="phoneNumber">Recognizable phone number</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>Canonicalized phone number</returns>
        /// <exception cref="ArgumentException">Invalid phone number</exception>
        public static string GetNormalizedPhoneNumber(string phoneNumber, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumber protoNumber = phoneNumberUtil.Parse(phoneNumber, countryCode);
                return phoneNumberUtil.Format(protoNumber, PhoneNumberFormat.INTERNATIONAL);
            }
            catch (NumberParseException err)
            {
                throw new ArgumentException("Failed to parse phone number: ", err.ToString());
            }
        }

        /// <summary>
        /// Validate a phone number.
        /// </summary>
        /// <param name="phoneNumber">Phone number to validate</param>
        /// <param name="countryCode">Country code</param>
        /// <returns>True if phone number is valid</returns>
        public static bool ValidatePhoneNumber(string phoneNumber, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            PhoneNumber protoNumber = phoneNumberUtil.Parse(phoneNumber, countryCode);
            return phoneNumberUtil.IsValidNumber(protoNumber);
        }

        /// <summary>
        /// Validate CMND number.
        /// </summary>
        /// <param name="cmndNumber">CMND number</param>
        /// <returns></returns>
        public static bool ValidateCmnd(string cmndNumber)
        {
            return cmndNumber.Length == 9 || cmndNumber.Length == 12;
        }

        /// <summary>
        /// Validate email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>True if email is valid</returns>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
