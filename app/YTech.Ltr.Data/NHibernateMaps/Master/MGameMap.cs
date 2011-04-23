using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Data.NHibernateMaps.Master
{
    public class MGameMap : IAutoMappingOverride<MGame>
    {
        #region Implementation of IAutoMappingOverride<MGame>

        public void Override(AutoMapping<MGame> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();
            mapping.OptimisticLock.Dirty();

            mapping.Table("M_GAME");
            mapping.Id(x => x.Id, "GAME_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.GameName, "GAME_NAME");
            mapping.Map(x => x.GamePrize, "GAME_PRIZE");
            mapping.Map(x => x.GameStatus, "GAME_STATUS");
            mapping.Map(x => x.GameDesc, "GAME_DESC");

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
