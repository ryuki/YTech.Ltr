using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Data.NHibernateMaps.Master
{
    public class MAgentCommMap : IAutoMappingOverride<MAgentComm>
    {
        #region Implementation of IAutoMappingOverride<MAgentComm>

        public void Override(AutoMapping<MAgentComm> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("dbo.M_AGENT_COMM");
            mapping.Id(x => x.Id, "COMM_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.AgentId, "AGENT_ID").LazyLoad();
            mapping.References(x => x.GameId, "GAME_ID").LazyLoad();
            mapping.Map(x => x.CommValue, "COMM_VALUE");
            mapping.Map(x => x.CommStatus, "COMM_STATUS");
            mapping.Map(x => x.CommDesc, "COMM_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            //mapping.Version(x => x.RowVersion).Column("ROW_VERSION");

        }

        #endregion
    }
}
