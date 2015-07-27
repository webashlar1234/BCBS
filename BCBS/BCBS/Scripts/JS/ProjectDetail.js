var alternateCode = "";
var contractId;
$(document).ready(function () {
    $("#settingitems").toggle();
    $(".menuProject").addClass("menuactive");
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed");
    $('form').validate({
        rules: {

            HighLevelBudget: {
                min: 0,
                max: 99999999.99,
                number: true,
                dollarsscents: true,
            },
            ChargeCode: {
                remote: {
                    url: baseURL + "/Project/IsChargeCodeExist",
                    type: "GET",
                    data: {
                        chargeCode: function () {
                            return $("#ChargeCode").val();
                        }
                    },
                    dataFilter: function (data) {
                        if ($("#ChargeCode").val() == alternateCode) {
                            console.log(data);
                            return true;
                        }
                        else {
                            return data;
                        }
                    }
                }
            }
        },
        messages: {
            ChargeCode: {
                remote: jQuery.validator.format("{0} charge Code is already taken.")
            }
        },
        highlight: function (element) {
            //$(element).closest('.form-group').removeClass('success').addClass('error');
            $(element).addClass("error");
            $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
            $(element).parent().addClass("has-error has-feedback");
        },
        success: function (element) {
            $(element).parent().children(".glyphicon-exclamation-sign").remove();
            $(element).parent().removeClass("has-error");
            $(element).parent().removeClass("has-feedback");
            $(element).removeClass("error");
            $(element).remove();
            //element.addClass('valid').closest('.form-group').removeClass('error').addClass('success');
        }
    });
    $("#btnProjectCancel").click(function () {
        window.location.href = baseURL + "/project/index";
    });
    $("#editcontract").click(function () {
        if (contractId > 0) {
            window.location.href = baseURL + "/contract/edit/" + contractId;
        }
    });
    $("#addcontract").click(function () {
        window.location.href = baseURL + "/contract/new";
        $.cookie('contractforproject', $("#Id").val(), { path: "/" });
    });
    //$("#ChargeCode").keyup(function () {
    //    checkChargeCodeExist($(this).val());
    //});
    //$("#ChargeCode").focusout(function () {
    //    checkChargeCodeExist($(this).val());
    //});
    if ($("#Id").val() > 0) {
        alternateCode = $("#ChargeCode").val();
        ProjectContractGridLoad($("#Id").val());
    }
});

function checkChargeCodeExist(value) {
    $.ajax({
        url: baseURL + "/Project/IsChargeCodeExist",
        data: { chargeCode: value },
        type: "GET",
        success: function (data) {
            console.log(data);
            if (data) {
                $("#ChargeCode").append("<span class='danger'>" + value + " Code already exist</span>");
                //alert("alreadey exist");
            }
        }
    })
}
function ProjectContractGridLoad(projectid) {
    $('#ProjectContractGrid').kendoGrid({
        dataSource: GetContractDetail(projectid),
        height: '350',
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
            //{
            //    field: "",
            //    title: "",
            //    template: "<input class='chkContract' id='#:Id#' type='checkbox'>",
            //    width: 30,
            //},
            {
                field: "Customer",
                title: "Customer",
            },
            {
                field: "Service",
                title: "Service",
            },
            //{
            //    field: "Project",
            //    title: "Project",
            //},
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
    //resizeGrid();
}

function GetContractDetail(projectid) {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        pageSize: 5,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Project/GetContractListbyProjectId",
                data: { id: projectid },
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

function getallSelectedRowId() {
    idstoDelete = [];
    $("#ProjectContractGrid").find("tr.k-state-selected").each(function (i, data) {
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
