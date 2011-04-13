using FluentNHibernate.Automapping;

using FluentNHibernate.Automapping.Alterations;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Data.NHibernateMaps.Trans
{
    public class TSalesMap : IAutoMappingOverride<TSales>
    {
        #region Implementation of IAutoMappingOverride<TSales>

        public void Override(AutoMapping<TSales> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();
            mapping.Cache.ReadWrite();

            mapping.Table("dbo.T_SALES");
            mapping.Id(x => x.Id, "SALES_ID")
                 .GeneratedBy.Assigned();

            mapping.References(x => x.AgentId, "AGENT_ID").LazyLoad(); 
            mapping.Map(x => x.SalesNo, "SALES_NO");
            mapping.Map(x => x.SalesDate, "SALES_DATE");
            mapping.Map(x => x.SalesTotal, "SALES_TOTAL");
            mapping.Map(x => x.SalesMustPaid, "SALES_MUST_PAID");
            mapping.Map(x => x.SalesStatus, "SALES_STATUS");
            mapping.Map(x => x.SalesDesc, "SALES_DESC");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Version(x => x.RowVersion).Column("ROW_VERSION");
        }

        #endregion
    }
}
