using HotelManagementSoftware.ViewModels.Utils;
using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for ChooseCustomerWindow.xaml
    /// </summary>
    public partial class ChooseCustomerWindow : Window
    {
        public event EventHandler<WindowEventArgs> DialogFinished;

        public ChooseCustomerWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ChooseCustomerWindowVM>();
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int customerId = (int)button.Tag;
            DialogFinished?.Invoke(this, new WindowEventArgs(customerId));
            Close();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerEditWindow window = new CustomerEditWindow();
            window.ShowDialog();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            ((ChooseCustomerWindowVM)DataContext).LoadCustomers();
        }
    }
}