using HotelManagementSoftware.ViewModels.Utils;
using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for ChooseRoomTypeWindow.xaml
    /// </summary>
    public partial class ChooseRoomTypeWindow : Window
    {
        public event EventHandler<WindowEventArgs> DialogFinished;
        public ChooseRoomTypeWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ChooseRoomTypeWindowVM>();
        }
        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int RoomTypeId = (int)button.Tag;
            DialogFinished?.Invoke(this, new WindowEventArgs(RoomTypeId));
            this.Close();
        }
    }
}
