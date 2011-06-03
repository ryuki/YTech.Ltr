using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using SharpArch.Core;
using SharpArch.Web.NHibernate;
using YTech.Ltr.ApplicationServices;
using YTech.Ltr.ApplicationServices.Helper;
using YTech.Ltr.Core.Master;
using YTech.Ltr.Core.RepositoryInterfaces;
using YTech.Ltr.Core.Trans;
using YTech.Ltr.Data.Repository;
using YTech.Ltr.Enums;
using YTech.Ltr.Web.Controllers.Helper;
using YTech.Ltr.Web.Controllers.ViewModel;
using Microsoft.Reporting.WebForms;
using YTech.Ltr.Web.Controllers.ViewModel.Report;

namespace YTech.Ltr.Web.Controllers.Transaction
{
    [HandleError]
    public class SalesController : Controller
    {
        private readonly ITSalesRepository _tSalesRepository;
        private readonly ITSalesDetRepository _tSalesDetRepository;
        private readonly IMGameRepository _mGameRepository;
        private readonly IMAgentRepository _mAgentRepository;
        private readonly ITResultRepository _tResultRepository;

        public SalesController(ITSalesRepository tSalesRepository, ITSalesDetRepository tSalesDetRepository, IMGameRepository mGameRepository, IMAgentRepository mAgentRepository, ITResultRepository tResultRepository)
        {
            Check.Require(tSalesRepository != null, "tSalesRepository may not be null");
            Check.Require(tSalesDetRepository != null, "tSalesDetRepository may not be null");
            Check.Require(mGameRepository != null, "mGameRepository may not be null");
            Check.Require(mAgentRepository != null, "mAgentRepository may not be null");
            Check.Require(tResultRepository != null, "tResultRepository may not be null");

            this._tSalesRepository = tSalesRepository;
            this._tSalesDetRepository = tSalesDetRepository;
            this._mGameRepository = mGameRepository;
            this._mAgentRepository = mAgentRepository;
            this._tResultRepository = tResultRepository;
        }

