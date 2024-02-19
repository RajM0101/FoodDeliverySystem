//BannerList

$(document).ready(function () {
    GetBannerList();
    $('#ddlBannerTypeId').select2();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#BannerSearch', function () {
    $("#BannerList").dataTable().fnDestroy();
    GetBannerList();
});

function GetBannerList() {
    $('#BannerList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetBannerList',    
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "Banner", "value": $("#txtBanner").val() },
                { "name": "BannerTypeId", "value": $("#ddlBannerTypeId").val() },
                { "name": "IsActive", "value": $("#ddlIsActive").val() });
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
            { "sName": "BannerId", "bSearchable": false, "bSortable": true },
            { "sName": "BannerType","bSearchable": false,"bSortable": true },
            { "sName": "Name","bSearchable": false,"bSortable": true },
            { "sName": "DisplayOrder","bSearchable": false,"bSortable": true },
            { "sName": "Action","bSearchable": false,"bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-banner" + "?BannerId=" + aoData[0]; 
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteBanner(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteBanner(BannerId) {
    $(".modal-header #BannerId").val(BannerId);
    $("#confirmDeleteBannerModal").modal('show');;
}

function DeleteBanner() {
    $("#confirmDeleteBannerModal").modal('hide');
    var BannerId = $("#BannerId").val();
    $.ajax({
        url: siteURLPortal + "Master/DeleteBanner",
        type: 'POST',
        data: { "BannerId": BannerId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.BannerDeleted);
                var oTable = $('#BannerList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.BannerDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}