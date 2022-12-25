using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservations : UserControl
    {
        public Reservations()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var createReservationsDialog = new ReservationEditWindow();
            createReservationsDialog.Show();
            createReservationsDialog.Closed += Window_Closed;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int reservationsId = (int)button.Tag;
            var createReservationsDialog = new ReservationEditWindow(reservationsId);
            createReservationsDialog.Show();
            createReservationsDialog.Closed += Window_Closed;
        }
        private void Window_Closed(object? sender, EventArgs e)
        {
            ((ReservationsVM)DataContext).GetAllReservation();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int reservationsId = (int)button.Tag;
            var cancelReservationWindow = new CancelReservationWindow(reservationsId);
            cancelReservationWindow.Show();
            cancelReservationWindow.Closed += Window_Closed;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
