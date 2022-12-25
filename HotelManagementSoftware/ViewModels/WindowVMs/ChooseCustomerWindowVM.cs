using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    class ChooseCustomerWindowVM : ObservableValidator
    {
        private CustomerBusiness customerBusiness;
        public ObservableCollection<Customer> Customers { get; } = new();
        public async void LoadCustomers()
        {
            Customers.Clear();
            List<Customer> customers = await customerBusiness.GetCustomers();
            customers.ForEach(i => Customers.Add(i));
        }
        public ChooseCustomerWindowVM(CustomerBusiness customerBusiness)
        {
            this.customerBusiness = customerBusiness;
            LoadCustomers();
        }
    }
}
