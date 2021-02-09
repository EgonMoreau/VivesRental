using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Order
{
    public class OrderSelectCustomerViewModel : ObservableObject, INavigatableViewModel
    {
        private ObservableCollection<CustomerResult> _customers;

        private CustomerResult _selectedCustomer;

        
        public RelayCommand NavigateToOrderCreation { get; private set; }

        
        private readonly ICustomerService _customerService;
        private readonly INavigator _navigator;

        public OrderSelectCustomerViewModel(ICustomerService customerService, INavigator navigator)
        {
            
            Application.Current.MainWindow.Width = 300;
            Application.Current.MainWindow.Height = 250;

            _navigator = navigator;
            _customerService = customerService;

            SelectedCustomer = new CustomerResult();
            
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
        public CustomerResult SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                RaisePropertyChanged();
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

        public void LoadCustomers(ObservableCollection<CustomerResult> customers)
        {
            Customers = customers;
        }

        public async Task LoadData()
        {
            try
            {
                var listCustomer = await GetCustomers();
                var customers = new ObservableCollection<CustomerResult>(listCustomer);
                LoadCustomers(customers);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
        }

        public void GoToOrder()
        {
            if (SelectedCustomer.FirstName != null)
            {
                _navigator.NavigateTo<OrderCreateViewModel>(SelectedCustomer);

            }
            else
            {
                MessageBox.Show("Please select a customer.");
            }
        }


        private void InstantiateCommands()
        {
            NavigateToOrderCreation = new RelayCommand(() => GoToOrder());
        }
    }
}
