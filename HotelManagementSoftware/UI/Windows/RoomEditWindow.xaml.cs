using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for RoomEditWindow.xaml
    /// </summary>
    public partial class RoomEditWindow : Window
    {
        public RoomEditWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<RoomEditWindowVM>();
            ((RoomEditWindowVM)DataContext).CreateRoom();
        }

        public RoomEditWindow(int roomId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<RoomEditWindowVM>();
            ((RoomEditWindowVM)DataContext).LoadRoomFromId(roomId);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure that you want to delete this room? This action cannot be undone.",
                "Delete this room?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await ((RoomEditWindowVM)DataContext).DeleteRoom();
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await ((RoomEditWindowVM)DataContext).SaveRoom())
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

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
