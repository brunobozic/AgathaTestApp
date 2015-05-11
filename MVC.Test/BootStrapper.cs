using Agathas.Storefront.Controllers.ActionArguments;
using Agathas.Storefront.Infrastructure.Authentication;
using Agathas.Storefront.Infrastructure.Configuration;
using Agathas.Storefront.Infrastructure.CookieStorage;
using Agathas.Storefront.Infrastructure.Domain.Events;
using Agathas.Storefront.Infrastructure.Email;
using Agathas.Storefront.Infrastructure.Logging;
using Agathas.Storefront.Infrastructure.Payments;
using Agathas.Storefront.Infrastructure.UnitOfWork;
using Agathas.Storefront.Model.Basket;
using Agathas.Storefront.Model.Categories;
using Agathas.Storefront.Model.Customers;
using Agathas.Storefront.Model.Orders;
using Agathas.Storefront.Model.Orders.Events;
using Agathas.Storefront.Model.Products;
using Agathas.Storefront.Model.Shipping;
using Agathas.Storefront.Services.DomainEventHandlers;
using Agathas.Storefront.Services.Implementations;
using Agathas.Storefront.Services.Interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Agathas.Storefront.UI.Web.MVC5
{
    public class BootStrapper
    {
        public static void ConfigureDependencies()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ControllerRegistry>();            });
        }

        public class ControllerRegistry : Registry
        {
            public ControllerRegistry()
            {
                // Repositories 
                For<IOrderRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.OrderRepository>();
                For<ICustomerRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.CustomerRepository>();
                For<IBasketRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.BasketRepository>();
                For<IDeliveryOptionRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.DeliveryOptionRepository>();
               
                For<ICategoryRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.CategoryRepository>();
                For<IProductTitleRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.ProductTitleRepository>();
                For<IProductRepository>().Use<Agathas.Storefront.Repository.NHibernateR.Repositories.ProductRepository>();
                For<IUnitOfWork>().Use<Agathas.Storefront.Repository.NHibernateR.NHUnitOfWork>();

                // Order Service
                For<IOrderService>().Use<OrderService>();

                // Payment
                For<IPaymentService>().Use<PayPalPaymentService>();

                // Handlers for Domain Events
                For<IDomainEventHandlerFactory>().Use<StructureMapDomainEventHandlerFactory>();
                For<IDomainEventHandler<OrderSubmittedEvent>>().Use<OrderSubmittedHandler>();

                // Product Catalogue                                         
                For<IProductCatalogService>().Use<ProductCatalogService>();

                // Product Catalogue & Category Service with Caching Layer Registration
                //   For<IProductCatalogService>().Use<ProductCatalogService>().
                //    WithName("RealProductCatalogueService");

                // Uncomment the line below to use the product service caching layer
                //For<IProductCatalogueService>().Use<CachedProductCatalogueService>()
                //    .CtorDependency<IProductCatalogueService>().Is(x => x.TheInstanceNamed("RealProductCatalogueService"));

                For<IBasketService>().Use<BasketService>();
                For<ICookieStorageService>().Use<CookieStorageService>();

                // Application Settings                 
                For<IApplicationSettings>().Use<WebConfigApplicationSettings>();

                // Logger
                For<ILogger>().Use<Log4NetAdapter>();

                // Email Service                 
                For<IEmailService>().Use<TextLoggingEmailService>();
                For<ICustomerService>().Use<CustomerService>();

                // Authentication
                For<IExternalAuthenticationService>().Use<JanrainAuthenticationService>();
                For<IFormsAuthentication>().Use<AspFormsAuthentication>();
                For<ILocalAuthenticationService>().Use<AspMembershipAuthentication>();

                // Controller Helpers
                For<IActionArguments>().Use<HttpRequestActionArguments>();
            }
        }
    }
}
