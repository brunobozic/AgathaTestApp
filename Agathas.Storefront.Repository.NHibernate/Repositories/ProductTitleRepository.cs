using Agathas.Storefront.Infrastructure.UnitOfWork;
using Agathas.Storefront.Model.Products;

namespace Agathas.Storefront.Repository.NHibernateR.Repositories
{
    public class ProductTitleRepository : Repository<ProductTitle, int>,
                                                           IProductTitleRepository
    {
        public ProductTitleRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
