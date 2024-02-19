//Package Expired Report
$(document).ready(function () {
    $('#ddlFollowUpStatus').select2();

    GetPackageExpiredReport();

    $("#btnSearch").on("click", function () {
        $("#dtPackageExpirationReport").dataTable().fnDestroy();
        GetPackageExpiredReport();
    });

    $("#btnExportExcel").on("click", function () {
        ExportExcelPackageExpiredReport();
    });
});

function GetPackageExpiredReport() {
    $('#dtPackageExpirationReport').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Report/GetPackageExpiredReport',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "ExpiredInLastXDays", "value": $("#txtExpiredInLastXDays").val() },
                { "name": "FollowUpStatus", "value": $("#ddlFollowUpStatus").val() });
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
            {
                "sName": "OrderId",
                "bVisible": true,
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "OrderDate",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "PackageId",
                "bVisible": false,
                "bSearchable": false,
                "bSortable": true
            },
            { "sName": "PackageType", "bSortable": false, "bVisible": false, },
            {
                "sName": "PackageName",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    if (aoData[3] == "3") {
                        var href = siteURLPortal + "add-edit-package" + "?PackageId=" + aoData[2];
                        return '<a href=\"' + href + '\">' + aoData[4] + '</a>';
                    }
                    else {
                        return aoData[4];
                    }
                }
            },
            {
                "sName": "DesignQty",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "DownloadQty",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "Price",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "UserName",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    var href = siteURLPortal + "add-edit-user" + "?UserId=" + aoData[11];
                        return '<a href=\"' + href + '\">' + aoData[8] + '</a>';
                }
            },
            {
                "sName": "MobileNo",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "FollowUpStatus",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "Details",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    var href = siteURLPortal + "followup-status-details" + "?OrderId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a>';
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });

}

function ExportExcelPackageExpiredReport() {
    var aoData = [];
    aoData.push({ "name": "FollowUpStatus", "value": $("#ddlFollowUpStatus").val() },
        { "name": "ExpiredInLastXDays", "value": $("#txtExpiredInLastXDays").val() });
    $.ajax({
        "dataType": 'json',
        "contentType": "application/json; charset=utf-8",
        "type": "GET",
        "url": siteURLPortal + "Report/ExportExcelPackageExpiredReport",
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
