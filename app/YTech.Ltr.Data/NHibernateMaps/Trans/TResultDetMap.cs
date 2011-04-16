using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Data.NHibernateMaps.Trans
{
    public class TResultDetMap : IAutoMappingOverride<TResultDet>
    {
        #region Implementation of IAutoMappingOverride<TResultDet>

        public void Override(AutoMapping<TResultDet> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();
            mapping.OptimisticLock.Dirty();

            mapping.Table("dbo.T_RESULT_DET");
            mapping.Id(x => x.Id, "RESULT_DET_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.ResultId, "RESULT_ID").LazyLoad();
            mapping.References(x => x.GameId, "GAME_ID").LazyLoad();
            mapping.Map(x => x.ResultDetOrderNo, "RESULT_DET_ORDER_NO");
            mapping.Map(x => x.ResultDetNumber, "RESULT_DET_NUMBER");
            mapping.Map(x => x.ResultDetStatus, "RESULT_DET_STATUS");
            mapping.Map(x => x.ResultDetDesc, "RESULT_DET_DESC");

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
        }

        #endregion
    }
}
