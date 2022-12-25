using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels
{
    public enum ArrivalsSearchBy
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

    public class ArrivalsVM : ObservableValidator
    {
        private string searchTerm = "";
        private ArrivalsSearchBy searchBy = ArrivalsSearchBy.CustomerName;
        private ReservationBusiness reservationBusiness;

        public ObservableCollection<Reservation> Arrivals { get; set; }

        public string SearchTerm
        {
            get => searchTerm;
            set => SetProperty(ref searchTerm, value);
        }

        public ArrivalsSearchBy SearchBy
        {
            get => searchBy;
            set => SetProperty(ref searchBy, value);
        }

        public ICommand SearchCommand { get; }

        public ArrivalsVM(ReservationBusiness reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
            Arrivals = new ObservableCollection<Reservation>();
            SearchCommand = new AsyncRelayCommand(Search);
            LoadArrivals();
        }

        public async void LoadArrivals()
        {
            List<Reservation> reservations = await reservationBusiness.GetArrivals();
            Arrivals.Clear();
            reservations.ForEach(i => Arrivals.Add(i));
        }

        private async Task Search()
        {
            if (searchTerm == "")
                LoadArrivals();
            else
            {
                switch (SearchBy)
                {
                    case ArrivalsSearchBy.Id:
                        await GetArrivalsByReservationId();
                        break;

                    case ArrivalsSearchBy.CustomerName:
                        await GetArrivalsByCustomerName();
                        break;

                    case ArrivalsSearchBy.CustomerIdNum:
                        await GetArrivalsByCustomerIdNum();
                        break;

                    case ArrivalsSearchBy.CustomerPhoneNumber:
                        await GetArrivalsByCustomerPhoneNumber();
                        break;
                }
            }
        }

        private async Task GetArrivalsByReservationId()
        {
            Reservation? reservation = await reservationBusiness.GetArrivalById(int.Parse(SearchTerm.Trim()));
            Arrivals.Clear();
            if (reservation != null)
                Arrivals.Add(reservation);
        }

        private async Task GetArrivalsByCustomerName()
        {
            List<Reservation> reservations = await reservationBusiness.GetArrivals(customerName: SearchTerm.Trim());
            Arrivals.Clear();
            reservations.ForEach(i => Arrivals.Add(i));
        }

        private async Task GetArrivalsByCustomerPhoneNumber()
        {
            List<Reservation> reservations = await reservationBusiness.GetArrivals(customerPhoneNumber: SearchTerm.Trim());
            Arrivals.Clear();
            reservations.ForEach(i => Arrivals.Add(i));
        }

        private async Task GetArrivalsByCustomerIdNum()
        {
            List<Reservation> reservations = await reservationBusiness.GetArrivals(customerIdentityNum: SearchTerm.Trim());
            Arrivals.Clear();
            reservations.ForEach(i => Arrivals.Add(i));
        }
    }
}