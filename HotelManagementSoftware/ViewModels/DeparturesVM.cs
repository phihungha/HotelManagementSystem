using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using HotelManagementSoftware.Data;
using HotelManagementSoftware.Business;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HotelManagementSoftware.ViewModels
{
    public enum DeparturesSearchBy
    {
        [Description("Reservation ID")]
        Id,

        [Description("Customer Name")]
        CustomerName,

        [Description("Customer Phone number")]
        CustomerPhoneNumber,

        [Description("Customer Citizen ID")]
        CustomerIdNum
    }

    public class DeparturesVM : ObservableValidator
    {
        private string searchTerm = "";
        private DeparturesSearchBy searchBy = DeparturesSearchBy.CustomerName;
        private ReservationBusiness reservationBusiness;

        public ObservableCollection<Reservation> Departures { get; set; }

        public string SearchTerm
        {
            get => searchTerm;
            set => SetProperty(ref searchTerm, value);
        }

        public DeparturesSearchBy SearchBy
        {
            get => searchBy;
            set => SetProperty(ref searchBy, value);
        }

        public ICommand SearchCommand { get; }

        public DeparturesVM(ReservationBusiness reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
            Departures = new ObservableCollection<Reservation>();
            SearchCommand = new AsyncRelayCommand(Search);
            LoadDepartures();
        }

        public async void LoadDepartures()
        {
            List<Reservation> reservations = await reservationBusiness.GetDepartures();
            Departures.Clear();
            reservations.ForEach(i => Departures.Add(i));
        }

        private async Task Search()
        {
            if (searchTerm == "")
                LoadDepartures();
            else
            {
                switch (SearchBy)
                {
                    case DeparturesSearchBy.Id:
                        await GetDeparturesByReservationId();
                        break;

                    case DeparturesSearchBy.CustomerName:
                        await GetDeparturesByCustomerName();
                        break;

                    case DeparturesSearchBy.CustomerIdNum:
                        await GetDeparturesByCustomerIdNum();
                        break;

                    case DeparturesSearchBy.CustomerPhoneNumber:
                        await GetDeparturesByCustomerPhoneNumber();
                        break;
                }
            }
        }

        private async Task GetDeparturesByReservationId()
        {
            Reservation? reservation = await reservationBusiness.GetArrivalById(int.Parse(SearchTerm.Trim()));
            Departures.Clear();
            if (reservation != null)
                Departures.Add(reservation);
        }

        private async Task GetDeparturesByCustomerName()
        {
            List<Reservation> reservations = await reservationBusiness.GetDepartures(customerName: SearchTerm.Trim());
            Departures.Clear();
            reservations.ForEach(i => Departures.Add(i));
        }

        private async Task GetDeparturesByCustomerPhoneNumber()
        {
            List<Reservation> reservations = await reservationBusiness.GetDepartures(customerPhoneNumber: SearchTerm.Trim());
            Departures.Clear();
            reservations.ForEach(i => Departures.Add(i));
        }

        private async Task GetDeparturesByCustomerIdNum()
        {
            List<Reservation> reservations = await reservationBusiness.GetDepartures(customerIdentityNum: SearchTerm.Trim());
            Departures.Clear();
            reservations.ForEach(i => Departures.Add(i));
        }
    }
}


