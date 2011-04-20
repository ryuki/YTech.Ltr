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
    public class SalesController : Controller
    {
        private readonly ITSalesRepository _tSalesRepository;
        private readonly ITSalesDetRepository _tSalesDetRepository;
        private readonly IMGameRepository _mGameRepository;
        private readonly IMAgentRepository _mAgentRepository;
        public SalesController(ITSalesRepository tSalesRepository, ITSalesDetRepository tSalesDetRepository, IMGameRepository mGameRepository, IMAgentRepository mAgentRepository)
        {
            Check.Require(tSalesRepository != null, "tSalesRepository may not be null");
            Check.Require(tSalesDetRepository != null, "tSalesDetRepository may not be null");
            Check.Require(mGameRepository != null, "mGameRepository may not be null");
            Check.Require(mAgentRepository != null, "mAgentRepository may not be null");

            this._tSalesRepository = tSalesRepository;
            this._tSalesDetRepository = tSalesDetRepository;
            this._mGameRepository = mGameRepository;
            this._mAgentRepository = mAgentRepository;
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
            string detNumber,gameId;
            decimal detValue = 0;

            for (int i = 0; i < 20; i++)
            {
                detNumber = formCollection[string.Format("txtSalesDetNumber_{0}", i)];
                if (!string.IsNullOrEmpty(detNumber))
                {
                    detValue = decimal.Parse(formCollection[string.Format("txtSalesDetValue_{0}", i)].Replace(",", ""));
                    gameId = formCollection[string.Format("gameId_{0}", i)];

                    det = new TSalesDet(sales);
                    det.SetAssignedIdTo(Guid.NewGuid().ToString());
                    det.SalesDetNumber = detNumber;
                    det.SalesDetValue = detValue;
                    if (!string.IsNullOrEmpty(detNumber))
                    {
                        det.GameId = _mGameRepository.Get(gameId);
                    }
                    det.CreatedDate = DateTime.Now;
                    det.CreatedBy = User.Identity.Name;
                    det.DataStatus = EnumDataStatus.New.ToString();
                    _tSalesDetRepository.Save(det);
                }
            }


        }
    }
}
