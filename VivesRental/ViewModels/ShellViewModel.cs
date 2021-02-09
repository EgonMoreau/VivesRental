using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.ViewModels.Customer;
using VivesRental.ViewModels.Dashboard;
using VivesRental.ViewModels.Order;
using VivesRental.ViewModels.Product;

namespace VivesRental.ViewModels
{
    public class ShellViewModel : ObservableObject, IViewModel
    {

        public INavigator Navigator { get; }

        public RelayCommand NavigateToDashboardCommand { get; private set; }
        public RelayCommand NavigateToKlantenBeheerCommand { get; private set; }
        public RelayCommand NavigateToOrderBeheerCommand { get; private set; }
        public RelayCommand NavigateToProductBeheerCommand { get; private set; }
        public RelayCommand NavigateToDetailCustomerCommand { get; private set; }

        public ShellViewModel(INavigator navigator)
        {
            Navigator = navigator;

            InstantiateCommands();

            Navigator.NavigateTo<DashboardViewModel>();
        }



        private void InstantiateCommands()
        {
            NavigateToDashboardCommand = new RelayCommand(() => Navigator.NavigateTo<DashboardViewModel>());
            NavigateToKlantenBeheerCommand = new RelayCommand(() => Navigator.NavigateTo<KlantBeheerViewModel>());
            NavigateToOrderBeheerCommand = new RelayCommand(() => Navigator.NavigateTo<OrderBeheerViewModel>());
            NavigateToProductBeheerCommand = new RelayCommand(() => Navigator.NavigateTo<ProductBeheerViewModel>());
            NavigateToDetailCustomerCommand = new RelayCommand(() => Navigator.NavigateTo<KlantToevoegenViewModel>());
        }
    }
}