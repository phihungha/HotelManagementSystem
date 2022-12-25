using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using HotelManagementSoftware.ViewModels.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels
{
    public enum SidebarPageName
    {
        Dashboard,
        Reservations,
        Arrivals,
        Departures,
        Customers,
        Rooms,
        RoomTypes,
        Housekeeping,
        Maintenance,
        Employees,
        Settings
    }

    public class LoggedInVM : ObservableObject
    {
        // List of sidebar page options
        public SidebarPageName[] SidebarPages { get; private set; } = {};

        public string CurrentEmployeeName { get; } = "";
        public EmployeeType CurrentEmployeeType { get; }

        // Currently selected page option on the sidebar
        private SidebarPageName currentPage = SidebarPageName.Dashboard;
        public SidebarPageName CurrentPage
        {
            get => currentPage;
            set
            {
                SetProperty(ref currentPage, value);
                NavigateToPage(currentPage);
            }
        }

        // View model of current page
        ObservableObject currentPageVM = App.Current.Services.GetRequiredService<DashboardVM>();
        public ObservableObject CurrentPageVM
        {
            get => currentPageVM;
            set => SetProperty(ref currentPageVM, value);
        }

        public ICommand LogoutCommand { get; }

        public LoggedInVM(EmployeeBusiness employeeBusiness)
        {
            Employee? currentEmployee = employeeBusiness.CurrentEmployee;
            if (currentEmployee != null)
            {
                CurrentEmployeeName = currentEmployee.Name;
                CurrentEmployeeType = currentEmployee.EmployeeType;
            }
            SetupAccessiblePageOptions();
            
            LogoutCommand = new RelayCommand(
                () => MainWindowNavigationUtils.NavigateTo(MainWindowPageName.Login)
            );
        }

        /// <summary>
        /// Setup sidebar page options that this employee can access.
        /// </summary>
        private void SetupAccessiblePageOptions()
        {
            switch (CurrentEmployeeType)
            {
                case EmployeeType.Receptionist:
                    SidebarPages = new SidebarPageName[]
                    {
                        SidebarPageName.Dashboard,
                        SidebarPageName.Reservations,
                        SidebarPageName.Arrivals,
                        SidebarPageName.Departures,
                        SidebarPageName.Customers,
                        SidebarPageName.Rooms,
                        SidebarPageName.RoomTypes,
                        SidebarPageName.Housekeeping,
                        SidebarPageName.Maintenance,
                    };
                    break;
                case EmployeeType.HousekeepingManager:
                    SidebarPages = new SidebarPageName[]
                    {
                        SidebarPageName.Dashboard,
                        SidebarPageName.Rooms,
                        SidebarPageName.RoomTypes,
                        SidebarPageName.Housekeeping,
                    };
                    break;
                case EmployeeType.MaintenanceManager:
                    SidebarPages = new SidebarPageName[]
                    {
                        SidebarPageName.Dashboard,
                        SidebarPageName.Rooms,
                        SidebarPageName.RoomTypes,
                        SidebarPageName.Maintenance,
                    };
                    break;
                case EmployeeType.Manager:
                    SidebarPages = new SidebarPageName[]
                    {
                        SidebarPageName.Dashboard,
                        SidebarPageName.Reservations,
                        SidebarPageName.Arrivals,
                        SidebarPageName.Departures,
                        SidebarPageName.Customers,
                        SidebarPageName.Rooms,
                        SidebarPageName.RoomTypes,
                        SidebarPageName.Housekeeping,
                        SidebarPageName.Maintenance,
                        SidebarPageName.Employees,
                        SidebarPageName.Settings
                    };
                    break;
            }
        }

        private void NavigateToPage(SidebarPageName pageName)
        {
            switch (pageName)
            {
                case SidebarPageName.Dashboard:
                    CurrentPageVM = App.Current.Services.GetRequiredService<DashboardVM>();
                    break;
                case SidebarPageName.Reservations:
                    CurrentPageVM = App.Current.Services.GetRequiredService<ReservationsVM>();
                    break;
                case SidebarPageName.Arrivals:
                    CurrentPageVM = App.Current.Services.GetRequiredService<ArrivalsVM>();
                    break;
                case SidebarPageName.Departures:
                    CurrentPageVM = App.Current.Services.GetRequiredService<DeparturesVM>();
                    break;
                case SidebarPageName.Customers:
                    CurrentPageVM = App.Current.Services.GetRequiredService<CustomersVM>();
                    break;
                case SidebarPageName.Rooms:
                    CurrentPageVM = App.Current.Services.GetRequiredService<RoomsVM>();
                    break;
                case SidebarPageName.RoomTypes:
                    CurrentPageVM = App.Current.Services.GetRequiredService<RoomTypesVM>();
                    break;
                case SidebarPageName.Housekeeping:
                    CurrentPageVM = App.Current.Services.GetRequiredService<HousekeepingVM>();
                    break;
                case SidebarPageName.Maintenance:
                    CurrentPageVM = App.Current.Services.GetRequiredService<MaintenanceVM>();
                    break;
                case SidebarPageName.Employees:
                    CurrentPageVM = App.Current.Services.GetRequiredService<EmployeesVM>();
                    break;
                case SidebarPageName.Settings:
                    CurrentPageVM = App.Current.Services.GetRequiredService<ConfigVM>();
                    break;
            }
        }
    }
}
