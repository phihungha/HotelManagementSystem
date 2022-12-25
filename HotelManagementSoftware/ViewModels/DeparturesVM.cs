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
    public enum SearchOption
    {
        [Description("Customer Name")]
        CustomerName,
        [Description("Room type name")]
        Roomtype,
        [Description("Employee Name")]
        Employee
    }
    public class DeparturesVM : ObservableValidator
    {
        public ObservableCollection<Reservation> Departure { get; set; }
        private ReservationBusiness reservationBusiness;
        private CustomerBusiness customerBusiness;

        private ReservationStatus status;
   
        public ReservationStatus Status
        {
            get => status;
            set => SetProperty(ref status, value, true);
        }

        public async void GetAllReservation()
        {
            if (reservationBusiness != null)
            {
                List<Reservation> reservations = await reservationBusiness.GetReservations();
                Departure.Clear();
                reservations.ForEach(roomtype =>
                {
                    Departure.Add(roomtype);
                });

            }
        }

        public DeparturesVM(ReservationBusiness? reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
            Departure = new ObservableCollection<Reservation>();
            GetAllReservation();


        }

        public async void LoadDepartures()
        {
            Departure.Clear();
            List<Reservation> reservations = await reservationBusiness.GetReservations();
            reservations.ForEach(i => Departure.Add(i));
        }
    }
}

