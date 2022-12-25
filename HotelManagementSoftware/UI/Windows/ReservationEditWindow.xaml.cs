using HotelManagementSoftware.UI.Windows;
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
    /// Interaction logic for CreateReservationWindow.xaml
    /// </summary>
    public partial class ReservationEditWindow : Window
    {
        public ReservationEditWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ReservationEditWindowVM>();
        }
        public ReservationEditWindow(int reservationId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ReservationEditWindowVM>();
            ((ReservationEditWindowVM)DataContext).LoadReservationFromId(reservationId);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void ChooseCustomer_Click(object sender, RoutedEventArgs e)
        {
            var chooseCustomer = new ChooseCustomerWindow();
            chooseCustomer.DialogFinished += ChooseCustomerWindow_DialogFinished;
            chooseCustomer.ShowDialog();

        }
        void ChooseCustomerWindow_DialogFinished(object sender, WindowEventArgs e)
        {
            ((ReservationEditWindowVM)DataContext).LoadCustomerFromId(e.Id);
        }

        void ChooseRoomWindow_DialogFinished(object sender, WindowEventArgs e)
        {
            ((ReservationEditWindowVM)DataContext).LoadRoomFromId(e.Id);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await ((ReservationEditWindowVM)DataContext).Save())
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

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChooseRoom_Click(object sender, RoutedEventArgs e)
        {
            DateTime Arrivaltime = ((ReservationEditWindowVM)DataContext).ArrivalTime;
            DateTime Departuretime = ((ReservationEditWindowVM)DataContext).DepartureTime;
            var chooseRoom = new ChooseRoomWindow(Arrivaltime,Departuretime);
            chooseRoom.DialogFinished += ChooseRoomWindow_DialogFinished;
            chooseRoom.ShowDialog();
        }
    }
}
