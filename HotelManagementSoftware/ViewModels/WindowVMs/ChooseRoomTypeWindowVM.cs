using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class ChooseRoomTypeWindowVM
    {
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        private RoomTypeBusiness? roomTypeBusiness;
        public RoomType SelectedRoomType { get; set; }
        public ChooseRoomTypeWindowVM(RoomTypeBusiness? roomTypeBusiness)
        {
            this.roomTypeBusiness = roomTypeBusiness;
            RoomTypes = new ObservableCollection<RoomType>();
            GetRoomType();
        }

        public async void GetRoomType()
        {
            if (roomTypeBusiness != null)
            {
                List<RoomType> roomTypes = await roomTypeBusiness.GetRoomTypes(null, null, null, null, null);
                RoomTypes.Clear();
                roomTypes.ForEach(roomtype =>
                {
                    RoomTypes.Add(roomtype);
                });

            }
        }

    }
}
