using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface IMGameRepository : INHibernateRepositoryWithTypedId<MGame, string>
    {
        decimal? GetCommissionByGameAndAgent(string agentId, string gameId);
    }
}
