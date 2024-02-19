function GetInquiryDetail() {
    $('#dtInquiryGrid').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Report/GetInquiryList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsUnread", "value": $("#ddlIsUnread").val() },
                { "name": "Name", "value": $("#name").val() });
        },
        "processing": true,
        "bLengthChange": true,
        "bInfo": true,
        "paging": true,
        "searching": false,
        "columnDefs": [],
        "order": [[4, "desc"]],
        "lengthMenu": [10, 25, 50, 75, 100],
        "aoColumns": [
            {"sName": "ContactRequestId","bVisible": false,"bSearchable": false,"bSortable": true},
            {"sName": "Country","bSearchable": false,"bSortable": true},
            { "sName": "MobileNo", "bSearchable": false, "bSortable": true },
            { "sName": "Email", "bSearchable": false, "bSortable": true },
            {"sName": "Name","bSearchable": false,"bSortable": true},
            {"sName": "Comment","bSearchable": false,"bSortable": true},
            {"sName": "ContactDate","bSearchable": false,"bSortable": true},
            {"sName": "Unread","bSearchable": false,"bSortable": true},
            {
                "sName": "ReadBy",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {

                    if (aoData[7] == "Yes") {

                        return '<a><i style="cursor:pointer;" title="ReadIt" onclick="javascript:ConfirmMarkUnread(' + aoData[0] + ',' + 1 + ')" class="fas fa-envelope"></i></a>'
                    }
                    else {
                        return aoData[8];
                    }
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });

}


function ConfirmMarkUnread(ContactRequestId, ReadBy) {
    $(".modal-header #ContactRequestId").val(ContactRequestId);
    $(".modal-header #ReadBy").val(ReadBy);

    $("#inquiryReadModal").modal('show');;
}

function MarkUnread() {
    $("#inquiryReadModal").modal('hide');
    var ContactRequestId = $("#ContactRequestId").val();
    var ReadBy = $("#ReadBy").val();
    $.ajax({
        url: siteURLPortal + "Report/MarkReadInquiry",
        type: 'POST',
        data: {
            "ContactRequestId": ContactRequestId,
            "ReadBy": ReadBy
        },
        dataType: "json",
    })
        .done(
            function (data, textStatus, jqXHR) {
                if (data.status == "1") {
                    showToastPortal('success', '', MessagePortal.InquiryRead);
                    var oTable = $('#dtInquiryGrid').dataTable();
                    oTable.fnClearTable(0);
                    oTable.fnDraw();
                }
            })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.InquiryReadFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}

