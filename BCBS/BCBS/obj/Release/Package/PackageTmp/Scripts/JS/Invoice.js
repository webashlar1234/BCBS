$(document).ready(function () {
    $("#formsitems").toggle();
    $(".menuSBF").addClass("menuactive");
    $(document).on('keypress', "#FromDate", function () {
        return false;
    });
    $(document).on('keypress', "#InvoiceDate", function () {
        return false;
    });
    $(document).on('keypress', "#ToDate", function () {
        return false;
    });
    $("#CustomerId").change(function () {
        if ($(this).val() != "") {
            $.get(baseURL + "/Customer/GenerateInvoice", { Id: $(this).val() }, function (data) {
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
                $('form').validate({
                    rules: {
                        InvoiceNumber: {
                            required: true,
                            //regex: "/^\d+(?:\.\d\d?)?$/",
                        },
                        InvoiceDate: {
                            required: true,
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
                field: "ActivityId",
                title: "ActivityId",
                hidden: true,
            },
             {
                 field: "ProjectCode",
                 title: "Project Code",
             },
            {
                field: "ProjectName",
                title: "Project",
            },
            {
                field: "ServiceName",
                title: "Service",
            },
            {
                field: "RC",
                title: "RC",
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
                footerTemplate: "<span class='pull-right'>Total</span>",
            },
            {
                field: "Amount",
                title: "Amount($)",
                template: "#if(Amount < 0){ #<span>$#=Math.abs(Amount)#<span># } else { #<span>$#=Amount#<span> # }#",
                //template: "$#=Amount#",
                footerTemplate: "$#= kendo.toString(sum, '0.00') #",
            },
        ],
        selectable: "multiple",
        resizable: true,
        dataBound: function () {
            var totalAmt = $(".k-footer-template td").last();
            totalAmt = totalAmt.html().split(':')[1];
            totalAmt = $.trim(totalAmt);
            $("#TotalAmount").val(totalAmt);
            GetActivityIds();
        }
    }).data("kendoGrid");

    //resizeGrid();
}
function GetActivitiesDetail() {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        //pageSize: 10,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Customer/GetActivitiesByCustomerId",
                data: { id: $("#CustomerId").val() },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                cache: false,
            },
        },
        aggregate: [
            { field: "Amount", aggregate: "sum" },
        ],
        schema: {
            model: {
                fields: {
                    ProjectName: { type: "string" },
                    ServiceName: { type: "string" },
                    RC: { type: "string" },
                    ProjectCode: { type: "string" },
                    Amount: { type: "number" },
                    Charges: { type: 'string' },
                    FromDate: { type: 'date' },
                    EndDate: { type: 'date' }
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
        var griddata = grid.data();
        if (griddata != null && griddata.length > 0) {
            var activities = [];
            $.each(griddata, function (index, data) {
                activities.push(data.ActivityId);
            });
            $("#Activities").val(activities.join(","));
        }
    }
}