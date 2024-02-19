function GetEmailDomain() {
    $('#dtEmailDomain').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Report/GetEmailDomainList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsBlocked", "value": $("#ddlIsBlocked").val() },
                { "name": "DomainName", "value": $("#domainName").val() });
        },
        "processing": true,
        "bLengthChange": true,
        "bInfo": true,
        "paging": true,
        "searching": false,
        "columnDefs": [],
        "order": [[1, "desc"]],
        "lengthMenu": [10, 25, 50, 75, 100],
        "aoColumns": [
            { "sName": "EmailDomainId", "bSearchable": false, "bSortable": true,"bVisible": false, },
            { "sName": "DomainName","bSearchable": false, "bSortable": true },
            { "sName": "NoOfCount","bSearchable": false,"bSortable": true},
            {"sName": "IsBlocked","bSearchable": false,"bSortable": true},
            {"sName": "CreatedDate","bSearchable": false,"bSortable": true},
            {"sName": "ModifiedDate","bSearchable": false,"bSortable": true},
            {"sName": "ModifiedBy","bSearchable": false,"bSortable": true},
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {

                    if (aoData[3] == "Yes") {

                        return '<a><i style="cursor:pointer;" title="IsBlocked" onclick="javascript:ConfirmChangeEmailDomain(' + aoData[0] + ',false)" class="feather icon-check"></i></a>'
                    }
                    else {
                        return '<i class="feather icon-x" style="cursor:pointer;" title="IsBlocked" onclick="javascript:ConfirmChangeEmailDomain(' + aoData[0] + ',true)"></i>'
                    }
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });

}


function ConfirmChangeEmailDomain(EmailDomainId, IsBlocked) {
    $(".modal-header #EmailDomainId").val(EmailDomainId);
    $(".modal-header #IsBlocked").val(IsBlocked);

    $("#changeEmailDomainModal").modal('show');;
}

function ChangeEmailDomain() {
    $("#changeEmailDomainModal").modal('hide');
    var EmailDomainId = $("#EmailDomainId").val();
    var IsBlocked = $("#IsBlocked").val();
    $.ajax({
        url: siteURLPortal + "Report/ChangeEmailDomain",
        type: 'POST',
        data: {
            "EmailDomainId": EmailDomainId,
            "IsBlocked": IsBlocked },
        dataType: "json",
    })
        .done(
            function (data, textStatus, jqXHR) {
                if (data.status == "1") {
                    showToastPortal('success', '', MessagePortal.EmailDomainIsBlocked);
                    var oTable = $('#dtEmailDomain').dataTable();
                    oTable.fnClearTable(0);
                    oTable.fnDraw();
                }
            })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.EmailDomainIsBlockedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}
