using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg.XmlHbmBinding;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Cfg;
using System.Reflection;

namespace Agathas.Storefront.Repository.NHibernateR.SessionStorage
{
    public class SessionFactory
    {
        private static ISessionFactory _sessionFactory;

        private static void Init()
        {
            Configuration config = new Configuration();

            //          <property name="connection.driver_class">NHibernate.Driver.SqlClientDriver</property>
            //<property name="connection.connection_string">
            //  <!--Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|Shop.mdf;Integrated Security=True;User Instance=True-->
            //  Data Source=localhost\SQLEXPRESS2012;Initial Catalog=SHOP.MDF;User Id=bbozic;Password=ajdede;
            //</property>
            //<property name="adonet.batch_size">10</property>
            //<property name="show_sql">true</property>
            //<property name="dialect">NHibernate.Dialect.MsSql2008Dialect</property>
            //<property name="use_outer_join">true</property>
            //<property name="command_timeout">60</property>
            //<property name="query.substitutions">true 1, false 0, yes 'Y', no 'N'</property>
            //<property name="proxyfactory.factory_class">
            //  NHibernate.ByteCode.LinFu.ProxyFactoryFactory, NHibernate.ByteCode.LinFu
            //</property>
            //<property name="connection.isolation">ReadCommitted</property>

            config.SetProperty(NHibernate.Cfg.Environment.Dialect, "NHibernate.Dialect.MsSql2008Dialect");
            config.SetProperty(NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof(NHibernate.Bytecode.DefaultProxyFactoryFactory).AssemblyQualifiedName);
            config.SetProperty(NHibernate.Cfg.Environment.ConnectionString, @"Data Source=DESKTOP-4DS2D0D\SQLEXPRESS;AttachDbFilename=|DataDirectory|Shop.mdf;Integrated Security=True;User Instance=True;");
            config.SetProperty(NHibernate.Cfg.Environment.FormatSql, "true");
            config.SetProperty(NHibernate.Cfg.Environment.ShowSql, "true");
            config.SetProperty(NHibernate.Cfg.Environment.CommandTimeout, "60");
            config.SetProperty(NHibernate.Cfg.Environment.QuerySubstitutions, "true 1, false 0, yes 'Y', no 'N'");
            config.SetProperty(NHibernate.Cfg.Environment.Isolation, "ReadCommitted");
            config.SetProperty(NHibernate.Cfg.Environment.BatchSize, "10");
      
            //config.AddAssembly(Assembly.GetCallingAssembly());
            log4net.Config.XmlConfigurator.Configure();

            config.AddAssembly("Agathas.Storefront.Repository.NHibernate");

      
            config.Configure();

            _sessionFactory = config.BuildSessionFactory();
        }

        private static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
                Init();

            return _sessionFactory;
        }

        private static ISession GetNewSession()
        {
            return GetSessionFactory().OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            ISessionStorageContainer sessionStorageContainer =
                                       SessionStorageFactory.GetStorageContainer();

            ISession currentSession = sessionStorageContainer.GetCurrentSession();

            if (currentSession == null)
            {
                currentSession = GetNewSession();
                sessionStorageContainer.Store(currentSession);
            }

            return currentSession;
        }
    }

}
