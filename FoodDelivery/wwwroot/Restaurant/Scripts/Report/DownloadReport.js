//Download Report
$(document).ready(function () {
    GetDownloadReport();

    $('#txtStartDate').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        todayHighlight: true,
        autoclose: true
    }).on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#txtEndDate').datepicker('setStartDate', minDate);
    });

    $("#btnSearch").on("click", function () {
        $("#dtPurchaseReport").dataTable().fnDestroy();
        GetDownloadReport();
    });
    $("#btnExportExcel").on("click", function () {
        ExportExcelDownloadReport();
    });
    $("#btnExportExcelUserFavouriteDesigns").on("click", function () {
        ExportExcelUserFavouriteDesigns();
    });
});


function GetDownloadReport() {
    $('#dtPurchaseReport').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Report/GetDownloadReport',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "StartDate", "value": $("#txtStartDate").val() },
                { "name": "EndDate", "value": $("#txtEndDate").val() },
                { "name": "DesignId", "value": $("#txtDesignId").val() },
                { "name": "UserId", "value": $("#txtUserId").val() });
        },
        "processing": true,
        "bLengthChange": true,
        "bInfo": true,
        "paging": true,
        "searching": false,
        "columnDefs": [],
        "order": [[0, "desc"]],
        "lengthMenu": [10, 25, 50, 75, 100],
        "aoColumns": [
            {"sName": "DownloadDate","bVisible": true,"bSearchable": false,"bSortable": true},
            {"sName": "SubSubCategory","bSearchable": false,"bSortable": false},
            {"sName": "DesignId","bVisible": true,"bSearchable": true,"bSortable": true,
                "mRender": function (data, type, aoData) {
                    var href = siteURLPortal + "add-edit-design" + "?DesignId=" + aoData[7];
                        return '<a href=\"' + href + '\">' + aoData[2] + '</a>';
                }
            },
            {"sName": "UserId","bSearchable": false,"bSortable": true,
                "mRender": function (data, type, aoData) {
                        var href = siteURLPortal + "add-edit-user" + "?UserId=" + aoData[3];
                        return '<a href=\"' + href + '\">' + aoData[3] + '</a>';
                }
            },
            { "sName": "Name", "bSearchable": false, "bSortable": true },
            { "sName": "MobileNo", "bSearchable": false, "bSortable": false },
            {"sName": "DownloadFrom","bSearchable": false,"bSortable": true}
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });

}

function ExportExcelDownloadReport() {
    var aoData = [];
    aoData.push({ "name": "StartDate", "value": $("#txtStartDate").val() },
        { "name": "EndDate", "value": $("#txtEndDate").val() },
        { "name": "DesignId", "value": $("#txtDesignId").val() },
        { "name": "UserId", "value": $("#txtUserId").val() });
    $.ajax({
        "dataType": 'json',
        "contentType": "application/json; charset=utf-8",
        "type": "GET",
        "url": siteURLPortal + "Report/ExportExcelDownloadReport",
        "data": aoData,
        "success": function (msg) {
            window.location = window.location.origin + "/Upload/" + msg.filename;

        },
        error: function (xhr, textStatus, error) {
            if (typeof console == "object") {
                console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
            }
        }
    });
}


function ExportExcelUserFavouriteDesigns() {
    var aoData = [];
    $.ajax({
        "dataType": 'json',
        "contentType": "application/json; charset=utf-8",
        "type": "GET",
        "url": siteURLPortal + "Report/ExportExcelUserFavouriteDesigns",
        "data": aoData,
        "success": function (msg) {
            window.location = window.location.origin + "/Upload/" + msg.filename;

        },
        error: function (xhr, textStatus, error) {
            if (typeof console == "object") {
                console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
            }
        }
    });
}