//PackageList

$(document).ready(function () {
    GetPackageList();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#PackageSearch', function () {
    $("#PackageList").dataTable().fnDestroy();
    GetPackageList();
});

function GetPackageList() {
    $('#PackageList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetPackageList',    
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "Name", "value": $("#txtName").val() },
                { "name": "DiscountInPercentage", "value": $("#txtDiscountInPercentage").val() },
                { "name": "IsAvailable", "value": $("#ddlIsAvailable").val() },
                { "name": "IsVegetarian", "value": $("#ddlIsVegetarian").val() },
                { "name": "IsBestSeller", "value": $("#ddlIsBestSeller").val() });
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
            { "sName": "PackageId", "bSearchable": false, "bSortable": true },
            {"sName": "Name","bSearchable": false,"bSortable": true},
            {"sName": "Price","bSearchable": false,"bSortable": true},
            {"sName": "DesignQty","bSearchable": false,"bSortable": true},
            {"sName": "ValidityInMonth","bSearchable": false,"bSortable": true},
            { "sName": "IsPublish", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-package" + "?PackageId=" + aoData[0]; 
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeletePackage(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeletePackage(PackageId) {
    $(".modal-header #PackageId").val(PackageId);
    $("#confirmDeletePackageModal").modal('show');;
}

function DeletePackage() {
    $("#confirmDeletePackageModal").modal('hide');
    var PackageId = $("#PackageId").val();
    $.ajax({
        url: siteURLPortal + "Master/DeletePackage",
        type: 'POST',
        data: { "PackageId": PackageId },
        dataType: "json",
    }).done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.PackageDeleted);
                var oTable = $('#PackageList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.PackageDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}