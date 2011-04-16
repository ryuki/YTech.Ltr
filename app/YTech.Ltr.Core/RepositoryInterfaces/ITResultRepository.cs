using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface ITResultRepository : INHibernateRepositoryWithTypedId<TResult, string>
    {
        TResult GetResultByDate(System.DateTime resultDate);

        void CalculatePrize(System.DateTime resultDate);
    }
}
