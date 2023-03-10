using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HotelManagementSoftware.ViewModels
{
    public class RoomTypesVM : ObservableValidator
    {
        private RoomTypeBusiness roomTypeBusiness;

        public bool CanEdit { get; } = false;

        public ObservableCollection<RoomType> RoomTypes { get; } = new();

        public RoomTypesVM(RoomTypeBusiness roomTypeBusiness, EmployeeBusiness employeeBusiness)
        {
            this.roomTypeBusiness = roomTypeBusiness;
            LoadAllRoomTypes();

            if (employeeBusiness.CurrentEmployee != null
                && employeeBusiness.CurrentEmployee.EmployeeType == EmployeeType.Manager)
                CanEdit = true;
        }

        public async void LoadAllRoomTypes()
        {
            RoomTypes.Clear();
            List<RoomType> roomTypes = await roomTypeBusiness.GetRoomTypes();
            roomTypes.ForEach(roomtype => RoomTypes.Add(roomtype));
        }
    }
}
