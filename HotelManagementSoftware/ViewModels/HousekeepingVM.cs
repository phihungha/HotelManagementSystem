using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using HotelManagementSoftware.UI.Windows;
using HotelManagementSoftware.ViewModels.WindowVMs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManagementSoftware.ViewModels
{
    public enum HouseKeepingSearchBy
    {
        [Description("Room number")]
        Room
    }

    public enum HouseKeepingFilterBy
    {
        [Description("Opened status")]
        OpenedStatus,
        [Description("Closed status")]
        ClosedStatus
    }

    public class HousekeepingVM : ObservableValidator
    {
        private HousekeepingBusiness housekeepingBusiness;
        private HouseKeepingSearchBy searchBy = HouseKeepingSearchBy.Room;
        public HouseKeepingSearchBy SearchBy
        {
            get => searchBy;
            set => SetProperty(ref searchBy, value);
        }

        private HouseKeepingFilterBy filterBy = HouseKeepingFilterBy.OpenedStatus;
        public HouseKeepingFilterBy FilterBy
        {
            get => filterBy;
            set => SetProperty(ref filterBy, value);
        }

        public ObservableCollection<HousekeepingRequest> HouseKeepingLists { get; } = new();

        private string textFilter = "";
        public string TextFilter
        {
            get { return textFilter; }
            set => SetProperty(ref textFilter, value);
        }
        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }
        public HousekeepingVM(HousekeepingBusiness housekeepingBusiness)
        {
            this.housekeepingBusiness = housekeepingBusiness;
            GetAllItem();

            SearchCommand = new AsyncRelayCommand(Search);
            FilterCommand = new AsyncRelayCommand(Filter);
        }

        private async Task Filter()
        {
            switch (FilterBy)
            {
                case HouseKeepingFilterBy.OpenedStatus:
                    await GetAllItemByOpenedStatus();
                    break;
                case HouseKeepingFilterBy.ClosedStatus:
                    await GetAllItemByClosedStatus();
                    break;
            }
        }

        private async Task GetAllItemByOpenedStatus()
        {
            if (housekeepingBusiness != null)
            {
                List<HousekeepingRequest> list = await housekeepingBusiness.GetHousekeepingRequests(status: HousekeepingRequestStatus.Opened);
                HouseKeepingLists.Clear();
                list.ForEach(item =>
                {
                    HouseKeepingLists.Add(item);
                });
            }
        }

        private async Task GetAllItemByClosedStatus()
        {
            if (housekeepingBusiness != null)
            {
                List<HousekeepingRequest> list = await housekeepingBusiness.GetHousekeepingRequests(status: HousekeepingRequestStatus.Closed);
                HouseKeepingLists.Clear();
                list.ForEach(item =>
                {
                    HouseKeepingLists.Add(item);
                });
            }
        }

        private async Task Search()
        {
            if (textFilter == "")
                GetAllItem();
            else
            {
                switch (SearchBy)
                {
                    case HouseKeepingSearchBy.Room:
                        await GetAllItemByRoom();
                        break;
                }
            }
        }
        public async Task GetAllItemByRoom()
        {
            int room;
            bool canConvert = Int32.TryParse(TextFilter.Trim(), out room);
            if (housekeepingBusiness != null && canConvert)
            {
                List<HousekeepingRequest> list = await housekeepingBusiness.GetHousekeepingRequests(roomNumber: room);
                HouseKeepingLists.Clear();
                list.ForEach(item =>
                {
                    HouseKeepingLists.Add(item);
                });
            }
        }

        public async void GetAllItem()
        {
            if (housekeepingBusiness != null)
            {
                List<HousekeepingRequest> list = await housekeepingBusiness.GetHousekeepingRequests();
                HouseKeepingLists.Clear();
                list.ForEach(item =>
                {
                    HouseKeepingLists.Add(item);
                });
            }
        }
    }
}
