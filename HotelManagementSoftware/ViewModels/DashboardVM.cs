using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;

namespace HotelManagementSoftware.ViewModels
{
    public class DashboardVM : ObservableObject
    {
        private ReservationBusiness reservationBusiness;

        private Timer currentTimeTimer = new Timer()
        {
            Interval = 1000,
            AutoReset = true,
            Enabled = true
        };
        
        private Timer upcomingListTimer = new Timer()
        {
            Interval = 60000,
            AutoReset = true,
            Enabled = true
        };

        public ObservableCollection<ReservationBusiness.UpcomingArrival> UpcomingArrivals { get; } = new();
        public ObservableCollection<ReservationBusiness.UpcomingDeparture> UpcomingDepartures { get; } = new();

        private DateTime currentTime;
        public DateTime CurrentTime
        {
            get => currentTime;
            set => SetProperty(ref currentTime, value);
        }

        private int upcomingArrivalNumber = 0;
        public int UpcomingArrivalNumber
        {
            get => upcomingArrivalNumber;
            set => SetProperty(ref upcomingArrivalNumber, value);
        }

        private int upcomingDepartureNumber = 0;
        public int UpcomingDepartureNumber
        {
            get => upcomingDepartureNumber;
            set => SetProperty(ref upcomingDepartureNumber, value);
        }

        private int reservationNumber = 0;
        public int ReservationNumber
        {
            get => reservationNumber;
            set => SetProperty(ref reservationNumber, value);
        }

        public DashboardVM(ReservationBusiness reservationBusiness)
        {
            this.reservationBusiness = reservationBusiness;
            currentTimeTimer.Elapsed += CurrentTimeTimer_Elapsed;
            upcomingListTimer.Elapsed += UpcomingListTimer_Elapsed;
            LoadData();
        }

        private void UpcomingListTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                LoadUpcomingLists();
            });
        }

        private void CurrentTimeTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        public async void LoadData()
        {
            LoadUpcomingLists();
            ReservationNumber = await reservationBusiness.GetReservedReservationNumber();
        }

        private async void LoadUpcomingLists()
        {
            List<ReservationBusiness.UpcomingArrival> upcomingArrivals
                = await reservationBusiness.GetUpcomingArrivals();
            upcomingArrivals.ForEach(i => UpcomingArrivals.Add(i));
            UpcomingArrivalNumber = upcomingArrivals.Count();

            List<ReservationBusiness.UpcomingDeparture> upcomingDepartures
                = await reservationBusiness.GetUpcomingDepartures();
            upcomingDepartures.ForEach(i => UpcomingDepartures.Add(i));
            UpcomingDepartureNumber = upcomingDepartures.Count();
        }
    }
}
