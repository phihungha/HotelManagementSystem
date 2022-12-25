using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MaintenanceEditWindow.xaml
    /// </summary>
    public partial class MaintenanceEditWindow : Window
    {
        public MaintenanceEditWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<MaintenanceEditWindowVM>();
        }

        public MaintenanceEditWindow(int RequestId)
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<MaintenanceEditWindowVM>();
            ((MaintenanceEditWindowVM)DataContext).GetCurrentRequestWithId(RequestId); 
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await((MaintenanceEditWindowVM)DataContext).SaveRequest())
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

        private async void CloseRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await((MaintenanceEditWindowVM)DataContext).CloseRequest())
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

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            ((MaintenanceEditWindowVM)DataContext).DeleteItem();
        }
    }
}
