//NiddleList

$(document).ready(function () {
    GetNiddleList();
    $('#ddlIsActive').select2();
});


$(document).on('click', '#NiddleSearch', function () {
    $("#NiddleList").dataTable().fnDestroy();
    GetNiddleList();
});

function GetNiddleList() {
    $('#NiddleList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetNiddleList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
                { "name": "NiddleName", "value": $("#txtNiddle").val() });
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
            { "sName": "NiddleId", "bSearchable": false, "bSortable": true },
            { "sName": "Niddle", "bSearchable": false, "bSortable": true },
            { "sName": "DisplayOrder", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-niddle" + "?NiddleId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteNiddle(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteNiddle(NiddleId) {
    $(".modal-header #NiddleId").val(NiddleId);
    $("#confirmDeleteNiddleModal").modal('show');;
}

function DeleteNiddle() {
    $("#confirmDeleteNiddleModal").modal('hide');
    var NiddleId = $("#NiddleId").val();
    $.ajax({
        url: siteURLPortal + "Master/DeleteNiddle",
        type: 'POST',
        data: { "NiddleId": NiddleId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.NiddleDeleted);
                var oTable = $('#NiddleList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.NiddleDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}
function onSuccessAddNiddle(result) {
    if (result.status == 1) {
        showToastPortal('success', '', MessagePortal.NiddleAdded);
        window.location.href = siteURLPortal + "niddles";
    }
    else {
        showToastPortal('danger', '', MessagePortal.NiddleDetialsFailed);
    }
}

function onFailAddNiddle() {
    showToastPortal('danger', '', MessagePortal.NiddleAddFailed);
}