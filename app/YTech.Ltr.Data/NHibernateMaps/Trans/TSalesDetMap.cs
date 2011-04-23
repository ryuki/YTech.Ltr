using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Data.NHibernateMaps.Trans
{
    public class TSalesDetMap : IAutoMappingOverride<TSalesDet>
    {
        #region Implementation of IAutoMappingOverride<TSalesDet>

        public void Override(AutoMapping<TSalesDet> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();
            mapping.OptimisticLock.Dirty();

            mapping.Table("T_SALES_DET");
            mapping.Id(x => x.Id, "SALES_DET_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.SalesId, "SALES_ID").LazyLoad();
            mapping.References(x => x.GameId, "GAME_ID").LazyLoad();
            mapping.Map(x => x.SalesDetNumber, "SALES_DET_NUMBER");
            mapping.Map(x => x.SalesDetValue, "SALES_DET_VALUE");
            mapping.Map(x => x.SalesDetComm, "SALES_DET_COMM");
            mapping.Map(x => x.SalesDetStatus, "SALES_DET_STATUS");
            mapping.Map(x => x.SalesDetDesc, "SALES_DET_DESC");
            mapping.Map(x => x.SalesDetPrize, "SALES_DET_PRIZE"); 

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
