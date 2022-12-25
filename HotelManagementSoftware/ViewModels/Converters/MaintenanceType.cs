using HotelManagementSoftware.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HotelManagementSoftware.ViewModels.Converters
{
    /// <summary>
    /// Convert a MaintenanceStatus enum values to string for displaying.
    /// </summary>
    public class MaintenanceStatusToString : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MaintenanceRequestStatus housekeepingStatus = (MaintenanceRequestStatus)value;
            switch (housekeepingStatus)
            {
                case MaintenanceRequestStatus.Opened:
                    return "Opened";
                case MaintenanceRequestStatus.Closed:
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
