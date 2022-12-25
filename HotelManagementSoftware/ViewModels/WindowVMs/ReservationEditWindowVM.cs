using HandyControl.Controls;
using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class ReservationEditWindowVM : ObservableValidator
    {
        private ReservationBusiness? reservationBusiness;
        private RoomBusiness? roomBusiness;
        private CustomerBusiness? customerBusiness;
        private EmployeeBusiness? employeeBusiness;
        private Customer? customer;
        private Reservation? reservation1;
        private Room? room;

        public ReservationEditWindowType reservationEditWindowType { get; set; }

        //Guest info
        #region Guest info
        private string cmnd;
        private string name;
        private Gender gender;
        private string phoneNumber;
        private DateTime birthDate;
        private string? email;
        private string address;

        public string CMND
        {
            get => cmnd;
            set
            {
                SetProperty(ref cmnd, value,true );
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
        #endregion

        //Room info
        #region Room info
        private int roomNumber;
        private RoomType? roomType;
        private string? note;
        private int floor;
        private decimal totalPayment;

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
                Load();
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
        #endregion

        // Stay info
        #region Stay info
        private DateTime arrivalTime=DateTime.Now;
        private DateTime departureTime = DateTime.Now.AddDays(1);
        private int numOfDay;
        private int person;
        public DateTime ArrivalTime {
            get => arrivalTime;
            set
            {
                SetProperty(ref arrivalTime, value, true);
                Load();
            }
        }
        public DateTime DepartureTime {
            get => departureTime;
            set
            {
                SetProperty(ref departureTime, value, true);
                Load();
            }
        }
        public int NumOfDay {
            get => numOfDay;
            set
            {
                SetProperty(ref numOfDay, value, true);
            }
        }
        public int Person {
            get => person;
            set
            {
                SetProperty(ref person, value, true);
            }
        }
        #endregion

        //Payment info
        private PaymentMethod[] paymentMethod;
        private PaymentMethod selectedPaymentMethod;
        private string? cardNumber;
        private DateTime? expireDate;

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
        public ReservationEditWindowVM(RoomBusiness? roomBusiness, CustomerBusiness? customerBusiness, ReservationBusiness? reservationBusiness, EmployeeBusiness? employeeBusiness)
        {
            this.roomBusiness = roomBusiness;
            this.customerBusiness = customerBusiness;
            this.reservationBusiness = reservationBusiness;
            this.employeeBusiness = employeeBusiness;
        }
        public async void LoadReservationFromId(int reservationId)
        {
            Reservation? reservation = await reservationBusiness.GetReservationById(reservationId);
            this.reservation1 = reservation;
            this.room = reservation.Room;
            LoadRoomFromId(reservation.Room.RoomId);
            this.customer = reservation.Customer;
            LoadCustomerFromId(reservation.Customer.CustomerId);
            Person = reservation.NumberOfPeople;
            ArrivalTime = reservation.ArrivalTime;
            DepartureTime = reservation.DepartureTime;
            TotalPayment = reservationBusiness.GetTotalRentFee(reservation);
            TimeSpan stayPeriod = reservation.DepartureTime - reservation.ArrivalTime;
            NumOfDay = (int)Math.Ceiling(stayPeriod.TotalDays);
        }
        public async void LoadRoomFromId(int RoomId)
        {
            Room? room = await roomBusiness.GetRoomById(RoomId);
            this.room = room;
            RoomNumber = room.RoomNumber;
            RoomType = room.RoomType;
            Note = room.Note;
            Floor = room.Floor;
        }
        public async void LoadCustomerFromId(int customerId)
        {
            Customer? customer = await customerBusiness.GetCustomerById(customerId);
            this.customer = customer;
            CMND = customer.IdNumber;
            Name = customer.Name;
            Gender = customer.Gender;
            BirthDate = customer.BirthDate;
            PhoneNumber = customer.PhoneNumber;
            Email = customer.Email;
            Address = customer.Address;
            SelectedPaymentMethod = customer.PaymentMethod;
            ExpireDate = customer.ExpireDate;
            CardNumber = customer.CardNumber;
        }
        #region command
        public ICommand CommandSave { get; }
        public ICommand CommandCancel { get; }

        public async Task<bool> Save()
        {
            if (reservationBusiness != null)
            {
                if (reservation1 != null)
                {
                    reservation1.ArrivalTime = ArrivalTime;
                    reservation1.DepartureTime = DepartureTime;
                    reservation1.NumberOfPeople = Person;
                    reservation1.Room = room;
                    reservation1.Customer = customer;
                    reservation1.Employee = employeeBusiness.CurrentEmployee;
                    await reservationBusiness.EditReservation(reservation1);
                    return true;
                }
                else
                {
                    Reservation reservation = new Reservation(ArrivalTime, DepartureTime, Person, room, customer, employeeBusiness.CurrentEmployee);
                    var result = System.Windows.MessageBox.Show(
        "Are you sure that you want to check in this Reservation now? This action cannot be undone.",
        "check in this Reservation?",
        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        await reservationBusiness.CreateReservation(reservation, true);
                    }
                    else
                    {
                        await reservationBusiness.CreateReservation(reservation, false);
                    }
                    return true;
                }
            }
            else 
                 return false;
           
        }

        
        public Action CloseAction { get; set; }
        #endregion
        public void Load()
        {
            if(room != null && room.RoomType != null)
            {
                TimeSpan stayPeriod = DepartureTime - ArrivalTime;
                NumOfDay = (int)Math.Ceiling(stayPeriod.TotalDays);
                TotalPayment = NumOfDay * room.RoomType.Rate;
            }
        }
        public enum ReservationEditWindowType
        {
            Add, Edit
        }
    }
}
