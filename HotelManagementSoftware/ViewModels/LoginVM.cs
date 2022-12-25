using HotelManagementSoftware.Business;
using HotelManagementSoftware.ViewModels.Utils;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels
{
    public class LoginVM : ObservableValidator
    {
        private readonly EmployeeBusiness employeeBusiness;

        private string userName = "";
        [Required(ErrorMessage = "Please enter user name")]
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value, true);
        }

        private string password = "";
        [Required(ErrorMessage = "Please enter password")]
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value, true);
        }

        private bool isLoginInfoIncorrect = false;
        public bool IsLoginInfoIncorrect
        {
            get => isLoginInfoIncorrect;
            set => SetProperty(ref isLoginInfoIncorrect, value);
        }

        public ICommand LoginCommand { get; }

        public LoginVM(EmployeeBusiness employeeBusiness)
        {
            LoginCommand = new RelayCommand(Login);
            this.employeeBusiness = employeeBusiness;
        }

        private async void Login()
        {
            ValidateAllProperties();
            if (await employeeBusiness.Login(UserName, Password))
            {
                MainWindowNavigationUtils.NavigateTo(MainWindowPageName.LoggedIn);
                IsLoginInfoIncorrect = false;
            }
            else
                IsLoginInfoIncorrect = true;
        }
    }

}
