//AreaList

$(document).ready(function () {
    GetAreaList();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#AreaSearch', function () {
    $("#AreaList").dataTable().fnDestroy();
    GetAreaList();
});

function GetAreaList() {
    $('#AreaList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetAreaList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
                { "name": "AreaName", "value": $("#txtArea").val() });
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
            { "sName": "AreaId", "bSearchable": false, "bSortable": true },
            {"sName": "Area", "bSearchable": false, "bSortable": true},
            {"sName": "DisplayOrder","bSearchable": false,"bSortable": true},
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-area" + "?AreaId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteArea(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteArea(AreaId) {
    $(".modal-header #AreaId").val(AreaId);
    $("#confirmDeleteAreaModal").modal('show');
}

function DeleteArea() {
    $("#confirmDeleteAreaModal").modal('hide');
    var AreaId = $("#AreaId").val();
    $.ajax({
        url: siteURLPortal + "Master/DeleteArea",
        type: 'POST',
        data: { "AreaId": AreaId },
        dataType: "json",
    }).done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.AreaDeleted);
                var oTable = $('#AreaList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.AreaDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}
function onSuccessAddArea(result) {
    if (result.status == 1) {
        showToastPortal('success', '', MessagePortal.AreaAdded);
        window.location.href = siteURLPortal + "areas"

    }
    else {
        showToastPortal('danger', '', MessagePortal.AreaDetialsFailed);
    }
}

function onFailAddArea() {
    showToastPortal('danger', '', MessagePortal.AreaAddFailed);
}