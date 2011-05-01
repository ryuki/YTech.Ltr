using YTech.Ltr.Data.NHibernateMaps;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Data.NHibernate;

namespace YTech.Ltr.Data
{
    public static class Initializer
    {
        public static void Init()
        {
            NHibernateSession.Init(new ThreadSessionStorage(),
                                   new[] { "YTech.Ltr.Data.dll" },
                                   new AutoPersistenceModelGenerator().Generate(),
                                   "NHibernate.config");
        }
    }
}
