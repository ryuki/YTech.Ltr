using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Data.NHibernateMaps.Trans
{
    public class TMsgMap : IAutoMappingOverride<TMsg>
    {
        #region Implementation of IAutoMappingOverride<TMsg>

        public void Override(AutoMapping<TMsg> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();
            mapping.OptimisticLock.Dirty();

            mapping.Table("T_MSG");
            mapping.Id(x => x.Id, "MSG_ID")
                 .GeneratedBy.Assigned(); 
            mapping.Map(x => x.MsgFrom, "MSG_FROM");
            mapping.Map(x => x.MsgTo, "MSG_TO");
            mapping.Map(x => x.MsgDate, "MSG_DATE");
            mapping.Map(x => x.MsgText, "MSG_TEXT");
            mapping.Map(x => x.MsgStatus, "MSG_STATUS");
            mapping.Map(x => x.MsgDesc, "MSG_DESC");

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
