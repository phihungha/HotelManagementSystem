using HotelManagementSoftware.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using HotelManagementSoftware.UI.Windows;


namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Maintenance.xaml
    /// </summary>
    public partial class Maintenance : UserControl
    {
        public Maintenance()
        {
            InitializeComponent();
        }

        private void AddMaintenanceRequest_Click(object sender, RoutedEventArgs e)
        {
            MaintenanceEditWindow window = new MaintenanceEditWindow();
            window.Show();
            window.Closed += Window_Closed;
        }
        private void Window_Closed(object? sender, EventArgs e)
        {
            ((MaintenanceVM)DataContext).GetAllItem();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int id = (int)button.Tag;

            MaintenanceEditWindow window = new MaintenanceEditWindow(id);
            window.Show();
            window.Closed += Window_Closed;

        }
    }
}
