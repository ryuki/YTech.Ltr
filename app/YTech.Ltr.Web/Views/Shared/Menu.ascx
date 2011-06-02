<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.Ltr.Web.Controllers.Transaction" %>
<%@ Import Namespace="YTech.Ltr.Web.Controllers.Master" %>

<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div>
        <div>
            <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %></div>
    </div>
    <% //if (Request.IsAuthenticated)
       {
%> 
    <h3>
        <a href="#">Data Pokok</a></h3>
    <div>
        <div>
           <%=Html.ActionLinkForAreas<AgentController>(c => c.Index(), "Master Agen") %>
            </div>     
    </div>
   
    <h3>
        <a href="#">Transaksi</a></h3>
    <div>
        <div>
             <%= Html.ActionLinkForAreas<SalesController>(c => c.Sales(), "Input Penjualan")%>
            </div>
        <div>
           <%= Html.ActionLinkForAreas<ResultController>(c => c.Result(null), "Input Nomor Keluar")%>
           </div>
      
    </div>
    
    <h3>
        <a href="#">Laporan</a></h3>
    <div>
        <div>
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReport.RptDetailSales), "Lap. Detail Penjualan")%>
        </div>
        <div>
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReport.RptRecapSalesByAgent), "Lap. Rekap Penjualan")%>
        </div>
        <div>
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReport.RptRecapSalesByGame), "Lap. Rekap Penjualan Per Game")%>
        </div>
        <div>
           <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReport.RptRecapWinSales), "Lap. Rekap Penjualan Yg Menang")%>
        </div>
        <div>
            <hr />
        </div>
       
    </div>
  
  
    <h3>
        <a href="#">Utiliti</a></h3>
<div>
    <div>
</div>
        <div>
            Backup Database</div>
    </div>
    <%
        }
%>
</div>