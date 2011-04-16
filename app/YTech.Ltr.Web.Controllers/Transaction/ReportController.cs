﻿using System;
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
        public ReportController(IMAgentRepository mAgentRepository, ITSalesDetRepository tSalesDetRepository)
        {
            Check.Require(mAgentRepository != null, "mAgentRepository may not be null");
            Check.Require(tSalesDetRepository != null, "tSalesDetRepository may not be null");

            this._mAgentRepository = mAgentRepository;
            this._tSalesDetRepository = tSalesDetRepository;
        }

        [Transaction]
        public ActionResult Report(EnumReport reports)
        {
            ReportParamViewModel viewModel = ReportParamViewModel.Create(_mAgentRepository);
            string title = string.Empty;
            switch (reports)
            {
                case EnumReport.RptRecapSales:
                    title = "Lap. Rekap Penjualan";
                    viewModel.ShowDateFrom = true;
                    viewModel.ShowDateTo = true;
                    viewModel.ShowAgent = true;

                    break;
            }
            ViewData["CurrentItem"] = title;

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]      // Helps avoid CSRF attacks
        [Transaction]                   // Wraps a transaction around the action
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Report(EnumReport report, ReportParamViewModel viewModel, FormCollection formCollection)
        {
            ReportDataSource[] repCol = new ReportDataSource[1];
            switch (report)
            {
                case EnumReport.RptRecapSales:
                    repCol[0] = GetSalesDet(viewModel.DateFrom.Value, viewModel.DateTo.Value, viewModel.AgentId);
                    break;
            }
            Session["ReportData"] = repCol;

            var e = new
            {
                Success = true,
                Message = "redirect",
                UrlReport = string.Format("{0}", report.ToString())
            };
            return Json(e, JsonRequestBehavior.AllowGet);
        }

        private ReportDataSource GetSalesDet(DateTime dateFrom, DateTime dateTo, string agentId)
        {
            IList<TSalesDet> dets = _tSalesDetRepository.GetListByDateAndAgent(dateFrom, dateTo, agentId);

            var list = from det in dets
                       select new
                       {
                           AgentId = det.SalesId.AgentId.Id,
                           AgentName = det.SalesId.AgentId.AgentName,
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
    }
}