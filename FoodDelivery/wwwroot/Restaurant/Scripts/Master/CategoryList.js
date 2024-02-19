//CategoryList

$(document).ready(function () {
    GetCategoryList();
    $('#ddlParentCategoryId').select2();
    $('#ddlSubCategory').select2();
    $('#ddlIsActive').select2();
});

$(document).on('click', '#CategorySearch', function () {
    $("#CategoryList").dataTable().fnDestroy();
    GetCategoryList();
});

function GetCategoryList() {
    $('#CategoryList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": siteURLPortal + 'Master/GetCategoryList',    
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "Search", "value": $("#txtCategory").val() },
                { "name": "CategoryId", "value": $("#ddlSubCategory").val() },
                { "name": "ParentCategoryId", "value": $("#ddlParentCategoryId").val() },
                { "name": "IsActive", "value": $("#ddlIsActive").val() });
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
            {
                "sName": "CategoryId",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "Category",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "ParentCategory",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "ParentCategoryId",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "DisplayOrder",
                "bSearchable": false,
                "bSortable": true
            },
            {
                "sName": "Action",
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, aoData) {
                    var href = "/backoffice/add-edit-category" + "?CategoryId=" + aoData[0];  
                    if (aoData[2] != null && aoData[5] == 2) //second level category so create board based on name and section based on first level's category
                    {
                        return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteCategory(' + aoData[0] + ')" class="fas fa-trash-alt"></i> <i style="cursor:pointer;" onclick="javascript:CreateOrUpdateBoard(' + aoData[0] + ')" title="Create/Update Pinterest Board" class="fab fa-pinterest-p fa-fw"></i>';
                    }
                    else {
                        return '<a href=\"' + href + '\"><i class="fas fa-edit" style="cursor:pointer;"></i></a> <i style="cursor:pointer;" onclick="javascript:ConfirmDeleteCategory(' + aoData[0] + ')" class="fas fa-trash-alt"></i>';
                    }
                }
            }
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}

function ConfirmDeleteCategory(CategoryId) {
    $(".modal-header #CategoryId").val(CategoryId);
    $("#confirmDeleteCategoryModal").modal('show');;
}

function DeleteCategory() {
    $("#confirmDeleteCategoryModal").modal('hide');
    var CategoryId = $("#CategoryId").val();

    $.ajax({
        url: siteURLPortal + "Master/DeleteCategory",
        type: 'POST',
        data: { "CategoryId": CategoryId },
        dataType: "json",
    }).done(function (data, textStatus, jqXHR) {
            if (data.status == "1") {
                showToastPortal('success', '', MessagePortal.CategoryDeleted);
                var oTable = $('#CategoryList').dataTable();
                oTable.fnClearTable(0);
                oTable.fnDraw();
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            showToastPortal('danger', '', MessagePortal.CategoryDeletedFailed);
        }).always(function (data, textStatus, errorThrown) {
            //write content here
        });
}

function FillSubCategory() {
    var CategoryId = $('#ddlParentCategoryId').val();
    $.ajax({
        url: siteURLPortal + "Master/FillSubCategory",
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

function CreateOrUpdateBoard(CategoryId) {
    $.ajax({
        url: siteURLPortal + "Dashboard/CreateOrUpdateBoard",
        type: 'POST',
        dataType: "json",
        data: { CategoryId: CategoryId },
    }).done(function (result) {
        if (result.Code == 201) {
            showToastPortal('success', '', 'Board created'); //board and section both gets created
        }
        else if (result.Code == 200 || result.Code == 175) {
            showToastPortal('success', '', 'Board updated'); //if no any change in board section name then we are getting 175 and it says invalid board name but actually it don't need to change
        }
        else {
            showToastPortal('danger', '', result.Message);
        }
    }).fail(function (result) {
        showToastPortal('danger', '', 'Something wents wrong to create an Board!');
    });
}
