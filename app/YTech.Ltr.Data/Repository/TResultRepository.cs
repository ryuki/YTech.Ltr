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
            //EXECUTE [SP_CALCULATE_PRIZE] 
            //   @SalesDetStatus = :detStatus
            //  ,@SalesDate = :resultDate

            //calculate use query, not SP cause sql compact not support SP
            //SQL CE not support update table from other table, use subquery in where
            sql.AppendLine(@"
             update T_SALES_DET
                set SALES_DET_STATUS = null
		            , SALES_DET_PRIZE = null
                    , DATA_STATUS = :DATA_STATUS
                    , MODIFIED_BY = :MODIFIED_BY
                    , MODIFIED_DATE = :MODIFIED_DATE
	            where SALES_ID in (
                    select s.SALES_ID
                    from T_SALES s
		            where s.SALES_DATE =  :resultDate
                );
            ");
            //update det
            //   set SALES_DET_STATUS = null
            //       ,SALES_DET_PRIZE = null
            //   from T_SALES_DET det, T_SALES s
            //   where det.SALES_ID = s.SALES_ID
            //       and s.SALES_DATE =  :resultDate;

            //update det
            //set SALES_DET_STATUS = :detStatus
            //    ,SALES_DET_PRIZE = prize.PRIZE_VALUE
            //from T_SALES_DET det, T_SALES s, T_RESULT res, T_RESULT_DET res_det, M_GAME g, M_GAME_PRIZE prize
            //where s.SALES_DATE = res.RESULT_DATE
            //    and det.SALES_ID = s.SALES_ID
            //    and res.RESULT_ID = res_det.RESULT_ID
            //    and det.GAME_ID = res_det.GAME_ID
            //    and g.GAME_ID = res_det.GAME_ID
            //    and s.SALES_DATE = :resultDate
            //    and res_det.RESULT_DET_NUMBER = det.SALES_DET_NUMBER
            //    and prize.GAME_ID = g.GAME_ID
            //    and res_det.RESULT_DET_ORDER_NO between prize.PRIZE_NO_START and prize.PRIZE_NO_END;

            ISQLQuery q = Session.CreateSQLQuery(sql.ToString());
            //q.SetString("detStatus", Enums.EnumSalesDetStatus.Win.ToString());
            q.SetDateTime("resultDate", resultDate);
            q.SetString("DATA_STATUS", Enums.EnumDataStatus.Updated.ToString());
            q.SetString("MODIFIED_BY", "");
            q.SetDateTime("MODIFIED_DATE", DateTime.Now);
            //set all detail sales to default
            q.ExecuteUpdate();

            //search detail that win
            sql = new StringBuilder();
            sql.AppendLine(@"
                select det.SALES_DET_ID , sum(prize.PRIZE_VALUE) as PRIZE_VALUE
                from T_SALES_DET det, T_SALES s, T_RESULT res, T_RESULT_DET res_det, M_GAME g, M_GAME_PRIZE prize
                where s.SALES_DATE = res.RESULT_DATE
                    and det.SALES_ID = s.SALES_ID
                    and res.RESULT_ID = res_det.RESULT_ID
                    and det.GAME_ID = res_det.GAME_ID
                    and g.GAME_ID = res_det.GAME_ID
                    and s.SALES_DATE = :resultDate
                    and res_det.RESULT_DET_NUMBER = det.SALES_DET_NUMBER
                    and prize.GAME_ID = g.GAME_ID
                    and res_det.RESULT_DET_ORDER_NO between prize.PRIZE_NO_START and prize.PRIZE_NO_END
                group by det.SALES_DET_ID;
            ");
            q = Session.CreateSQLQuery(sql.ToString());
            q.SetDateTime("resultDate", resultDate);
            q.AddScalar("SALES_DET_ID", NHibernateUtil.String);
            q.AddScalar("PRIZE_VALUE", NHibernateUtil.Decimal);
            IList list = q.List();
            object[] obj;
            string detailId;
            decimal? prize;
            TSalesDet det = null;
            for (int i = 0; i < list.Count; i++)
            {
                obj = (object[])list[i];
                detailId = obj[0].ToString();
                prize = Convert.ToDecimal(obj[1]);
                det = Session.Get(typeof(TSalesDet), detailId) as TSalesDet;
                if (det != null)
                {
                    det.DataStatus = Enums.EnumDataStatus.Updated.ToString();
                    det.ModifiedBy = "";
                    det.ModifiedDate = DateTime.Now;
                    det.SalesDetStatus = Enums.EnumSalesDetStatus.Win.ToString();
                    det.SalesDetPrize = prize;
                    Session.Update(det);
                }

            }
        }
    }
}
