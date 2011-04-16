using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Data.NHibernateMaps.Trans
{
    public class TResultMap : IAutoMappingOverride<TResult>
    {
        #region Implementation of IAutoMappingOverride<TResult>

        public void Override(AutoMapping<TResult> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();
            mapping.OptimisticLock.Dirty();

            mapping.Table("dbo.T_RESULT");
            mapping.Id(x => x.Id, "RESULT_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.ResultDate, "RESULT_DATE");
            mapping.Map(x => x.ResultStatus, "RESULT_STATUS");
            mapping.Map(x => x.ResultDesc, "RESULT_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Version(x => x.RowVersion)
                .Column("ROW_VERSION")
                //.CustomType("BinaryBlob")
                .CustomSqlType("Timestamp")
                .Not.Nullable()
                .Generated.Always();

            mapping.HasMany(x => x.ResultDets)
                .AsBag()
                .Inverse()
                .KeyColumn("RESULT_ID")
                .LazyLoad()
                .Cascade.All(); 
        }

        #endregion
    }
}
