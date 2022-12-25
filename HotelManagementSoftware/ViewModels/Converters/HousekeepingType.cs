using HotelManagementSoftware.Data;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HotelManagementSoftware.ViewModels.Converters
{
    /// <summary>
    /// Convert a HousekeepingType enum values to string for displaying.
    /// </summary>
    public class HousekeepingStatusToString : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            HousekeepingRequestStatus housekeepingStatus = (HousekeepingRequestStatus)value;
            switch (housekeepingStatus)
            {
                case HousekeepingRequestStatus.Opened:
                    return "Opened";
                case HousekeepingRequestStatus.Closed:
                    return "Closed";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
