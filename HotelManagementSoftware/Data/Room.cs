using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSoftware.Data
{
    public class Room
    {
        public int RoomId { get; set; }

        public int RoomNumber { get; set; }

        public RoomType? RoomType { get; set; }

        public int Floor { get; set; }

        public string? Note { get; set; }

        public List<Reservation> Reservations { get; set; } = new();

        public RoomStatus Status { get; set; }

        /// <summary>
        /// Constructor for EF. DO NOT USE THIS
        /// </summary>
        public Room(int roomNumber, int floor, RoomStatus status)
        {
            Floor = floor;
            Status = status;
            RoomNumber = roomNumber;
        }

        public Room(int roomNumber,
                    RoomType roomType,
                    int floor,
                    RoomStatus status = RoomStatus.Usable,
                    string? note = null)
            : this(roomNumber, floor, status)
        {
            RoomType = roomType;
            Note = note;
        }
    }

    public enum RoomStatus
    {
        [Description("Usable")]
        Usable,
        [Description("Closed")]
        Closed
    }

    public class RoomType
    {
        public int RoomTypeId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int Capacity { get; set; }

        [Column(TypeName = "decimal(18,0)")]
        public decimal Rate { get; set; }

        public List<Room> Rooms { get; set; } = new();

        public RoomType(string name, int capacity, decimal rate, string? description)
        {
            Name = name;
            Capacity = capacity;
            Rate = rate;
            Description = description;
        }
    }

    public class Configuration
    {
        public int ConfigurationId { get; set; }

        public string Name { get; set; } = "";

        public int Value { get; set; }
    }
}
