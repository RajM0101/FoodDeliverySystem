
$(document).ready(function () {
    GetRestaurantList();
});

$(document).on('click', '#RestaurantSearch', function () {
    $("#RestaurantList").dataTable().fnDestroy();
    GetRestaurantList();
});

function GetRestaurantList() {
    $('#RestaurantList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": adminSiteURLPortal + 'AdminUser/GetRestaurantList',
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
            { "sName": "RestaurantID", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "OwnerName", "bSearchable": false, "bSortable": true },
            {
                "sName": "RestaurantName",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    var href = adminSiteURLPortal + "foodlist" + "?RestaurantID=" + aoData[0] +"&Name=Food";
                    return '<a href=\"' + href + '\">' + aoData[2] + '</a>';

                }
            },
            { "sName": "Email", "bSearchable": false, "bSortable": true },
            { "sName": "MobileNo", "bSearchable": false, "bSortable": true },
            { "sName": "Address", "bSearchable": false, "bSortable": true },
            { "sName": "ZipCode", "bSearchable": false, "bSortable": true },
            { "sName": "RestaurantStatus", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/adminuser/change-restaurant-status" + "?RestaurantID=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}
