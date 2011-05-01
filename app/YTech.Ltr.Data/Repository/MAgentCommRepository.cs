using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;

namespace YTech.Ltr.Data.Repository
{
    public class MAgentCommRepository : NHibernateRepositoryWithTypedId<MAgentComm, string>, IMAgentCommRepository
    {
        public IList<MAgentComm> GetByAgentId(string agentId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   
                select comm
                from MAgentComm as comm
                    left outer join comm.AgentId as agent
                where comm.AgentId.Id = :agentId ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("agentId", agentId);
            return q.List<MAgentComm>();
        }

        public void DeleteByAgent(string agentId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   
                delete MAgentComm comm
                where comm.AgentId.Id = :agentId ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetString("agentId", agentId);
            q.ExecuteUpdate();
        }
    }
}
