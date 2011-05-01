using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace YTech.Ltr.ApplicationServices.CastleWindsor
{
    public static class ServiceLocatorInitializer
    {
        public static void Init()
        {
            IWindsorContainer container = new WindsorContainer();
            //Register all the Contrib Components
            SharpArchContrib.Castle.CastleWindsor.ComponentRegistrar.AddComponentsTo(container);
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}
