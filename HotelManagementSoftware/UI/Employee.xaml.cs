using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : UserControl
    {
        public Employee()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int customerId = (int)button.Tag;
            EmployeeEditWindow window = new EmployeeEditWindow(customerId);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeEditWindow window = new EmployeeEditWindow();
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            ((EmployeesVM)DataContext).GetAllEmployees();
        }
    }
}
