using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeEditWindow.xaml
    /// </summary>
    public partial class EmployeeEditWindow : Window
    {
        public EmployeeEditWindow()
        {
            InitializeComponent();
            DataContext= App.Current.Services.GetRequiredService<EmployeeEditWindowVM>(); 
        }

        public EmployeeEditWindow(int employeeId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<EmployeeEditWindowVM>();
            ((EmployeeEditWindowVM)DataContext).LoadEmployeeFromId(employeeId);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure that you want to delete this employee? This action cannot be undone.",
                "Delete this employee?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await ((EmployeeEditWindowVM)DataContext).DeleteEmployee();
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await ((EmployeeEditWindowVM)DataContext).SaveEmployee())
                    Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
