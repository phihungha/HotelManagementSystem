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
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels
{
    public enum MaintenanceSearchBy
    {
        [Description("Room number")]
        Room
    }
    public enum MaintenanceFilterBy
    {
        [Description("Opened status")]
        OpenedStatus,
        [Description("Closed status")]
        ClosedStatus
    }
    public class MaintenanceVM : ObservableValidator
    {
        private MaintenanceBusiness maintenanceBusiness;
        private MaintenanceSearchBy searchBy = MaintenanceSearchBy.Room;
        public MaintenanceSearchBy SearchBy
        {
            get => searchBy;
            set => SetProperty(ref searchBy, value);
        }

        private MaintenanceFilterBy filterBy = MaintenanceFilterBy.OpenedStatus;
        public MaintenanceFilterBy FilterBy
        {
            get => filterBy;
            set => SetProperty(ref filterBy, value);
        }

        private string textFilter = "";
        public string TextFilter
        {
            get { return textFilter; }
            set => SetProperty(ref textFilter, value);
        }
        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }

        public ObservableCollection<MaintenanceRequest> MaintenanceRequestLists { get; } = new();

        public MaintenanceVM(MaintenanceBusiness maintenanceBusiness)
        {
            this.maintenanceBusiness = maintenanceBusiness;
            GetAllItem();
            FilterCommand = new AsyncRelayCommand(Filter);
            SearchCommand = new AsyncRelayCommand(Search);
        }


        private async Task Filter()
        {
            switch (FilterBy)
            {
                case MaintenanceFilterBy.OpenedStatus:
                    await GetAllItemByOpenedStatus();
                    break;
                case MaintenanceFilterBy.ClosedStatus:
                    await GetAllItemByClosedStatus();
                    break;
            }
        }
        private async Task GetAllItemByOpenedStatus()
        {
            if (maintenanceBusiness != null)
            {
                List<MaintenanceRequest> list = await maintenanceBusiness.GetMaintenanceRequests(status: MaintenanceRequestStatus.Opened);
                MaintenanceRequestLists.Clear();
                list.ForEach(item =>
                {
                    MaintenanceRequestLists.Add(item);
                });
            }
        }

        private async Task GetAllItemByClosedStatus()
        {
            if (maintenanceBusiness != null)
            {
                List<MaintenanceRequest> list = await maintenanceBusiness.GetMaintenanceRequests(status: MaintenanceRequestStatus.Closed);
                MaintenanceRequestLists.Clear();
                list.ForEach(item =>
                {
                    MaintenanceRequestLists.Add(item);
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
                    case MaintenanceSearchBy.Room:
                        await GetAllItemByRoom();
                        break;
                }
            }
        }
        /*   public void executeEditIssueAction()
           {
               MaintenanceEditWindow window = new MaintenanceEditWindow();
               MaintenanceEditWindowVM vm = App.Current.Services.GetRequiredService<MaintenanceEditWindowVM>();
               vm.MaintenanceVM = this;

               vm.MaintenanceRequestType = MaintenanceRequestType.Edit;
               vm.CurrentRequestId = SelectedItemMaintenanceRequest.MaintenanceRequestId;

               if (vm.CloseAction == null)
               {
                   vm.CloseAction = new Action(window.Close);
               }

               if (SelectedItemMaintenanceRequest.Status.Equals(MaintenanceRequestStatus.Closed))
               {
                   vm.IsEnabled = false;
               }
               else
               {
                   vm.IsEnabled = true;
               }

               window.DataContext = vm;
               window.ShowDialog();

           }
           public void executeAddIssueAction()
           {
               MaintenanceEditWindow window = new MaintenanceEditWindow();
               MaintenanceEditWindowVM vm = App.Current.Services.GetRequiredService<MaintenanceEditWindowVM>();
               vm.MaintenanceVM = this;

               vm.MaintenanceRequestType = MaintenanceRequestType.Add;
               vm.CurrentRequestId = null;

               if (vm.CloseAction == null)
               {
                   vm.CloseAction = new Action(window.Close);
               }
               window.DataContext = vm;

               window.ShowDialog();
           }*/

        public async Task GetAllItemByRoom()
        {
            int room;
            bool canConvert = Int32.TryParse(TextFilter, out room);
            if (maintenanceBusiness != null && canConvert)
            {
                List<MaintenanceRequest> list = await maintenanceBusiness.GetMaintenanceRequests(roomNumber: room);
                MaintenanceRequestLists.Clear();
                list.ForEach(item =>
                {
                    MaintenanceRequestLists.Add(item);
                });
            }
        }

        public async void GetAllItem()
        {
            if (maintenanceBusiness != null)
            {
                List<MaintenanceRequest> list = await maintenanceBusiness.GetMaintenanceRequests();
                MaintenanceRequestLists.Clear();
                list.ForEach(item =>
                {
                    MaintenanceRequestLists.Add(item);
                });
            }
        }
    }
}
