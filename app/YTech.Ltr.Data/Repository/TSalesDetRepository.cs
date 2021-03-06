﻿using System;
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
    public class TSalesDetRepository : NHibernateRepositoryWithTypedId<TSalesDet, string>, ITSalesDetRepository
    {
        public IList<TSalesDet> GetListByDateAndAgent(DateTime dateFrom, DateTime dateTo, string agentId, string gameId, string salesDetStatus)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TSalesDet as det
                                    left outer join det.SalesId s
                                    where 1=1 ");
            if (!string.IsNullOrEmpty(agentId))
            {
                sql.AppendLine(@"   and s.AgentId.Id = :agentId");
            }
            if (!string.IsNullOrEmpty(gameId))
            {
                sql.AppendLine(@"   and det.GameId.Id = :gameId");
            }
            if (!string.IsNullOrEmpty(salesDetStatus))
            {
                sql.AppendLine(@"   and det.SalesDetStatus = :salesDetStatus");
            }
            sql.AppendLine(@"   and s.SalesDate between :dateFrom and :dateTo ");
            IQuery q = Session.CreateQuery(sql.ToString());
            if (!string.IsNullOrEmpty(agentId))
            {
                q.SetString("agentId", agentId);
            }
            if (!string.IsNullOrEmpty(gameId))
            {
                q.SetString("gameId", gameId);
            }
            if (!string.IsNullOrEmpty(salesDetStatus))
            {
                q.SetString("salesDetStatus", salesDetStatus);
            }
            q.SetDateTime("dateFrom", dateFrom);
            q.SetDateTime("dateTo", dateTo);
            return q.List<TSalesDet>();
        }
    }
}
