using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;

namespace YTech.Ltr.Data.Repository
{
    public class MAgentCommRepository : NHibernateRepositoryWithTypedId<MAgentComm, string>, IMAgentCommRepository
    {
    }
}
