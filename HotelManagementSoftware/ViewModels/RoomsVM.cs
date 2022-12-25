using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HotelManagementSoftware.ViewModels
{
    public class RoomsVM: ObservableValidator
    {
        private RoomBusiness roomBusiness;
        private FloorBusiness floorBusiness;

        public bool CanEdit { get; } = false;

        public ObservableCollection<Room> Rooms { get; } = new();
        public ObservableCollection<string> FloorOptions { get; } = new();

        private string floorOption = "All";
        public string FloorOption
        {
            get => floorOption;
            set
            {
                SetProperty(ref floorOption, value);
                LoadRooms();
            }
        }

        public RoomsVM(RoomBusiness roomBusiness, FloorBusiness floorBusiness, EmployeeBusiness employeeBusiness)
        {
            this.roomBusiness = roomBusiness;
            this.floorBusiness = floorBusiness;

            if (employeeBusiness.CurrentEmployee != null
                && employeeBusiness.CurrentEmployee.EmployeeType == EmployeeType.Manager)
                CanEdit = true;

            SetFloorValues();
            LoadRooms();
        }

        public async void SetFloorValues()
        {
            int maxFloorNumber = await floorBusiness.GetMaxFloorNumber();
            FloorOptions.Add("All");
            for (int i = 1; i <= maxFloorNumber; i++)
                FloorOptions.Add(i.ToString());
        }

        public async void LoadRooms()
        {
            int? floorNumberToLoad = null;
            if (floorOption != "All")
                floorNumberToLoad = int.Parse(floorOption);

            Rooms.Clear();
            List<Room> rooms = await roomBusiness.GetRooms(floorNumberToLoad, null, null);
            rooms.ForEach(roomtype => Rooms.Add(roomtype));
        }
    }
}
