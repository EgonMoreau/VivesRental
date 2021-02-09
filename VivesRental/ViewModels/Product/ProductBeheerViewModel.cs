using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Product
{
    public class ProductBeheerViewModel : ObservableObject, INavigatableViewModel
    {
        private readonly IProductService _productService;
        private IList<ProductResult> _products;
        private readonly INavigator _navigator;

        private ProductResult _selectedProduct;

        public RelayCommand NavigateToProductDetailCommand { get; private set; }
        public RelayCommand NavigateToProductCreateCommand { get; private set; }
        public RelayCommand DeleteProductCommand { get; private set; }

        public ProductBeheerViewModel(IProductService productService, INavigator navigator)
        {
            Application.Current.MainWindow.Width = 1280;
            Application.Current.MainWindow.Height = 720;

            _productService = productService;
            _navigator = navigator;

            _ = LoadData();


            InstantiateCommands();
        }

        public ProductResult SelectedProductResult
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged();
            }
        }

        public IList<ProductResult> Products
        {
            get => _products;
            set
            {
                _products = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadData()
        {
            try
            {
                var products = await GetProducts();
                LoadProducts(products);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }

        public async Task<IList<ProductResult>> GetProducts()
        {
            try
            {
                return await _productService.AllAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }

        public async Task DeleteProduct()
        {

            try
            {
                var product = SelectedProductResult;
                if (await DeleteData(product) == true)
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


        public void LoadProducts(IList<ProductResult> products)
        {
            Products = products;
        }

        public async Task<bool> DeleteData(ProductResult product)
        {
            try
            {
                return await _productService.RemoveAsync(product.Id);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }

        public void InstantiateCommands()
        {
            try
            {
                NavigateToProductDetailCommand = new RelayCommand(() => _navigator.NavigateTo<ProductDetailViewModel>(SelectedProductResult));
                NavigateToProductCreateCommand = new RelayCommand(() => _navigator.NavigateTo<ProductCreateViewModel>());
                DeleteProductCommand = new RelayCommand(async () => await DeleteProduct());

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }
    }
}