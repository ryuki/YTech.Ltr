using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Core.RepositoryInterfaces
{
    public interface ITMsgRepository : INHibernateRepositoryWithTypedId<TMsg, string>
    {
        bool GetMsgLikes(System.DateTime salesDate, string sms);
    }
}
