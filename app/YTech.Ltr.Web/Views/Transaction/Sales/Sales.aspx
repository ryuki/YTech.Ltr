<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<YTech.Ltr.Web.Controllers.ViewModel.SalesViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .yellowbg
        {
            background-color: Yellow;
        }
        .inputform
        {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if (false)
       { %>
    <script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
    <% } %>
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
        <input id='btnSubmit' type='submit' value='Simpan' />
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
            <tr>
                <td>
                    No :
                </td>
                <td>
                    <%=Html.TextBox("SalesNo", Model.SalesNo, new { style = "width:120px;" })%>
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
                autoOpen: false,
                modal: true,
                buttons: {
                    "Tutup": function () {
                        $(this).dialog("close");
                    },
                    "Tutup dan Input Baru": function () {
                        window.location.href = window.location.href;
                    }
                }
            });
            $("#SalesDate").datepicker({ dateFormat: "dd-M-yy" });
            refreshSalesDets();
        });

        function refreshSalesDets() {
            var ex = $('#tableAgentSales');
            var tbl = '<tr>';
            var no = 0;

            var detNumberId, detValueId, gameId;
            for (var j = 0; j <= 1; j++) {
                tbl += '<td><table><tr><th align="center">Aksi</th><th align="center">Game</th><th align="center">Nomor</th><th align="center">Nilai</th></tr>';
                for (var i = 0; i < 20; i++) {
                    no = (j * 20) + i;
                    detNumberId = 'txtSalesDetNumber_' + no;
                    detValueId = 'txtSalesDetValue_' + no;
                    gameId = 'gameId_' + no;
                    //alert(detNumberId);
                    //alert(detValueId);
                    tbl += '<tr>' +
                       '<td><input type="button" value="Clear" class="clear" /></td>' +
                       '<td><select  id="' + gameId + '" name="' + gameId + '"><option value="D1">D1</option><option value="WING">WING</option><option value="D2" selected>D2</option><option value="D3">D3</option><option value="D4">D4</option><option value="D3.BB">D3 (BB)</option><option value="D4.BB">D4 (BB)</option></select></td>' +
                       '<td><input id="' + detNumberId + '" name="' + detNumberId + '" type="text" class="inputform" /></td>' +
                       '<td><input id="' + detValueId + '" name="' + detValueId + '" type="text" class="number inputform" /></td>' +
                       '</tr>';
                }
                tbl += '</td></table>';
            }

            ex.html(tbl);
            $(".number").autoNumeric();
            $(".number").attr("style", "text-align:right;");
            $(".clear").click(function () {
                $(this).parent().parent().find("input").not(":first").val("");
            });
            $(".clear").parent().parent().hover(
                function () {
                    $(this).addClass("yellowbg");
                },
                function () {
                    $(this).removeClass("yellowbg");
                }
            );

        }

        function onSavedSuccess(e) {
            var json = e.get_response().get_object();
            var success = json.Success;
            var msg = json.Message;
            if (success) {
                $('#btnSubmit').attr('disabled', 'disabled');
            }
            $('#dialog p:first').text(msg);
            $("#dialog").dialog("open");
        }
    </script>
</asp:Content>
