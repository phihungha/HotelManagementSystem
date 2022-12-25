using System;
using System.Globalization;
using System.Windows.Data;

namespace HotelManagementSoftware.ViewModels.Converters
{
    public class TimeSpanToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var remainingTime = (TimeSpan)value;
            return $"{remainingTime.Hours:00} hour(s), {remainingTime.Minutes:00} minute(s)";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
