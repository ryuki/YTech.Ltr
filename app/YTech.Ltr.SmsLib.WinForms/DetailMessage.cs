using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.Ltr.SmsLib.WinForms
{
    public class DetailMessage
    {
        public string GameId { get; set; }
        public string SalesNumber { get; set; }
        public decimal? SalesValue { get; set; }
        public bool IsBB { get; set; }
        public bool IsHBR { get; set; }
        public string SalesDesc { get; set; }
    }
}
