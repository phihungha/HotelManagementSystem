using HotelManagementSoftware.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace HotelManagementSoftware.UI.Validators
{
    public class CancelFeePercentageValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var percent = (value as BindingGroup).Items[0] as ReservationCancelFeePercent;
            if (percent.DayNumberBeforeArrival < 0)
                return new ValidationResult(false, "Day number cannot be smaller than 0");
            if (percent.PercentOfTotal > 100 || percent.PercentOfTotal < 0)
                return new ValidationResult(false, "Invalid percentage value");
            return ValidationResult.ValidResult;
        }
    }
}
