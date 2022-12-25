using HotelManagementSoftware.Business;
using HotelManagementSoftware.ViewModels.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace HotelManagementSoftware.ViewModels
{
    public class MainWindowVM : ObservableObject
    {
        ObservableObject currentPageVM;
        public ObservableObject CurrentPageVM
        {
            get => currentPageVM;
            set => SetProperty(ref currentPageVM, value);
        }

        public MainWindowVM()
        {
            WeakReferenceMessenger.Default.Register<MainWindowNavigation>(
                this, (recipient, message) => NavigateTo(message.Value));
            NavigateTo(MainWindowPageName.Login);
        }

        public void NavigateTo(MainWindowPageName ui)
        {
            switch (ui)
            {
                case MainWindowPageName.Login:
                    CurrentPageVM = App.Current.Services.GetRequiredService<LoginVM>();
                    break;
                case MainWindowPageName.LoggedIn:
                    CurrentPageVM = App.Current.Services.GetRequiredService<LoggedInVM>();
                    break;
            }
        }
    }
}
