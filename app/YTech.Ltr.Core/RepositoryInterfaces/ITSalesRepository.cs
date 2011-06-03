using System.Collections;
using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface ITSalesRepository : INHibernateRepositoryWithTypedId<TSales, string>
    {
        IList GetSalesRecap(string orderCol, string orderBy, int pageIndex, int maxRows,ref int totalRows);

        void DeleteByDate(System.DateTime salesDate);
    }
}
