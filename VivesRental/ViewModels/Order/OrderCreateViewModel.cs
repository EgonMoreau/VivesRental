using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using VivesRental.Core;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Order
{
    public class OrderCreateViewModel : ObservableObject, INavigatableViewModel
    {
        private ArticleResult _articleResult;
        private OrderResult _orderResult;
        private ArticleReservationResult _selectedReservationResult;
        private CustomerResult _customerResult;
        private Model.Customer _customer;
        private ProductResult _selectedProduct;
        private IList<ProductResult> _products;
        private IList<ArticleReservationResult> _reservation;
        private IList<ArticleResult> _articleResults;

        public RelayCommand ToevoegenArticleCommand { get; private set; }
        public RelayCommand DeleteReservationCommand { get; private set; }
        public RelayCommand CreateOrderCommand { get; private set; }

        private readonly IProductService _productService;
        private readonly INavigator _navigator;
        private readonly IArticleReservationService _articleReservationService;
        private readonly IArticleService _articleService;
        private readonly IOrderLineService _orderLineService;
        private readonly IOrderService _orderService;


        public OrderCreateViewModel(IProductService productService, INavigator navigator, IArticleReservationService articleReservationService, IArticleService articleService, IOrderLineService orderLineService,
            IOrderService orderService)
        {
            Application.Current.MainWindow.Width = 1280;
            Application.Current.MainWindow.Height = 720;

            _productService = productService;
            _navigator = navigator;
            _articleReservationService = articleReservationService;
            _articleService = articleService;
            _orderLineService = orderLineService;
            _orderService = orderService;

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

            _ = LoadData();

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
        public ArticleResult ArticleResult
        {
            get => _articleResult;
            set
            {
                _articleResult = value;
                RaisePropertyChanged();
            }
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

        public Model.Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
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

        public ProductResult SelectedProductResult
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged();
            }
        }

        public ArticleReservationResult SelectedReservationResult
        {
            get => _selectedReservationResult;
            set
            {
                _selectedReservationResult = value;
                RaisePropertyChanged();
            }
        }
        public IList<ArticleReservationResult> Reservation
        {
            get => _reservation;
            set
            {
                _reservation = value;
                RaisePropertyChanged();
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

        public void LoadProducts(IList<ProductResult> products)
        {
            Products = products;
        }



        public void LoadReservations(IList<ArticleReservationResult> reservation)
        {
            Reservation = reservation;
        }


        public async Task LoadData()
        {
            try
            {
                var reservations = await GetReservation();
                LoadReservations(reservations);
                var products = await GetProducts();
                LoadProducts(products);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }

        public async Task ToevoegenReservation()
        {
            try
            {
                _articleResults = await _articleService.GetAvailableArticlesAsync(SelectedProductResult.Id);

                await AddReservation(CustomerResult, _articleResults[0]);

                await LoadData();

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
                var reservation = SelectedReservationResult;
                if (await DeleteReservation(reservation) == true)
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

        public async Task CreateOrder()
        {
            try
            {
                OrderResult = await AddOrder();
                var reservations = new ObservableCollection<ArticleReservationResult>(await GetReservation());
                foreach (var r in reservations)
                {
                    await DeleteReservation(r);
                    if (await AddOrderLine(OrderResult.Id, r.ArticleId) == true)
                    {
                        await LoadData();
                    }
                }
                _navigator.NavigateTo<OrderBeheerViewModel>();

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }

        public async Task<ArticleReservationResult> AddReservation(CustomerResult customer, ArticleResult article)
        {
            try
            {
                return await _articleReservationService.CreateAsync(customer.Id, article.Id);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<bool> DeleteReservation(ArticleReservationResult reservation)
        {
            try
            {
                return await _articleReservationService.RemoveAsync(reservation.Id);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<IList<ArticleReservationResult>> GetReservation()
        {
            try
            {
                return await _articleReservationService.AllAsync();

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<OrderResult> AddOrder()
        {
            try
            {
                return await _orderService.CreateAsync(CustomerResult.Id);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }
        public async Task<ArticleResult> GetArticle(ArticleReservationResult a)
        {
            try
            {
                return await _articleService.GetAsync(a.ArticleId);

            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<bool> AddOrderLine(Guid orderGuid, Guid articleGuid)
        {

            try
            {
                return await _orderLineService.RentAsync(orderGuid, articleGuid);
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
                ToevoegenArticleCommand = new RelayCommand(async () => await ToevoegenReservation());
                DeleteReservationCommand = new RelayCommand(async () => await DeleteData());
                CreateOrderCommand = new RelayCommand(async () => await CreateOrder());
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
    }
}