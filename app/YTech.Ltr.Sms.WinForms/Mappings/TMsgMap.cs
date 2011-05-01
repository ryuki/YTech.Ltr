using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using YTech.Ltr.Sms.WinForms.Entities;

namespace YTech.Ltr.Sms.WinForms.Mappings
{
    public class TMsgMap : ClassMap<TMsg>
    {
        public TMsgMap()
        {
            //DynamicUpdate();
            //DynamicInsert();
            //SelectBeforeUpdate();
            //Cache.ReadWrite();
            //OptimisticLock.Dirty();

            Table("T_MSG");
            Id(x => x.Id, "MSG_ID");
            Map(x => x.MsgFrom, "MSG_FROM");
            Map(x => x.MsgTo, "MSG_TO");
            Map(x => x.MsgDate, "MSG_DATE");
            Map(x => x.MsgText, "MSG_TEXT");
        }
    }
}
