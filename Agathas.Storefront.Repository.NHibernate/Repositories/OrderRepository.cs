using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agathas.Storefront.Infrastructure.UnitOfWork;
using Agathas.Storefront.Model.Orders;

namespace Agathas.Storefront.Repository.NHibernateR.Repositories
{
    public class OrderRepository : Repository<Order, int>, IOrderRepository
    {
        public OrderRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }

}
