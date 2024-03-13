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

window.addEventListener("load", function (event) {
    var historyTraversal = event.persisted ||
        (typeof window.performance != "undefined" &&
            window.performance.navigation.type === 2);
    if (historyTraversal) {
        // Handle page restore.
        window.location.reload();
    }
});

function showToastPortal(ToastType, Title, Message, Timeout) {
    $.toaster({ priority: ToastType, title: Title, message: Message, timeout: Timeout });
}
//#endregion

var MessagePortal =
{

    OrderDetailAdded: "OrderDeatil added successfully.",
    OrderDetailAddFailed: "OrderDeatil Addition failed.",
    OrderPlaced: "Order Placed successfully.",
    OrderPlacedFailed: "Order Placed failed.",

    InquiryRead: "Inquiry Read successfully.",
    InquiryReadFailed: "Inquiry Read to failed.",

    EmailDomainIsBlocked: "IsBlocked successfully.",
    EmailDomainIsBlockedFailed: "IsBlocked to failed.",

    UserAdded: "User added successfully.",
    UserAddFailed: "User addition failed.",
    UserDeleted: "User deleted succesfully.",
    UserDeleteFailed: "User failed to delete.",

    DesignAddEdit: "Design added successfully.",
    DesignAddEditFailed: "Design Addition failed.",
    DesignDeleted: "Design deleted succesfully.",
    DesignDeleteFailed: "Design failed to delete.",

    PackageAdded: "Package details added successfully.",
    PackageAddFailed: "Package add details failed.",
    PackageDetialsFailed: "Package details Addition failed.",
    PackageDeleted: "Package details deleted successfully.",
    PackageDeletedFailed: "Package details failed to delete.",

    AreaAdded: "Area details added successfully.",
    AreaAddFailed: "Area add details failed.",
    AreaDetialsFailed: "Area details Addition failed.",
    AreaDeleted: "Area details deleted successfully.",
    AreaDeletedFailed: "Area details failed to delete.",

    NiddleAdded: "Niddle details added successfully.",
    NiddleAddFailed: "Niddle add details failed.",
    NiddleDetialsFailed: "Niddle details Addition failed.",
    NiddleDeleted: "Niddle details deleted successfully.",
    NiddleDeletedFailed: "Niddle details failed to delete.",

    FileAdded: "File details added successfully.",
    FileAddFailed: "File add details failed",
    FileDetialsFailed: "File details Addition failed",
    FileDeleted: "File details deleted successfully",
    FileDeletedFailed: "File details failed to delete",

    TypeAdded: "Type details added successfully",
    TypeAddFailed: "Type add details failed",
    TypeDetialsFailed: "Type details Addition failed.",
    TypeDeleted: "Type details deleted successfully.",
    TypeDeletedFailed: "Type details failed to delete",

    CategoryAdded: "Category details added successfully.",
    CategoryUpdated: "Category details update successfully.",
    CategoryAddFailed: "Category add details failed.",
    CategoryDetialsFailed: "Category details Addition failed.",
    CategoryDeleted: "Category details deleted successfully.",
    CategoryDeletedFailed: "Category details failed to delete.",

    BannerAdded: "Banner details added successfully",
    BannerAddFailed: "Banner add details failed",
    BannerDetialsFailed: "Banner details Addition failed",
    BannerDeleted: "Banner details deleted successfully",
    BannerDeletedFailed: "Banner details failed to delete",

    SettingUpdate: "Setting details update successfully",
    SettingDetialsFailed: "Setting update details failed",
    SettingUpdateFailed: "Setting details Addition failed",

    AdminUserAdded: "Admin User added successfully",
    AdminUserAddFailed: "Admin User Addition failed.",
    AdminUserDeleted: "Admin User deleted successfully.",
    AdminUserDeleteFailed: "Admin User failed to delete",

    CMSPageAdded: "CMSPage added successfully",
    CMSPageAddFailed: "CMSPage Addition failed",
    CMSPageDeleted: "CMSPage deleted succesfully",
    CMSPageDeleteFailed: "CMSPage failed to delete",
    CMSPageDeleteFailed: "CMSPage failed to delete"
};