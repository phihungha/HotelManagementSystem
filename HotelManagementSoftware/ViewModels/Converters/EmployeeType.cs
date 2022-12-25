using HotelManagementSoftware.Data;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HotelManagementSoftware.ViewModels.Converters
{
    /// <summary>
    /// Convert a EmployeeType enum values to string for displaying.
    /// </summary>
    public class EmployeeTypeToString : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EmployeeType employeeType = (EmployeeType)value;
            switch (employeeType)
            {
                case EmployeeType.Receptionist:
                    return "Receptionist";
                case EmployeeType.HousekeepingManager:
                    return "Housekeeping manager";
                case EmployeeType.MaintenanceManager:
                    return "Maintenance manager";
                case EmployeeType.Manager:
                    return "Manager";
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
