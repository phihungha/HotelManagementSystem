using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for CancelReservationWindow.xaml
    /// </summary>
    public partial class CancelReservationWindow : Window
    {
        public CancelReservationWindow(int reservationId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CancelReservationWindowVM>();
            ((CancelReservationWindowVM)DataContext).LoadReservationFromId(reservationId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                await ((CancelReservationWindowVM)DataContext).Cancel();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            this.Close();
        }
    }
}
