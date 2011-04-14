using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Web.Controllers.ViewModel.Report
{
   public class SalesDetViewModel : TSalesDet
   {
       public string AgentName { get; set; }
       public DateTime? SalesDate { get; set; }
       public string GameName { get; set; }
    }
}
