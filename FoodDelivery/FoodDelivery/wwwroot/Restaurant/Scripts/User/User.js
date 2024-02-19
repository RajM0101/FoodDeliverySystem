function GetUserList() {
    $('#dtUser').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'User/GetUserList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
                { "name": "RegistrationStatus", "value": $("#ddlRegistrationStatus").val() },
                { "name": "DeviceCount", "value": $("#deviceCount").val() },
                { "name": "Name", "value": $("#name").val() });
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
            { "sName": "UserId", "bSearchable": false, "bSortable": true },
            { "sName": "MobileNo", "bSearchable": false, "bSortable": true },
            { "sName": "Email", "bSearchable": false, "bSortable": true },
            { "sName": "Name", "bSearchable": false, "bSortable": true },
            { "sName": "DeviceCount", "bSearchable": false, "bSortable": true },
            { "sName": "RegistrationStatus", "bSearchable": false, "bSortable": true },
            { "sName": "DownloadDate", "bSearchable": false, "bSortable": true },
            { "sName": "CreatedDate", "bSearchable": false, "bSortable": true },
            { "sName": "RegisteredFrom", "bSearchable": false, "bSortable": true },
            {
                "sName": "DefaultOTP", "bSearchable": false, "bSortable": true,
                "mRender": function (data, type, oObj) {

                    return '<div class=' + oObj[0] + '>' + oObj[9] + '</div>';
                }
            },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-user" + "?UserId=" + aoData[0];
                    return '<a><i style="cursor:pointer;" title="Reset OTP" onclick="javascript:UpdateDefaultOTP(' + aoData[0] + ')" class="feather icon-refresh-cw "></i></a>  <a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteUser(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';   
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function UpdateDefaultOTP(UserId) {
    $.ajax({
        url: siteURLPortal + "User/UserDefaultOTP",
        type: 'POST',
        data: { "UserId": UserId },
        dataType: "json",
        success: function (data, textStatus, jqXHR) {
            $("div." + data.UserId + "").replaceWith('<div class=' + data.UserId + ' >' + data.OTPGenerated + '</div>');
        },
        error: function (errormessage) {
            alert(errormessage);
        }
    });
}

function ConfirmDeleteUser(UserId) {
    $(".modal-header #UserId").val(UserId);
    $("#confirmDeleteUserModal").modal('show');;
}

function DeleteUser() {
    $("#confirmDeleteUserModal").modal('hide');
    var UserId = $("#UserId").val();
    $.ajax({
        url: siteURLPortal + "User/DeleteUser",
        type: 'POST',
        data: { "UserId": UserId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.UserDeleted);
                var oTable = $('#dtUser').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.UserDeleteFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}

function ExportExcelUserReport() {
    var aoData = [];
    aoData.push({ "name": "IsActive", "value": $("#ddlIsActive").val() },
        { "name": "RegistrationStatus", "value": $("#ddlRegistrationStatus").val() },
        { "name": "DeviceCount", "value": $("#deviceCount").val() },
        { "name": "Name", "value": $("#name").val() });
    $.ajax({
        dataType: 'json',
        type: "GET",
        url: siteURLPortal + "User/ExportExcelUserReport",
        data: aoData,
        success: function (msg) {
            window.location = window.location.origin + "/Upload/" + msg.filename;

        },
        error: function (xhr, textStatus, error) {
            if (typeof console == "object") {
                console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
            }
        }
    });
}

function GetCountryList() {
    $.ajax({
        type: "Get",
        url: siteURLPortal+"User/GetCountrylist",
        contentType: "json",
        success: function (response) {
            $("#CountryId").html('');
            var optionString = "<option value=''>-- Select Country --</option>";
            $.each(response, function (i, item) {
                optionString += "<option value=\"" + item.ID + "\">" + item.Name + "</option>"

            });
            $("#CountryId").html(optionString);
            $("#CountryId").val($("#hiddenCountryID").val()).trigger('change');
        }
    });
}

function formatCountry(country) {
    if (!country.id) {
        return country.text;
    }
    var CountryAbbr = country.text.split(',')[0];
    var CountryCode = country.text.split(',')[1];
    var CountryName = country.text.split(',')[2];
    if (CountryAbbr != null && CountryAbbr != undefined && CountryAbbr != "null")
        var $country = $(
            '<span class="fi fi-' + CountryAbbr.toLowerCase() + ' fis"></span>' +
            '<span class="flag-text"><b>' + '  ' + CountryName + '</b> +' + CountryCode + '</span>'
        );
    return $country;
};
function formatSelectionType(country) {
    if (!country.id) {
        return country.text;
    }
    var CountryAbbr = country.text.split(',')[0];
    var CountryCode = country.text.split(',')[1];
    var CountryName = country.text.split(',')[2];
    if (CountryAbbr != null && CountryAbbr != undefined && CountryAbbr != "null")
        var $country = $(
            '<span class="fi fi-' + CountryAbbr.toLowerCase() + ' fis"></span>' +
            '<span class="flag-text">' + "   +" + CountryCode + '</span>'
        );
    return $country;
};

function onSuccessAddEditUSer(result) {
    if (result.state == 1 || result.state == 2) {
        showToastPortal('success', '', MessagePortal.UserAdded);
        window.location.href = siteURLPortal + "users"
    }
    else {
        showToastPortal('danger', '', MessagePortal.UserAddFailed);
    }
}

function onFailAddEditUSer() {
    showToastPortal('danger', '', MessagePortal.UserAddFailed);
}