
$(document).ready(function () {
    GetFoodList();
});

$(document).on('click', '#FoodSearch', function () {
    $("#FoodList").dataTable().fnDestroy();
    GetFoodList();
});

function GetFoodList() {
    $('#FoodList').dataTable({
        "responsive": true,
        "bServerSide": true,
        "sAjaxSource": adminSiteURLPortal + 'AdminUser/GetFoodList',
        "fnServerParams": function (aoData, fnCallback) {
            aoData.push({ "name": "RestaurantID", "value": RestaurantID });
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
            { "sName": "RestaurantID", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "FoodID", "bSearchable": false, "bSortable": true, "bVisible": false },
            { "sName": "FoodName", "bSearchable": false, "bSortable": true },
            { "sName": "Price", "bSearchable": false, "bSortable": true },
            { "sName": "Ingredient", "bSearchable": false, "bSortable": true },
            { "sName": "IsBestSeller", "bSearchable": false, "bSortable": true },
            { "sName": "IsVegetarian", "bSearchable": false, "bSortable": true },
            {
                "sName": "Rate", "bSearchable": false, "bSortable": true,
                "mRender": function (data, type, aoData) {
                    var ID = aoData[1];
                    var Val = aoData[7];
                    var html = `<div class="col-md-12 rating_`+ID+` faPrimaryColor" style="color:#ffe339;font-size:25px"><input type="text" id="ProspectRating_` + ID + `" Name="ProspectRating_` + ID + `" Class="form-control field-input hidden" value="`+Val+`" /></div>
                    <script>
                        $(".rating_`+ID+`").rate();
                        var options = {
                            initial_value: `+Val+`,
                            readonly: true,
                            change_once: true,
                        }
                        //$("#ProspectRating_`+ ID + `").val();
                        $(".rating_`+ ID +`").rate(options);
                        $(".rating_`+ID+`").rate("setValue", $("#ProspectRating_`+ ID+`").val());
                        $(".rating_`+ID+`").rate("destroy");
                    </script>
                    `;
                    return html;
                }
            },
            
        ],
        oLanguage: {
            sEmptyTable: "No Records Found."
        }
    });
}
