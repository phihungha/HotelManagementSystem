using System;
using System.ComponentModel.DataAnnotations;
using HotelManagementSoftware.Data;
using HotelManagementSoftware.Utils;

namespace HotelManagementSoftware.ViewModels.Validators
{
    public sealed class CardNumberCheckAttribute : ValidationAttribute
    {
        private string propertyName;

        public CardNumberCheckAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            object? instance = validationContext.ObjectInstance,
                    otherValue = instance.GetType()?.GetProperty(propertyName)?.GetValue(instance);

            if (otherValue == null)
                throw new ArgumentException("Invalid parameter");

            if ((PaymentMethod)otherValue != PaymentMethod.Cash)
            {
                if (value == null || (string)value == "")
                    return new("Card number cannot be empty");

                if (!int.TryParse((string)value, out _))
                    return new("Card number cannot have non-numeric characters");
            }

            return ValidationResult.Success;
        }
    }
}
