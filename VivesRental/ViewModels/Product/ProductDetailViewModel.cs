using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows;
using ControlzEx.Standard;
using VivesRental.Core;
using VivesRental.Model;
using VivesRental.Navigation;
using VivesRental.Services.Contracts;
using VivesRental.Services.Filters;
using VivesRental.Services.Results;

namespace VivesRental.ViewModels.Product
{
    class ProductDetailViewModel : ObservableObject, INavigatableViewModel
    {
        private ProductResult _productResult;
        private Model.Product _product;
        private int _amount;
        private ArticleStatus _selectedArticleStatus;
        private ObservableCollection<ArticleResult> _articleResult;

        public RelayCommand DeleteArticleCommand { get; private set; }
        public RelayCommand AddArticleCommand { get; private set; }
        public RelayCommand ChangeArticleCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }

        private readonly IArticleService _articleService;
        private readonly IProductService _productService;
        private readonly INavigator _navigator;

        private ArticleResult _selectedArticle;

        public ProductDetailViewModel(INavigator navigator, IArticleService articleService, IProductService productService)
        {
            _navigator = navigator;
            _articleService = articleService;
            _productService = productService;

            if (_navigator.Data != null)
            {
                ProductResult = (ProductResult)_navigator.Data;
            }

            Product = new Model.Product
            {
                Id = ProductResult.Id,
                Name = ProductResult.Name,
                Description = ProductResult.Description,
                Manufacturer = ProductResult.Manufacturer,
                RentalExpiresAfterDays = ProductResult.RentalExpiresAfterDays,
                Publisher = ProductResult.Publisher
            };

            _ = LoadData();

            InstantiateCommands();
        }

        public List<ArticleStatus> StatusEnums { get; } =
            new List<ArticleStatus>()
            {
                {ArticleStatus.Normal},
                {ArticleStatus.Lost},
                {ArticleStatus.InRepair},
                {ArticleStatus.Destroyed},
                {ArticleStatus.Broken},
            };

        public Model.Product Product
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged();
            }

        }


        public ProductResult ProductResult
        {
            get => _productResult;
            set
            {
                _productResult = value;
                RaisePropertyChanged();
            }
        }

        public ArticleStatus SelectedStatus
        {
            get => _selectedArticleStatus;
            set
            {
                _selectedArticleStatus = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ArticleResult> Articles
        {
            get => _articleResult;
            set
            {
                _articleResult = value;
                RaisePropertyChanged();
            }
        }
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadData()
        {
            try
            {
                var listArticles = await GetArticles();
                var articles = new ObservableCollection<ArticleResult>(listArticles);
                LoadArticles(articles);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }

        public async Task<IList<ArticleResult>> GetArticles()
        {
            try
            {
                var article = new ArticleFilter();
                article.ProductId = ProductResult.Id;
                return await _articleService.FindAsync(article);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }

        public void LoadArticles(ObservableCollection<ArticleResult> articles)
        {
            Articles = articles;
        }

        public ArticleResult SelectedArticleResult
        {
            get => _selectedArticle;
            set
            {
                _selectedArticle = value;
                RaisePropertyChanged();
            }

        }

        public async Task DeleteData()
        {
            try
            {
                var article = SelectedArticleResult;
                if (await DeleteCustomer(article) == true)
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

        public async Task AddArticle()
        {

            try
            {
                var article = new Article { ProductId = ProductResult.Id, Status = ArticleStatus.Normal };
                var i = 0;
                while (i != Amount)
                {
                    await CreateArticle(article);
                    i++;
                }

                await LoadData();
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }

        public async Task ChangeArticle()
        {

            try
            {
                if (await UpdateArticle(SelectedArticleResult.Id, SelectedStatus) == true)
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

        private async Task EditProduct()
        {

            try
            {
                var product = Product;
                await EditProductAsync(product);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }



        public async Task<bool> DeleteCustomer(ArticleResult article)
        {
            try
            {
                return await _articleService.RemoveAsync(article.Id);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }

        public async Task<ArticleResult> CreateArticle(Article article)
        {
            try
            {
                return await _articleService.CreateAsync(article);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }
        public async Task<bool> UpdateArticle(Guid article, ArticleStatus status)
        {
            try
            {
                return await _articleService.UpdateStatusAsync(article, status);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }
        }

        public async Task<ProductResult> EditProductAsync(Model.Product product)
        {

            try
            {
                return await _productService.EditAsync(product);
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
                DeleteArticleCommand = new RelayCommand(async () => await DeleteData());
                AddArticleCommand = new RelayCommand(async () => await AddArticle());
                ChangeArticleCommand = new RelayCommand(async () => await ChangeArticle());
                EditCommand = new RelayCommand(async () => await EditProduct());
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een probleem met de database. Contacteer een admin.");
                throw;
            }

        }

    }
}