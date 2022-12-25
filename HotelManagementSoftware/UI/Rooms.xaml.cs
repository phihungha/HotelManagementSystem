using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Room.xaml
    /// </summary>
    public partial class Rooms : UserControl
    {
        public Rooms()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int roomId = (int)button.Tag;
            RoomEditWindow window = new RoomEditWindow(roomId);
            window.Show();
            window.Closed += Window_Closed;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            RoomEditWindow window = new RoomEditWindow();
            window.Show();
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object? sender, EventArgs e)
        {
            ((RoomsVM)DataContext).LoadRooms();
        }
    }
}
