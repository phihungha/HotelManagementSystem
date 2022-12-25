using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class CancelReservationWindowVM : ObservableValidator
    {
        private int reservationId;

        private string cancel1;
        public string Cancel1
        {
            get => cancel1;
            set
            {
                SetProperty(ref cancel1, value);
            }
        }

        private decimal cancelFee;
        public decimal CancelFee
        {
            get => cancelFee;
            set
            {
                SetProperty(ref cancelFee, value);
            }
        }
        private ReservationBusiness? reservationBusiness;
        public CancelReservationWindowVM(ReservationBusiness? reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
        }
        public async void LoadReservationFromId(int reservationId)
        {

            Reservation? reservation = await reservationBusiness.GetReservationById(reservationId);
            CancelFee = await reservationBusiness.GetCancelFee(reservation);
            this.reservationId = reservationId;
            TimeSpan stayPeriod = reservation.ArrivalTime - DateTime.Now;
            int x = (int)Math.Ceiling(stayPeriod.TotalDays);
            Cancel1 = "Cancellation happened " + x + " before arrival time" + DateTime.Now;
        }
        public async Task Cancel()
        {
                Reservation? reservation = await reservationBusiness.GetReservationById(reservationId);
                await reservationBusiness.CancelReservation(reservation);
        }
    }
}
