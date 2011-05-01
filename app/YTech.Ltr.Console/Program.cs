using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using YTech.Ltr.Core;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Data;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Castle.CastleWindsor;
using YTech.Ltr.ApplicationServices.CastleWindsor;

namespace YTech.Ltr.Console
{
    public class Program
    {
        private static IWindsorContainer container;
        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting up");

            InitializeServiceLocator();

            ServiceJob();

            System.Console.WriteLine("All done");
            System.Console.ReadLine();
        }

        protected static void InitializeServiceLocator()
        {
            container = new WindsorContainer();

            //container.Register(
            //        Component
            //        .For(typeof(ISessionFactoryKeyProvider))
            //        .ImplementedBy(typeof(DefaultSessionFactoryKeyProvider))
            //        .Named("sessionFactoryKeyProvider"));

            //container.Register(
            //        Component
            //            .For(typeof(IEntityDuplicateChecker))
            //            .ImplementedBy(typeof(EntityDuplicateChecker))
            //            .Named("entityDuplicateChecker"));

            //container.Register(
            //        Component
            //            .For(typeof(IRepository<>))
            //            .ImplementedBy(typeof(Repository<>))
            //            .Named("repositoryType"));

            //container.Register(
            //        Component
            //            .For(typeof(INHibernateRepository<>))
            //            .ImplementedBy(typeof(NHibernateRepository<>))
            //            .Named("nhibernateRepositoryType"));

            //container.Register(
            //        Component
            //            .For(typeof(IRepositoryWithTypedId<,>))
            //            .ImplementedBy(typeof(RepositoryWithTypedId<,>))
            //            .Named("repositoryWithTypedId"));

            //container.Register(
            //        Component
            //            .For(typeof(INHibernateRepositoryWithTypedId<,>))
            //            .ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>))
            //            .Named("nhibernateRepositoryWithTypedId"));

            YTech.Ltr.ApplicationServices.CastleWindsor.ComponentRegistrar.AddComponentsTo(container);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            // Since this just looks for the assembly, leave it be.
            Initializer.Init();
        }

        public static void ServiceJob()
        {
            var productRepository = container.Resolve<IMGameRepository>();
            var productCount = productRepository.GetAll();

            foreach (MGame mGame in productCount)
            {
                System.Console.WriteLine("Complete, # of products: " + mGame.Id + " name " + mGame.GameName);
            }

            System.Console.WriteLine("Complete, # of products: " + productCount.Count);
        }
    }


}
