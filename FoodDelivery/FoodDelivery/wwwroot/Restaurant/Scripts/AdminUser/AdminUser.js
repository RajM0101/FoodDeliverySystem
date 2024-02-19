$(document).ready(function () {
    GetAdminUsers();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#AdminUserSearch', function () {
    $("#dtAdminUser").dataTable().fnDestroy();
    GetAdminUsers();
});


function GetAdminUsers() {
    $('#dtAdminUser').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'AdminUser/GetAdminUserList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
                { "name": "FirstName", "value": $("#firstName").val() });
        },
        "processing": true,
        "bLengthChange": true,
        "bInfo": true,
        "paging": true,
        "searching": false,
        "columnDefs": [],
        "order": [[0, "asc"]],
        "lengthMenu": [10, 25, 50, 75, 100],
        "aoColumns": [
            { "sName": "AdminUserId", "Searchable": false, "bSortable": true },
            { "sName": "MobileNo", "bSearchable": false, "bSortable": true },
            { "sName": "FirstName", "bSearchable": false, "bSortable": true },
            { "sName": "LastName", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-admin-user" + "?AdminUserId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteAdminUser(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteAdminUser(AdminUserId) {
    $(".modal-header #AdminUserId").val(AdminUserId);
    $("#confirmDeleteAdminUserModal").modal('show');;
}

function DeleteAdminUser() {
    $("#confirmDeleteAdminUserModal").modal('hide');
    var AdminUserId = $("#AdminUserId").val();
    $.ajax({
        url: siteURLPortal + "AdminUser/DeleteAdminUser",
        type: 'POST',
        data: { "AdminUserId": AdminUserId },
        dataType: "json",
    })
        .done(
            function (data, textStatus, jqXHR) {
                if (data.status == "1") {
                    showToastPortal('success', '', MessagePortal.AdminUserDeleted);
                    var oTable = $('#dtAdminUser').dataTable();
                    oTable.fnClearTable(0);
                    oTable.fnDraw();
                }
            })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.AdminUserDeleteFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}

function onSuccessAddEditAdminUSer(result) {
    if (result.state == 1) {
        window.location.href = siteURLPortal + "admin-users"
        showToastPortal('success', '', MessagePortal.AdminUserAdded);
    }
    else {
        showToastPortal('danger', '', MessagePortal.AdminUserAddFailed);
    }
}

function onFailAddEditAdminUSer() {
    showToastPortal('danger', '', MessagePortal.AdminUserAddFailed);
}