//FileList

$(document).ready(function () {
    GetTypeList();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#TypeSearch', function () {
    $("#TypeList").dataTable().fnDestroy();
    GetTypeList();
});

function GetTypeList() {
    $('#TypeList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetTypeList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
                { "name": "TypeName", "value": $("#txtType").val() });
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
            {"sName": "TypeId","bSearchable": false,"bSortable": true},
            {"sName": "Type","bSearchable": false,"bSortable": true},
            {"sName": "DisplayOrder","bSearchable": false,"bSortable": true},
            {"sName": "Action","bSearchable": false,"bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-type" + "?TypeId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteType(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteType(TypeId) {
    $(".modal-header #TypeId").val(TypeId);
    $("#confirmDeleteTypeModal").modal('show');;
}

function DeleteType() {
    $("#confirmDeleteTypeModal").modal('hide');
    var TypeId = $("#TypeId").val();
    $.ajax({
        url: siteURLPortal + "Master/DeleteType",
        type: 'POST',
        data: { "TypeId": TypeId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.TypeDeleted);
                var oTable = $('#TypeList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.TypeDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}
function onSuccessAddType(result) {
    if (result.status == 1) {
        window.location.href = siteURLPortal + "types"
        showToastPortal('success', '', MessagePortal.TypeAdded);
    }
    else {
        showToastPortal('danger', '', MessagePortal.TypeDetialsFailed);
    }
}

function onFailAddType() {
    showToastPortal('danger', '', MessagePortal.TypeAddFailed);
}