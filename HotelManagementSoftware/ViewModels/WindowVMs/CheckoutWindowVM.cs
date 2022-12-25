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
    public class CheckoutWindowVM : ObservableValidator
    {
        private ReservationBusiness? reservationBusiness;
        private RoomBusiness? roomBusiness;
        private CustomerBusiness? customerBusiness;
        private EmployeeBusiness? employeeBusiness;
        private Customer? customer;
        private Reservation? reservation;
        private Room? room;

        #region private variables
        private int reservationId;
        private string cmnd;
        private string name;
        private Gender gender;
        private string phoneNumber;
        private DateTime birthDate;
        private string? email;
        private string address;
        private int roomNumber;
        private RoomType? roomType;
        private string? note;
        private int floor;
        private decimal totalPayment;
        private DateTime arrivalTime;
        private DateTime departureTime;
        private int numOfDay;
        private int person;
        private PaymentMethod[] paymentMethod;
        private PaymentMethod selectedPaymentMethod;
        private string? cardNumber;
        private DateTime? expireDate;
        #endregion

        #region constructors
        public string CMND
        {
            get => cmnd;
            set
            {
                SetProperty(ref cmnd, value, true);
            }
        }
        public int ReservationId
        {
            get => reservationId;
            set
            {
                SetProperty(ref reservationId, value, true);
            }
        }
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value, true);
            }
        }
        public Gender Gender
        {
            get => gender;
            set
            {
                SetProperty(ref gender, value, true);
            }
        }
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                SetProperty(ref phoneNumber, value, true);
            }
        }
        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                SetProperty(ref birthDate, value, true);
            }
        }
        public string? Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value, true);
            }
        }
        public string Address
        {
            get => address;
            set
            {
                SetProperty(ref address, value, true);
            }
        }

        public int RoomNumber
        {
            get => roomNumber;
            set
            {
                SetProperty(ref roomNumber, value, true);
            }
        }
        public RoomType? RoomType
        {
            get => roomType;
            set
            {
                SetProperty(ref roomType, value, true);
            }
        }
        public string? Note
        {
            get => note;
            set
            {
                SetProperty(ref note, value, true);
            }
        }
        public int Floor
        {
            get => floor;
            set
            {
                SetProperty(ref floor, value, true);
            }
        }
        public decimal TotalPayment
        {
            get => totalPayment;
            set
            {
                SetProperty(ref totalPayment, value, true);
            }
        }

        public DateTime ArrivalTime
        {
            get => arrivalTime;
            set
            {
                SetProperty(ref arrivalTime, value, true);
            }
        }
        public DateTime DepartureTime
        {
            get => departureTime;
            set
            {
                SetProperty(ref departureTime, value, true);
            }
        }
        public int NumOfDay
        {
            get => numOfDay;
            set
            {
                SetProperty(ref numOfDay, value, true);
            }
        }
        public int Person
        {
            get => person;
            set
            {
                SetProperty(ref person, value, true);
            }
        }
        public PaymentMethod[] PaymentMethod
        {
            get => paymentMethod;
            set
            {
                SetProperty(ref paymentMethod, value, true);
            }
        }
        public PaymentMethod SelectedPaymentMethod
        {
            get => selectedPaymentMethod;
            set
            {
                SetProperty(ref selectedPaymentMethod, value, true);
            }
        }
        public string? CardNumber
        {
            get => cardNumber;
            set
            {
                SetProperty(ref cardNumber, value, true);
            }
        }
        public DateTime? ExpireDate
        {
            get => expireDate;
            set
            {
                SetProperty(ref expireDate, value, true);
            }
        }
        #endregion

        public CheckoutWindowVM(CustomerBusiness customerBusiness,
                                    ReservationBusiness reservationBusiness, RoomBusiness roomBusiness, EmployeeBusiness employeeBusiness)
        {
            this.roomBusiness = roomBusiness;
            this.customerBusiness = customerBusiness;
            this.reservationBusiness = reservationBusiness;
            this.employeeBusiness = employeeBusiness;


        }


        public async void LoadReservationFromId(int reservationsId)
        {
            Reservation? reservation = await reservationBusiness.GetReservationById(reservationsId);
            Room? room = await roomBusiness.GetRoomById(reservation.Room.RoomId);
            Customer? customer = await customerBusiness.GetCustomerById(reservation.Customer.CustomerId);
            this.reservation = reservation;

            //Room Information
            this.room = room;
            RoomNumber = room.RoomNumber;
            RoomType = room.RoomType;
            Note = room.Note;
            Floor = room.Floor;

            //Customer Information
            this.customer = customer;
            CMND = customer.IdNumber;
            Name = customer.Name;
            Gender = customer.Gender;
            BirthDate = customer.BirthDate;
            PhoneNumber = customer.PhoneNumber;
            Email = customer.Email;
            Address = customer.Address;

            //Reservation Information
            Person = reservation.NumberOfPeople;
            ArrivalTime = reservation.ArrivalTime;
            DepartureTime = reservation.DepartureTime;
            TotalPayment = reservationBusiness.GetTotalRentFee(reservation);
            TimeSpan stayPeriod = reservation.DepartureTime - reservation.ArrivalTime;
            NumOfDay = (int)Math.Ceiling(stayPeriod.TotalDays);
        }
        public async Task CheckOut()
        {
            if (reservation != null)
                await reservationBusiness.CheckOut(reservation);
        }

    }
}

