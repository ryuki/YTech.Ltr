<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function refreshEmployeeTable() {
            var ex = $('#tableAgentSales');
            var tbl = '<tr><th>Aksi</th><th>Game</th><th>Nomor</th><th>Nilai</th><th>Komisi (%)</th><th>Total</th></tr>';

            ex.html(tbl);

            for (var i = 0; i <= 20; i++) {
                tbl += '<tr>' +
                       '<td><input type="button" value="Clear" /></td>' +
                       '<td><select></select></td>' +
                       '<td><input />' +
                       '<td><input />' +
                       '<td><input />' +
                       '<td><input />' +
                       '</tr>';
            }

            ex.html(tbl);
        }

        $(document).ready(function () {
            refreshEmployeeTable();
        });
    </script>
    <div id="control" title="">
        Agen : <select></select>
        <br />
        Tanggal : <input id='txtSalesDate' name='txtSalesDate' type='text' value=''/>
    </div>
    <hr />
    <div id="tableform" title="" >
        <table id="tableAgentSales" style="width:100%;border-style:solid;">
        </table>
    </div>
</asp:Content>
