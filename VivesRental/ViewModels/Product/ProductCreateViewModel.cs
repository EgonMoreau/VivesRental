using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xaml.Behaviors.Core;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;

namespace VivesRental.ViewModels.Product
{
    class ProductCreateViewModel : ObservableObject, INavigatableViewModel
    {
        private readonly IProductService _productService;
        private readonly INavigator _navigator;

        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand NavigateToProductBeheerCommand { get; private set; }

        public ProductCreateViewModel(IProductService productService, INavigator navigator)
        {
            _productService = productService;
            _navigator = navigator;

            Product = new Model.Product();

            InstantiateCommands();
        }

        private Model.Product _product;

        public Model.Product Product
        {
            get => _product;
            set
            {

                _product = value;
                RaisePropertyChanged();
            }

        }

        private async Task CreateProduct()
        {
            if (string.IsNullOrWhiteSpace(Product.Description))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(Product.Manufacturer))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(Product.Publisher))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                return;
            }
            if (Product.RentalExpiresAfterDays == 0)
            {
                return;
            }

            try
            {
                await _productService.CreateAsync(Product);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
            
            _navigator.NavigateTo<ProductBeheerViewModel>();
        }

        private void InstantiateCommands()
        {
            try
            {
                CreateCommand = new RelayCommand(async () => await CreateProduct());
                NavigateToProductBeheerCommand = new RelayCommand(() => _navigator.NavigateTo<ProductBeheerViewModel>());
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
    }
}
