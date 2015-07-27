$(document).ready(function () {
    $("#formsitems").toggle();
    $(".menuGBF").addClass("menuactive");
    $(document).on('keypress', "#FromDate", function () {
        return false;
    });
    $(document).on('keypress', "#InvoiceDate", function () {
        return false;
    });
    $(document).on('keypress', "#ToDate", function () {
        return false;
    });
    $(document).on('keypress', "#datefrom", function () {
        return false;
    });
    $(document).on('keypress', "#dateto", function () {
        return false;
    });
    $("#CustomerId").change(function () {
        if ($(this).val() > 0) {
            bindContractDDLbyCustomerId($(this).val());
            $("#InvoiceDiv").html("");
        }
        else {
            $(".contract").hide();
            //var ddlStr = "<option value=''>--Select Contract--</option>";
            $("#ContractId").html("");
            $("#InvoiceDiv").html("");
        }
    });
    $("#ContractId").change(function () {
        if ($(this).val() != "" && $(this).val() != null && $("#CustomerId").val() > 0) {
            $.get(baseURL + "/Customer/GenerateInvoice", { Id: $("#CustomerId").val() }, function (data) {
                $("#InvoiceDiv").html(data);
                ContractGridLoad();
                var startDate = new Date('01/01/2012');
                var FromEndDate = new Date();
                var ToEndDate = new Date();
                ToEndDate.setDate(ToEndDate.getDate() + 365);
                var nowTemp = new Date();
                var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
                var datefrom = $("#FromDate").val();

                $("#InvoiceDate").datepicker({
                    "setDate": new Date(datefrom),
                    "autoclose": true
                }).on('changeDate', function (ev) {
                    $(this).datepicker('hide');
                });
                var checkin = $('#FromDate').datepicker({
                    "setDate": new Date(datefrom),
                    onRender: function (date) {
                        //return date.valueOf() < now.valueOf() ? 'disabled' : '';
                    }
                }).on('changeDate', function (ev) {
                    if (ev.date.valueOf() > checkout.date.valueOf()) {
                        var newDate = new Date(ev.date)
                        newDate.setDate(newDate.getDate() + 1);
                        checkout.setValue(newDate);
                    }
                    else {
                        var newDate = new Date(ev.date)
                        newDate.setDate(newDate.getDate() + 1);
                        checkout.setValue(checkout.date);
                    }
                    checkin.hide();
                    //$('#ToDate')[0].focus();
                }).data('datepicker');
                var checkout = $('#ToDate').datepicker({
                    "setDate": new Date(datefrom),
                    onRender: function (date) {
                        return date.valueOf() < checkin.date.valueOf() ? 'disabled' : '';
                    }
                }).on('changeDate', function (ev) {
                    checkout.hide();
                }).data('datepicker');
                var datefrom = $('#datefrom').datepicker({
                    //"setDate": new Date(datefrom),
                    format: 'mm/dd/yyyy',
                    onRender: function (date) {
                    }
                }).on('changeDate', function (ev) {
                    if (ev.date.valueOf() > dateto.date.valueOf()) {
                        var newDate = new Date(ev.date)
                        newDate.setDate(newDate.getDate());
                        dateto.setValue(newDate);
                    }
                    else {
                        var newDate = new Date(ev.date)
                        newDate.setDate(newDate.getDate());
                        //checkout.setValue(checkout.date);
                    }
                    datefrom.hide();
                    $("#dateto").datepicker('update');
                    //$('#EndDate')[0].focus();
                }).data('datepicker');

                var dateto = $('#dateto').datepicker({
                    format: 'mm/dd/yyyy',
                    //"setDate": new Date(datefrom),
                    onRender: function (date) {
                        if (date.valueOf() < datefrom.date.valueOf()) {
                            return 'disabled';
                        }
                    },
                }).on('changeDate', function (ev) {
                    if (ev.date.valueOf() < datefrom.date.valueOf()) {
                        var newDate = new Date(ev.date)
                        newDate.setDate(newDate.getDate());
                        //checkout.setValue(newDate);
                    }
                    else {
                        var newDate = new Date(ev.date)
                        newDate.setDate(newDate.getDate());
                        //checkout.setValue(checkout.date);
                    }
                    dateto.hide();
                }).data('datepicker');
                jQuery.validator.addMethod("uploadFile", function (val, element) {
                    if (element.files.length > 0) {
                        var size = element.files[0].size;
                        if (size > 3145728)// checks the file more than 3 MB
                        {
                            return false;
                        } else {
                            return true;
                        }
                    }
                    else {
                        return true;
                    }
                }, "File size should not be more than 3MB.");
                $('form').validate({
                    rules: {
                        InvoiceNumber: {
                            required: true,
                            //regex: "/^\d+(?:\.\d\d?)?$/",
                        },
                        InvoiceDate: {
                            required: true,
                        },
                        file: {
                            uploadFile: true,
                        }
                    },
                    highlight: function (element) {
                        //$(element).closest('.form-group').removeClass('success').addClass('error');
                        $(element).addClass("error");
                        if ($(element).attr("id") != "datefrom" && $(element).attr("id") != "dateto" && $(element).attr("id") != "status") {
                            $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
                        }
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
            });
        }
        else {
            $("#InvoiceDiv").html("");
        }
    });
    $(document).on("change", "#IsDeffered", function () {
        if ($(this).val() == "true") {
            $(".DefferedAccount").show();
        }
        else {
            $("#DefferedAccount").val("");
            $(".DefferedAccount").hide();
        }
    });
    $(document).on("click", "#btnCustomerCancel", function () {
        $("#InvoiceDiv").html("");
        $("#CustomerId").val("");
    });
    $(document).on("click", ".removeactivity", function () {
        var id = $(this).attr("id");
        var grid = $("#ContractGrid").data("kendoGrid").dataSource;
        if (grid != undefined || grid != null) {
            var griddata = grid.data();
            if (griddata != null && griddata.length > 0) {
                if (griddata.length == 1) {
                    $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                   "<strong>Warning! </strong>Atleast 1 activity required..!! " +
                   "</div>");
                    $(".alert").show();
                    $(".alert").delay(4000).slideUp(200, function () {
                        $(this).alert('close');
                        $("#message").html("");
                    });
                    return false;
                }
                var indextoremove;
                $.each(griddata, function (index, data) {
                    if (data.Id == id) {
                        indextoremove = index;
                        //$("#ContractGrid").data("kendoGrid").dataSource.data(newdatasource);
                        return false;
                    }
                });
                try {
                    grid.data().splice(indextoremove, 1);
                }
                catch (e) {
                    $("#ContractGrid").data("kendoGrid").dataSource.data(grid.data());
                    GetActivityIds();
                    var totalAmt = $(".k-footer-template td").filter(function (el, data) { return data.textContent.indexOf("$") > -1 });
                    totalAmt = totalAmt.html().split('$')[1];
                    totalAmt = $.trim(totalAmt);
                    $("#TotalAmount").val(totalAmt);
                }
                //$("#Activities").val(activities.join(","));
            }
        }
    });
    $(document).on("click", "#btnfilter", function () {
        $("#datefrom").removeAttr("required");
        $("#dateto").removeAttr("required");
        if ($("#datefrom").val() != "" && $("#dateto").val() == "") {
            $("#dateto").attr("required", "required");
        }
        else if ($("#dateto").val() != "" && $("#datefrom").val() == "") {
            $("#datefrom").attr("required", "required");
        }

        if ($("#datefrom").valid() && $("#dateto").valid()) {
            $("#datefrom").removeAttr("required");
            $("#dateto").removeAttr("required");
            //var fromdate = $("#datefrom").data("kendoDatePicker").value();
            //var todate = $("#dateto").data("kendoDatePicker").value();
            var fromdate = new Date($("#datefrom").val());
            var todate = new Date($("#dateto").val());
            var status = $("#status").val();
            if (status != "") {
                if ($("#datefrom").val() != "" && $("#datefrom").val() != "") {
                    $("#ContractGrid").data("kendoGrid").dataSource.filter({
                        logic: "and",
                        filters: [
                          { field: "Status", operator: "contains", value: status },
                          {
                              logic: "and",
                              filters: [
                                  { field: "FromDate", operator: "gte", value: fromdate },
                                  { field: "EndDate", operator: "lte", value: todate }
                              ]
                          }
                        ]
                    });
                }
                else {
                    $("#ContractGrid").data("kendoGrid").dataSource.filter({
                        logic: "and",
                        filters: [
                          { field: "Status", operator: "contains", value: status },
                        ]
                    });
                }

            } else {

                if ($("#datefrom").val() != "" && $("#dateto").val() != "") {
                    $("#ContractGrid").data("kendoGrid").dataSource.filter({
                        logic: "and",
                        filters: [
                            { field: "FromDate", operator: "gte", value: fromdate },
                            { field: "EndDate", operator: "lte", value: todate }
                        ]
                    });
                } else {
                    var datasource = $("#ContractGrid").data("kendoGrid").dataSource;
                    //Clear filters:
                    datasource.filter([]);
                }
            }

        }
        else {
            var status = $("#status").val();
            if (status != "") {
                $("#ContractGrid").data("kendoGrid").dataSource.filter({
                    logic: "or",
                    filters: [
                        { field: "Status", operator: "contains", value: status },
                    ]
                });
            }
            else {
                var datasource = $("#ContractGrid").data("kendoGrid").dataSource;
                //Clear filters:
                datasource.filter([]);
            }
        }
        GetActivityIds();
    });
    $(document).on("click", "#btnclearfilter", function () {
        var datasource = $("#ContractGrid").data("kendoGrid").dataSource;
        //Clear filters:
        datasource.filter([]);
        $("#datefrom").val("");
        $("#dateto").val("");
        $("#status").val("");
    });
});
function ContractGridLoad() {
    $('#ContractGrid').kendoGrid({
        dataSource: GetActivitiesDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: false,
        resizable: true,
        //change: onRowSelectChange,
        columns: [
            {
                field: "Id",
                title: "ID",
                hidden: true,
            },
            //{
            //    field: "",
            //    title: "",
            //    template: "<input class='chkActivity' id='#:Id#' type='checkbox'>",
            //    width: 30,
            //},
            {
                field: "ContractId",
                title: "Contract Id",
                hidden: true,
            },
             {
                 field: "ContractCode",
                 title: "Contract Code",
                 hidden: true,
             },
             {
                 field: "ProjectName",
                 title: "Project Name",
                 template: '#:changeTemplate(FeesType,ProjectName,Service,Rate,Volume)#',
             },
             //{
             //    field: "Service",
             //    title: "Service",
             //},
             {
                 field: "GLAccount",
                 title: "G/L Account",
                 width: "100px",
             },
            {
                field: "FromDate",
                title: "From Date",
                template: '#= kendo.toString(FromDate, "MM/dd/yyyy") #',
                width: "100px",
            },
            {
                field: "EndDate",
                title: "End Date",
                template: '#= kendo.toString(EndDate, "MM/dd/yyyy") #',
                width: "100px",
            },
             {
                 field: "Charges",
                 title: "Charges",
                 template: "#if(Charges == 'true'){ #<span>Revenue<span># } else { #<span>Expense<span> # }#",
                 width: "80px",
             },
            {
                field: "Estimate",
                title: "Value",
                template: "# if(Estimate == 'true'){ #<span>Actual<span># } else { #<span>Estimate<span> # } #",
                footerTemplate: "<span class='pull-right'>Total</span>",
                width: "80px",
            },
            {
                field: "Amount",
                title: "Amount($)",
                template: "#if(Amount < 0){ #<span>$#=kendo.toString(Math.abs(Amount),'n')#<span># } else { #<span>$#=kendo.toString(Amount,'n')#<span> # }#",
                //template: "$#=Amount#",
                footerTemplate: "$#= kendo.toString(sum, 'n') #",
                width: "100px",
            },
            {
                field: "Id",
                title: " ",
                template: "<a class='btn btn-primary' href='" + baseURL + "/contract/editactivity/#=Id#'>View Activity</a>",
            },
            {
                field: "Id",
                title: " ",
                width: "20px",
                template: "<span id='#=Id#' class='glyphicon glyphicon-remove removeactivity'></span>",
            }
        ],
        selectable: "multiple",
        resizable: true,
        dataBound: function () {
            var totalAmt = $(".k-footer-template td").filter(function (el, data) { return data.textContent.indexOf("$") > -1 });
            totalAmt = totalAmt.html().split('$')[1];
            totalAmt = $.trim(totalAmt);
            $("#TotalAmount").val(totalAmt);
            GetActivityIds();
        }
    }).data("kendoGrid");

    //resizeGrid();
}
function changeTemplate(FeesType, ProjectName, Service, Rate, Volume) {
    if (FeesType == "Transaction")
        return ProjectName + " (" + Volume + " " + Service + " @ $" + kendo.toString(Rate, "n") + ")";
    else if (FeesType == "Hourly")
        return ProjectName + "-" + Service + " (" + Volume + " Hours @ $" + kendo.toString(Rate, "n") + ")";
    else
        return ProjectName + "-" + Service;
}

function GetActivitiesDetail() {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        //pageSize: 10,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Customer/GetPlanActivitiesByContactId",
                data: { id: $("#ContractId").val().join(",") },
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
                    ProjectName: { type: 'string' },
                    Service: { type: 'string' },
                    GLAccount: { type: 'string' },
                }
            }
        },
        //aggregate: [
        //{ field: "Amount", aggregate: "sum" }
        //],
    });
    return source;
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
function GetActivityIds() {
    var grid = $("#ContractGrid").data("kendoGrid").dataSource;
    if (grid != undefined || grid != null) {
        var griddata = grid.view();//grid.data();
        if (griddata != null && griddata.length > 0) {
            var activities = [];
            $.each(griddata, function (index, data) {
                if (data.items != null) {
                    $.each(data.items, function (i, itemdata) {
                        activities.push(itemdata.Id);
                    });
                }
            });
            $("#Activities").val(activities.join(","));
            $("input[type = 'submit']").removeAttr("disabled");
        }
        else {
            $("#Activities").val("");
            $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                   "<strong>Warning! </strong>No activity found..!! " +
                   "</div>");
            $(".alert").show();
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
                $("#message").html("");
            });
            $("input[type = 'submit']").attr("disabled", "disabled");
        }
    }
}
function bindContractDDLbyCustomerId(customerid) {
    $.ajax({
        url: baseURL + "/Customer/GetContractByCustomerIdHaveProjectedActivity",
        data: { Id: customerid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (resultdata) {
            var ddlStr = "";
            if (resultdata != null) {
                $.each(resultdata, function (index, data) {
                    ddlStr += "<option value='" + data.ContractId + "'>" + data.ContractCode + "</option>";
                });
            }
            $("#ContractId").html("");
            $("#ContractId").html(ddlStr);
            $('#ContractId').multiselect('destroy');
            $('#ContractId').multiselect({
                //includeSelectAllOption: true,
                numberDisplayed: 2,
                nonSelectedText: '--Select Contract--',
            });
            $(".multiselect").addClass("noRadius");
            $(".multiselect").parent().addClass("noRadius");
            $(".contract").show();

        },
        failure: function (data) {
            //var ddlStr = "<option value=''>--Select Contract--</option>";
            $("#ContractId").html("");
        }
    });
}
