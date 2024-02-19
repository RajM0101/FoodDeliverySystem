//FileList

$(document).ready(function () {
    GetFileList();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#FileSearch', function () {
    $("#FileList").dataTable().fnDestroy();
    GetFileList();
});

function GetFileList() {
    $('#FileList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetFileList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
                { "name": "FileName", "value": $("#txtFile").val() });
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
            {"sName": "FileId","bSearchable": false,"bSortable": true},
            {"sName": "File","bSearchable": false,"bSortable": true},
            {"sName": "DisplayOrder","bSearchable": false,"bSortable": true},
            {
                "sName": "Action","bSearchable": false,"bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-file" + "?FileId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteFile(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteFile(FileId) {
    $(".modal-header #FileId").val(FileId);
    $("#confirmDeleteFileModal").modal('show');;
}

function DeleteFile() {
    $("#confirmDeleteFileModal").modal('hide');
    var FileId = $("#FileId").val();
    $.ajax({
        url: siteURLPortal + "Master/DeleteFile",
        type: 'POST',
        data: { "FileId": FileId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.FileDeleted);
                var oTable = $('#FileList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.FileDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}
function onSuccessAddFile(result) {
    if (result.status == 1) {
        showToastPortal('success', '', MessagePortal.FileAdded);
        window.location.href = siteURLPortal + "files"
    }
    else {
        showToastPortal('danger', '', MessagePortal.FileDetialsFailed);
    }
}

function onFailAddFile() {
    showToastPortal('danger', '', MessagePortal.FileAddFailed);
}