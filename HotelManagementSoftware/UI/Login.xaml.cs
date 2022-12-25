using HotelManagementSoftware.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagementSoftware.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginVM)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
