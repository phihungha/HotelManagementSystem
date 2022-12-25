using HotelManagementSoftware.Data;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HotelManagementSoftware.ViewModels.Converters
{
    public class RoomStatusToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RoomStatus roomStatus = (RoomStatus)value;
            switch (roomStatus)
            {
                case RoomStatus.Usable:
                    return "Usable";
                case RoomStatus.Closed:
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
