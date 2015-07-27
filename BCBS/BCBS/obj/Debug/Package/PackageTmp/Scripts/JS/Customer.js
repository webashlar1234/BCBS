var customerId;
var idstoDelete = [];
$(document).ready(function () {
    $("#settingitems").toggle();
    $(".menuCustomers").addClass("menuactive");
    $("#deletecustomer").hide();
    $(".alert").delay(4000).slideUp(200, function () {
        $(this).alert('close');
    });
    $(".dropdown").click(function () {
        $(this).find(".menu-expand").toggle();
    })
    CustomerGridLoad();
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget)
        var recipient = button.data('whatever')
        var modal = $(this)
        modal.find('.modal-title').text('New message to ' + recipient)
        modal.find('.modal-body input').val(recipient)
    });
    $("#addcustomer").click(function () {
        window.location.href = baseURL + "/customer/new";
    })
    $("#editcustomer").click(function () {
        if (customerId > 0) {
            window.location.href = baseURL + "/customer/edit/" + customerId;
        }
    });
    $("#btnconfirmdelete").click(function () {
        $('#myModal').modal('hide');
        deleteCustomerbyid();
    });
    $(document).on("click", ".chkCustomer", function () {
        var chkbox = $(this);
        if ($(this).is(":checked")) {
            //uncheckallchkbox(chkbox.attr("id"));
            $(this).parents("tr").addClass("k-state-selected");
        }
        else {
            $(this).parents(".k-state-selected").removeClass("k-state-selected");
            customerId = undefined;
            $("#editcustomer").hide();
            $("#deletecustomer").hide();
        }
        getallSelectedRowId();
    });
    $(window).resize(function () {
        resizeGrid();
        if ($("#wrapper").hasClass("toggled")) {

            $("#wrapper").removeClass("toggled");
        }
    });
});
function getallSelectedRowId() {
    idstoDelete = [];
    $("#CustomerGrid").find("tr.k-state-selected").each(function (i, data) {
        idstoDelete.push(data.firstChild.textContent);
    });
    if (idstoDelete.length > 1) {
        $("#editcustomer").hide();
        $("#deletecustomer").show();
    }
    else if (idstoDelete.length == 1) {
        if (idstoDelete == "") {
            $("#editcustomer").hide();
            $("#deletecustomer").hide();
        }
        else {
            customerId = idstoDelete[0];
            $("#editcustomer").show();
            $("#deletecustomer").show();
        }
    }
    else {
        $("#editcustomer").hide();
        $("#deletecustomer").hide();
    }
}
function uncheckallchkbox(selectedid) {
    $(".chkCustomer").each(function () {
        if ($(this).attr("id") != selectedid) {
            $(this).attr("checked", false);
        }
    });
}

function deleteCustomerbyid() {
    $.ajax({
        type: "GET",
        url: baseURL + "/Customer/DeleteCustomerById",
        data: { id: idstoDelete.join(',') },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            if (data == false) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>Customer delete failed!! " +
                    "</div>");
            }
            else {

                $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Success!</strong>Customer deleted Successfully!! " +
                    "</div>");
            }
            CustomerGridLoad();
            $(".alert").show();
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
        },
        failure: function (data) {
            console.log(data);
        }
    });
}

function CustomerGridLoad() {
    $('#CustomerGrid').kendoGrid({
        dataSource: GetCustomerDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: true,
        resizable: true,
        change: onRowSelectChange,
        columns: [
            {
                field: "Id",
                title: "ID",
                width: 50,
                hidden: true,
            },
            {
                field: "",
                title: "",
                template: "<input class='chkCustomer' id='#:Id#' type='checkbox'>",
                width: 30,
            },
            {
                field: "Name",
                title: "Name",
            },
            {
                field: "ChargeCode",
                title: "Customer Code",
            },
            {
                field: "CustomerType",
                title: "Customer Type",
            },
            {
                field: "CustomerAddress",
                title: "Customer Address",
            },
            {
                field: "City",
                title: "City",
            },
            //{
            //    field: "PostalCode",
            //    title: "Postal Code",
            //},
            //{
            //    field: "State",
            //    title: "State",
            //},
            //{
            //    field: "Country",
            //    title: "Country",
            //},
            //{
            //    field: "FirstName",
            //    title: "First Name",
            //},
            //{
            //    field: "LastName",
            //    title: "Last Name",
            //},
            //{
            //    field: "Phone",
            //    title: "Phone",
            //},
            //{
            //    field: "Fax",
            //    title: "Fax",
            //},
            //{
            //    field: "Occupation",
            //    title: "Occupation",
            //},
            //{
            //    field: "Email",
            //    title: "Email",
            //},
            //{
            //    field: "Status",
            //    title: "Status",
            //},
        ],
        selectable: "multiple",
        resizable: true,
    }).data("kendoGrid");
    resizeGrid();
}

function GetCustomerDetail() {
    var source = new kendo.data.DataSource({
        //data: GetCustomerListDataSource(),
        autoSync: true,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Customer/GetCustomerList",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                cache: false,
            },
        },
        pageSize: 10,
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    Name: { type: "string" },
                    ChargeCode: { type: "string" },
                    CustomerType: { type: "string" },
                    CustomerAddress: { type: "string" },
                    City: { type: "string" },
                    PostalCode: { type: "string" },
                    State: { type: "string" },
                    Country: { type: "string" },
                    FirstName: { type: "string" },
                    LastName: { type: "string" },
                    Phone: { type: "string" },
                    Fax: { type: "string" },
                    Occupation: { type: "string" },
                    Email: { type: "string" },
                    Status: { type: "string" }
                }
            }
        },
    });
    return source;
}

function GetCustomerListDataSource() {
    var source;
    $.ajax({
        type: "GET",
        url: baseURL + "/Customer/GetCustomerList",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            source = data;
        },
        failure: function (data) {
            console.log(data);
        }
    });
    return source;
}

function onRowSelectChange(arg) {
    var grid = arg.sender;
    var currentDataItem = grid.dataItem(this.select());
    if (currentDataItem != undefined) {
        $("#editcustomer").show();
        $("#deletecustomer").show();
        customerId = currentDataItem.Id;
        //uncheckallchkbox(customerId);
    }
    getallSelectedRowId();
}

function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#CustomerGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}