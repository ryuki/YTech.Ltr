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
    public class ResultViewModel
    {
        public static ResultViewModel Create(ITResultRepository tResultRepository, DateTime? resultDate)
        {
            ResultViewModel viewModel = new ResultViewModel();

            //IList<TResult> list = tResultRepository.GetAll();
            viewModel.ResultDate = DateTime.Today;
            return viewModel;
        }

        public DateTime? ResultDate { get; set; }
        public string prizeD4_1 { get; set; }
        public string prizeD4_2 { get; set; }
        public string prizeD4_3 { get; set; }
    }
}
