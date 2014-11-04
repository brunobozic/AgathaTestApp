using Agathas.Storefront.Infrastructure.UnitOfWork;
using Agathas.Storefront.Model.Categories;

namespace Agathas.Storefront.Repository.NHibernateR.Repositories
{
    public class CategoryRepository : Repository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }

}
