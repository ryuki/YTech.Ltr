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
        public string prizeD4_4_1 { get; set; }
        public string prizeD4_4_2 { get; set; }
        public string prizeD4_4_3 { get; set; }
        public string prizeD4_4_4 { get; set; }
        public string prizeD4_4_5 { get; set; }
        public string prizeD4_4_6 { get; set; }
        public string prizeD4_4_7 { get; set; }
        public string prizeD4_4_8 { get; set; }
        public string prizeD4_4_9 { get; set; }
        public string prizeD4_4_10 { get; set; }

        public string prizeD4_5_1 { get; set; }
        public string prizeD4_5_2 { get; set; }
        public string prizeD4_5_3 { get; set; }
        public string prizeD4_5_4 { get; set; }
        public string prizeD4_5_5 { get; set; }
        public string prizeD4_5_6 { get; set; }
        public string prizeD4_5_7 { get; set; }
        public string prizeD4_5_8 { get; set; }
        public string prizeD4_5_9 { get; set; }
        public string prizeD4_5_10 { get; set; }

    }
}
