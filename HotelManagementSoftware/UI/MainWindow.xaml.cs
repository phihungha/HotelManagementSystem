using HotelManagementSoftware.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace HotelManagementSoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<MainWindowVM>();
        }
    }
}
