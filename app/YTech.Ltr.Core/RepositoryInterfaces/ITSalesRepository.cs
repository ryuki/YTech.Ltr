﻿using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface ITSalesRepository : INHibernateRepositoryWithTypedId<TSales, string>
    {
    }
}