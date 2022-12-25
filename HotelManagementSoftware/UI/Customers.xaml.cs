using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : UserControl
    {
        public Customers()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int customerId = (int)button.Tag;
            CustomerEditWindow window = new CustomerEditWindow(customerId);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerEditWindow window = new CustomerEditWindow();
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
           ((CustomersVM)DataContext).LoadCustomers();
        }
    }
}
