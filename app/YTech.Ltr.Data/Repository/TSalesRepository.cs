using System;
using System.Collections;
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
    public class TSalesRepository : NHibernateRepositoryWithTypedId<TSales, string>, ITSalesRepository
    {
        public IList GetSalesRecap(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"  from TSales as s ");
            // left outer join s.SalesDets per


            string queryCount = string.Format(" select count(s.Id) {0}", sql);
            IQuery q = Session.CreateQuery(queryCount);

            totalRows = Convert.ToInt32(q.UniqueResult());


            string query = string.Format(" select s.SalesDate, count(s.Id) {0} group by s.SalesDate", sql);
            q = Session.CreateQuery(query);
            q.SetMaxResults(maxRows);
            q.SetFirstResult((pageIndex - 1) * maxRows);
            IList list = q.List();
            return list;
        }

        public void DeleteByDate(DateTime salesDate)
        {
            StringBuilder sql = new StringBuilder();
            //delete detail sales
            sql.AppendLine(@" delete from TSalesDet as det where det.SalesId.Id in (select Id from TSales as s where s.SalesDate = :SalesDate) ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("SalesDate", salesDate);
            q.ExecuteUpdate();
            //delete sales
            sql = new StringBuilder();
            sql.AppendLine(@" delete from TSales as s where s.SalesDate = :SalesDate ");
            q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("SalesDate", salesDate);
            q.ExecuteUpdate();
            ////delete sms
            //sql = new StringBuilder();
            //sql.AppendLine(@" delete from TMsg as s where s.MsgDate between :SalesDate and :SalesDateTo ");
            //q = Session.CreateQuery(sql.ToString());
            //q.SetDateTime("SalesDate", salesDate);
            //q.SetDateTime("SalesDateTo", salesDate.AddDays(1));
            //q.ExecuteUpdate();
        }
    }
}
