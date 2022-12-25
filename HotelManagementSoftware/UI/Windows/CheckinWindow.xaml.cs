using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for CheckinWindow.xaml
    /// </summary>
    public partial class CheckinWindow : Window
    {
        public CheckinWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CheckinWindowVM>();

        }


        public CheckinWindow(int reservationId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CheckinWindowVM>();
            ((CheckinWindowVM)DataContext).LoadReservationFromId(reservationId);
        }
        private async void Checkin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ((CheckinWindowVM)DataContext).CheckIn();
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
