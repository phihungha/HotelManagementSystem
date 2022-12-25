using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for CustomerEditWindow.xaml
    /// </summary>
    public partial class CustomerEditWindow : Window
    {

        public CustomerEditWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CustomerEditWindowVM>();
            ((CustomerEditWindowVM)DataContext).CreateCustomer();
        }

        public CustomerEditWindow(int customerId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CustomerEditWindowVM>();
            ((CustomerEditWindowVM)DataContext).LoadCustomerFromId(customerId);
        }


        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure that you want to delete this customer? This action cannot be undone.",
                "Delete this customer?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await ((CustomerEditWindowVM)DataContext).DeleteCustomer();
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await ((CustomerEditWindowVM)DataContext).SaveCustomer())
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
