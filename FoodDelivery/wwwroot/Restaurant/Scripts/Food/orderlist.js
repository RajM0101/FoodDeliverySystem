
$(document).ready(function () {
    GetOrderList();
});

$(document).on('click', '#FoodItemSearch', function () {
    $("#OrderList").dataTable().fnDestroy();
    GetOrderList();
});

function GetOrderList() {
    $('#OrderList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Food/GetRestaurantOrderList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "Name", "value": $("#txtName").val() });
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
            { "sName": "OrderId", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "OrderDetailID", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "FoodId", "bSearchable": false, "bSortable": true },
            { "sName": "Name", "bSearchable": false, "bSortable": true },
            { "sName": "Qauntity", "bSearchable": false, "bSortable": true },
            { "sName": "OrderDate", "bSearchable": false, "bSortable": true },
            { "sName": "Price", "bSearchable": false, "bSortable": true },
            { "sName": "OrderStatus", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/restaurant/change-order-status" + "?OrderDetailID=" + aoData[1];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a>';

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
        url: siteURLPortal + "Food/DeleteFoodItem",
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