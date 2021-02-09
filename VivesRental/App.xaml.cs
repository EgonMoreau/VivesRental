using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VivesRental.Navigation;
using VivesRental.Repository.Core;
using VivesRental.Services;
using VivesRental.Services.Contracts;
using VivesRental.ViewModels;
using VivesRental.ViewModels.Customer;
using VivesRental.ViewModels.Dashboard;
using VivesRental.ViewModels.Order;
using VivesRental.ViewModels.Product;
using VivesRental.Views;
using VivesRental.Views.Klanten;
using VivesRental.Views.Orders;
using VivesRental.Views.Products;

namespace VivesRental
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider ServiceProvider { get; set; }
        private IConfiguration Configuration { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var shell = ServiceProvider.GetRequiredService<Shell>();
            shell.Show();


            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //Database
            services.AddSingleton<IVivesRentalDbContext, VivesRentalDbContext>();
            services.AddSingleton<INavigator, Navigator>();

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            services.AddDbContext<VivesRentalDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("myconn")));

            //Navigation


            //Views
            services.AddTransient<Shell>();
            services.AddTransient<DashboardView>();
            services.AddTransient<KlantBeheerViewModel>();
            services.AddTransient<KlantToevoegenView>();
            services.AddTransient<KlantDetailViewModel>();
            services.AddTransient<OrderBeheerView>();
            services.AddTransient<OrderCreateView>();
            services.AddTransient<OrderSelectCustomerView>();
            services.AddTransient<OrderDetailView>();
            services.AddTransient<ProductBeheerView>();
            services.AddTransient<ProductDetailview>();
            services.AddTransient<ProductCreateView>();
  


            //ViewModels
            services.AddTransient<ShellViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<OrderBeheerViewModel>();
            services.AddTransient<OrderCreateViewModel>();
            services.AddTransient<OrderSelectCustomerViewModel>();
            services.AddTransient<OrderDetailViewModel>();
            services.AddTransient<KlantBeheerViewModel>();
            services.AddTransient<KlantDetailViewModel>();
            services.AddTransient<KlantToevoegenViewModel>();
            services.AddTransient<ProductBeheerViewModel>();
            services.AddTransient<ProductDetailViewModel>();
            services.AddTransient<ProductCreateViewModel>();
       


            //Services
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IOrderLineService, OrderLineService>();
            services.AddTransient<IArticleReservationService, ArticleReservationService>();
        }
    }
}