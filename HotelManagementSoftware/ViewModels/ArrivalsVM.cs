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

    public class ArrivalsVM : ObservableValidator
    {
        private string searchTerm = "";
        private CustomersSearchBy searchBy = CustomersSearchBy.Name;
        private  ReservationBusiness reservationBusiness;
        private ReservationStatus status;
        private CustomerBusiness customerBusiness;

        public ObservableCollection<Reservation> Arrival { get; set;}
        public ReservationStatus Status
        {
            get => status;
            set => SetProperty(ref status, value, true);
        }
        public string SearchTerm
        {
            get => searchTerm;
            set => SetProperty(ref searchTerm, value);
        }

        public CustomersSearchBy SearchBy
        {
            get => searchBy;
            set => SetProperty(ref searchBy, value);
        }
        public async void GetAllReservation()
        {
            if (reservationBusiness != null)
            {
                List<Reservation> reservations = await reservationBusiness.GetReservations();
                Arrival.Clear();
                reservations.ForEach(roomtype =>
                {
                    Arrival.Add(roomtype);
                });

            }
        }

        public ICommand SearchCommand { get; }

        public ArrivalsVM(ReservationBusiness? reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
            Arrival = new ObservableCollection<Reservation>();
            SearchCommand = new AsyncRelayCommand(Search);
            GetAllReservation();


        }

   
        public async void LoadArrivals()
        {
            Arrival.Clear();
            List<Reservation> reservations = await reservationBusiness.GetReservations();
            reservations.ForEach(i => Arrival.Add(i));
        }

        private async Task Search()
        {
            if (searchTerm == "")
                GetAllReservation();
            else
            {
                switch (SearchBy)
                {
                    case CustomersSearchBy.Name:
                        await GetCustomersByName();
                        break;
                    case CustomersSearchBy.IdNumber:
                        await GetCustomersByID();
                        break;
                    case CustomersSearchBy.PhoneNumber:
                        await GetCustomersByPhoneNumber();
                        break;
                }
            }
        }

        private async Task GetCustomersByName()
        {
            if (reservationBusiness != null && SearchTerm != null)
            {
                List<Reservation> reservations = await reservationBusiness.GetReservationByCustomerName(SearchTerm.Trim());
                Arrival.Clear();
                reservations.ForEach(item =>
                {
                    Arrival.Add(item);
                });
            }
        }

        private async Task GetCustomersByID()
        {
            if (reservationBusiness != null && SearchTerm != null)
            {
                List<Reservation> reservations = await reservationBusiness.GetReservationByCustomerID(SearchTerm.Trim());
                Arrival.Clear();
                reservations.ForEach(item =>
                {
                    Arrival.Add(item);
                });
            }
        }

        private async Task GetCustomersByPhoneNumber()
        {
            if (reservationBusiness != null && SearchTerm != null)
            {
                List<Reservation> reservations = await reservationBusiness.GetReservationByCustomerPhoneNumber(SearchTerm.Trim());
                Arrival.Clear();
                reservations.ForEach(item =>
                {
                    Arrival.Add(item);
                });
            }
        }


    }
}
