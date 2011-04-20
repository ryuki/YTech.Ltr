<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<YTech.Ltr.Web.Controllers.ViewModel.SalesViewModel>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
       { %>
    <%= Html.AntiForgeryToken() %>
<%= Html.Hidden("SalesId", ViewData.Model.SalesId)%>
    <div id="control" title="">
    <input id='btnSubmit' type='submit' value='Simpan'/>
        <table>
            <tr>
                <td>
                    Agen :
                </td>
                <td>
                    <%= Html.DropDownList("AgentId", Model.AgentList)%>
                </td>
            </tr>
            <tr>
                <td>
                    Tanggal :
                </td>
                <td>
                    <%=Html.TextBox("SalesDate", Model.SalesDate != null ? Model.SalesDate.Value.ToString(CommonHelper.DateFormat) : "", new { style = "width:120px;" })%>
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <div id="tableform" title="">
        <table id="tableAgentSales" style="width: 100%; border-style: solid;">
        </table>
    </div>
    
    <% } %>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });
            $("#SalesDate").datepicker({ dateFormat: "dd-M-yy" });
            refreshSalesDets();
        });

        function refreshSalesDets() {
            var ex = $('#tableAgentSales');
            var tbl = '<tr><th align="center">Aksi</th><th align="center">Game</th><th align="center">Nomor</th><th align="center">Nilai</th></tr>';

            var detNumberId, detValueId;
            for (var i = 0; i <= 20; i++) {
                detNumberId = 'txtSalesDetNumber_' + i;
                detValueId = 'txtSalesDetValue_' + i;
                //alert(detNumberId);
                //alert(detValueId);
                tbl += '<tr>' +
                       '<td><input type="button" value="Clear" /></td>' +
                       '<td><select  id="gameId_' + i + '" name="gameId_' + i + '"><option value="D2">D2</option><option value="D3">D3</option><option value="D4">D4</option></select></td>' +
                       '<td><input id="' + detNumberId + '" name="' + detNumberId + '" type="text" /></td>' +
                       '<td><input id="' + detValueId + '" name="' + detValueId + '" type="text" /></td>' +
                       '</tr>';
            }

            ex.html(tbl);
        }
        
        function onSavedSuccess(e) {
            var json = e.get_response().get_object();
            var success = json.Success;
            var msg = json.Message;
            if (success == false) {
                if (success) {
                    $('#btnSubmit').attr('disabled', 'disabled');
                }
            }
            $('#dialog p:first').text(msg);
            $("#dialog").dialog("open");
        }
    </script>
</asp:Content>
