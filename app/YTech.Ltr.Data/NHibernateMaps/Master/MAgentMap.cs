using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Master;

namespace YTech.Ltr.Data.NHibernateMaps.Master
{
    public class MAgentMap : IAutoMappingOverride<MAgent>
    {
        #region Implementation of IAutoMappingOverride<MAgent>

        public void Override(AutoMapping<MAgent> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("dbo.M_AGENT");
            mapping.Id(x => x.Id, "AGENT_ID")
                 .GeneratedBy.Assigned();

            mapping.Map(x => x.AgentName, "AGENT_NAME");
            mapping.Map(x => x.AgentStatus, "AGENT_STATUS");
            mapping.Map(x => x.AgentDesc, "AGENT_DESC");

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
