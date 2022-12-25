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

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for CancelReservationWindow.xaml
    /// </summary>
    public partial class CancelReservationWindow : Window
    {
        public CancelReservationWindow(int reservationId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<CancelReservationWindowVM>();
            ((CancelReservationWindowVM)DataContext).LoadReservationFromId(reservationId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                await ((CancelReservationWindowVM)DataContext).Cancel();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }                
                this.Close();
        }
    }
}
