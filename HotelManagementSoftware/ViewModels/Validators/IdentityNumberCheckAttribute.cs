using System;
using System.ComponentModel.DataAnnotations;
using HotelManagementSoftware.Data;
using HotelManagementSoftware.Utils;

namespace HotelManagementSoftware.ViewModels.Validators
{
    public sealed class IdentityNumberCheckAttribute : ValidationAttribute
    {
        private string propertyName;

        public IdentityNumberCheckAttribute(string propertyName)
        {
            this.propertyName = propertyName;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            

            object? instance = validationContext.ObjectInstance,
                    otherValue = instance.GetType()?.GetProperty(propertyName)?.GetValue(instance);

            if (otherValue == null)
                throw new ArgumentException("Invalid parameter");

            IdNumberType idNumberType = (IdNumberType)otherValue;

            if (idNumberType == IdNumberType.Cmnd)
            {
                if (value == null || (string)value == "")
                    return new("Identity number cannot be empty");
                if (!ValidationUtils.ValidateCmnd((string)value))
                    return new("Invalid CMND number");
            }

            if (idNumberType == IdNumberType.Passport 
                && (value == null || (string)value == ""))
                return new("Passport number cannot be empty");

            return ValidationResult.Success;
        }
    }
}
