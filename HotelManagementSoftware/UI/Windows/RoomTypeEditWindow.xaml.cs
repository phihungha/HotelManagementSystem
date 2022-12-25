using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for RoomTypeEditWindow.xaml
    /// </summary>
    public partial class RoomTypeEditWindow : Window
    {
        public RoomTypeEditWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<RoomTypeEditWindowVM>();
        }

        public RoomTypeEditWindow(int roomTypeId)
            : this()
        {
            ((RoomTypeEditWindowVM)DataContext).LoadRoomTypeFromId(roomTypeId);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure that you want to delete this room type? This action cannot be undone.",
                "Delete this room type?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                await ((RoomTypeEditWindowVM)DataContext).DeleteRoomType();
                Close();
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await ((RoomTypeEditWindowVM)DataContext).SaveRoomType())
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
