using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore.Query.Internal;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;
using VivesRental.ViewModels.Order;

namespace VivesRental.ViewModels.Dashboard
{
    public class DashboardViewModel : ObservableObject, INavigatableViewModel
    {
        private Guid _orderGuid;
        private readonly IOrderService _orderService;
        private readonly INavigator _navigator;

        public RelayCommand NavigateToOrderDetailCommand { get; private set; }

        public DashboardViewModel(INavigator navigator, IOrderService orderService)
        {

            _navigator = navigator;
            _orderService = orderService;
            InstantiateCommands();
        }

        public Guid OrderGuid
        {
            get => _orderGuid;
            set
            {
                _orderGuid = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadOrder()
        {
            try
            {
                var order = await GetOrderDao(OrderGuid);
                _navigator.NavigateTo<OrderDetailViewModel>(order);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }


        public async Task<OrderResult> GetOrderDao(Guid id)
        {
            try
            {
                return await _orderService.GetAsync(id);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }


        private void InstantiateCommands()
        {
            try
            {
                NavigateToOrderDetailCommand = new RelayCommand(async () => await LoadOrder());
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }
    }
}