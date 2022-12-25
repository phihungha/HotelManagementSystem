using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Arrivals.xaml
    /// </summary>
    public partial class Arrivals : UserControl
    {
        public Arrivals()
        {
            InitializeComponent();
        }

        private void CheckinButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int reservationsId = (int)button.Tag;
            CheckinWindow window = new CheckinWindow(reservationsId);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            ((ArrivalsVM)DataContext).LoadArrivals();
        }
    }
}
