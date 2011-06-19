<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div id='popup'>
        <iframe width='100%' height='340px' id="popup_frame" frameborder="0"></iframe>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            });
            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function (event, ui) {
                    $("#list").trigger("reloadGrid");
                }
            });
            var deleteDialog = {
                url: '<%= Url.Action("DeleteSalesById", "Sales") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Penjualan Baru";
            $.jgrid.edit.editCaption = "Edit Penjualan";
            $.jgrid.del.caption = "Hapus Penjualan";
            $.jgrid.del.msg = "Anda yakin menghapus Penjualan yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("ListSalesList", "Sales") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['', 'Tanggal', 'Agen', 'No'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: true }, hidedlg: true, hidden: true, editable: false },
                    { name: 'SalesDate', index: 'SalesDate', width: 100, align: 'left', editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'AgentId', index: 'AgentId', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                    { name: 'SalesNo', index: 'SalesNo', width: 100, align: 'left', editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true }
                    ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'SalesDate',
                sortorder: "desc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Penjualan',
                autowidth: true,

                ondblClickRow: function (rowid, iRow, iCol, e) {

                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: true, search: false, refresh: true
                },
                null,
                null,
                deleteDialog
            );
        });   
    </script>
</asp:Content>
