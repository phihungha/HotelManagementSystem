using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HotelManagementSoftware.Data
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public IdNumberType IdNumberType { get; set; }

        public string IdNumber { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public Country? Country { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? CardNumber { get; set; }
        public DateTime? ExpireDate { get; set; }

        public List<Reservation> Reservations { get; } = new();

        /// <summary>
        /// Constructor for EF. DO NOT USE THIS
        /// </summary>
        public Customer(string name,
                        DateTime birthDate,
                        IdNumberType idNumberType,
                        string idNumber,
                        Gender gender,
                        string phoneNumber,
                        string address,
                        string city,
                        string province,
                        PaymentMethod paymentMethod)
        {
            Name = name;
            IdNumber = idNumber;
            BirthDate = birthDate;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Address = address;
            City = city;
            Province = province;
            PaymentMethod = paymentMethod;
            IdNumberType = idNumberType;
        }

        public Customer(string name,
                        DateTime birthDate,
                        IdNumberType idNumberType,
                        string idNumber,
                        Gender gender,
                        string phoneNumber,
                        string address,
                        string city,
                        string province,
                        Country country,
                        PaymentMethod paymentMethod,
                        string? cardNumber = null,
                        DateTime? expireDate = null)
            : this(name,
                   birthDate,
                   idNumberType,
                   idNumber,
                   gender,
                   phoneNumber,
                   address,
                   city,
                   province,
                   paymentMethod)
        {
            Country = country;
            CardNumber = cardNumber;
            ExpireDate = expireDate;
        }
    }

    public enum PaymentMethod
    {
        [Description("Cash")]
        Cash,
        [Description("Visa")]
        Visa,
        [Description("Mastercard")]
        Mastercard
    }

    public enum IdNumberType
    {
        [Description("CMND")]
        Cmnd,
        [Description("Passport")]
        Passport
    }

    public enum Gender
    {
        [Description("Male")]
        Male,
        [Description("Female")]
        Female
    }

    public class Country
    {
        public int CountryId { get; set; }

        public string CountryCode { get; set; }

        public string Name { get; set; }

        public List<Customer> Reservations { get; set; } = new();

        public Country(string countryCode, string name)
        {
            Name = name;
            CountryCode = countryCode;
        }
    }
}
