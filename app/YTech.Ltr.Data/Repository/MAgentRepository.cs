using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;

namespace YTech.Ltr.Data.Repository
{
    public class MAgentRepository : NHibernateRepositoryWithTypedId<MAgent, string>, IMAgentRepository
    {
        public IEnumerable<MAgent> GetPagedAgentList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(MAgent));

            //calculate total rows
            totalRows = Session.CreateCriteria(typeof(MAgent))
                .SetProjection(Projections.RowCount())
                .FutureValue<int>().Value;

            //get list results
            criteria.SetMaxResults(maxRows)
              .SetFirstResult((pageIndex - 1) * maxRows)
              .AddOrder(new Order(orderCol, orderBy.Equals("asc") ? true : false))
              ;

            IEnumerable<MAgent> list = criteria.List<MAgent>();
            return list;
        }
    }
}
