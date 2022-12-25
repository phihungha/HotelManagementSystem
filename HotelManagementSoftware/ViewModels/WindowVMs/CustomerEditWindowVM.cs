using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HotelManagementSoftware.Business;
using System.Collections.ObjectModel;
using HotelManagementSoftware.ViewModels.Validators;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class CustomerEditWindowVM : ObservableValidator
    {
        private CustomerBusiness customerBusiness;
        private CountryBusiness countryBusiness;
        private EmployeeBusiness employeeBusiness;

        private Customer? customer;

        private bool canDelete = false;

        public bool CanDelete
        {
            get => canDelete;
            set => SetProperty(ref canDelete, value);
        }

        private bool isUsingBankCard = false;
        public bool IsUsingBankCard
        {
            get => isUsingBankCard;
            set
            {
                SetProperty(ref isUsingBankCard, value);
                if (value == true)
                {
                    CardNumber = "";
                    ExpireDate = DateTime.Now.Date.AddYears(5);
                }
                else
                {
                    CardNumber = "";
                    ExpireDate = null;
                }
            } 
        }

        private string idNumber = "";
        private IdNumberType idNumberType = IdNumberType.Cmnd;
        private string name = "";
        private Gender gender = Gender.Male;
        private DateTime birthDate = new DateTime(1970, 1, 1);
        private string phoneNumber = "";
        private string? email = "";
        private string address = "";
        private string city = "";
        private string province = "";
        private Country? country = null;
        private PaymentMethod paymentMethod = PaymentMethod.Cash;
        private string? cardNumber = null;
        private DateTime? expireDate = null;

        [Required(ErrorMessage = "Name cannot be empty")]
        [MinLength(2, ErrorMessage = "Name cannot be shorter than 2 character")]
        [MaxLength(100, ErrorMessage = "Name should be shorter than 100 character")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        public IdNumberType IdNumberType
        {
            get => idNumberType;
            set => SetProperty(ref idNumberType, value, true);
        }

        [IdentityNumberCheck(nameof(IdNumberType))]
        public string IdNumber
        {
            get => idNumber;
            set => SetProperty(ref idNumber, value, true);
        }

        public Gender Gender
        {
            get => gender;
            set => SetProperty(ref gender, value, true);
        }

        public ObservableCollection<Country> Countries { get; } = new();

        public Country? Country
        {
            get => country;
            set => SetProperty(ref country, value, true);
        }

        public string CountryCode => Country?.CountryCode;

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

        [Required(ErrorMessage = "City cannot be empty")]
        public string City
        {
            get => city;
            set => SetProperty(ref city, value, true);
        }


        [Required(ErrorMessage = "Province/State cannot be empty")]
        public string Province
        {
            get => province;
            set => SetProperty(ref province, value, true);
        }

        public DateTime MinimumBirthDate => DateTime.Now.AddYears(-18);

        public DateTime BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value, true);
        }

        public PaymentMethod PaymentMethod
        {
            get => paymentMethod;
            set
            {
                SetProperty(ref paymentMethod, value, true);
                if (value != PaymentMethod.Cash)
                    IsUsingBankCard = true;
                else
                    IsUsingBankCard = false;
            }
        }

        [CardNumberCheck(nameof(PaymentMethod))]
        public string? CardNumber
        {
            get => cardNumber;
            set => SetProperty(ref cardNumber, value, true);
        }

        public DateTime MinimumExpireDate
        {
            get => DateTime.Now.Date.AddDays(7);
        }

        public DateTime? ExpireDate
        {
            get => expireDate;
            set => SetProperty(ref expireDate, value, true);
        }

        public CustomerEditWindowVM(CustomerBusiness customerBusiness,
                                    CountryBusiness countryBusiness,
                                    EmployeeBusiness employeeBusiness)
        {
            this.customerBusiness = customerBusiness;
            this.countryBusiness = countryBusiness;
            this.employeeBusiness = employeeBusiness;
        }

        public async void CreateCustomer()
        {
            await Populate();
        }

        private async Task Populate()
        {
            List<Country> result = await countryBusiness.GetAllCountries();
            result.ForEach(i => Countries.Add(i));
            Country = result.First(i => i.CountryCode == "VN");
        }

        public async void LoadCustomerFromId(int customerId)
        {
            await Populate();

            Customer? customer = await customerBusiness.GetCustomerById(customerId);

            if (customer == null)
                return;

            this.customer = customer;

            if (employeeBusiness.CurrentEmployee != null
                && employeeBusiness.CurrentEmployee.EmployeeType != EmployeeType.Manager)
                CanDelete = false;
            else
                CanDelete = true;

            if (customer.Country != null)
                Country = Countries.First(i => i.CountryCode == customer.Country.CountryCode);

            IdNumberType = customer.IdNumberType;
            IdNumber = customer.IdNumber;
            Name = customer.Name;
            Gender = customer.Gender;
            BirthDate = customer.BirthDate;
            PhoneNumber = customer.PhoneNumber;
            Email = customer.Email;
            Address = customer.Address;
            City = customer.City;
            Province = customer.Province;
            PaymentMethod = customer.PaymentMethod;
            ExpireDate = customer.ExpireDate;
            CardNumber = customer.CardNumber;
        }

        public async Task<bool> SaveCustomer()
        {
            ValidateAllProperties();
            if (GetErrors().Count() != 0)
                return false;

            if (customer != null)
            {
                customer.IdNumber = IdNumber;
                customer.IdNumberType = IdNumberType;
                customer.Name = Name;
                customer.Gender = Gender;
                customer.BirthDate = BirthDate;
                customer.PhoneNumber = PhoneNumber;
                customer.Email = Email;
                customer.Address = Address;
                customer.City = City;
                customer.Province = Province;
                customer.Country = Country;
                customer.PaymentMethod = PaymentMethod;
                customer.ExpireDate = ExpireDate;
                customer.CardNumber = CardNumber;

                await customerBusiness.EditCustomer(customer);
            }
            else
            {
                if (Country == null)
                    return false;

                var newCustomer = new Customer(Name,
                                               BirthDate,
                                               IdNumberType,
                                               IdNumber,
                                               Gender,
                                               PhoneNumber,
                                               Address,
                                               City,
                                               Province,
                                               Country,
                                               PaymentMethod,
                                               CardNumber,
                                               ExpireDate);
                await customerBusiness.CreateCustomer(newCustomer);
            }
            return true;
        }

        public async Task DeleteCustomer()
        {
            if (customer != null)
                await customerBusiness.DeleteCustomer(customer);
        }
    }
}
