using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

namespace YTech.Ltr.SmsLib.WinForms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            InitializeServiceLocator();

            IMGameRepository mGameRepository = container.Resolve<IMGameRepository>();
            var tSalesRepository = container.Resolve<ITSalesRepository>();
            var tSalesDetRepository = container.Resolve<ITSalesDetRepository>();
            var tMsgRepository = container.Resolve<ITMsgRepository>();
            var mAgentRepository = container.Resolve<IMAgentRepository>();

            ////set thrown error
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(tSalesRepository, tSalesDetRepository, mGameRepository, mAgentRepository, tMsgRepository));
        }
        private static IWindsorContainer container;
        private static void InitializeServiceLocator()
        {
            container = new WindsorContainer();
            YTech.Ltr.ApplicationServices.CastleWindsor.ComponentRegistrar.AddComponentsTo(container);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            // Since this just looks for the assembly, leave it be.
            Initializer.Init();
        }
    }
}
