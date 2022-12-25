using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace HotelManagementSoftware.ViewModels.Utils
{
    public enum MainWindowPageName
    {
        Login,
        LoggedIn
    }

    public class MainWindowNavigation : ValueChangedMessage<MainWindowPageName>
    {
        public MainWindowNavigation(MainWindowPageName value) : base(value)
        {
        }
    }

    public class MainWindowNavigationUtils
    {
        public static void NavigateTo(MainWindowPageName ui)
        {
            WeakReferenceMessenger.Default.Send(new MainWindowNavigation(ui));
        }
    }
}
