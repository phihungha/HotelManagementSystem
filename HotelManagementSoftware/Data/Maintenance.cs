using System;
using System.Collections.Generic;

namespace HotelManagementSoftware.Data
{
    public class MaintenanceRequest
    {
        public int MaintenanceRequestId { get; set; }

        public int? OpenEmployeeId { get; set; }
        public Employee? OpenEmployee { get; set; }

        public int? CloseEmployeeId { get; set; }
        public Employee? CloseEmployee { get; set; }

        public Room? Room { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? CloseTime { get; set; }

        public string? Note { get; set; }

        public MaintenanceRequestStatus Status { get; set; }

        public List<MaintenanceItem> MaintenanceItems { get; set; } = new();

        /// <summary>
        /// Constructor for EF. DO NOT USE THIS
        /// </summary>
        public MaintenanceRequest(DateTime startTime,
                                  DateTime endTime,
                                  MaintenanceRequestStatus status)
        {
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }

        public MaintenanceRequest(int openEmployeeId,
                                  Employee openEmployee,
                                  Room room,
                                  DateTime startTime,
                                  DateTime endTime,
                                  string? note = null,
                                  MaintenanceRequestStatus status = MaintenanceRequestStatus.Opened)
            : this(startTime, endTime, status)
        {
            OpenEmployeeId = openEmployeeId;
            OpenEmployee = openEmployee;
            Room = room;
            Note = note;
        }
    }

    public enum MaintenanceRequestStatus
    {
        Opened,
        Closed
    }

    public class MaintenanceItem
    {
        public int MaintenanceItemId { get; set; }

        public MaintenanceRequest MaintenanceRequest { get; set; } = null!;

        public string Name { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public MaintenanceItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
