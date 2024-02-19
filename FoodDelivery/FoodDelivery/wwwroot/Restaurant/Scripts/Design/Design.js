function GetDesignList() {
    $('#dtDesign').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Design/GetDesignList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push(
                { "name": "Name", "value": $("#name").val() },
                { "name": "StartDate", "value": $("#startDate").val() },
                { "name": "EndDate", "value": $("#endDate").val() },
                { "name": "CategoryId", "value": $("#ddlCategory").val() },
                { "name": "SubCategoryId", "value": $("#ddlSubCategory").val() },
                { "name": "SubSubCategoryId", "value": $("#ddlSubSubCategory").val() },
                { "name": "DesignId", "value": $("#designId").val() },
                { "name": "Stitch", "value": $("#stitch").val() },
                { "name": "Width", "value": $("#width").val() },
                { "name": "AreaId", "value": $("#ddlArea").val() },
                { "name": "FileId", "value": $("#ddlFile").val().join(",") },
                { "name": "TypeId", "value": $("#ddlType").val().join(",") },
                { "name": "NiddleId", "value": $("#ddlNiddle").val() },
                { "name": "Height", "value": $("#height").val() },
                { "name": "AdminUserId", "value": $("#ddlAdminUser").val() },
                { "name": "IsPublish", "value": $("#ddlIsPublish").val() });
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
            { "sName": "DesignId", "bSearchable": false, "bSortable": true },
            { "sName": "Name", "bSearchable": false, "bSortable": true },
            { "sName": "CategorySEOUrl", "bVisible": false, "bSearchable": false, "bSortable": true },
            { "sName": "Category", "bSearchable": false, "bSortable": true },
            { "sName": "SubCategory", "bSearchable": false, "bSortable": true },
            { "sName": "SubSubCategory","bSearchable": false,"bSortable": true },
            { "sName": "IsPublish","bSearchable": false,"bSortable": true },
            { "sName": "ChangedBy","bSearchable": false,"bSortable": true },
            { "sName": "CreatedOn","bSearchable": false, "bSortable": true },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-design" + "?DesignId=" + aoData[0];
                    var designDetailHref = "/design-details/" + aoData[2] + "/" + aoData[9] + "-" + aoData[0];
                    var pinViewHtml = '';
                    if (aoData[10] != null)
                    {
                        pinViewHtml = '<a target="_blank" href=\"https://in.pinterest.com/pin/' + aoData[10] + '\" title="View Pin" ><i class="fab fa-pinterest fa-fw" style="cursor:pointer;"></i></a>';
                    }
                    return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteDesign(' + aoData[0] + ')" class="fas fa-trash-alt"></i> <a target="_blank" href=\"' + designDetailHref + '\"><i class="fas fa-eye" style="cursor:pointer;"></i></a><i style="cursor:pointer;" onclick="javascript:CreateOrUpdatePinterestPin(' + aoData[0] + ')" title="Create/Update Pinterest Pin" class="fab fa-pinterest-p fa-fw"></i>' + pinViewHtml;

                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteDesign(DesignId) {
    $(".modal-header #DesignId").val(DesignId);
    $("#confirmDeleteDesignModal").modal('show');;
}

function DeleteDesign(DesignId) {
    $("#confirmDeleteDesignModal").modal('hide');
    var DesignId = $("#DesignId").val();
    $.ajax({
        url: siteURLPortal + "Design/DeleteDesign",
        type: 'POST',
        data: { "DesignId": DesignId },
        dataType: "json",
    })
        .done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.DesignDeleted);
                var oTable = $('#dtDesign').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.DesignDeleteFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}

function FillSubCategory() {
    var CategoryId = $('#ddlCategory').val();
    $.ajax({
        url: siteURLPortal + "Design/FillSubCategory",
        type: "GET",
        dataType: "JSON",
        data: { CategoryId: CategoryId },
        success: function (SubCategoryList) {
            $("#ddlSubCategory").html(""); // clear before appending new list
            $.each(SubCategoryList, function (i, SubCategory) {
                $("#ddlSubCategory").append(
                    $('<option></option>').val(SubCategory.CategoryId).html(SubCategory.Name));
            });
            $("#ddlSubSubCategory").html("");
        }
    });
}

function FillSubSubCategory() {
    var SubCategoryId = $('#ddlSubCategory').val();
    $.ajax({
        url: siteURLPortal + "Design/FillSubCategory",
        type: "GET",
        dataType: "JSON",
        data: { CategoryId: SubCategoryId, IsSubSubCategory: true },
        success: function (SubCategoryList) {
            $("#ddlSubSubCategory").html(""); // clear before appending new list
            $.each(SubCategoryList, function (i, SubCategory) {
                $("#ddlSubSubCategory").append(
                    $('<option></option>').val(SubCategory.CategoryId).html(SubCategory.Name));
            });
        }
    });
}


function onSuccessAddEditDesign(result) {
    if (result) {
        $.ajax({
            url: siteURLPortal + "Design/CopyFileToCloudStorage",
            type: "POST",
            dataType: "JSON",
            data: { DesignId: result.getDesignSeoUrlModel.DesignId, CategorySEOurl: result.getDesignSeoUrlModel.CategorySEOurl, DesignImageName: DesignImageName, DesignFileName: DesignFileName, sourceBucket: "designs/images/original/default-category", sourceBucketThumb: "designs/images/thumbnail/default-category" },
            success: function () {
                showToastPortal('success', '', MessagePortal.DesignAddEdit);
                window.location.href = siteURLPortal + "designs"
            }
        });
    }
    else {
        showToastPortal('danger', '', MessagePortal.DesignAddEditFailed);
    }
}
function onFailAddEditDesign() {
    showToastPortal('danger', '', MessagePortal.DesignAddEditFailed);
}

function CreateOrUpdatePinterestPin(DesignId) {
    $.ajax({
        url: siteURLPortal + "Pinterest/CreateOrUpdatePinterestPin",
        type: 'POST',
        dataType: "json",
        data: { DesignId: DesignId },
    }).done(function (result) {
        if (result.Code == 0) {
            showToastPortal('danger', '', result.Message);
        }
        else if (result.Code == 201) {
            showToastPortal('success', '', 'Pin created');
        }
        else if (result.Code == 200) {
            showToastPortal('success', '', 'Pin updated');
        }
        else {
            showToastPortal('danger', '', result.Message);
        }   
    }).fail(function (result) {
        alert('You already have a Pin with this name')
    });
}