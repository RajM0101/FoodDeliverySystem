//#region Common
function OnAjaxBegin(id) {
    $(id).prop("disabled", true);
    $(id).find('i').removeClass("hide");
}
function OnAjaxComplete(id) {
    $(id).prop("disabled", false);
    $(id).find('i').addClass("hide");
}

$(document).ajaxStart(function () {
    showLoading();
});

$(document).ajaxStop(function () {
    if ($.active <= 1) {
        hideLoading();
    }
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="tooltip"]').on("mouseleave", function () {
        $(this).tooltip("hide");
    });
    $('[data-toggle="popover"]').popover();
    //$('[data-toggle="popover"]').on("mouseleave", function () {
    //    $(this).popover("hide");
    //})
});

function showLoading() {
    $('#loading,#loadingbar').show();
}

function hideLoading() {
    $('#loading,#loadingbar').hide();
}

function Select2Portal(param) {
    $("#" + param + "").select2();
}

function showToastPortal(ToastType, Title, Message, Timeout) {
    $.toaster({ priority: ToastType, title: Title, message: Message, timeout: Timeout });
}
//#endregion

var MessagePortal =
{
    //frontendsite 
    UserLogin: "You are sucessfully logged-in.",
    UserLogout: "You are sucessfully logged-out.",
    UserLogoutFailed: "We are unable to logout! Please try again.",
    UserUpdate: "Your profile has been updated successfully.",
    UserUpdateFailed: "We are unable to update your profile! Please try again.",
    LoginForFavourite: "You first need to <a href='##siteUrl##login-register'>login</a>, to add it into your favourites",
    AddFavourite: "Design successfully added into your favourites.",
    RemoveFavourite: "Design successfully removed from your favourites.",
    LoingForDownload: "Please <a href='##siteUrl##login-register'>login/create</a> new account to download this design.",
    LoingForAddTocart: "Please <a href='##siteUrl##login-register'>login/create</a> new account to make order.",
    DownloadDesignCase0: "We are unable to process, please try after some times.",
    DownloadDesignCase1: "Design successfully downloaded",
    DownloadDesignCase2: "Design successfully downloaded",
    DownloadDesignCase3: "Your Package is expired or you have not bought any Package. Please click OK to buy our package.",
    DownloadDesignCase4: "You have logged in from another device, please login again to download.",
    DownloadDesignCase5: "You have reached the daily download limit.",
    DownloadDesignFail: "Opps! Something wents wrong, Please try after some time.",
    AddTocartSuccess: "Add Food to cart successfully",
    AddTocartAlredyExist: "Food is alredy added to cart",
    QauntityAddTocart: "Qauntity is added to cart",
    AddTocartFail: "Opps! Something wents wrong, Please try after some time.",
    RemoveFromcartSuccess: "Remove Food from cart",
    RemoveFromcartFail: "Opps! Something wents wrong, Please try after some time.",
    MessageSendSuccess: "Message send sucessfully.",
    MessageSendFailed: "Message send failed.",
    InValidCaptcha: "Captcha Is Not Valid.",
    UserInfoUpdated: "User info updated successfully.",
    UserInfoDetialsFailed: "User info updated failed.",
    LoginForPurchase: "Please <a href='##siteUrl##login-register'>login/create</a> new account to purchase packages.",
    LoginForHeartIconFavourite: "Please <a href='##siteUrl##login-register'>login/register</a> into your account to view your Favourite designs.",
};

var pagenumber = 0;
var noOfRecords;
var ptotal;
var pSize = 20;
var displayStart = 0;

$(document).ready(function () {
    noOfRecords = $("#NoOfDesignCount").val();
    ptotal = noOfRecords;
    if (parseInt(pagenumber) == 0) {
        pagenumber = 1;
    }

    SetPagination(ptotal, pagenumber);

    $("#PaymentCurrencyId").select2({
        templateResult: formatCurrencyIsoCode,
        templateSelection: formatCurrencyIsoCodeSelectionType
    });

    $("#PaymentCurrencyId").change(function () {
        var CurrencyISOCode = $("#PaymentCurrencyId").val();
        GetPackageDetailByCurrency(CurrencyISOCode);
    });

    if ($("#searchDesign").val() != null) {
        GetDesigns();
    }

    $(".input-number__input").change(function () {
        debugger
        var Qauntity = $(this).val();
        var FoodId = $(this).data('foodid');
        if ($("#userid").val() == null) {
            showToastPortal('danger', '', MessagePortal.LoingForAddTocart.replace("##siteUrl##", siteURL), 60000);
        }
        else {
            RequestUserCartDetails(FoodId, Qauntity);
        }
    });
});
$(document).off('click', '#divPurchasepackage').on('click', '#divPurchasepackage', function () {
    if ($("#userid").val() == null) {
        showToastPortal('danger', '', MessagePortal.LoginForPurchase.replace("##siteUrl##", siteURL), 5000);
        return false;
    }
});
function GetPackageDetailByCurrency(CurrencyISOCode) {
    $.ajax({
        url: siteURL + "Home/GetPackageDetailByCurrency",
        type: "POST",
        data: { CurrencyISOCode: CurrencyISOCode },
        success: function (result) {
            $("#purchasePackageDiv").html(result);
        }
    });
}

