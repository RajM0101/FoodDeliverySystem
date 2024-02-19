function GetPurchaseReport() {
    $('#dtPurchaseReport').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Report/GetPurchaseReportList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push(
                { "name": "StartDate", "value": $("#txtStartDate").val() },
                { "name": "EndDate", "value": $("#txtEndDate").val() },
                { "name": "OrderStatus", "value": $("#ddlOrderStatus").val() },
                { "name": "PackageName", "value": $("#txtPackageName").val() },
                { "name": "UserName", "value": $("#txtUserName").val() },
                { "name": "MobileNo", "value": $("#txtMobileNo").val() });
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
            { "sName": "OrderId", "bSortable": true, },
            { "sName": "OrderDate", "bSortable": true, },
            { "sName": "PackageId", "bSortable": false, "bVisible": false, },
            { "sName": "PackageType", "bSortable": false, "bVisible": false, },
            {
                "sName": "PackageName",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    if (aoData[3] == "3") {
                        var href = siteURLPortal + "add-edit-package" + "?PackageId=" + aoData[2];
                        return '<a href=\"' + href + '\">' + aoData[4] + '</a>';
                    }
                    else {
                        return aoData[4];
                    }
                }
            },

            { "sName": "DesignQty", "bSortable": false, },
            { "sName": "DownloadQty", "bSortable": false, },
            { "sName": "Price", "bSortable": true, },
            {
                "sName": "UserName",
                "bSearchable": false,
                "bSortable": true,
                "mRender": function (data, type, aoData) {
                    var href = siteURLPortal + "add-edit-user" + "?UserId=" + aoData[11];
                    return '<a href=\"' + href + '\">' + aoData[8] + '</a>';
                }
            },
            { "sName": "MobileNo", "bSortable": false, },
            { "sName": "OrderStatus", "bSortable": true, },
            {
                "sName": "Detail",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "add-edit-order-details" + "?OrderId=" + aoData[0];
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a>';

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ExportExcelPurchaseReport() {
    var aoData = [];
    aoData.push(
        { "name": "StartDate", "value": $("#txtStartDate").val() },
        { "name": "EndDate", "value": $("#txtEndDate").val() },
        { "name": "OrderStatus", "value": $("#ddlOrderStatus").val() },
        { "name": "PackageName", "value": $("#txtPackageName").val() },
        { "name": "UserName", "value": $("#txtUserName").val() },
        { "name": "MobileNo", "value": $("#txtMobileNo").val() });
    $.ajax({
        dataType: 'json',
        type: "GET",
        url: siteURLPortal + "Report/ExportExcelPurchaseReport",
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
        url: siteURLPortal + "Report/GetCountrylist",
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
