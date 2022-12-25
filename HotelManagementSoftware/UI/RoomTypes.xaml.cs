using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for RoomType.xaml
    /// </summary>
    public partial class RoomTypes : UserControl
    {
        public RoomTypes()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int roomTypeId = (int)button.Tag;
            RoomTypeEditWindow window = new RoomTypeEditWindow(roomTypeId);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            RoomTypeEditWindow window = new RoomTypeEditWindow();
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            ((RoomTypesVM)DataContext).LoadAllRoomTypes();
        }
    }
}
