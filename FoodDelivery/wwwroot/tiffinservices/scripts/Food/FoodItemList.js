
$(document).ready(function () {
    GetFoodItemList();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#FoodItemSearch', function () {
    $("#FoodItemList").dataTable().fnDestroy();
    GetFoodItemList();
});

function GetFoodItemList() {
    $('#FoodItemList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": tiffinSiteURLPortal + 'TiffinServicesFood/GetFoodItemList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "Name", "value": $("#txtName").val() },
                { "name": "DiscountInPercentage", "value": $("#txtDiscountInPercentage").val() },
                { "name": "IsBestSeller", "value": $("#ddlIsBestSeller").val() },
                { "name": "IsVegetarian", "value": $("#ddlIsVegetarian").val() },
                { "name": "IsAvailable", "value": $("#ddlIsAvailable").val() });
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
            { "sName": "FoodId", "bSearchable": false, "bSortable": true },
            { "sName": "FoodName", "bSearchable": false, "bSortable": true },
            { "sName": "Price", "bSearchable": false, "bSortable": true },
            { "sName": "DiscountInPercentage", "bSearchable": false, "bSortable": true },
            { "sName": "IsJainAvailable", "bSearchable": false, "bSortable": true },
            { "sName": "IsBestSeller", "bSearchable": false, "bSortable": true },
            { "sName": "IsVegetarian", "bSearchable": false, "bSortable": true },
            { "sName": "IsAvailable", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/tiffinservices/add-edit-food" + "?FoodId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteFoodItem(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteFoodItem(FoodItemId) {
    $(".modal-header #FoodItemId").val(FoodItemId);
    $("#confirmDeleteFoodItemModal").modal('show');;
}

function DeleteFoodItem() {
    $("#confirmDeleteFoodItemModal").modal('hide');
    var FoodItemId = $("#FoodItemId").val();
    $.ajax({
        url: tiffinSiteURLPortal + "TiffinServicesFood/DeleteFoodItem",
        type: 'POST',
        data: { "FoodItemId": FoodItemId },
        dataType: "json",
    }).done(function (data, textStatus, jqXHR) {
        if (data.result.AllowToDelete == true) {
            showToastPortal('success', '', MessagePortal.FoodItemDeleted);
            var oTable = $('#FoodItemList').dataTable();
            oTable.fnClearTable(0);
            oTable.fnDraw();
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        showToastPortal('danger', '', MessagePortal.FoodItemDeletedFailed);
    }).always(function (data, textStatus, errorThrown) {
        //write content here
    });
}