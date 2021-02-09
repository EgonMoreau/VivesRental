using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Customer
{
    public class KlantBeheerViewModel : ObservableObject, INavigatableViewModel
    {
        private readonly ICustomerService _customerService;
        private ObservableCollection<CustomerResult> _customers;
        private readonly INavigator _navigator;


        public RelayCommand NavigateToToevoegenCommand { get; private set; }
        public RelayCommand NavigateToDetailCommand { get; private set; }
        public RelayCommand DeleteCustomerCommand { get; private set; }

        private CustomerResult _selectedCustomer;



        public KlantBeheerViewModel(ICustomerService customerService, INavigator navigator)
        {
            Application.Current.MainWindow.Width = 1280;
            Application.Current.MainWindow.Height = 720;

            _navigator = navigator;
            _customerService = customerService;
            

            _ = LoadData();

            InstantiateCommands();
        }

        public CustomerResult SelectedCustomerResult
        {
            get => _selectedCustomer; 
            set
            {
                _selectedCustomer = value;
                RaisePropertyChanged();
            }
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

        public async Task DeleteData()
        {
            try
            {
                var customer = SelectedCustomerResult;
                if (await DeleteCustomer(customer) == true)
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

        public void LoadCustomers(ObservableCollection<CustomerResult> customers)
        {
            Customers = customers;
        }

        public async Task<bool> DeleteCustomer(CustomerResult customer)
        {
            try
            {
                return await _customerService.RemoveAsync(customer.Id);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        private void InstantiateCommands()
        {
            NavigateToToevoegenCommand = new RelayCommand(() => _navigator.NavigateTo<KlantToevoegenViewModel>());
            NavigateToDetailCommand = new RelayCommand(() => _navigator.NavigateTo<KlantDetailViewModel>(SelectedCustomerResult));
            DeleteCustomerCommand = new RelayCommand(async () => await DeleteData());
        }
    }
}