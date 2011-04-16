<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<YTech.Ltr.Web.Controllers.ViewModel.ResultViewModel>"
    EnableViewState="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Ajax.BeginForm(new AjaxOptions
                                       {
                                           //UpdateTargetId = "status",
                                           InsertionMode = InsertionMode.Replace,
                                           OnSuccess = "onSavedSuccess"
                                       }

          ))
       { %>
    <%= Html.AntiForgeryToken() %>
    <table>
        <tr>
            <td colspan="2">
                <%=Html.TextBox("ResultDate", Model.ResultDate != null ? Model.ResultDate.Value.ToString(CommonHelper.DateFormat) : "", new { style = "width:120px;" })%>
                <input id="btnOk" type="button" value="OK" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="prizeD4_1">
                    1.</label>
            </td>
            <td>
                <input id="prizeD4_1" name="prizeD4_1" type="text" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="prizeD4_2">
                    2.</label>
            </td>
            <td>
                <input id="prizeD4_2" name="prizeD4_2" type="text" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="prizeD4_3">
                    3.</label>
            </td>
            <td>
                <input id="prizeD4_3" name="prizeD4_3" type="text" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="prizeD4_4_1">
                    4.</label>
            </td>
            <td>
                <input id="prizeD4_4_1" name="prizeD4_4_1" type="text" />
                <input id="prizeD4_4_6" name="prizeD4_4_6" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_4_2" name="prizeD4_4_2" type="text" />
                <input id="prizeD4_4_7" name="prizeD4_4_7" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_4_3" name="prizeD4_4_3" type="text" />
                <input id="prizeD4_4_8" name="prizeD4_4_8" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_4_4" name="prizeD4_4_4" type="text" />
                <input id="prizeD4_4_9" name="prizeD4_4_9" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_4_5" name="prizeD4_4_5" type="text" />
                <input id="prizeD4_4_10" name="prizeD4_4_10" type="text" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="prizeD4_5_1">
                    5.</label>
            </td>
            <td>
                <input id="prizeD4_5_1" name="prizeD4_5_1" type="text" />
                <input id="prizeD4_5_6" name="prizeD4_5_6" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_5_2" name="prizeD4_5_2" type="text" />
                <input id="prizeD4_5_7" name="prizeD4_5_7" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_5_3" name="prizeD4_5_3" type="text" />
                <input id="prizeD4_5_8" name="prizeD4_5_8" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_5_4" name="prizeD4_5_4" type="text" />
                <input id="prizeD4_5_9" name="prizeD4_5_9" type="text" />
            </td>
        </tr>
        <tr>
            <td>
               &nbsp;
            </td>
            <td>
                <input id="prizeD4_5_5" name="prizeD4_5_5" type="text" />
                <input id="prizeD4_5_10" name="prizeD4_5_10" type="text" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <input id="btnSubmit" type="submit" value="Simpan" /> &nbsp;&nbsp; 
                <input id="btnCalculate" type="button" value="Hitung" />
            </td>
        </tr>
    </table>
    <% } %>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false
            });
            $("#ResultDate").datepicker({ dateFormat: "dd-M-yy" });
            $("#ResultDate").change(function () {
                GetDetailPrize();
            });
            $("#btnOk").click(function () {
                GetDetailPrize();
            });
            $("#ResultDate").change();
            $("#btnCalculate").click(function () {
                CalculatePrize();
            });
        });

        function CalculatePrize() {
            var resultDate = $("#ResultDate").val();
            var results = $.ajax({ url: '<%= Url.Action("CalculatePrize","Result") %>?resultDate=' + resultDate, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the results.'); } }).responseText;
            var jsonResults = $.parseJSON(results);
            var success = jsonResults.Success;
            var msg = jsonResults.Message;
            $('#dialog p:first').text(msg);
            $("#dialog").dialog("open");
        }

        function GetDetailPrize() {
            $('#btnSubmit').attr('disabled', '');
            var resultDate = $("#ResultDate").val();
            var results = $.ajax({ url: '<%= Url.Action("GetDetailByDate","Result") %>?resultDate=' + resultDate, async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the results.'); } }).responseText;
            var jsonResults = $.parseJSON(results);
            //                alert(jsonResults.prizeD4_1);
            //                alert(jsonResults.prizeD4_2);
            //                alert(jsonResults.prizeD4_3);
            $("#prizeD4_1").val(jsonResults.prizeD4_1);
            $("#prizeD4_2").val(jsonResults.prizeD4_2);
            $("#prizeD4_3").val(jsonResults.prizeD4_3);
            $("#prizeD4_4_1").val(jsonResults.prizeD4_4_1);
            $("#prizeD4_4_2").val(jsonResults.prizeD4_4_2);
            $("#prizeD4_4_3").val(jsonResults.prizeD4_4_3);
            $("#prizeD4_4_4").val(jsonResults.prizeD4_4_4);
            $("#prizeD4_4_5").val(jsonResults.prizeD4_4_5);
            $("#prizeD4_4_6").val(jsonResults.prizeD4_4_6);
            $("#prizeD4_4_7").val(jsonResults.prizeD4_4_7);
            $("#prizeD4_4_8").val(jsonResults.prizeD4_4_8);
            $("#prizeD4_4_9").val(jsonResults.prizeD4_4_9);
            $("#prizeD4_4_10").val(jsonResults.prizeD4_4_10);

            $("#prizeD4_5_1").val(jsonResults.prizeD4_5_1);
            $("#prizeD4_5_2").val(jsonResults.prizeD4_5_2);
            $("#prizeD4_5_3").val(jsonResults.prizeD4_5_3);
            $("#prizeD4_5_4").val(jsonResults.prizeD4_5_4);
            $("#prizeD4_5_5").val(jsonResults.prizeD4_5_5);
            $("#prizeD4_5_6").val(jsonResults.prizeD4_5_6);
            $("#prizeD4_5_7").val(jsonResults.prizeD4_5_7);
            $("#prizeD4_5_8").val(jsonResults.prizeD4_5_8);
            $("#prizeD4_5_9").val(jsonResults.prizeD4_5_9);
            $("#prizeD4_5_10").val(jsonResults.prizeD4_5_10);
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
