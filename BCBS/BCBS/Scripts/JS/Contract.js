var contractId;
var idstoDelete = [];
$(document).ready(function () {
    $(".menuContract").addClass("menuactive");
    $("#deletecontract").hide();

    $(".alert").delay(4000).slideUp(200, function () {
        $(this).alert('close');
        $("#message").html("");
    });
    $(".dropdown").click(function () {
        $(this).find(".menu-expand").toggle();
    })
    ContractGridLoad();
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget)
        var recipient = button.data('whatever')
        var modal = $(this)
        modal.find('.modal-title').text('New message to ' + recipient)
        modal.find('.modal-body input').val(recipient)
    });
    $("#addcontract").click(function () {
        window.location.href = baseURL + "/contract/new";
    });
    //$("#addactivity").click(function () {
    //    window.location.href = baseURL + "/Contract/NewActivity/" + contractId;
    //});
    $("#editcontract").click(function () {
        if (contractId > 0) {
            window.location.href = baseURL + "/contract/edit/" + contractId;
        }
    });
    //$("#viewactivity").click(function () {
    //    if (contractId > 0) {
    //        window.location.href = baseURL + "/Contract/Activities/" + contractId;
    //    }
    //});
    $("#btnconfirmdelete").click(function () {
        $('#myModal').modal('hide');
        deleteContractbyid();
    });
    $(document).on("click", ".chkContract", function () {
        var chkbox = $(this);
        if ($(this).is(":checked")) {
            $(this).parents("tr").addClass("k-state-selected");
            //uncheckallchkbox(chkbox.attr("id"));
        }
        else {
            $(this).parents(".k-state-selected").removeClass("k-state-selected");
            contractId = undefined;
            $("#editcontract").hide();
            $("#deletecontract").hide();
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
    $("#ContractGrid").find("tr.k-state-selected").each(function (i, data) {
        idstoDelete.push(data.firstChild.textContent);
    });
    if (idstoDelete.length > 1) {
        $("#editcontract").hide();
        //$("#viewactivity").hide();
        //$("#addactivity").hide();
        $("#deletecontract").show();
    }
    else if (idstoDelete.length == 1) {
        if (idstoDelete == "") {
            $("#editcontract").hide();
            //$("#viewactivity").hide();
            //$("#addactivity").hide();
            $("#deletecontract").hide();
        }
        else {
            contractId = idstoDelete[0];
            //$("#addactivity").show();
            $("#editcontract").show();
            //$("#viewactivity").show();
            $("#deletecontract").show();
        }
    }
    else {
        //$("#addactivity").hide();
        $("#editcontract").hide();
        //$("#viewactivity").hide();
        $("#deletecontract").hide();
    }
}
function uncheckallchkbox(selectedid) {
    $(".chkContract").each(function () {
        if ($(this).attr("id") != selectedid) {
            $(this).attr("checked", false);
        }
    });
}

function deleteContractbyid() {
    $.ajax({
        type: "GET",
        url: baseURL + "/Contract/DeleteContractById",
        data: { id: idstoDelete.join(',') },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            if (data == false) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>Contract delete failed!! " +
                    "</div>");
            }
            else {
                $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Success!</strong>Contract deleted Successfully!! " +
                    "</div>");
            }
            ContractGridLoad();
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

function ContractGridLoad() {
    $('#ContractGrid').kendoGrid({
        dataSource: GetContractDetail(),
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
                template: "<input class='chkContract' id='#:Id#' type='checkbox'>",
                width: 30,
            },
            {
                field: "Customer",
                title: "Customer",
            },
            {
                field: "Service",
                title: "Service",
            },
            {
                field: "Project",
                title: "Project",
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
                field: "Status",
                title: "Status",
            },
            {
                field: "Dirrection",
                title: "Charges",
                template: "#if(Dirrection == 'true'){ #<span>Revenue<span># } else { #<span>Expense<span> # }#",
            },
            {
                field: "Estimate",
                title: "Value",
                template: "# if(Estimate == 'true'){ #<span>Actual<span># } else { #<span>Estimate<span> # } #",
            },
            {
                field: "Amount",
                title: "Amount($)",
                //template: "$#=Amount#",
                template: '$#= kendo.toString(Amount,"n") #',

                //footerTemplate: "Total Count: #=sum#",
            },
            {
                field: "Id",
                title: " ",
                template: "<a class='btn btn-primary' href='" + baseURL + "/contract/newactivity/#=Id#'>Add Activity</a>",
            },
        {
            field: "Id",
            title: " ",
            template: "<a class='btn btn-primary' href='" + baseURL + "/contract/activities/#=Id#'>View Activities</a>",
        }
        ],
        selectable: "multiple",
        resizable: true,
    }).data("kendoGrid");
    resizeGrid();
}

function GetContractDetail() {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        pageSize: 10,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Contract/GetContractList",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                cache: false,
            },
        },
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    Customer: { type: "string" },
                    Service: { type: "string" },
                    Project: { type: "string" },
                    FromDate: { type: "date" },
                    EndDate: { type: "date" },
                    Status: { type: "string" },
                    Dirrection: { type: "string" },
                    Estimate: { type: "string" },
                    Amount: { type: "number" },
                }
            }
        },
        //aggregate: [
        //{ field: "Amount", aggregate: "sum" }
        //],
    });
    return source;
}

function GetContractListDataSource() {
    var source;
    $.ajax({
        type: "GET",
        url: baseURL + "/Contract/GetContractList",
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
        $("#editcontract").show();
        $("#deletecontract").show();
        contractId = currentDataItem.Id;
        //uncheckallchkbox(contractId);
        getallSelectedRowId();
    }
}

function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#ContractGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}