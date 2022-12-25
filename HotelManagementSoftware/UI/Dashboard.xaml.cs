using HotelManagementSoftware.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void NewReservationBtn_Click(object sender, RoutedEventArgs e)
        {
            var createReservationDialog = new ReservationEditWindow();
            createReservationDialog.Show();
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int reservationId = (int)button.Tag;
            var checkoutWindow = new CheckoutWindow(reservationId);
            checkoutWindow.Show();
            checkoutWindow.Closed += Window_Closed;
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int reservationId = (int)button.Tag;
            var checkInWindow = new CheckinWindow(reservationId);
            checkInWindow.Show();
            checkInWindow.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            ((DashboardVM)DataContext).LoadData();
        }
    }
}
