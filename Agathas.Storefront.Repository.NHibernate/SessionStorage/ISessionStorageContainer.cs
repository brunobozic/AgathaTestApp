using NHibernate;

namespace Agathas.Storefront.Repository.NHibernateR.SessionStorage
{
    public interface ISessionStorageContainer
    {
        ISession GetCurrentSession();
        void Store(ISession session);
    }

}
