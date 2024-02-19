
function GetCMSPages() {
    $('#dtCMSPages').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'CMSPage/GetCMSPageList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "PageTitle", "value": $("#pageTitle").val() },
                { "name": "ParentCMSPageId", "value": $("#ddlParentCMSPageId").val() },
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
            {
                "sName": "CMSPageId",
                "bSearchable": false,
                "bSortable": false
            },
            { "sName": "CMSPage" },
            { "sName": "ParentCMSPage" },
            { "sName": "DisplayOrder" },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-cms-page" + "?CMSPageId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteCMSPage(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteCMSPage(CMSPageId) {
    $(".modal-header #CMSPageId").val(CMSPageId);
    $("#confirmDeleteCMSPageModal").modal('show');;
}

function DeleteCMSPage(CMSPageId) {
    $("#confirmDeleteCMSPageModal").modal('hide');
    var CMSPageId = $("#CMSPageId").val();
    $.ajax({
        url: siteURLPortal + "CMSPage/DeleteCMSPage",
        type: 'POST',
        data: { "CMSPageId": CMSPageId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.CMSPageDeleted);
                var oTable = $('#dtCMSPages').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.CMSPageDeleteFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}
function onSuccessAddEditCMSPage(result) {
    if (result.state == 1) {
        showToastPortal('success', '', MessagePortal.CMSPageAdded);
        window.location.href = siteURLPortal + "cms-pages"
    }
    else {
        showToastPortal('danger', '', MessagePortal.CMSPageAddFailed);
    }
}

function onFailAddEditCMSPage() {
    showToastPortal('danger', '', MessagePortal.CMSPageAddFailed);
}    