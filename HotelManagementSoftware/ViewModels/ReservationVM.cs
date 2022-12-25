using HandyControl.Controls;
using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels
{
    public class ReservationsVM : ObservableValidator
    {
        public ObservableCollection<Reservation> Reservations { get; private set; }
        public Reservation SelectedReservations { get; set; }
        public ReservationBusiness? reservationBusiness;
        public TimeFilter ArrivalTimeFilter { get; private set; } = new();
        public TimeFilter DepartureTimeFilter { get; private set; } = new();
        public DateTime? a_low { get; set; }
        public DateTime? a_high { get; set; }
        public DateTime? d_low { get; set; }
        public DateTime? d_high { get; set; }
        private ReservationStatus status;
        private SearchOption option;
        private string textFilter;
        public string TextFilter
        {
            get => textFilter;
            set => SetProperty(ref textFilter, value, true);
        }
        public SearchOption Option
        {
            get => option;
            set => SetProperty(ref option, value, true);
        }
        public ReservationStatus Status
        {
            get => status;
            set => SetProperty(ref status, value, true);
        }
        public ReservationsVM(ReservationBusiness? reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
            Reservations = new ObservableCollection<Reservation>();
            GetAllReservation();
            SearchCommand = new AsyncRelayCommand(Search);
            ResetCommand = new AsyncRelayCommand(Reset);
        }
        #region command
        public ICommand CommandAdd { get; }
        public ICommand CommandDelete { get; }
        public ICommand CommandEdit { get; }
        public void executeAddAction()
        {

        }
        public void executeDeleteAction()
        {

        }
        public void executeEditAction()
        {

        }
        #endregion
        public class TimeFilter
        {
            public bool Enable { get; set; }
            public DateTime low { get; set; } = DateTime.Now;
            public DateTime high { get; set; } = DateTime.Now.AddDays(1);
        }
        public enum SearchOption
        {
            [Description("Customer Name")]
            CustomerName,
            [Description("Room type name")]
            Roomtype,
            [Description("Employee Name")]
            Employee
        }
        public async void GetAllReservation()
        {
            if (reservationBusiness != null)
            {
                List<Reservation> reservations = await reservationBusiness.GetReservations();
                Reservations.Clear();
                reservations.ForEach(roomtype =>
                {
                    Reservations.Add(roomtype);
                });

            }
        }
        public ICommand SearchCommand { get; }
        public ICommand ResetCommand { get; }
        public async Task Reset()
        {
            GetAllReservation();
        }
        public async Task Search()
        {
            if (ArrivalTimeFilter.Enable == false)
            {
                a_high = null;
                a_low = null;
            }else
            {
                a_high = ArrivalTimeFilter.high;
                a_low = ArrivalTimeFilter.low;
            }
            if (DepartureTimeFilter.Enable == false)
            {
                d_low = null;
                d_high = null;
            }else
            {
                d_low = DepartureTimeFilter.low;
                d_high = DepartureTimeFilter.high;
            }
            if (TextFilter == "") TextFilter = null;
            List<Reservation> reservations = new();
            switch (option)
            {
                case SearchOption.CustomerName:
                    reservations = await reservationBusiness.GetReservations(Status, TextFilter, null, null, null, a_low, a_high, d_low, d_high, null, null);
                    break;
                case SearchOption.Roomtype:
                    reservations = await reservationBusiness.GetReservations(Status, null, null, TextFilter, null, a_low, a_high, d_low, d_high, null, null);
                    break;
                case SearchOption.Employee:
                    reservations = await reservationBusiness.GetReservations(Status, null, null, null, TextFilter, a_low, a_high, d_low, d_high, null, null);
                    break;
            }
            Reservations.Clear();
            reservations.ForEach(roomtype =>
            {
                Reservations.Add(roomtype);
            });
        }
    }
}
