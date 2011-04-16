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

            //get prize for D2 and D3 games
            string prizeD2 = viewModel.prizeD4_1.Substring(2, 2);
            string prizeD3 = viewModel.prizeD4_1.Substring(1, 3);

            //save details
            SaveResultDet(result, EnumGame.D2, 1, prizeD2);
            SaveResultDet(result, EnumGame.D3, 1, prizeD3);
            SaveResultDet(result, EnumGame.D4_1, 1, viewModel.prizeD4_1);
            SaveResultDet(result, EnumGame.D4_2, 1, viewModel.prizeD4_2);
            SaveResultDet(result, EnumGame.D4_3, 1, viewModel.prizeD4_3);
            SaveResultDet(result, EnumGame.D4_4, 1, viewModel.prizeD4_4_1);
            SaveResultDet(result, EnumGame.D4_4, 2, viewModel.prizeD4_4_2);
            SaveResultDet(result, EnumGame.D4_4, 3, viewModel.prizeD4_4_3);
            SaveResultDet(result, EnumGame.D4_4, 4, viewModel.prizeD4_4_4);
            SaveResultDet(result, EnumGame.D4_4, 5, viewModel.prizeD4_4_5);
            SaveResultDet(result, EnumGame.D4_4, 6, viewModel.prizeD4_4_6);
            SaveResultDet(result, EnumGame.D4_4, 7, viewModel.prizeD4_4_7);
            SaveResultDet(result, EnumGame.D4_4, 8, viewModel.prizeD4_4_8);
            SaveResultDet(result, EnumGame.D4_4, 9, viewModel.prizeD4_4_9);
            SaveResultDet(result, EnumGame.D4_4, 10, viewModel.prizeD4_4_10);

            SaveResultDet(result, EnumGame.D4_5, 1, viewModel.prizeD4_5_1);
            SaveResultDet(result, EnumGame.D4_5, 2, viewModel.prizeD4_5_2);
            SaveResultDet(result, EnumGame.D4_5, 3, viewModel.prizeD4_5_3);
            SaveResultDet(result, EnumGame.D4_5, 4, viewModel.prizeD4_5_4);
            SaveResultDet(result, EnumGame.D4_5, 5, viewModel.prizeD4_5_5);
            SaveResultDet(result, EnumGame.D4_5, 6, viewModel.prizeD4_5_6);
            SaveResultDet(result, EnumGame.D4_5, 7, viewModel.prizeD4_5_7);
            SaveResultDet(result, EnumGame.D4_5, 8, viewModel.prizeD4_5_8);
            SaveResultDet(result, EnumGame.D4_5, 9, viewModel.prizeD4_5_9);
            SaveResultDet(result, EnumGame.D4_5, 10, viewModel.prizeD4_5_10);

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
            string prizeD4_4_1 = string.Empty;
            string prizeD4_4_2 = string.Empty;
            string prizeD4_4_3 = string.Empty;
            string prizeD4_4_4 = string.Empty;
            string prizeD4_4_5 = string.Empty;
            string prizeD4_4_6 = string.Empty;
            string prizeD4_4_7 = string.Empty;
            string prizeD4_4_8 = string.Empty;
            string prizeD4_4_9 = string.Empty;
            string prizeD4_4_10 = string.Empty;

            string prizeD4_5_1 = string.Empty;
            string prizeD4_5_2 = string.Empty;
            string prizeD4_5_3 = string.Empty;
            string prizeD4_5_4 = string.Empty;
            string prizeD4_5_5 = string.Empty;
            string prizeD4_5_6 = string.Empty;
            string prizeD4_5_7 = string.Empty;
            string prizeD4_5_8 = string.Empty;
            string prizeD4_5_9 = string.Empty;
            string prizeD4_5_10 = string.Empty;


            //check if list return data, get result detail
            if (list.Count > 0)
            {
                prizeD4_1 = GetResultDet(list, EnumGame.D4_1.ToString(), 1);
                prizeD4_2 = GetResultDet(list, EnumGame.D4_2.ToString(), 1);
                prizeD4_3 = GetResultDet(list, EnumGame.D4_3.ToString(), 1);
                prizeD4_4_1 = GetResultDet(list, EnumGame.D4_4.ToString(), 1);
                prizeD4_4_2 = GetResultDet(list, EnumGame.D4_4.ToString(), 2);
                prizeD4_4_3 = GetResultDet(list, EnumGame.D4_4.ToString(), 3);
                prizeD4_4_4 = GetResultDet(list, EnumGame.D4_4.ToString(), 4);
                prizeD4_4_5 = GetResultDet(list, EnumGame.D4_4.ToString(), 5);
                prizeD4_4_6 = GetResultDet(list, EnumGame.D4_4.ToString(), 6);
                prizeD4_4_7 = GetResultDet(list, EnumGame.D4_4.ToString(), 7);
                prizeD4_4_8 = GetResultDet(list, EnumGame.D4_4.ToString(), 8);
                prizeD4_4_9 = GetResultDet(list, EnumGame.D4_4.ToString(), 9);
                prizeD4_4_10 = GetResultDet(list, EnumGame.D4_4.ToString(), 10);

                prizeD4_5_1 = GetResultDet(list, EnumGame.D4_5.ToString(), 1);
                prizeD4_5_2 = GetResultDet(list, EnumGame.D4_5.ToString(), 2);
                prizeD4_5_3 = GetResultDet(list, EnumGame.D4_5.ToString(), 3);
                prizeD4_5_4 = GetResultDet(list, EnumGame.D4_5.ToString(), 4);
                prizeD4_5_5 = GetResultDet(list, EnumGame.D4_5.ToString(), 5);
                prizeD4_5_6 = GetResultDet(list, EnumGame.D4_5.ToString(), 6);
                prizeD4_5_7 = GetResultDet(list, EnumGame.D4_5.ToString(), 7);
                prizeD4_5_8 = GetResultDet(list, EnumGame.D4_5.ToString(), 8);
                prizeD4_5_9 = GetResultDet(list, EnumGame.D4_5.ToString(), 9);
                prizeD4_5_10 = GetResultDet(list, EnumGame.D4_5.ToString(), 10);
            }

            //json it
            var res = new
            {
                prizeD4_1,
                prizeD4_2,
                prizeD4_3,
                prizeD4_4_1,
                prizeD4_4_2,
                prizeD4_4_3,
                prizeD4_4_4,
                prizeD4_4_5,
                prizeD4_4_6,
                prizeD4_4_7,
                prizeD4_4_8,
                prizeD4_4_9,
                prizeD4_4_10,
                prizeD4_5_1,
                prizeD4_5_2,
                prizeD4_5_3,
                prizeD4_5_4,
                prizeD4_5_5,
                prizeD4_5_6,
                prizeD4_5_7,
                prizeD4_5_8,
                prizeD4_5_9,
                prizeD4_5_10,
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        private string GetResultDet(IList<TResultDet> list, string gameId, int orderNo)
        {
            return (from det in list
                    where det.GameId.Id == gameId && det.ResultDetOrderNo == orderNo
                    select det.ResultDetNumber).First();
        }

        [Transaction]
        public ActionResult CalculatePrize(DateTime? resultDate)
        {
            bool Success = true;
            string Message = string.Empty;
            _tResultRepository.DbContext.BeginTransaction();

            try
            {
                _tResultRepository.CalculatePrize(resultDate.Value);
                _tResultRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                Success = true;
                Message = "Nomor keluar berhasil dihitung.";
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
    }
}
