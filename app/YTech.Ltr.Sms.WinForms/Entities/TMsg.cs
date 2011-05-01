using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.Ltr.Sms.WinForms.Entities
{
    public class TMsg
    {
        public virtual string Id { get; set; }
        public virtual string MsgFrom { get; set; }
        public virtual string MsgTo { get; set; }
        public virtual DateTime? MsgDate { get; set; }
        public virtual string MsgText { get; set; }
    }
}
