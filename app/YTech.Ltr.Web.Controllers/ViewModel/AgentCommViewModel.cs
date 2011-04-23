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
    public class AgentCommViewModel
    {
        public static AgentCommViewModel Create(IMGameRepository mGameRepository)
        {
            AgentCommViewModel viewModel = new AgentCommViewModel();

            viewModel.ListGames = mGameRepository.GetAll();
            return viewModel;
        }

        public IList<MGame> ListGames { get; set; }
    }
}
