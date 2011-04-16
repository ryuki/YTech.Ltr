using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface ITResultDetRepository : INHibernateRepositoryWithTypedId<TResultDet, string>
    {
        IList<TResultDet> GetListByDate(System.DateTime resultDate);
    }
}
