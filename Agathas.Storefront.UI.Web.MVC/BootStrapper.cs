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
using Agathas.Storefront.Repository.NHibernateR;
using Agathas.Storefront.Repository.NHibernateR.Repositories;
using Agathas.Storefront.Services.DomainEventHandlers;
using Agathas.Storefront.Services.Implementations;
using Agathas.Storefront.Services.Interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Agathas.Storefront.UI.Web.MVC
{
    public class BootStrapper
    {
        public static void ConfigureDependencies()
        {
            ObjectFactory.Initialize(x => x.AddRegistry<ControllerRegistry>());
        }

        public class ControllerRegistry : Registry
        {
            public ControllerRegistry()
            {
                // Repositories
                For<IOrderRepository>().Use
                    <OrderRepository>();
                For<ICustomerRepository>().Use
                    <CustomerRepository>();
                For<IBasketRepository>().Use
                    <BasketRepository>();
                For<IDeliveryOptionRepository>().Use
                    <DeliveryOptionRepository>();

                For<ICategoryRepository>().Use
                    <CategoryRepository>();
                For<IProductTitleRepository>().Use
                    <ProductTitleRepository>();
                For<IProductRepository>().Use
                    <ProductRepository>();
                For<IUnitOfWork>().Use
                    <NHUnitOfWork>();

                // Order Service
                For<IOrderService>().Use
                    <OrderService>();

                // Payment
                For<IPaymentService>().Use
                    <PayPalPaymentService>();

                // Handlers for Domain Events
                For<IDomainEventHandlerFactory>().Use<StructureMapDomainEventHandlerFactory>();
                For<IDomainEventHandler<OrderSubmittedEvent>>()
                    .Use<OrderSubmittedHandler>();

                // Product Catalogue
                For<IProductCatalogService>().Use
                    <ProductCatalogService>();

                // Product Catalogue & Category Service with Caching Layer Registration
                For<IProductCatalogService>().Use<ProductCatalogService>().Named("RealProductCatalogueService");
                //  .WithName("RealProductCatalogueService");

                // Uncomment the line below to use the product service caching layer
                //ForRequestedType<IProductCatalogueService>().Use<CachedProductCatalogueService>()
                //    .CtorDependency<IProductCatalogueService>().Is(x => x.TheInstanceNamed("RealProductCatalogueService"));

                For<IBasketService>().Use
                    <BasketService>();
                For<ICookieStorageService>().Use
                    <CookieStorageService>();

                // Application Settings
                For<IApplicationSettings>().Use
                    <WebConfigApplicationSettings>();

                // Logger
                For<ILogger>().Use
                    <Log4NetAdapter>();

                // Email Service
                For<IEmailService>().Use
                    <TextLoggingEmailService>();

                For<ICustomerService>().Use
                    <CustomerService>();

                // Authentication
                For<IExternalAuthenticationService>().Use<JanrainAuthenticationService>();
                For<IFormsAuthentication>().Use<AspFormsAuthentication>();
                For<ILocalAuthenticationService>().Use<AspMembershipAuthentication>();

                // Controller Helpers
                For<IActionArguments>().Use
                    <HttpRequestActionArguments>();
            }
        }
    }
}