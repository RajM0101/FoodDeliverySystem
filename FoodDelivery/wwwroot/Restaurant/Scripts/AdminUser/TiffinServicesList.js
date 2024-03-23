
$(document).ready(function () {
    GetTiffinServicesList();
});

$(document).on('click', '#TiffinServicesSearch', function () {
    $("#TiffinServicesList").dataTable().fnDestroy();
    GetTiffinServicesList();
});

function GetTiffinServicesList() {
    $('#TiffinServicesList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": adminSiteURLPortal + 'AdminUser/GetTiffinServicesList',
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
            { "sName": "TiffinServicesID", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "OwnerName", "bSearchable": false, "bSortable": true },
            {
                "sName": "TiffinServicesName",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    var href = adminSiteURLPortal + "foodlist" + "?RestaurantID=" + aoData[0]+"&Name=Tiffin";
                    return '<a href=\"' + href + '\">' + aoData[2] + '</a>';

                }
            },
            { "sName": "Email", "bSearchable": false, "bSortable": true },
            { "sName": "MobileNo", "bSearchable": false, "bSortable": true },
            { "sName": "Address", "bSearchable": false, "bSortable": true },
            { "sName": "ZipCode", "bSearchable": false, "bSortable": true },
            { "sName": "TiffinServicesStatus", "bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/adminuser/change-tiffinservices-status" + "?TiffinServicesID=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}
