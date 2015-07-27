var AcknowledgementId;
var idstoDelete = [];
$(document).ready(function () {
    $("#formsitems").toggle();
    $(".menuACK").addClass("menuactive");
    AcknowledgementGridLoad();
    $("#addAcknowledgement").click(function () {
        window.location.href = baseURL + "/customer/acknowledgementform";
    });
    $("#btnapprove").click(function () {
        $.ajax({
            url: baseURL + "/customer/ApproveAcknowledgement",
            data: { ackId: AcknowledgementId },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
                if (data == true) {
                    $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                     "<strong>Acknowledgement Approved ! </strong>Contract Added Successfully!! " +
                     "</div>");
                    $(".alert").show();
                    $(".alert").delay(4000).slideUp(200, function () {
                        $(this).alert('close');
                    });
                }
                else {
                    $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>Acknowledgement not Approved!! " +
                    "</div>");
                    $(".alert").show();
                    $(".alert").delay(4000).slideUp(200, function () {
                        $(this).alert('close');
                    });
                }
                AcknowledgementGridLoad();
            },
            failure: function (data) {
                console.log(data);
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>Acknowledgement not Approved!! " +
                    "</div>");
                $(".alert").show();
                $(".alert").delay(4000).slideUp(200, function () {
                    $(this).alert('close');
                });
            }
        });
    });
    $("#btnView").click(function () {
        window.location.href = baseURL + "/customer/viewAcknowledgeform/" + AcknowledgementId;
    });
    $("#btnconfirmdelete").click(function () {
        $('#myModal').modal('hide');
        deleteAcknowledgementbyid();
    });
});


function AcknowledgementGridLoad() {
    $('#AcknowledgementGrid').kendoGrid({
        dataSource: GetAcknowledgementDetail(),
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
                field: "CustomerName",
                title: "Customer",
            },
            {
                field: "ProjectName",
                title: "Project",
            },

            {
                field: "Status",
                title: "Status",
            },
            {
                field: "Id",
                title: " ",
                template: "<a class='btn btn-primary' href='" + baseURL + "/customer/viewacknowledgeform/#=Id#'>View</a>",
            }
        ],
        selectable: "multiple",
        resizable: true,
    }).data("kendoGrid");
    resizeGrid();
}

function GetAcknowledgementDetail() {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        pageSize: 15,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Customer/GetAcknowledgementList",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                cache: false,
            },
        },
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    CustomerName: { type: "string" },
                    ProjectName: { type: "string" },
                    Status: { type: "string" },
                }
            }
        },
        //aggregate: [
        //{ field: "Amount", aggregate: "sum" }
        //],
    });
    return source;
}

function GetAcknowledgementListDataSource() {
    var source;
    $.ajax({
        type: "GET",
        url: baseURL + "/Customer/GetAcknowledgementList",
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
        //$("#editAcknowledgement").show();
        //$("#deleteAcknowledgement").show();
        //$("#btnView").show();
        if (currentDataItem.Status != "Approved") {
            $("#btnapprove").show();
        }
        else {
            $("#btnapprove").hide();
        }
        AcknowledgementId = currentDataItem.Id;
        //uncheckallchkbox(AcknowledgementId);
        getallSelectedRowId(currentDataItem.Status);
    }
}
function getallSelectedRowId(status) {
    idstoDelete = [];
    $("#AcknowledgementGrid").find("tr.k-state-selected").each(function (i, data) {
        idstoDelete.push(data.firstChild.textContent);
    });
    if (idstoDelete.length > 1) {
        //$("#btnapprove").hide();
        //$("#viewactivity").hide();
        //$("#addactivity").hide();
        $("#deleteAcknowledgement").show();
    }
    else if (idstoDelete.length == 1) {
        if (idstoDelete == "") {
            //$("#btnapprove").hide();
            //$("#viewactivity").hide();
            //$("#addactivity").hide();
            $("#deleteAcknowledgement").hide();
        }
        else {
            activityId = idstoDelete[0];
            //$("#addactivity").show();
            if (status != "Approved") {
                $("#btnapprove").show();
            }
            else {
                $("#btnapprove").hide();
            }
            //$("#viewactivity").show();
            $("#deleteAcknowledgement").show();
        }
    }
    else {
        //$("#addactivity").show();
        //$("#btnapprove").hide();
        //$("#viewactivity").show();
        $("#deleteAcknowledgement").hide();
    }
}
function deleteAcknowledgementbyid() {
    $.ajax({
        type: "GET",
        url: baseURL + "/customer/DeleteAcknowledgementById",
        data: { Ids: idstoDelete.join(',') },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            if (data == false) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>Acknowledgement delete failed!! " +
                    "</div>");
            }
            else {
                $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Success!</strong>Acknowledgement deleted Successfully!! " +
                    "</div>");
            }
            AcknowledgementGridLoad();
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

function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#AcknowledgementGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}