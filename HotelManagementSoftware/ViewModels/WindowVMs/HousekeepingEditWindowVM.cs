using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels.WindowVMs
{
    public class HousekeepingEditWindowVM : ObservableValidator
    {
        private HousekeepingBusiness housekeepingBusiness;
        private EmployeeBusiness employeeBusiness;
        private RoomBusiness roomBusiness;
        private HousekeepingRequest? housekeeping = null;

        private bool canClose = false;
        public bool CanClose
        {
            get => canClose;
            set
            {
                SetProperty(ref canClose, value);
            }
        }

        private bool canUseCloseRequest;
        public bool CanUseCloseRequest
        {
            get => canUseCloseRequest;
            set
            {
                SetProperty(ref canUseCloseRequest, value);
            }
        }

        private bool canNotClose = true;
        public bool CanNotClose
        {
            get => canNotClose;
            set
            {
                SetProperty(ref canNotClose, value);
            }
        }

        private Visibility visibilityCbx = Visibility.Visible;
        public Visibility VisibilityCbx
        {
            get => visibilityCbx;
            set
            {
                SetProperty(ref visibilityCbx, value);
            }
        }

        private Visibility visibilityTextbox = Visibility.Hidden;
        public Visibility VisibilityTextbox
        {
            get => visibilityTextbox;
            set
            {
                SetProperty(ref visibilityTextbox, value);
            }
        }
        private bool isEnabled = true;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                SetProperty(ref isEnabled, value);
            }
        }


        public ObservableCollection<Room> RoomLists { get; set; } = new();


        public DateTime MinStartDay { get; set; }
        public DateTime MaxStartDay { get; set; }
        public DateTime MinEndDay { get; set; }
        public DateTime DefaultDate { get; set; }

        #region private variables
        private int roomNumber;
        private Room room;
        private DateTime startTime = DateTime.Now;
        private DateTime endTime = DateTime.Now.AddDays(1);
        private DateTime? closeTime;
        private HousekeepingRequestStatus status = HousekeepingRequestStatus.Opened;
        private string? note;
        #endregion

        #region property validation

        public Room Room
        {
            get => room;
            set => SetProperty(ref room, value, true);
        }

        [Required(ErrorMessage = "Start time cannot be empty")]
        public DateTime StartTime
        {
            get => startTime;
            set
            {
                SetProperty(ref startTime, value, true);
            }
        }

        [Required(ErrorMessage = "End time cannot be empty")]
        public DateTime EndTime
        {
            get => endTime;
            set
            {
                SetProperty(ref endTime, value, EndTime > StartTime);
            }
        }

        public DateTime? CloseTime
        {
            get => closeTime;
            set => SetProperty(ref closeTime, value, true);
        }

        public HousekeepingRequestStatus Status
        {
            get => status;
            set => SetProperty(ref status, value, true);
        }

        public string? Note
        {
            get => note;
            set => SetProperty(ref note, value, true);
        }

        public int RoomNumber
        {
            get => roomNumber;
            set => SetProperty(ref roomNumber, value, true);
        }
        #endregion

        public HousekeepingEditWindowVM(HousekeepingBusiness housekeepingBusiness, EmployeeBusiness employeeBusiness, RoomBusiness roomBusiness)
        {
            this.roomBusiness = roomBusiness;
            this.employeeBusiness = employeeBusiness;
            this.housekeepingBusiness = housekeepingBusiness;
            FillRoomCombobox();
        }

        public void setUpDatePicker()
        {
            MinStartDay = new DateTime(1970, 1, 1);
            MaxStartDay = DateTime.Now.AddYears(-18);

            MinEndDay = MaxStartDay.AddDays(1);
            DefaultDate = DateTime.Now;

            StartTime = MaxStartDay;
            EndTime = MinEndDay;
        }

        private async void FillRoomCombobox()
        {
            if (roomBusiness != null)
            {
                List<Room> rooms = await roomBusiness.GetRooms();
                RoomLists.Clear();
                rooms.ForEach(room =>
                {
                    RoomLists.Add(room);
                });

            }
        }

        public void setDisplayForAdd()
        {
            VisibilityCbx = Visibility.Visible;
            VisibilityTextbox = Visibility.Hidden;
            IsEnabled = true;
        }

        public void setDisplayForEdit()
        {
            VisibilityCbx = Visibility.Hidden;
            VisibilityTextbox = Visibility.Visible;
            IsEnabled = true;
        }

        public async void GetCurrentRequestWithId(int id)
        {
            HousekeepingRequest? request = await housekeepingBusiness.GetHousekeepingRequestById(id);

            if (request == null)
            {
                return;
            }

            housekeeping = request;

            CanClose = true;
            CanNotClose = false;
            setUpDatePicker();
            setDisplayForEdit();
            if (housekeeping.Status.Equals(HousekeepingRequestStatus.Closed))
            {
                IsEnabled = false;
            }

            CheckCloseRequest();

            Room = housekeeping.Room;
            RoomNumber = housekeeping.Room.RoomNumber;
            StartTime = housekeeping.StartTime;
            EndTime = housekeeping.EndTime;
            CloseTime = housekeeping.CloseTime;
            Status = housekeeping.Status;
            Note = housekeeping.Note;

        }

        private void CheckCloseRequest()
        {
            Employee? employee = employeeBusiness.CurrentEmployee;
            if (employee == null) return;

            if (employee.EmployeeType.Equals(EmployeeType.HousekeepingManager) || employee.EmployeeType.Equals(EmployeeType.Manager))
            {
                CanUseCloseRequest = true;
            }
            else
            {
                CanUseCloseRequest = false;
            }
        }

        public async Task<bool> SaveRequest()
        {
            ValidateAllProperties();
            if (GetErrors().Count() != 0)
                return false;

            if (housekeeping != null)
            {
                housekeeping.StartTime = StartTime;
                housekeeping.EndTime = EndTime;
                housekeeping.Note = Note;
                await housekeepingBusiness.EditHousekeepingRequest(housekeeping);
            }
            else
            {
                if (employeeBusiness.CurrentEmployee != null)
                {
                    Employee openEmployee = employeeBusiness.CurrentEmployee;
                    var request = new HousekeepingRequest(openEmployee.EmployeeId,
                                       openEmployee,
                                       Room,
                                       StartTime,
                                       EndTime,
                                       Note);
                    await housekeepingBusiness.CreateHousekeepingRequest(request);
                }
            }
            return true;
        }

        public async Task<bool> CloseRequest()
        {
            ValidateAllProperties();
            if (GetErrors().Count() != 0)
                return false;

            if (housekeepingBusiness == null || employeeBusiness == null || housekeeping == null) return false;
            Employee? current = employeeBusiness.CurrentEmployee;
            housekeeping.Room = room;

            if (current == null) return false;
            await housekeepingBusiness.CloseHousekeepingRequest(housekeeping, DateTime.Now, current);

            return true;
        }
    }
}
