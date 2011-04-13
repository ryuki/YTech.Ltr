using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using YTech.Ltr.Core;
using YTech.Ltr.Data.NHibernateMaps.Conventions;
using SharpArch.Core.DomainModel;
using SharpArch.Data.NHibernate.FluentNHibernate;

namespace YTech.Ltr.Data.NHibernateMaps
{

    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {

        #region IAutoPersistenceModelGenerator Members

        public AutoPersistenceModel Generate()
        {
            return AutoMap.AssemblyOf<Class1>(new AutomappingConfiguration())
                .Conventions.Setup(GetConventions())
                .IgnoreBase<Entity>()
                .IgnoreBase(typeof(EntityWithTypedId<>))
                .UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
        }

        #endregion

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.ForeignKeyConvention>();
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.HasManyConvention>();
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.HasManyToManyConvention>();
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.ManyToManyTableNameConvention>();
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.PrimaryKeyConvention>();
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.ReferenceConvention>();
                c.Add<YTech.Ltr.Data.NHibernateMaps.Conventions.TableNameConvention>();
            };
        }
    }
}
