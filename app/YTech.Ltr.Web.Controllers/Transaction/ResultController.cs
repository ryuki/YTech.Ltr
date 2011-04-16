using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;
using YTech.Ltr.Data.Repository;
using YTech.Ltr.Enums;
using YTech.Ltr.Web.Controllers.ViewModel;
using Microsoft.Reporting.WebForms;
using YTech.Ltr.Web.Controllers.ViewModel.Report;

namespace YTech.Ltr.Web.Controllers.Transaction
{
    [HandleError]
    public class ResultController : Controller
    {
        private readonly ITResultRepository _tResultRepository;
        private readonly ITResultDetRepository _tResultDetRepository;
        private readonly IMGameRepository _mGameRepository;
        public ResultController(ITResultRepository tResultRepository, ITResultDetRepository tResultDetRepository, IMGameRepository mGameRepository)
        {
            Check.Require(tResultRepository != null, "tResultRepository may not be null");
            Check.Require(tResultDetRepository != null, "tResultDetRepository may not be null");
            Check.Require(mGameRepository != null, "mGameRepository may not be null");

            this._tResultRepository = tResultRepository;
            this._tResultDetRepository = tResultDetRepository;
            this._mGameRepository = mGameRepository;
        }

        [Transaction]
        public ActionResult Result(DateTime? resultDate)
        {
            ResultViewModel viewModel = ResultViewModel.Create(_tResultRepository, resultDate);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Result(ResultViewModel viewModel, FormCollection formCollection)
        {
            _tResultRepository.DbContext.BeginTransaction();
            TResult result = _tResultRepository.GetResultByDate(viewModel.ResultDate.Value);
            if (result != null)
                _tResultRepository.Delete(result);

            result = new TResult();
            result.SetAssignedIdTo(Guid.NewGuid().ToString());
            result.ResultDate = viewModel.ResultDate;
            result.CreatedDate = DateTime.Now;
            result.CreatedBy = User.Identity.Name;
            result.DataStatus = EnumDataStatus.New.ToString();
            result.ResultDets.Clear();
            _tResultRepository.Save(result);

            SaveResultDet(result, EnumGame.D4_1, 1, viewModel.prizeD4_1);
            SaveResultDet(result, EnumGame.D4_2, 1, viewModel.prizeD4_2);
            SaveResultDet(result, EnumGame.D4_3, 1, viewModel.prizeD4_3);

            bool Success = true;
            string Message = string.Empty;
            try
            {
                _tResultRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                Success = true;
                Message = "Nomor keluar berhasil disimpan.";
            }
            catch (Exception ex)
            {
                _tResultRepository.DbContext.RollbackTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Failed;
                Success = false;
                Message = ex.GetBaseException().Message;
            }

            var e = new
            {
                Success,
                Message
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private void SaveResultDet(TResult result, EnumGame game, int orderNo, string resultNumber)
        {
            TResultDet det = new TResultDet(result);
            det.SetAssignedIdTo(Guid.NewGuid().ToString());
            det.ResultDetOrderNo = orderNo;
            det.ResultDetNumber = resultNumber;
            det.GameId = _mGameRepository.Get(game.ToString());
            det.CreatedDate = DateTime.Now;
            det.CreatedBy = User.Identity.Name;
            det.DataStatus = EnumDataStatus.New.ToString();
            _tResultDetRepository.Save(det);
        }

        [Transaction]
        public ActionResult GetDetailByDate(DateTime? resultDate)
        {
            IList<TResultDet> list = _tResultDetRepository.GetListByDate(resultDate.Value);
            string prizeD4_1 = string.Empty;
            string prizeD4_2 = string.Empty;
            string prizeD4_3 = string.Empty;

            //check if list return data, get result detail
            if (list.Count > 0)
            {
                prizeD4_1 = GetResultDet(list, EnumGame.D4_1.ToString(), 1);
                prizeD4_2 = GetResultDet(list, EnumGame.D4_2.ToString(), 1);
                prizeD4_3 = GetResultDet(list, EnumGame.D4_3.ToString(), 1);  
            }

            //json it
            var res = new
            {
                prizeD4_1,
                prizeD4_2,
                prizeD4_3
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private string GetResultDet(IList<TResultDet> list, string gameId, int orderNo)
        {
            return (from det in list
                    where det.GameId.Id == gameId && det.ResultDetOrderNo == orderNo
                    select det.ResultDetNumber).First();
        }
    }
}
