using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class RoomTypeEditWindowVM : ObservableValidator
    {
        private RoomTypeBusiness roomTypeBusiness;

        private RoomType? roomType;

        private bool canDelete = false;
        public bool CanDelete
        {
            get => canDelete;
            set => SetProperty(ref canDelete, value);
        }

        private decimal rate = 100000;
        private int capacity = 2;
        private string name = "";
        private string? description;

        [Required(ErrorMessage = "Rate cannot be empty")]
        [Range(1000, double.MaxValue, ErrorMessage = "Rate needs to be more than 1000 VND")]
        public decimal Rate
        {
            get => rate;
            set => SetProperty(ref rate, value, true);
        }

        public int Capacity
        {
            get => capacity;
            set => SetProperty(ref capacity, value, true);
        }

        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value, true);
        }

        public string? Description
        {
            get => description;
            set => SetProperty(ref description, value, true);
        }

        public RoomTypeEditWindowVM(RoomTypeBusiness roomTypeBusiness)
        {
            this.roomTypeBusiness = roomTypeBusiness;
        }

        public async void LoadRoomTypeFromId(int roomTypeId)
        {
            RoomType? roomType = await roomTypeBusiness.GetRoomTypeById(roomTypeId);

            if (roomType == null)
                return;

            this.roomType = roomType;
            CanDelete = true;

            Name = roomType.Name;
            Rate = roomType.Rate;
            Capacity = roomType.Capacity;
            Description = roomType.Description;
        }

        public async Task<bool> SaveRoomType()
        {
            ValidateAllProperties();
            if (GetErrors().Count() != 0)
                return false;

            if (roomType != null)
            {
                roomType.Name = Name;
                roomType.Rate = Rate;
                roomType.Capacity = Capacity;
                roomType.Description = Description;
                await roomTypeBusiness.EditRoomType(roomType);
            } 
            else
            {
                var newRoomType = new RoomType(Name, Capacity, Rate, Description);
                await roomTypeBusiness.AddRoomType(newRoomType);
            }

            return true;
        }

        public async Task DeleteRoomType()
        {
            if (roomType != null)
                await roomTypeBusiness.RemoveRoomType(roomType);
        }
    }
}
