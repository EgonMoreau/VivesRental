using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Order
{
    class OrderDetailViewModel : ObservableObject, INavigatableViewModel
    {
        private readonly IOrderLineService _orderLineService;
        private readonly INavigator _navigator;

        private OrderResult _orderResult;
        private OrderLineResult _orderLineResult;
        private Model.Order _order;

        private ObservableCollection<OrderLineResult> _orderLines;

        public RelayCommand ReturnOrderLineCommand { get; private set; }

        public OrderDetailViewModel(IOrderLineService orderLineService, INavigator navigator)
        {
            _orderLineService = orderLineService;
            _navigator = navigator;

            if (_navigator.Data != null)
            {
                OrderResult = (OrderResult)_navigator.Data;
            }

            _ = LoadData();

            InstantiateCommands();
        }

        public OrderResult OrderResult
        {
            get => _orderResult;
            set
            {
                _orderResult = value;
                RaisePropertyChanged();
            }
        }

        public OrderLineResult SelectedOrderLine
        {
            get => _orderLineResult;
            set
            {
                _orderLineResult = value;
                RaisePropertyChanged();
            }
        }

        public Model.Order Order
        {
            get => _order;
            set
            {
                _order = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OrderLineResult> OrderLines
        {
            get => _orderLines;
            set
            {
                _orderLines = value;
                RaisePropertyChanged();
            }
        }

        public void LoadCustomers(ObservableCollection<OrderLineResult> orderLines)
        {
            OrderLines = orderLines;
        }

        public async Task LoadData()
        {
            try
            {
                var orderlines = new ObservableCollection<OrderLineResult>(await GetData(OrderResult.Id));
                LoadCustomers(orderlines);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }

        public async Task ReturnOrderLine()
        {
            try
            {
                var order = SelectedOrderLine;
                var time = DateTime.UtcNow;
                if (await ReturnOrderDao(order, time) == true)
                {
                    await LoadData();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }

        public async Task<bool> ReturnOrderDao(OrderLineResult order, DateTime time)
        {
            try
            {
                return await _orderLineService.ReturnAsync(order.Id, time);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }

        public async Task<IList<OrderLineResult>> GetData(Guid id)
        {
            try
            {
                return await _orderLineService.FindByOrderIdAsync(id);

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
                ReturnOrderLineCommand = new RelayCommand(async () => await ReturnOrderLine());

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }

    }
}