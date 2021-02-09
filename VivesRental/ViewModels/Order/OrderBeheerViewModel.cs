using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using VivesRental.Core;
using VivesRental.Model;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Order
{
    public class OrderBeheerViewModel : ObservableObject, INavigatableViewModel
    {
        private ObservableCollection<CustomerResult> _customers;
        private ObservableCollection<ArticleResult> _articles;
        private ObservableCollection<OrderResult> _orders;


        private CustomerResult _customer;
        private int _amountOrders;
        private int _amountOpenOrders;
        private OrderResult _selectedOrderResult;

        private readonly ICustomerService _customerService;
        private readonly IArticleService _articleService;
        private readonly IOrderService _orderService;
        private readonly INavigator _navigator;

        public RelayCommand NavigateToCreateOrder { get; private set; }
        public RelayCommand NavigateToDetailCommand { get; private set; }
        public RelayCommand ReturnOrderCommand { get; private set; }

        public OrderBeheerViewModel(ICustomerService customerService, IArticleService articleService, INavigator navigator, IOrderService orderService)
        {
            Application.Current.MainWindow.Width = 1280;
            Application.Current.MainWindow.Height = 720;

            _customerService = customerService;
            _orderService = orderService;
            _customer = Customer;
            _navigator = navigator;

            _articleService = articleService;

            _ = LoadData();

            InstantiateCommands();
        }

        public ObservableCollection<CustomerResult> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                RaisePropertyChanged();
            }
        }

        public CustomerResult Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ArticleResult> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<OrderResult> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                RaisePropertyChanged();
            }
        }
        public OrderResult SelectedOrderResult
        {
            get => _selectedOrderResult;
            set
            {
                _selectedOrderResult = value;
                RaisePropertyChanged();
            }
        }
        public int amountOrders
        {
            get => _amountOrders;
            set
            {
                _amountOrders = value;
                RaisePropertyChanged();
            }
        }

        public int amountOpenOrders
        {
            get => _amountOpenOrders;
            set
            {
                _amountOpenOrders = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadData()
        {
            try
            {
                var listCustomer = await GetCustomers();
                var customers = new ObservableCollection<CustomerResult>(listCustomer);

                var listOrders = await GetOrders();
                var orders = new ObservableCollection<OrderResult>(listOrders);

                var openOrders = new ObservableCollection<OrderResult>();
                foreach (var r in orders)
                {
                    if (r.ReturnedAt == null)
                    {
                        openOrders.Add(r);
                    }
                }

                amountOpenOrders = openOrders.Count;

                var orderAmount = listOrders.Count;
                amountOrders = orderAmount;

                LoadCustomers(customers);
                LoadOrders(orders);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }


        }

        public void LoadCustomers(ObservableCollection<CustomerResult> customers)
        {
            Customers = customers;
        }
        public void LoadOrders(ObservableCollection<OrderResult> orders)
        {
            Orders = orders;
        }


        public async Task ReturnOrder()
        {
            try
            {
                var order = SelectedOrderResult;
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

        public async Task<IList<CustomerResult>> GetCustomers()
        {

            try
            {
                return await _customerService.AllAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<IList<OrderResult>> GetOrders()
        {

            try
            {
                return await _orderService.AllAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<bool> ReturnOrderDao(OrderResult order, DateTime time)
        {

            try
            {
                return await _orderService.ReturnAsync(order.Id, time);
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
                NavigateToCreateOrder = new RelayCommand(() => _navigator.NavigateTo<OrderSelectCustomerViewModel>());
                NavigateToDetailCommand = new RelayCommand(() => _navigator.NavigateTo<OrderDetailViewModel>(SelectedOrderResult));
                ReturnOrderCommand = new RelayCommand(async () => await ReturnOrder());
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }
    }
}