using System.Threading.Tasks;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;


namespace VivesRental.ViewModels.Customer
{
    public class KlantDetailViewModel : ObservableObject, INavigatableViewModel
    {
        private CustomerResult _customerResult;
        private Model.Customer _customer;

        private readonly ICustomerService _customerService;
        private readonly INavigator _navigator;

        public RelayCommand NavigateToKlantBeheerCommand { get; private set; }
        public RelayCommand BewerkKlantCommand { get; private set; }



        public KlantDetailViewModel(ICustomerService customerService, INavigator navigator)
        {
            _customerService = customerService;
            _navigator = navigator;
            if (_navigator.Data != null)
            {
                CustomerResult = (CustomerResult)_navigator.Data;
            }

            Customer = new Model.Customer
            {
                Id = CustomerResult.Id,
                FirstName = CustomerResult.FirstName,
                LastName = CustomerResult.LastName,
                Email = CustomerResult.Email,
                PhoneNumber = CustomerResult.PhoneNumber
            };


            InstantiateCommands();
        }


        public CustomerResult CustomerResult
        {
            get => _customerResult;
            set
            {
                _customerResult = value;
                RaisePropertyChanged();
            }
        }

        public Model.Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                RaisePropertyChanged();
            }

        }

        public async Task EditData()
        {

            var customer = Customer;
            await EditCustomer(customer);
            _navigator.NavigateTo<KlantBeheerViewModel>();

        }

        public async Task<CustomerResult> EditCustomer(Model.Customer customer)
        {
            return await _customerService.EditAsync(customer);

        }

        private void InstantiateCommands()
        {
            NavigateToKlantBeheerCommand = new RelayCommand(() => _navigator.NavigateTo<KlantBeheerViewModel>());
            BewerkKlantCommand = new RelayCommand(async () => await EditData());

        }
    }
}



