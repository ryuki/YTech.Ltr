using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;

namespace YTech.Ltr.Web.Controllers.ViewModel
{
    public class AgentCommViewModel
    {
        public static AgentCommViewModel Create(IMGameRepository mGameRepository, string agentId)
        {
            AgentCommViewModel viewModel = new AgentCommViewModel();

            IList<MGame> listGame = mGameRepository.GetAll();
            CommissionViewModel comm = null;
            IList<CommissionViewModel> list = new List<CommissionViewModel>();
            MGame game = null;
            for (int i = 0; i < listGame.Count; i++)
            {
                comm = new CommissionViewModel();
                game = listGame[i];
                comm.GameId = game.Id;
                comm.GameName = game.GameName;
                comm.Commission = mGameRepository.GetCommissionByGameAndAgent(agentId, game.Id);
                list.Add(comm);
            }
            viewModel.ListComms = list;
            return viewModel;
        }

        public IList<CommissionViewModel> ListComms { get; set; }
    }

    public class CommissionViewModel
    {
        public string GameName { get; set; }
        public string GameId { get; set; }
        public decimal? Commission { get; set; }
    }
}
