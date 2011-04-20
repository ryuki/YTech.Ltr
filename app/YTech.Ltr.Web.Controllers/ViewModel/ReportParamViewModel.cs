using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;

namespace YTech.Ltr.Web.Controllers.ViewModel
{
    public class ReportParamViewModel
    {
        public static ReportParamViewModel Create(IMAgentRepository mAgentRepository,IMGameRepository mGameRepository)
        {
            ReportParamViewModel viewModel = new ReportParamViewModel();

            IList<MAgent> list = mAgentRepository.GetAll();
            MAgent costCenter = new MAgent();
            costCenter.AgentName = "-Semua Agen-";
            list.Insert(0, costCenter);
            viewModel.AgentList = new SelectList(list, "Id", "AgentName");

            IList<MGame> listGame = mGameRepository.GetAll();
            MGame game = new MGame();
            game.GameName = "-Semua Game-";
            listGame.Insert(0, game);
            viewModel.GameList = new SelectList(listGame, "Id", "GameName");

            viewModel.DateFrom = DateTime.Today;
            viewModel.DateTo = DateTime.Today;
            return viewModel;
        }

        public bool ShowReport { get; internal set; }
        public string ExportFormat { get; internal set; }
        public string Title { get; internal set; }

        public bool ShowDateFrom { get; internal set; }
        public bool ShowDateTo { get; internal set; }
        public bool ShowAgent { get; internal set; }
        public bool ShowGame { get; internal set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string AgentId { get; set; }
        public string GameId { get; set; }

        public SelectList AgentList { get; internal set; }
        public SelectList GameList { get; internal set; }
    }
}
