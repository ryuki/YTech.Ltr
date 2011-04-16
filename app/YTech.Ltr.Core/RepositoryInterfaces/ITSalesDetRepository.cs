using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface ITSalesDetRepository : INHibernateRepositoryWithTypedId<TSalesDet, string>
    {
        IList<TSalesDet> GetListByDateAndAgent(System.DateTime dateFrom, System.DateTime dateTo, string agentId, string salesDetStatus);
    }
}
