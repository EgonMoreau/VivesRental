using System;
using System.Threading.Tasks;
using System.Windows;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;

namespace VivesRental.ViewModels.Customer
{
    public class KlantToevoegenViewModel : ObservableObject, INavigatableViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly INavigator _navigator;

        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand NavigateToKlantBeheerCommand { get; private set; }

        public KlantToevoegenViewModel(ICustomerService customerService, INavigator navigator)
        {
            _customerService = customerService;
            _navigator = navigator;

            Customer = new Model.Customer();

            InstantiateCommands();
        }

        private Model.Customer _customer;

        public Model.Customer Customer
        {
            get => _customer;
            set
            {

                _customer = value;
                RaisePropertyChanged();

            }

        }

        private void InstantiateCommands()
        {
            try
            {
                CreateCommand = new RelayCommand(async () => await CreateCustomer());
                NavigateToKlantBeheerCommand = new RelayCommand(() => _navigator.NavigateTo<KlantBeheerViewModel>());
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }


        }

        private async Task CreateCustomer()
        {
            if (string.IsNullOrWhiteSpace(Customer.FirstName))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(Customer.LastName))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(Customer.Email))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(Customer.PhoneNumber))
            {
                return;
            }

            try
            {
                await _customerService.CreateAsync(Customer);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

            _navigator.NavigateTo<KlantBeheerViewModel>();
        }

    }
}