        [Transaction]
        public ActionResult Sales()
        {
            SalesViewModel viewModel = SalesViewModel.Create(_mAgentRepository);

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sales(SalesViewModel viewModel, FormCollection formCollection)
        {
            _tSalesRepository.DbContext.BeginTransaction();
            TSales sales = _tSalesRepository.Get(viewModel.SalesId);
            if (sales != null)
                _tSalesRepository.Delete(sales);

            sales = new TSales();
            sales.SetAssignedIdTo(Guid.NewGuid().ToString());
            sales.SalesDate = viewModel.SalesDate;
            sales.SalesNo = viewModel.SalesNo;
            if (!string.IsNullOrEmpty(viewModel.AgentId))
            {
                sales.AgentId = _mAgentRepository.Get(viewModel.AgentId);
            }

            sales.CreatedDate = DateTime.Now;
            sales.CreatedBy = User.Identity.Name;
            sales.DataStatus = EnumDataStatus.New.ToString();
            sales.SalesDets.Clear();
            _tSalesRepository.Save(sales);

            SaveSalesDets(sales, formCollection);

            bool Success = true;
            string Message = string.Empty;
            try
            {
                _tSalesRepository.DbContext.CommitTransaction();
                TempData[EnumCommonViewData.SaveState.ToString()] = EnumSaveState.Success;
                Success = true;
                Message = "Penjualan berhasil disimpan.";
            }
            catch (Exception ex)
            {
                _tSalesRepository.DbContext.RollbackTransaction();
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

        private void SaveSalesDets(TSales sales, FormCollection formCollection)
        {
            TSalesDet det = null;
            string detNumber, gameId;
            decimal detValue = 0;
            IDictionary<string, MGame> dictGame = GetDictGame();
            MGame game = null;
            var agentComms = (from agentComm in sales.AgentId.AgentComms
                              select agentComm).ToList();
            decimal? comm = null;

            for (int i = 0; i < 40; i++)
            {
                detNumber = formCollection[string.Format("txtSalesDetNumber_{0}", i)];
                if (!string.IsNullOrEmpty(detNumber))
                {
                    detValue = decimal.Parse(formCollection[string.Format("txtSalesDetValue_{0}", i)].Replace(",", ""));
                    gameId = formCollection[string.Format("gameId_{0}", i)];
                    if (!string.IsNullOrEmpty(gameId))
                    {
                        if (gameId.Equals("D4.BB"))
                        {
                            dictGame.TryGetValue("D4", out game);
                        }
                        else if (gameId.Equals("D3.BB"))
                        {
                            dictGame.TryGetValue("D3", out game);
                        }
                        else
                        {
                            dictGame.TryGetValue(gameId, out game);
                        }
                        if (agentComms.Count > 0)
                        {
                            comm = (from agentComm in agentComms
                                    where agentComm.GameId == game
                                    select agentComm.CommValue).First();
                        }

                    }

                    //recursive and calculate for BB
                    if (gameId.Equals("D4.BB"))
                    {
                        SaveDetsForBB(sales, detNumber, detValue, game, comm, 4);
                    }
                    else if (gameId.Equals("D3.BB"))
                    {
                        SaveDetsForBB(sales, detNumber, detValue, game, comm, 3);
                    }
                    else
                    {
                        SaveSalesDet(sales, detNumber, detValue, game, comm, null);
                    }
                }
            }
        }

        private void SaveDetsForBB(TSales sales, string detNumber, decimal detValue, MGame gameId, decimal? comm, int leng)
        {
            var result = detNumber.AllPermutations().Where(x => x.Length == leng);
            foreach (var res in result)
            {
                SaveSalesDet(sales, res, detValue, gameId, comm, string.Format("BB : {0}", detNumber));
            }
        }

        private IDictionary<string, MGame> GetDictGame()
        {
            IDictionary<string, MGame> dictGame = new Dictionary<string, MGame>();
            IList<MGame> listGame = _mGameRepository.GetAll();
            foreach (MGame game in listGame)
            {
                dictGame.Add(new KeyValuePair<string, MGame>(game.Id, game));
            }
            return dictGame;
        }

        private void SaveSalesDet(TSales sales, string detNumber, decimal detValue, MGame gameId, decimal? comm, string desc)
        {
            TSalesDet det = new TSalesDet(sales);
            det.SetAssignedIdTo(Guid.NewGuid().ToString());
            det.SalesDetNumber = detNumber;
            det.SalesDetValue = detValue;
            det.GameId = gameId;
            det.SalesDetComm = comm;
            det.SalesDetDesc = desc;
            det.CreatedDate = DateTime.Now;
            det.CreatedBy = User.Identity.Name;
            det.DataStatus = EnumDataStatus.New.ToString();
            _tSalesDetRepository.Save(det);
        }


        [Transaction]
        public ActionResult SalesRecap()
        {
            return View();
        }

        [Transaction]
        public ActionResult ListSalesRecap(string sidx, string sord, int page, int rows)
        {
            int totalRecords = 0;
            IList recaps = _tSalesRepository.GetSalesRecap(sidx, sord, page, rows, ref totalRecords);
            IList<RecapSalesViewModel> listResult = new List<RecapSalesViewModel>();
            RecapSalesViewModel det = new RecapSalesViewModel();
            object[] obj;
            for (int i = 0; i < recaps.Count; i++)
            {
                obj = (object[])recaps[i];
                det = new RecapSalesViewModel();
                det.SalesDate = Convert.ToDateTime(obj[0]);
                det.CountSalesDet = Convert.ToInt32(obj[1]);
                listResult.Add(det);
            }
            int pageSize = rows;
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from result in listResult
                    select new
                    {
                        i = result.SalesDate,
                        cell = new string[] {
                           result.SalesDate.HasValue ? result.SalesDate.Value.ToString(Helper.CommonHelper.DateFormat):null,
                           result.SalesDate.HasValue ? result.SalesDate.Value.ToString(Helper.CommonHelper.DateFormat):null, 
                           result.CountSalesDet.HasValue ? result.CountSalesDet.Value.ToString() : null
                        }
                    }).ToArray()
            };


            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [Transaction]
        public ActionResult DeleteSales(RecapSalesViewModel viewModel, FormCollection formCollection)
        {
            DateTime salesDate = Convert.ToDateTime(formCollection["id"]);
            _tSalesRepository.DbContext.BeginTransaction();
            try
            {
                _tSalesRepository.DeleteByDate(salesDate);
                TResult result = _tResultRepository.GetResultByDate(salesDate);
                if (result != null)
                    _tResultRepository.Delete(result);

                _tSalesRepository.DbContext.CommitChanges();
            }
            catch (Exception e)
            {
                _tSalesRepository.DbContext.RollbackTransaction();
                return Content(e.GetBaseException().Message);
            }

            return Content("Data Penjualan berhasil dihapus");
        }
    }
}
