<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="YTech.Ltr.Web.Controllers" %>
<%@ Import Namespace="YTech.Ltr.Web.Controllers.Transaction" %>

<div id="accordion">
    <h3>
        <a href="#">Home</a></h3>
    <div>
        <div>
            <%=Html.ActionLinkForAreas<HomeController>(c => c.Index(), "Home") %></div>
    </div>
    <% if (Request.IsAuthenticated)
       {
%> 
    <h3>
        <a href="#">Data Pokok</a></h3>
    <div>
        <div>
           Master Agen
            </div>
        <div>
           Master Hadiah 
           </div>       
    </div>
   
    <h3>
        <a href="#">Transaksi</a></h3>
    <div>
        <div>
            Penjualan
            </div>
        <div>
           <%= Html.ActionLinkForAreas<ResultController>(c => c.Result(null), "Input Nomor Keluar")%>
           </div>
      
    </div>
    
    <h3>
        <a href="#">Laporan</a></h3>
    <div>
        <div>
            <%= Html.ActionLinkForAreas<ReportController>(c => c.Report(EnumReport.RptRecapSales), "Lap. Rekap Penjualan")%>
        </div>
        <div>
          
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
            Ganti Password</div>
        <div>
            Backup Database</div>
    </div>
    <%
        }
%>
</div>