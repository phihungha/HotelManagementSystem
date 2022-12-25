using HotelManagementSoftware.ViewModels.Utils;
using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Shapes;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for ChooseRoomWindow.xaml
    /// </summary>
    public partial class ChooseRoomWindow : Window
    {
        public event EventHandler<WindowEventArgs> DialogFinished;
        public ChooseRoomWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ChooseRoomWindowVM>();
        }
        public ChooseRoomWindow(DateTime Arrivaltime, DateTime Departuretime)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ChooseRoomWindowVM>();
            ((ChooseRoomWindowVM)DataContext).LoadRooms(Arrivaltime, Departuretime);

        }
        private void ChooseRoomTypeButton_Click(object sender, RoutedEventArgs e)
        {
            var chooseRoomType = new ChooseRoomTypeWindow();
            chooseRoomType.DialogFinished += ChooseRoomTypeWindow_DialogFinished;
            chooseRoomType.ShowDialog();
            
        }
        void ChooseRoomTypeWindow_DialogFinished(object sender, WindowEventArgs e)
        {
            ((ChooseRoomWindowVM)DataContext).GetUsableRoom(e.Id);
        }
        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int RoomId = (int)button.Tag;
            DialogFinished?.Invoke(this, new WindowEventArgs(RoomId));
            this.Close();
        }
    }
}
