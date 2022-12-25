using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HotelManagementSoftware.Data
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string Cmnd { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string Address { get; set; }

        public string? HashedPassword { get; set; }

        public string? Salt { get; set; }

        public List<HousekeepingRequest> OpenedHousekeepingRequests { get; set; } = new();
        public List<HousekeepingRequest> ClosedHousekeepingRequests { get; set; } = new();
        public List<MaintenanceRequest> OpenedMaintenanceRequests { get; set; } = new();
        public List<MaintenanceRequest> ClosedMaintenanceRequests { get; set; } = new();
        public List<Reservation> Reservations { get; set; } = new();

        /// <summary>
        /// Constructor for EF. DO NOT USE THIS
        /// </summary>
        public Employee(string name,
                        string userName,
                        EmployeeType employeeType,
                        Gender gender,
                        DateTime birthDate,
                        string cmnd,
                        string phoneNumber,
                        string address)
        {
            Name = name;
            UserName = userName;
            EmployeeType = employeeType;
            Gender = gender;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Address = address;
            Cmnd = cmnd;
        }

        public Employee(string name,
                        string userName,
                        EmployeeType employeeType,
                        Gender gender,
                        DateTime birthDate,
                        string cmnd,
                        string phoneNumber,
                        string address,
                        string? email = null)
            : this(name, userName, employeeType, gender, birthDate, cmnd, phoneNumber, address)
        {
            Email = email;
        }
    }

    public enum EmployeeType
    {
        [Description("Receptionist")]
        Receptionist,
        [Description("Manager")]
        Manager,
        [Description("Housekeeping Manager")]
        HousekeepingManager,
        [Description("Maintenance Manager")]
        MaintenanceManager
    }
}
