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
    public class ReportController : Controller
    {
        private readonly IMAgentRepository _mAgentRepository;
        private readonly ITSalesDetRepository _tSalesDetRepository;
        private readonly IMGameRepository _mGameRepository;
        public ReportController(IMAgentRepository mAgentRepository, ITSalesDetRepository tSalesDetRepository, IMGameRepository mGameRepository)
        {
            Check.Require(mAgentRepository != null, "mAgentRepository may not be null");
            Check.Require(tSalesDetRepository != null, "tSalesDetRepository may not be null");
            Check.Require(mGameRepository != null, "mGameRepository may not be null");

            this._mAgentRepository = mAgentRepository;
            this._tSalesDetRepository = tSalesDetRepository;
            this._mGameRepository = mGameRepository;
        }

        [Transaction]
        public ActionResult Report(EnumReport rpt)
        {
            ReportParamViewModel viewModel = ReportParamViewModel.Create(_mAgentRepository, _mGameRepository);
            string title = string.Empty;
            switch (rpt)
            {
                case EnumReport.RptDetailSales:
                    title = "Lap. Detail Penjualan";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAgent = true;

                    break;
                case EnumReport.RptRecapSalesByAgent:
                    title = "Lap. Rekap Penjualan";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAgent = true;
                    viewModel.ShowGame = true;

                    break;
                case EnumReport.RptRecapSalesByGame:
                    title = "Lap. Rekap Penjualan Per Game";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAgent = true;
                    viewModel.ShowGame = true;

                    break;
                case EnumReport.RptRecapWinSales:
                    title = "Lap. Rekap Penjualan Yg Menang";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAgent = true;
                    viewModel.ShowGame = true;

                    break;
            }
            ViewData["CurrentItem"] = title;

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Report(EnumReport rpt, ReportParamViewModel viewModel, FormCollection formCollection)
        {
            ReportDataSource[] repCol = new ReportDataSource[1];
            switch (rpt)
            {
                case EnumReport.RptDetailSales:
                    repCol[0] = GetSalesDet(viewModel.DateFrom.Value, viewModel.DateTo.Value, viewModel.AgentId, viewModel.GameId);
                    break;
                case EnumReport.RptRecapSalesByAgent:
                    repCol[0] = GetSalesDet(viewModel.DateFrom.Value, viewModel.DateTo.Value, viewModel.AgentId, viewModel.GameId);
                    break;
                case EnumReport.RptRecapSalesByGame:
                    repCol[0] = GetSalesDet(viewModel.DateFrom.Value, viewModel.DateTo.Value, viewModel.AgentId, viewModel.GameId);
                    break;
                case EnumReport.RptRecapWinSales:
                    repCol[0] = GetSalesDet(viewModel.DateFrom.Value, viewModel.DateTo.Value, viewModel.AgentId, viewModel.GameId, EnumSalesDetStatus.Win.ToString());
                    break;
            }
            Session["ReportData"] = repCol;

            var e = new
            {
                Success = true,
                Message = "redirect",
                UrlReport = string.Format("{0}", rpt.ToString())
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private ReportDataSource GetSalesDet(DateTime dateFrom, DateTime dateTo, string agentId, string gameId, string salesDetStatus)
        {
            IList<TSalesDet> dets = _tSalesDetRepository.GetListByDateAndAgent(dateFrom, dateTo, agentId, gameId, salesDetStatus);

            var list = from det in dets
                       select new
                       {
                           AgentId = det.SalesId.AgentId != null ? det.SalesId.AgentId.Id : null,
                           AgentName = det.SalesId.AgentId != null ? det.SalesId.AgentId.AgentName : null,
                           det.GameId.GameName,
                           det.SalesDetComm,
                           det.SalesDetDesc,
                           det.SalesDetMustPaid,
                           det.SalesDetNumber,
                           det.SalesDetPrize,
                           det.SalesDetStatus,
                           det.SalesDetTotal,
                           det.SalesDetValue,
                           det.SalesId.SalesDate
                       }
            ;

            ReportDataSource reportDataSource = new ReportDataSource("SalesDetViewModel", list.ToList());
            return reportDataSource;
        }

        private ReportDataSource GetSalesDet(DateTime dateFrom, DateTime dateTo, string agentId, string gameId)
        {
            return GetSalesDet(dateFrom, dateTo, agentId, gameId, null);
        }
    }
}