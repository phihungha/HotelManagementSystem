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
    class ReservationStatusToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReservationStatus reservationStatus = (ReservationStatus)value;
            switch (reservationStatus)
            {
                case ReservationStatus.Cancelled:
                    return "Cancelled";
                case ReservationStatus.CheckedIn:
                    return "CheckedIn";
                case ReservationStatus.CheckedOut:
                    return "CheckedOut";
                case ReservationStatus.Reserved:
                    return "Reserved";
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
