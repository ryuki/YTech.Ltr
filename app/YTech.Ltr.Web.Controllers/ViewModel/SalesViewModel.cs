using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Web.Controllers.ViewModel
{
    public class SalesViewModel
    {
        public static SalesViewModel Create(IMAgentRepository mAgentRepository)
        {
            SalesViewModel viewModel = new SalesViewModel();

            viewModel.SalesDate = DateTime.Today;

            IList<MAgent> list = mAgentRepository.GetAll();
            MAgent agent = new MAgent();
            agent.AgentName = "-Pilih Agen-";
            list.Insert(0, agent);
            viewModel.AgentList = new SelectList(list, "Id", "AgentName");
            viewModel.SalesId = Guid.NewGuid().ToString();
            return viewModel;
        }

        public string SalesId { get; set; }
        public DateTime? SalesDate { get; set; }
        public SelectList AgentList { get; set; }
        public string AgentId { get; set; }
    }
}
