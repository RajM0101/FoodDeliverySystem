//Settings

$(document).ready(function () {
    $("#btnSitemap").on("click", function () {
        GenerateSitemapXML();
    });
});
function GenerateSitemapXML() {
    $.ajax({
        url: siteURLPortal + "Master/SiteMapGenerate",
        type: "GET",
        dataType: "json",
        data: {},
        success: function (msg) {
            window.open(siteURL + "/upload/xml/" + msg.filename, '_blank');
        },
        error: function (xhr, textStatus, error) {
            if (typeof console == "object") {
                console.log(xhr.status + "," + xhr.responseText + "," + textStatus + "," + error);
            }
        }
    });
}