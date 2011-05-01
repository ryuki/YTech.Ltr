using Castle.Windsor;
using SharpArch.Core.PersistenceSupport.NHibernate;
using SharpArch.Data.NHibernate;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Web.Castle;
using Castle.MicroKernel.Registration;
using SharpArch.Core.CommonValidator;
using SharpArch.Core.NHibernateValidator.CommonValidatorAdapter;

namespace YTech.Ltr.ApplicationServices.CastleWindsor
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
            AddApplicationServicesTo(container);

            container.Register(
                Component
                    .For(typeof(IValidator))
                    .ImplementedBy(typeof(Validator))
                    .Named("validator"));
        }

        private static void AddApplicationServicesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes
                .FromAssemblyNamed("YTech.Ltr.ApplicationServices")
                .Pick()
                .WithService.FirstInterface());
        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                AllTypes
                .FromAssemblyNamed("YTech.Ltr.Data")
                .Pick()
                .WithService.FirstNonGenericCoreInterface("YTech.Ltr.Core"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                    Component
                    .For(typeof(ISessionFactoryKeyProvider))
                    .ImplementedBy(typeof(DefaultSessionFactoryKeyProvider))
                    .Named("sessionFactoryKeyProvider"));
            container.Register(
                    Component
                        .For(typeof(IEntityDuplicateChecker))
                        .ImplementedBy(typeof(EntityDuplicateChecker))
                        .Named("entityDuplicateChecker"));

            container.Register(
                    Component
                        .For(typeof(IRepository<>))
                        .ImplementedBy(typeof(Repository<>))
                        .Named("repositoryType"));

            container.Register(
                    Component
                        .For(typeof(INHibernateRepository<>))
                        .ImplementedBy(typeof(NHibernateRepository<>))
                        .Named("nhibernateRepositoryType"));

            container.Register(
                    Component
                        .For(typeof(IRepositoryWithTypedId<,>))
                        .ImplementedBy(typeof(RepositoryWithTypedId<,>))
                        .Named("repositoryWithTypedId"));

            container.Register(
                    Component
                        .For(typeof(INHibernateRepositoryWithTypedId<,>))
                        .ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>))
                        .Named("nhibernateRepositoryWithTypedId"));
        }
    }

    /// <summary>
    /// Implementation of <see cref="ISessionFactoryKeyProvider" /> that uses
    /// the <see cref="SessionFactoryAttribute" /> to determine the session
    /// factory key.
    /// </summary>
    public class DefaultSessionFactoryKeyProvider : ISessionFactoryKeyProvider
    {
        public string GetKey()
        {
            return NHibernateSession.DefaultFactoryKey;
        }

        /// <summary>
        /// Gets the session factory key.
        /// </summary>
        /// <param name="anObject">An object that may have the <see cref="SessionFactoryAttribute"/> applied.</param>
        /// <returns></returns>
        public string GetKeyFrom(object anObject)
        {
            return SessionFactoryAttribute.GetKeyFrom(anObject);
        }
    }

    public interface ISessionFactoryKeyProvider
    {
        /// <summary>
        /// Gets the session factory key.
        /// </summary>
        /// <returns></returns>
        string GetKey();

        /// <summary>
        /// Gets the session factory key.
        /// </summary>
        /// <param name="anObject">An optional object that may have an attribute used to determine the session factory key.</param>
        /// <returns></returns>
        string GetKeyFrom(object anObject);
    }
}
