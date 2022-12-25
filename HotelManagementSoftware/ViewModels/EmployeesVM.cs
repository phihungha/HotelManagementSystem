using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.Input;
using HotelManagementSoftware.Business;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HotelManagementSoftware.ViewModels
{
    public enum EmployeesSearchBy
    {
        [Description("Name")]
        Name,
        [Description("Identity number")]
        IdNumber,
        [Description("Phone number")]
        PhoneNumber
    }

    public class EmployeesVM : ObservableValidator
    {
        private EmployeeBusiness employeeBusiness;

        private EmployeesSearchBy searchBy = EmployeesSearchBy.Name;
        public EmployeesSearchBy SearchBy
        {
            get => searchBy;
            set => SetProperty(ref searchBy, value);
        }

        private string textFilter = "";
        public string TextFilter
        {
            get { return textFilter; }
            set => SetProperty(ref textFilter, value);
        }

        public ObservableCollection<Employee> Employees { get; } = new();

        public ICommand SearchCommand { get; }

        public EmployeesVM(EmployeeBusiness employeeBusiness)
        {
            this.employeeBusiness = employeeBusiness;
            SearchCommand = new AsyncRelayCommand(Search);
            GetAllEmployees();
        }

        public async void GetAllEmployees()
        {
            List<Employee> employees = await employeeBusiness.GetAllEmloyees();
            Employees.Clear();
            employees.ForEach(employee =>
            {
                Employees.Add(employee);
            });
        }

        private async Task Search()
        {
            if (textFilter == "")
                GetAllEmployees();
            else
            {
                switch (SearchBy)
                {
                    case EmployeesSearchBy.Name:
                        await GetEmployeesFilterByName();
                        break;
                    case EmployeesSearchBy.IdNumber:
                        await GetEmployeesFilterByIDCard();
                        break;
                    case EmployeesSearchBy.PhoneNumber:
                        await GetEmployeesFilterByPhone();
                        break;
                }
            }
        }

        private async Task GetEmployeesFilterByName()
        {
            if (employeeBusiness != null && TextFilter != null)
            {
                List<Employee> employees = await employeeBusiness.GetEmployeesByName(TextFilter.Trim());
                Employees.Clear();
                employees.ForEach(item =>
                {
                    Employees.Add(item);
                });
            }
        }

        private async Task GetEmployeesFilterByIDCard()
        {
            if (employeeBusiness != null && TextFilter != null)
            {
                Employee? employee = await employeeBusiness.GetEmployeeByCmndNumber(TextFilter.Trim());
                Employees.Clear();

                if (employee != null)
                {
                    Employees.Add(employee);
                }
            }
        }

        private async Task GetEmployeesFilterByPhone()
        {
            if (employeeBusiness != null && TextFilter != null)
            {
                Employee? employee = await employeeBusiness.GetEmployeeByPhoneNumber(TextFilter.Trim());
                Employees.Clear();

                if (employee != null)
                {
                    Employees.Add(employee);
                }
            }
        }
    }
}
