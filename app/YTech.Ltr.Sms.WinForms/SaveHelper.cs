using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GsmComm.PduConverter;
using NHibernate;
using YTech.Ltr.Sms.WinForms.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace YTech.Ltr.Sms.WinForms
{
    public static class SaveHelper
    {
        internal static void SaveMessage(GsmComm.PduConverter.SmsPdu smsPdu)
        {
            // Received message
            SmsDeliverPdu data = (SmsDeliverPdu)smsPdu;

            // create our NHibernate session factory
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    TMsg msg = new TMsg();
                    msg.Id = Guid.NewGuid().ToString();
                    msg.MsgDate = data.SCTimestamp.ToDateTime();
                    msg.MsgFrom = data.OriginatingAddress;
                    msg.MsgTo = "";
                    msg.MsgText = data.UserDataText;

                    // save both stores, this saves everything else via cascading
                    session.SaveOrUpdate(msg);

                    transaction.Commit();
                }
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
            .Database(MsSqlCeConfiguration.Standard
                //.ConnectionString("Data Source=E:\\My Project\\MVC Project\\Solutions\\YTech.Ltr\\app\\YTech.Ltr.Web\\DB_LTR.sdf;Password=DIGITAL$;")
                //.ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle")
                .ConnectionString(x => x.Is("Data Source=E:\\My Project\\MVC Project\\Solutions\\YTech.Ltr\\app\\YTech.Ltr.Web\\DB_LTR.sdf;Password=DIGITAL$;"))
                )
                             .Mappings(x => x.FluentMappings.AddFromAssemblyOf<TMsg>().ExportTo("E:\\"))
                             
            .BuildSessionFactory();
        }




    }
}
