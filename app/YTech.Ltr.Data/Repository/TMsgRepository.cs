using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Data.Repository
{
    public class TMsgRepository : NHibernateRepositoryWithTypedId<TMsg, string>, ITMsgRepository
    {
        public bool GetMsgLikes(DateTime salesDate, string sms)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select count(msg.Id)
                                from TSales as s
                                    join s.MsgId msg
                                    where 1=1 ");
            sql.AppendLine(@"   and s.SalesDate = :salesDate");
            sql.AppendLine(@"   and s.MsgId.MsgText like :sms");

            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("salesDate", salesDate);
            q.SetString("sms", sms);
            var noOfRow = Convert.ToInt64(q.UniqueResult());
            return (noOfRow > 0);
        }

        public IList<TMsg> GetMsgNotRead()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select msg
                                from TMsg msg
                                    where 1=1 ");
            sql.AppendLine(@"   and msg.MsgStatus = null");

            IQuery q = Session.CreateQuery(sql.ToString()); 
            return q.List<TMsg>();
        }
    }
}