function SubCategory(ParentCategoryId, CategorySEOUrl, CategoryName, Type) {
    $.ajax({
        url: siteURL + "category/" + CategorySEOUrl,
        type: 'GET',
        data: { "CategorySEOUrl": CategorySEOUrl, "Type": Type },
        success: function (result) {
            $("#subCategoryModalBody").html(result);
        },
        error: function (xhr, textStatus, error) {
            if (typeof console == "object") {
                console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
            }
        }
    });
    $(".modal-header #ParentCategoryId").val(ParentCategoryId);
    $(".modal-header #subCategoryModalTitle").html(CategoryName);
    $("#subCategoryModal").modal('show');
    $("#subCategoryModal").modal({ backdrop: 'static', keyboard: false, show: 'true' });
}

function SetActiveFilterPanel() {
    var count = 0;

    var designBySearchHtml = "";
    designBySearchHtml += '<div class="applied-filters">  <ul class="applied-filters__list">';
    designBySearchHtml += '<li class="applied-filters__item">' +
        '<div>Design By Search:</div >' +
        '</li>';
    var filterSubCategoryHtml = "";
    filterSubCategoryHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    filterSubCategoryHtml += '<li class="applied-filters__item">' +
        '<div>Design By Category:</div >' +
        '</li>';
    var stitchHtml = "";
    stitchHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    stitchHtml += '<li class="applied-filters__item">' +
        '<div>Number Of Stitch:</div >' +
        '</li>';
    var areaHtml = "";
    areaHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    areaHtml += '<li class="applied-filters__item">' +
        '<div>Area:</div >' +
        '</li>';
    var nidddleHtml = "";
    nidddleHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    nidddleHtml += '<li class="applied-filters__item">' +
        '<div>Number Of Niddle/Color:</div >' +
        '</li>';
    var heightHtml = "";
    heightHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    heightHtml += '<li class="applied-filters__item">' +
        '<div>Height:</div >' +
        '</li>';
    var widthHtml = "";
    widthHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    widthHtml += '<li class="applied-filters__item">' +
        '<div>Width:</div >' +
        '</li>';
    var favoritedOnHtml = "";
    favoritedOnHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    favoritedOnHtml += '<li class="applied-filters__item">' +
        '<div>Favorite Between: </div>' +
        '</li>';
    var favoriteToHtml = "";
    var downloadOnHtml = "";
    downloadOnHtml = '<div class="applied-filters">  <ul class="applied-filters__list">';
    downloadOnHtml += '<li class="applied-filters__item">' +
        '<div>Download Between: </div>' +
        '</li>';
    var downloadToHtml = "";
 
    var designBySearch = $("#searchDesign").map(function () {
        designBySearchHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter">
            `+ $(this).val() + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('designBySearch',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;

        return $(this).val()
    }).get().join(",");

    var favoriteFrom = $("#startDate").map(function () {
        favoritedOnHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter">
            `+ $(this).val() + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('favoriteFrom',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;

        return $(this).val()
    }).get().join(",");

    if ($("#endDate").val() != '') {
        var favoriteTo = $("#endDate").map(function () {
            favoriteToHtml = favoritedOnHtml += '<div>To:</div>' + `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter">
            `+ $(this).val() + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('favoriteTo',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;

            return $(this).val()
        }).get().join(",");
    }

    var downloadFrom = $("#startDateDownload").map(function () {
        downloadOnHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter">
            `+ $(this).val() + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('downloadFrom',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;

        return $(this).val()
    }).get().join(",");

    if ($("#endDateDownload").val() != '') {
        var downloadTo = $("#endDateDownload").map(function () {
            downloadToHtml = downloadOnHtml += '<div>To:</div>' + `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter">
            `+ $(this).val() + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('downloadTo',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;

            return $(this).val()
        }).get().join(",");
    }

    var filterSubCategory = $("#chkDesignByCategory:checked").map(function () {
        filterSubCategoryHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter">
            `+ $(this).data('title') + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('filterSubCategory',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;
        count++;
        return $(this).data('title')
    }).get().join(",");

    var stitch = $("#chkStitch:checked").map(function () {
        stitchHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter" >
            `+ $(this).data('title') + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('stitch',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;
        count++;
        return $(this).data('title')
    }).get().join(",");

    var area = $("#chkArea:checked").map(function () {
        areaHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter" >
            `+ $(this).data('title') + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('area',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;
        count++;
        return $(this).data('title')
    }).get().join(",");

    var niddle = $("#chkNiddle:checked").map(function () {
        nidddleHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter" >
            `+ $(this).data('title') + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('niddle',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;
        count++;
        return $(this).data('title')
    }).get().join(",");

    var height = $("#chkHeight:checked").map(function () {
        heightHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter" >
            `+ $(this).data('title') + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('height',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;
        count++;
        return $(this).data('title')
    }).get().join(",");

    var width = $("#chkWidth:checked").map(function () {
        widthHtml += `<li class="applied-filters__item">
            <a class="applied-filters__button applied-filters__button--filter" >
            `+ $(this).data('title') + `
            <svg width="9" height="9" style="cursor:pointer;" onclick="javascript:ClearOneFilter('width',`+ $(this).val() + `)">
            <path d="M9,8.5L8.5,9l-4-4l-4,4L0,8.5l4-4l-4-4L0.5,0l4,4l4-4L9,0.5l-4,4L9,8.5z" />
            </svg>
            </a>
            </li>`;
        count++;
        return $(this).data('title')
    }).get().join(",");

    designBySearchHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('designBySearch')" class="applied-filters__button applied-filters__button--clear">Clear All Design by Search</button></li>`;
    designBySearchHtml += '</ul></div>';
    favoritedOnHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('favoriteFrom')" class="applied-filters__button applied-filters__button--clear">Clear All Design by FavouritedOn</button></li>`;
    favoritedOnHtml += '</ul></div>';
    downloadOnHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('downloadFrom')" class="applied-filters__button applied-filters__button--clear">Clear All Design by DownloadedOn</button></li>`;
    downloadOnHtml += '</ul></div>';
    filterSubCategoryHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('filterSubCategory')" class="applied-filters__button applied-filters__button--clear">Clear All Design by Category</button></li>`;
    filterSubCategoryHtml += '</ul></div>';
    stitchHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('stitch')" class="applied-filters__button applied-filters__button--clear">Clear All Stitch</button></li>`;
    stitchHtml += '</ul></div>';
    areaHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('area')" class="applied-filters__button applied-filters__button--clear">Clear All Area</button></li>`;
    areaHtml += '</ul></div>';
    nidddleHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('niddle')" class="applied-filters__button applied-filters__button--clear">Clear All Niddle/color</button></li>`;
    nidddleHtml += '</ul></div>';
    heightHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('height')" class="applied-filters__button applied-filters__button--clear">Clear All Height</button></li>`;
    heightHtml += '</ul></div>';
    widthHtml += `<li class="applied-filters__item"><button type="button" onclick="javascript:ClearAllFilter('width')" class="applied-filters__button applied-filters__button--clear">Clear All Width</button></li>`;
    widthHtml += '</ul></div>';
   
    if (designBySearch == "") {
        designBySearchHtml = "";
        $("#divActiveFilters").html();
    }
    if (favoriteFrom == "") {
        favoritedOnHtml = "";
        $("#divActiveFilters").html();
    }
    if (favoriteTo == "") {
        favoriteToHtml = "";
        $("#divActiveFilters").html();
    }
    if (downloadFrom == "") {
        downloadOnHtml = "";
        $("#divActiveFilters").html();
    }
    if (downloadTo == "") {
        downloadToHtml = "";
        $("#divActiveFilters").html();
    }
    if (filterSubCategory == "") {
        filterSubCategoryHtml = "";
        $("#divActiveFilters").html();
    }
    if (stitch == "") {
        stitchHtml = "";
        $("#divActiveFilters").html();
    }
    if (area == "") {
        areaHtml = "";
        $("#divActiveFilters").html();
    }
    if (niddle == "") {
        nidddleHtml = "";
        $("#divActiveFilters").html();
    }
    if (height == "") {
        heightHtml = "";
        $("#divActiveFilters").html();
    }
    if (width == "") {
        widthHtml = "";
        $("#divActiveFilters").html();
    }
    
    var mainHtml = designBySearchHtml + favoritedOnHtml + downloadOnHtml + filterSubCategoryHtml + stitchHtml + areaHtml + nidddleHtml + heightHtml + widthHtml;
    if (mainHtml != "") {
        $("#mainDivActiveFilter").css("display", "block");
    }
    else {
        $("#mainDivActiveFilter").css("display", "none");
    }
    const mediaQuery = window.matchMedia('(max-width : 992px)')
    if (mediaQuery.matches) {
        $("#mainDivActiveFilter").css("display", "block");
    }
    $("#divActiveFilters").siblings().remove();// remove all html
    $("#divActiveFilters").parent().append(mainHtml);
    $("#activeFilterCount").html(count); //mobile fliter count

}
    
function SetPagination(ptotal, pagenumber) {
    if (parseInt(pagenumber) == 0 || pagenumber == undefined) {
        pagenumber = 1;
    }
    //$("#pagination").pagination({
    //    total: parseInt(ptotal),
    //    pageSize: pSize,
    //    showPageList: false,
    //    showRefresh: false,
    //    loading: false,
    //    pageNumber: pagenumber,
    //    onSelectPage: function (pageNumber, pageSize) {
    //        GetDesigns(displayStart, pageSize, pageNumber)
    //    },
    //    onChangePageSize: function (pageSize) {
    //    },
    //    onBeforeRefresh: function (pageNumber, pageSize) {
    //    },
    //    onRefresh: function (pageNumber, pageSize) {
    //    },
    //    onValidate: function () {
    //        return true;
    //    },
    //});
}

function ClearOneFilter(type, id) {
    switch (type) {
        case "designBySearch":
            $("#searchDesign").val("");
            break;
        case "favoriteFrom":
            $("#startDate").val("");
            break;
        case "favoriteTo":
            $("#endDate").val("");
            break;
        case "downloadFrom":
            $("#startDateDownload").val("");
            $("#endDateDownload").val("");
            break;
        case "downloadTo":
            $("#endDateDownload").val("");
            break;
        case "filterSubCategory":
            $("#chkDesignByCategory[value='" + id + "']").prop("checked", false);
            break;
        case "stitch":
            $("#chkStitch[value='" + id + "']").prop("checked", false);
            break;
        case "area":
            $("#chkArea[value='" + id + "']").prop("checked", false);
            break;
        case "niddle":
            $("#chkNiddle[value='" + id + "']").prop("checked", false);
            break;
        case "height":
            $("#chkHeight[value='" + id + "']").prop("checked", false);
            break;
        case "width":
            $("#chkWidth[value='" + id + "']").prop("checked", false);
            break;
        default:
    }
    GetDesigns();
    const mediaQuery = window.matchMedia('(max-width : 992px)')
    if (mediaQuery.matches) {
        $("#mainDivActiveFilter").css("display", "block");
    }
}

function ClearAllFilter(type) {
    switch (type) {
        case "designBySearch":
            $("#searchDesign").val("");
            break;
        case "favoriteFrom":
            $("#startDate").val("");
            $("#endDate").val("");
            break;
        case "downloadFrom":
            $("#startDateDownload").val("");
            $("#endDateDownload").val("");
            break;
        case "filterSubCategory":
            $("#chkDesignByCategory:checked").prop("checked", false);
            break;
        case "stitch":
            $("#chkStitch:checked").prop("checked", false);
            break;
        case "area":
            $("#chkArea:checked").prop("checked", false);
            break;
        case "niddle":
            $("#chkNiddle:checked").prop("checked", false);
            break;
        case "height":
            $("#chkHeight:checked").prop("checked", false);
            break;
        case "width":
            $("#chkWidth:checked").prop("checked", false);
            break;
        default:
    }
    GetDesigns();
    const mediaQuery = window.matchMedia('(max-width : 992px)')
    if (mediaQuery.matches) {
        $("#mainDivActiveFilter").css("display", "block");
    }
}


function AddOrRemoveFavourite(DesignId) {
    if ($("#userid").val() == null) {
        showToastPortal('danger', '', MessagePortal.LoginForFavourite.replace("##siteUrl##", siteURL), 60000);
    }
    else {
        if ($("#favouriteIcon[value='" + DesignId + "']").hasClass("favouriteIconChecked")) {
            $("#favouriteIcon[value='" + DesignId + "']").removeClass("favouriteIconChecked");
            $("#IsAddFavourite").val(0);
        }
        else {
            $("#favouriteIcon[value='" + DesignId + "']").addClass("favouriteIconChecked");
            $("#IsAddFavourite").val(1);
        }
        $("#DesignId").val(DesignId);

        var model = ({
            "DesignId": $("#DesignId").val(),
            "IsAddFavourite": $("#IsAddFavourite").val()
        });

        $.ajax({
            type: "POST",
            data: model,
            url: siteURL + "Secure/AddOrRemoveFavourite"
        }).done(function (result) {
            if (result.isFavourite.IsAddFavourite == 1) {
                showToastPortal('success', '', MessagePortal.AddFavourite, 3000);
            }
            else {
                showToastPortal('success', '', MessagePortal.RemoveFavourite, 3000);
            }
        }).fail(function (result) {
            showToastPortal('danger', '', 'please try again');
        });;
    }

}

function formatCountry(country) {
    if (!country.id) {
        return country.text;
    }
    var CountryAbbr = (country.text.split(',')[0]).trim();
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
    var CountryAbbr = (country.text.split(',')[0]).trim();
    var CountryCode = country.text.split(',')[1];
    var CountryName = country.text.split(',')[2];
    if (CountryAbbr != null && CountryAbbr != undefined && CountryAbbr != "null")
        var $country = $(
            '<span class="fi fi-' + CountryAbbr.toLowerCase() + ' fis"></span>' +
            '<span class="flag-text">' + "   +" + CountryCode + '</span>'
        );
    return $country;
};

function ResendOTP(Model) {
    $.ajax({
        url: siteURL + "Home/ResendOTP",
        type: "POST",
        data: { model: Model },
        cache: false,
        success: function (result) {
            showToastPortal(result.messagetype, '', result.message, 5000);
        }
    });
}


/* Download */
function DownloadDesignConfirm(DesignId) {
    if ($("#userid").val() == null) {
        showToastPortal('danger', '', MessagePortal.LoingForDownload.replace("##siteUrl##", siteURL), 60000);
    }
    else {
        DownloadDesignConfirmCallback(DesignId);
    }
}


function DownloadDesignConfirmCallback(DesignId) {
    RequestDownloadFile(DesignId);
}

function NoPackageFoundConfirmCallback() {
    callAnotherPage('packagelist.html');
}

function RequestDownloadFile(DesignId) {

    var sessionUserId = $("#userid").val();
    //async:false for allowing iOs dowloading in new tab using external link
    $.ajax({
        url: siteURL + "Secure/DownloadDesign",
        type: 'POST',
        async: false,
        data: ({ "DesignId": DesignId, "UserId": sessionUserId }),
        success: function (result) {
            if (result) {
                switch (result.downloadDesignResponse.RetStatus) {
                    case 0:
                        showToastPortal('danger', '', MessagePortal.DownloadDesignCase0, 3000);
                        break;
                    case 1:
                        window.location.href = siteURL + "download-file";
                        showToastPortal('success', '', MessagePortal.DownloadDesignCase1, 3000);
                        break;
                    case 2:
                        window.location.href = siteURL + "download-file";
                        showToastPortal('success', '', MessagePortal.DownloadDesignCase2, 3000);
                        break;
                    case 3:
                        showToastPortal('danger', '', MessagePortal.DownloadDesignCase3, 3000);
                        break;
                    case 4:
                        showToastPortal('danger', '', MessagePortal.DownloadDesignCase4, 3000);
                        break;
                    case 5:
                        showToastPortal('danger', '', MessagePortal.DownloadDesignCase5, 3000);
                        break;
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.DownloadDesignFail, 3000);
        }
    });
}


function formatCurrencyIsoCode(country) {
    if (!country.id) {
        return country.text;
    }
    var CountryAbbr = (country.text.split(',')[0]).trim();
    var CountryCode = country.text.split(',')[1];
    var CountryName = country.text.split(',')[2];
    if (CountryAbbr != null && CountryAbbr != undefined && CountryAbbr != "null")
        var $country = $(
            '<span class="fi fi-' + CountryAbbr + ' fis"></span>' +
            '<span class="flag-text"><b>' + '  ' + CountryAbbr + '</span>'
        );
    return $country;
};
function formatCurrencyIsoCodeSelectionType(country) {
    if (!country.id) {
        return country.text;
    }
    var CountryAbbr = (country.text.split(',')[0]).trim();
    var CountryCode = country.text.split(',')[1];
    var CountryName = country.text.split(',')[2];
    if (CountryAbbr != null && CountryAbbr != undefined && CountryAbbr != "null")
        var $country = $(
            '<span class="fi fi-' + CountryAbbr + ' fis"></span>' +
            '<span class="flag-text">' + "" + CountryAbbr + '</span>'
        );
    return $country;
};

function HeartIcon() {
    if ($("#userid").val() == null) {
        showToastPortal('danger', '', MessagePortal.LoginForHeartIconFavourite.replace("##siteUrl##", siteURL), 60000);
    }
    else {
        window.location.href = siteURL + "favourite-design";
    }
}


/* Add Design To Cart */
function AddFoodToCartConfirm(FoodId) {
    if ($("#userid").val() == null) {
        showToastPortal('danger', '', MessagePortal.LoingForAddTocart.replace("##siteUrl##", siteURL), 60000);
    }
    else {
        RequestUserCartDetails(FoodId, 1);
    }
}

function AddToCart(FoodId) {
    if ($("#userid").val() == null) {
        showToastPortal('danger', '', MessagePortal.LoingForAddTocart.replace("##siteUrl##", siteURL), 60000);
    }
    else {
        var Qauntity = $(".input-number__input").val();
        RequestUserCartDetails(FoodId, parseInt(Qauntity) + 1);
    }
}

function RequestUserCartDetails(FoodId, Qauntity) {
    //async:false for allowing iOs dowloading in new tab using external link
    $.ajax({
        url: siteURL + "Secure/AddFoodToCart",
        type: 'POST',
        async: false,
        data: ({ "FoodId": FoodId, "Qauntity": Qauntity }),
        success: function (result) {
            $(".topHeaderCartIcon .indicator__counter").html(result.Data.TotalCount);
            //$(".topHeaderCartIcon .indicator__value").html(result.Data.TotalPrice);
            $(".mobileTopHeaderCartIcon .mobile-indicator__counter").html(result.Data.TotalCount);
            if (result.Data.result == 1)  // 1 - inserting new value
            {
                showToastPortal('success', '', MessagePortal.AddTocartSuccess, 2000);
                setTimeout(function () { location.reload() }, 1000);
            }
            else if (result.Data.result == 2)  //2 -not add duplicate value
            {
                showToastPortal('warning', '', MessagePortal.AddTocartAlredyExist, 2000);
            }
            else if (result.Data.result == 3) {
                showToastPortal('success', '', MessagePortal.AddTocartSuccess, 2000);
                setTimeout(function () { location.reload() }, 1000);
            }
            else {
                showToastPortal('danger', '', MessagePortal.AddTocartFail, 3000);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.AddTocartFail, 3000);
        }
    });
}

function RemoveFromCart(FoodId, IsDeleteFromCart) {
    $.ajax({
        url: siteURL + "Secure/RemoveFoodFromCart",
        type: "POST",
        data: { FoodId: FoodId, IsDeleteFromCart: IsDeleteFromCart },
        cache: false,
        success: function (result) {
            if (IsDeleteFromCart == false) {
                if (result.RemoveFoodFromCart == 1) {
                    window.location.href = siteURL + "cart-detail"
                    showToastPortal('success', '', MessagePortal.RemoveFromcartSuccess, 3000);
                }
                else if (result.RemoveFoodFromCart == 3) {
                    showToastPortal('success', '', MessagePortal.RemoveFromcartSuccess, 3000);
                    setTimeout(function () { location.reload() }, 2000);
                }
                else {
                    showToastPortal('danger', '', MessagePortal.RemoveFromcartFail, 3000);
                }
            }
            else if (IsDeleteFromCart == true) {
                setTimeout(function () { location.reload() }, 2000);
                $("#TopHeaderCartList").html(result);     //When TopMenuCart in remove Design Its Only return View
                showToastPortal('success', '', MessagePortal.RemoveFromcartSuccess, 3000);
            }
            else {
                showToastPortal('danger', '', MessagePortal.RemoveFromcartFail, 3000);
            }
        }
    });
}