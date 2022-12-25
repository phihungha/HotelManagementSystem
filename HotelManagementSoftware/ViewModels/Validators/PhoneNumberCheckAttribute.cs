using System;
using System.ComponentModel.DataAnnotations;
using HotelManagementSoftware.Utils;

namespace HotelManagementSoftware.ViewModels.Validators
{
    public sealed class PhoneNumberCheckAttribute : ValidationAttribute
    {
        private string propertyName;

        public PhoneNumberCheckAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new("Phone number cannot be empty");

            object? instance = validationContext.ObjectInstance,
                    otherValue = instance.GetType()?.GetProperty(propertyName)?.GetValue(instance);

            if (otherValue == null)
                throw new ArgumentException("Invalid parameter");

            string countryCode = (string)otherValue;

            string phoneNumber = (string)value;

            if (phoneNumber == "")
                return new("Phone number cannot be empty");

            if (ValidationUtils.ValidatePhoneNumber(phoneNumber, countryCode))
                return ValidationResult.Success;

            return new("Invalid phone number");
        }
    }
}
