using System;
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
    public class TResultDetRepository : NHibernateRepositoryWithTypedId<TResultDet, string>, ITResultDetRepository
    {
        public IList<TResultDet> GetListByDate(DateTime resultDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select det
                                from TResultDet as det
                                    left outer join det.ResultId res
                                    where res.ResultDate = :resultDate ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("resultDate", resultDate);
            return q.List<TResultDet>();
        }
    }
}
