var activityId;
var idstoDelete = [];
$(document).ready(function () {
    $(".alert").delay(4000).slideUp(200, function () {
        $(this).alert('close');
    });
    ActivityGridLoad();
    $(document).on("click", ".chkActivity", function () {
        var chkbox = $(this);
        if ($(this).is(":checked")) {
            $(this).parents("tr").addClass("k-state-selected");
            //uncheckallchkbox(chkbox.attr("id"));
        }
        else {
            $(this).parents(".k-state-selected").removeClass("k-state-selected");
            activityId = undefined;
            $("#editactivity").hide();
            $("#deleteactivity").hide();
        }
        getallSelectedRowId();
    });
    $("#btnconfirmdelete").click(function () {
        $('#myModal').modal('hide');
        deleteActivitybyid();
    });
    $("#editactivity").click(function () {
        window.location.href = baseURL + "/contract/editactivity/" + activityId;
    });
    $("#addactivity").click(function () {
        window.location.href = baseURL + "/contract/newactivity/" + $("#ContractId").val();
    });

    $("#SearchActivity").keyup(function () {
        var val = $('#SearchActivity').val();
        $("#ActivityGrid").data("kendoGrid").dataSource.filter({
            logic: "or",
            filters: [
                          {
                              field: "ContractCode",
                              operator: "contains",
                              value: val
                          },
            ]
        });
    });
    if ($("#ContractId").val() > 0) {
        $("#addactivity").show();
    }
});

function ActivityGridLoad() {
    $('#ActivityGrid').kendoGrid({
        dataSource: GetActivityDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: false,
        resizable: true,
        change: onRowSelectChange,
        columns: [
            {
                field: "Id",
                title: "ID",
                hidden: true,
            },
            {
                field: "",
                title: "",
                template: "<input class='chkActivity' id='#:Id#' type='checkbox'>",
                width: 30,
            },
            {
                field: "ContractId",
                title: "Contract Id",
                hidden: true,
            },
             {
                 field: "ActivityCode",
                 title: "Activity Code",
             },
            {
                field: "FromDate",
                title: "From Date",
                template: '#= kendo.toString(FromDate, "MM/dd/yyyy") #',
            },
            {
                field: "EndDate",
                title: "End Date",
                template: '#= kendo.toString(EndDate, "MM/dd/yyyy") #',
            },
             {
                 field: "Charges",
                 title: "Charges",
                 template: "#if(Charges == 'true'){ #<span>Revenue<span># } else { #<span>Expense<span> # }#",
             },
            {
                field: "Estimate",
                title: "Value",
                template: "# if(Estimate == 'true'){ #<span>Actual<span># } else { #<span>Projected<span> # } #",
                footerTemplate: "<span class='pull-right'>Total</span>",
            },
            {
                field: "Amount",
                title: "Amount($)",
                template: "#if(Amount < 0){ #<span>$#=kendo.toString(Math.abs(Amount),'n')#<span># } else { #<span>$#=kendo.toString(Amount,'n')#<span> # }#",
                //template: "$#=Amount#",
                footerTemplate: "$#= kendo.toString(sum, 'n') #",
            },
            {
                field: "IsBilled",
                title: "Invoice",
                template: "# if(IsBilled == true){ #<span>Sent<span># } else { #<span>Pending<span> # } #",
            },
        ],
        selectable: "multiple",
        resizable: true,
        dataBound: function () {
            //var totalAmt = $(".k-footer-template td").last();
            //totalAmt = totalAmt.html().split(':')[1];
            //totalAmt = $.trim(totalAmt);
            //$("#TotalAmount").val(totalAmt);
        }
    }).data("kendoGrid");

    resizeGrid();
}
function onRowSelectChange(arg) {
    var grid = arg.sender;
    var currentDataItem = grid.dataItem(this.select());
    if (currentDataItem != undefined) {
        $("#editactivity").show();
        $("#deleteactivity").show();
        activityId = currentDataItem.Id;
        //uncheckallchkbox(contractId);

    }
    getallSelectedRowId();
}
function GetActivityDetail() {
    var source;
    if ($("#ContractId").val() > 0) {
        source = GetActivitiesByContractId($("#ContractId").val());
    }
    else {
        source = GetAllActivity()
    }
    return source;
}
function GetAllActivity() {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        //pageSize: 10,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Contract/GetAllActivities",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                cache: false,
            },
        },
        aggregate: [
            { field: "Amount", aggregate: "sum" },
        ],
        group: { field: "ContractCode" },
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    ContractId: { type: "int" },
                    FromDate: { type: "date" },
                    EndDate: { type: "date" },
                    Amount: { type: "number" },
                    Charges: { type: 'string' },
                    Estimate: { type: 'string' },
                    ContractCode: { type: 'string' },
                    IsBilled: { type: 'bool' },
                    ActivityCode: { type: 'string' }
                }
            }
        },

    });
    return source;
}
function GetActivitiesByContractId(contractid) {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        //pageSize: 10,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Contract/GetActivitiesByContractId",
                data: { contractId: contractid },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                cache: false,
            },
        },
        aggregate: [
            { field: "Amount", aggregate: "sum" },
        ],
        group: { field: "ContractCode" },
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    ContractId: { type: "int" },
                    FromDate: { type: "date" },
                    EndDate: { type: "date" },
                    Amount: { type: "number" },
                    Charges: { type: 'string' },
                    Estimate: { type: 'string' },
                    ContractCode: { type: 'string' },
                    IsBilled: { type: 'bool' },
                    ActivityCode: { type: 'string' }
                }
            }
        },
        //aggregate: [
        //{ field: "Ammount", aggregate: "sum" }
        //],
    });
    return source;
}

function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#ActivityGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}

function getallSelectedRowId() {
    idstoDelete = [];
    $("#ActivityGrid").find("tr.k-state-selected").each(function (i, data) {
        idstoDelete.push(data.children[1].textContent);
    });
    if (idstoDelete.length > 1) {
        $("#editactivity").hide();
        //$("#viewactivity").hide();
        //$("#addactivity").hide();
        $("#deleteactivity").show();
    }
    else if (idstoDelete.length == 1) {
        if (idstoDelete == "") {
            $("#editactivity").hide();
            //$("#viewactivity").hide();
            //$("#addactivity").hide();
            $("#deleteactivity").hide();
        }
        else {
            activityId = idstoDelete[0];
            //$("#addactivity").show();
            $("#editactivity").show();
            //$("#viewactivity").show();
            $("#deleteactivity").show();
        }
    }
    else {
        //$("#addactivity").show();
        $("#editactivity").hide();
        //$("#viewactivity").show();
        $("#deleteactivity").hide();
    }
}

function deleteActivitybyid() {
    $.ajax({
        type: "GET",
        url: baseURL + "/Contract/DeleteActivityById",
        data: { id: idstoDelete.join(',') },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            if (data == false) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>Activity delete failed!! " +
                    "</div>");
            }
            else {
                $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Success!</strong>Activity deleted Successfully!! " +
                    "</div>");
            }
            ActivityGridLoad();
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