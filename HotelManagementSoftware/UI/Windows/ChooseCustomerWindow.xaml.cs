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

namespace HotelManagementSoftware.UI.Windows
{
    /// <summary>
    /// Interaction logic for ChooseCustomerWindow.xaml
    /// </summary>
    public partial class ChooseCustomerWindow : Window
    {
        public event EventHandler<WindowEventArgs> DialogFinished;
        public ChooseCustomerWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<ChooseCustomerWindowVM>();
        }
        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int customerId = (int)button.Tag;
            DialogFinished?.Invoke(this, new WindowEventArgs(customerId));
            this.Close();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerEditWindow window = new CustomerEditWindow();
            window.ShowDialog();
        }

    }
}
