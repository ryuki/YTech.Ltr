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
    public class TResultRepository : NHibernateRepositoryWithTypedId<TResult, string>, ITResultRepository
    {
        public TResult GetResultByDate(DateTime resultDate)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select res
                                from TResult res
                                    where res.ResultDate = :resultDate ");
            IQuery q = Session.CreateQuery(sql.ToString());
            q.SetDateTime("resultDate", resultDate);
            q.SetMaxResults(1);
            return q.UniqueResult<TResult>();
        }
    }
}
