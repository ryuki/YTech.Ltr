<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPopup.master" Inherits="System.Web.Mvc.ViewPage<AgentCommViewModel>" %>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">
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
        <thead>
            <tr>
                <th>
                    Game
                </th>
                <th>
                    Komisi
                </th>
            </tr>
        </thead>
        <tbody>
            <%
                CommissionViewModel comm;
                for (int i = 0; i < Model.ListComms.Count; i++)
                {
                    comm = Model.ListComms[i]; %>
            <tr>
                <td>
                    <input id="hidGameId_<%= comm.GameId %>" type="hidden" value="<%= comm.GameId %>" />
                    <%= comm.GameName%>
                </td>
                <td>
                    <input id="txtComm_<%= comm.GameId %>" name="txtComm_<%= comm.GameId %>" type="text" value="<%= comm.Commission.HasValue ? comm.Commission.Value.ToString(CommonHelper.NumberFormat) : null %>" />
                </td>
            </tr>
            <% } %>
        </tbody>
        <tfoot>
        <tr>
        <td colspan="2" align="center">
         <input id="btnSubmit" type="submit" value="Simpan" />
        </td>
        </tr>
        </tfoot>
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
        });

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
