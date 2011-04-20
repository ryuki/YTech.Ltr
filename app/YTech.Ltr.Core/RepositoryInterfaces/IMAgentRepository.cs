using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface IMAgentRepository : INHibernateRepositoryWithTypedId<MAgent, string>
    {
        IEnumerable<MAgent> GetPagedAgentList(string orderCol, string orderBy, int pageIndex, int maxRows,
                                                     ref int totalRows);
    }
}
