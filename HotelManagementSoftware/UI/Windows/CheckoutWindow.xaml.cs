using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        public CheckoutWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CheckoutWindowVM>();

        }

        public CheckoutWindow(int reservationId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CheckoutWindowVM>();
            ((CheckoutWindowVM)DataContext).LoadReservationFromId(reservationId);
        }

        private async void Checkout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ((CheckoutWindowVM)DataContext).CheckOut();
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
