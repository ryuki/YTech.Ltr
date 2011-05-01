using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
   public interface IMAgentCommRepository : INHibernateRepositoryWithTypedId<MAgentComm, string>
    {
        IList<MAgentComm> GetByAgentId(string agentId);

        void DeleteByAgent(string agentId);
    }
}
