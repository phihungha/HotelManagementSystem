using HotelManagementSoftware.Business;
using HotelManagementSoftware.Data;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HotelManagementSoftware.ViewModels
{
    public class ConfigVM : ObservableValidator
    {
        private ReservationCancelFeePercentBusiness cancelFeeBusiness;
        private FloorBusiness floorBusiness;

        private int floorNumber = 1;

        public int FloorNumber
        {
            get => floorNumber;
            set => SetProperty(ref floorNumber, value);
        }

        public ObservableCollection<ReservationCancelFeePercent> CancelFeePercents { get; } = new();

        private List<ReservationCancelFeePercent> currentCancelFeePercents = new();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ConfigVM(
            ReservationCancelFeePercentBusiness cancelFeeBusiness, 
            FloorBusiness floorBusiness)
        {
            this.cancelFeeBusiness = cancelFeeBusiness;
            this.floorBusiness = floorBusiness;

            SaveCommand = new AsyncRelayCommand(Save);
            CancelCommand = new AsyncRelayCommand(Cancel);

            LoadConfig();
        }

        private async void LoadConfig()
        {
            FloorNumber = await floorBusiness.GetMaxFloorNumber();
            CancelFeePercents.Clear();
            currentCancelFeePercents.Clear();
            List<ReservationCancelFeePercent> percents = await cancelFeeBusiness.GetPercents();
            percents.ForEach(i => {
                CancelFeePercents.Add(i);
                currentCancelFeePercents.Add(i);
            });
        }

        private async Task Cancel()
        {
            LoadConfig();
        }

        private async Task Save()
        {

            try
            {
                await floorBusiness.SetMaxFloorNumber(FloorNumber);

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }

            List<ReservationCancelFeePercent> percents = CancelFeePercents.ToList();
            await cancelFeeBusiness.Update(CancelFeePercents.ToList());

            List<ReservationCancelFeePercent> deletedPercents = currentCancelFeePercents.Except(percents).ToList();
            await cancelFeeBusiness.Delete(deletedPercents);

            LoadConfig();
        }
    }
}
