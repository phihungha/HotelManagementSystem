using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using Microsoft.Toolkit.Mvvm.Input;
using HotelManagementSoftware.Data;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using HotelManagementSoftware.Business;
using System.Windows;
using HotelManagementSoftware.ViewModels.Validators;
using System.Threading.Tasks;
using System.Linq;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class EmployeeEditWindowVM : ObservableValidator
    {
        private EmployeeBusiness employeeBusiness;

        private Employee? employee;

        private bool canDelete = false;
        public bool CanDelete
        {
            get => canDelete;
            set => SetProperty(ref canDelete, value);
        }

        private string name = "";
        private string userName = "";
        private Gender gender = Gender.Male;
        private EmployeeType employeeType = EmployeeType.Receptionist;
        private DateTime birthDate = new DateTime(1970, 1, 1);
        private string cmnd = "";
        private string phoneNumber = "";
        private string? email;
        private string address = "";
        private string password = "";

        [Required(ErrorMessage = "Name cannot be empty")]
        [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 character")]
        [MaxLength(100, ErrorMessage = "Name should be shorter than 100 character")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        [Required(ErrorMessage = "Username cannot be empty")]
        [MinLength(5, ErrorMessage = "Username cannot be shorter than 5 character")]
        [MaxLength(50, ErrorMessage = "Username should be shorter than 50 character")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]+$", ErrorMessage = "Username should be a letter")]
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value, true);
        }

        public Gender Gender
        {
            get => gender;
            set => SetProperty(ref gender, value, true);
        }

        public EmployeeType EmployeeType
        {
            get => employeeType;
            set => SetProperty(ref employeeType, value, true);
        }

        public DateTime MinimumBirthDate => DateTime.Now.AddYears(-18);

        public DateTime BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value, true);
        }

        public IdNumberType IdNumberType { get; } = IdNumberType.Cmnd;

        [IdentityNumberCheck(nameof(IdNumberType))]
        public string Cmnd
        {
            get => cmnd;
            set => SetProperty(ref cmnd, value, true);
        }

        public string CountryCode { get; } = "VN";

        [PhoneNumberCheck(nameof(CountryCode))]
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value, true);
        }

        [EmailAddress]
        public string? Email
        {
            get => email;
            set => SetProperty(ref email, value, true);
        }

        [Required(ErrorMessage = "Address cannot be empty")]
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value, true);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value, true);
        }

        public EmployeeEditWindowVM(EmployeeBusiness employeeBusiness)
        {
            this.employeeBusiness = employeeBusiness;
        }

        public async void LoadEmployeeFromId(int employeeId)
        {
            Employee? employee = await employeeBusiness.GetEmployeeById(employeeId);

            if (employee == null)
                return;

            this.employee = employee;
            CanDelete = true;

            Cmnd = employee.Cmnd;
            Name = employee.Name;
            UserName = employee.UserName;
            Gender = employee.Gender;
            BirthDate = employee.BirthDate;
            PhoneNumber = employee.PhoneNumber;
            Email = employee.Email;
            Address = employee.Address;
        }

        public async Task<bool> SaveEmployee()
        {
            ValidateAllProperties();
            if (GetErrors().Count() != 0)
                return false;

            if (employee != null)
            {
                employee.Name = Name;
                employee.UserName = UserName;
                employee.Gender = Gender;
                employee.EmployeeType = EmployeeType;
                employee.BirthDate = BirthDate;
                employee.Cmnd = Cmnd;
                employee.PhoneNumber = PhoneNumber;
                employee.Email = Email;
                employee.Address = Address;
                await employeeBusiness.EditEmployee(employee);

                if (Password != "")
                    await employeeBusiness.ChangePassword(employee, Password);
            }
            else
            {
                var employee = new Employee(Name,
                                            UserName,
                                            EmployeeType,
                                            Gender,
                                            BirthDate,
                                            Cmnd,
                                            PhoneNumber,
                                            Address,
                                            Email);
                await employeeBusiness.CreateEmployee(employee, Password);
            }
            return true;
        }

        public async Task DeleteEmployee()
        {
            if (employee != null)
                await employeeBusiness.DeleteEmployee(employee);
        }
    }
}
