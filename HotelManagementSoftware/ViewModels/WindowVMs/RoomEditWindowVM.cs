using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class RoomEditWindowVM : ObservableValidator
    {
        private RoomBusiness roomBusiness;
        private RoomTypeBusiness roomTypeBusiness;
        private FloorBusiness floorBusiness;

        private Room? room;

        private bool canDelete = false;
        public bool CanDelete
        {
            get => canDelete;
            set => SetProperty(ref canDelete, value);
        }

        private int maxFloor;
        public int MaxFloor
        {
            get => maxFloor;
            private set => SetProperty(ref maxFloor, value);
        }

        private int number;
        private int floor;
        private RoomType? roomType;
        private RoomStatus status;
        private string? note;

        public ObservableCollection<RoomType> RoomTypes { get; } = new();

        [Required(ErrorMessage = "Number cannot be empty")]
        public int Number
        {
            get => number;
            set => SetProperty(ref number, value, true);
        }

        public int Floor
        {
            get => floor;
            set => SetProperty(ref floor, value, true);
        }

        public string? Note
        {
            get => note;
            set => SetProperty(ref note, value, true);
        }

        public RoomStatus Status
        {
            get => status;
            set => SetProperty(ref status, value, true);
        }

        public RoomType? RoomType
        {
            get => roomType;
            set => SetProperty(ref roomType, value, true);
        }

        public RoomEditWindowVM(RoomBusiness roomBusiness, RoomTypeBusiness roomTypeBusiness, FloorBusiness floorBusiness)
        {
            this.roomBusiness = roomBusiness;
            this.roomTypeBusiness = roomTypeBusiness;
            this.floorBusiness = floorBusiness;
        }

        public async void CreateRoom()
        {
            await Populate();
        }

        private async Task Populate()
        {
            MaxFloor = await floorBusiness.GetMaxFloorNumber();

            List<RoomType> result = await roomTypeBusiness.GetRoomTypes();
            result.ForEach(i => RoomTypes.Add(i));
            RoomType = result.FirstOrDefault();
        }

        public async void LoadRoomFromId(int roomId)
        {
            await Populate();

            Room? room = await roomBusiness.GetRoomById(roomId);

            if (room == null)
                return;

            this.room = room;
            CanDelete = true;

            if (room.RoomType != null)
                RoomType = RoomTypes.First(i => i.Name == room.RoomType.Name);

            Number = room.RoomNumber;
            Floor = room.Floor;
            Status = room.Status;
            Note = room.Note;
        }

        public async Task<bool> SaveRoom()
        {
            ValidateAllProperties();
            if (GetErrors().Count() != 0)
                return false;

            if (room != null)
            {
                room.RoomNumber = Number;
                room.RoomType = RoomType;
                room.Floor = Floor;
                room.Status = Status;
                room.Note = Note;
                await roomBusiness.EditRoom(room);
            }
            else
            {
                if (RoomType == null)
                    return false;

                Room room = new Room(Number, RoomType, Floor, Status, Note);
                await roomBusiness.AddRoom(room);
            }

            return true;
        }

        public async Task DeleteRoom()
        {
            if (room != null)
                await roomBusiness.RemoveRoom(room);
        }
    }
}
