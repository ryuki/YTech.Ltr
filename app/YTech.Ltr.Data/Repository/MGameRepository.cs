using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;

namespace YTech.Ltr.Data.Repository
{
    public class MGameRepository : NHibernateRepositoryWithTypedId<MGame, string>, IMGameRepository
    {
        public decimal? GetCommissionByGameAndAgent(string agentId,string gameId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select comm.CommValue
                                from MAgentComm as comm
                                where comm.AgentId.Id = :agentId
                                    and comm.GameId.Id = :gameId        ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("agentId", agentId);
            q.SetString("gameId", gameId);
            q.SetMaxResults(1);
            return q.UniqueResult<decimal?>();
        }
    }
}
