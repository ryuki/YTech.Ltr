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
    public class TResultRepository : NHibernateRepositoryWithTypedId<TResult, string>, ITResultRepository
    {
        public TResult GetResultByDate(DateTime resultDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select res
                                from TResult res
                                    where res.ResultDate = :resultDate ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("resultDate", resultDate);
            q.SetMaxResults(1);
            return q.UniqueResult<TResult>();
        }

        public void CalculatePrize(DateTime resultDate)
        {
            //            StringBuilder sql = new StringBuilder();
            //            sql.AppendLine(@"   
            //            update TSalesDet det
            //                set det.SalesDetStatus = :detStatus
            //                , det.SalesDetPrize = g.GamePrize
            //            from TSalesDet det, 
            //                TSales s, 
            //                TResult res, 
            //                TResultDet res_det, 
            //                MGame g
            //            where s.SalesDate = res.ResultDate
            //	            and det.SalesId.Id = s.Id
            //	            and res.Id = res_det.ResultId.Id
            //	            and det.GameId.Id = res_det.GameId.Id
            //	            and g.Id = res_det.GameId.Id
            //                and s.SalesDate = :resultDate ");
            //            IQuery q = Session.CreateQuery(sql.ToString());
            //            q.SetString("detStatus", Enums.EnumSalesDetStatus.Win.ToString());
            //            q.SetDateTime("resultDate", resultDate);
            //q.ExecuteUpdate();

            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"
            EXECUTE [SP_CALCULATE_PRIZE] 
               @SalesDetStatus = :detStatus
              ,@SalesDate = :resultDate
            ");

            ISQLQuery q = Session.CreateSQLQuery(sql.ToString());
            q.SetString("detStatus", Enums.EnumSalesDetStatus.Win.ToString());
            q.SetDateTime("resultDate", resultDate);
            q.ExecuteUpdate();
        }
    }
}
