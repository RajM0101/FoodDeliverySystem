
$(document).ready(function () {
    GetUserList();
});

$(document).on('click', '#UserSearch', function () {
    $("#UserList").dataTable().fnDestroy();
    GetUserList();
});

function GetUserList() {
    $('#UserList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": adminSiteURLPortal + 'AdminUser/GetUserList',
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
            { "sName": "UserID", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "Name", "bSearchable": false, "bSortable": true},
           
            { "sName": "MobileNo", "bSearchable": false, "bSortable": true },
            { "sName": "Address", "bSearchable": false, "bSortable": true }
           
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}
