var source;
var chart;
var chartType;
var startDate = new Date('01/01/2012');
var FromEndDate = new Date();
var ToEndDate = new Date();
ToEndDate.setDate(ToEndDate.getDate() + 365);
$(document).ready(function () {

    $("#reportsitem").toggle();
    $(".menuPRE").addClass("menuactive");

    var datefrom = $("#FromDate").val();
    $("#FromDate").keypress(function () {
        return false;
    });
    $("#EndDate").keypress(function () {
        return false;
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

    }).data('datepicker');
    var checkout = $('#EndDate').datepicker({
        "setDate": new Date(datefrom),
        onRender: function (date) {
            return date.valueOf() < checkin.date.valueOf() ? 'disabled' : '';
        },
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');
    $(".btncharttype").click(function () {
        if (!$(this).hasClass("activeChart")) {
            $(".activeChart").each(function () { $(this).removeClass("activeChart") });
            $(this).addClass("activeChart");
            if ($(this).attr("id").indexOf("project") > 0) {
                SetFilterDataInCookie("ProjectWise");
            }
            else if ($(this).attr("id").indexOf("service") > 0) {
                SetFilterDataInCookie("ServiceWise");
            }
            else if ($(this).attr("id").indexOf("plan") > 0) {
                SetFilterDataInCookie("PlanWise");
            }
            GetAccuralListDataSource();
        }
    });
    var chartDetail;

    chartDetail = $.cookie('RevenueExpenseChart');
    if (chartDetail != undefined) {
        chartDetail = chartDetail.split(",");
        $("#SearchBy").val(chartDetail[0]);
        if (chartDetail[0] == "MonthYear") {
            $(".fromdate").hide();
            $(".enddate").hide();
            $(".month").show();
            $(".year").show();
            $("#Month").val(chartDetail[1]);
            $("#Year").val(chartDetail[2]);
        }
        else if (chartDetail[0] == "FromToDate") {
            $(".month").hide();
            $(".year").hide();
            $(".fromdate").show();
            $(".enddate").show();
            $("#FromDate").val(chartDetail[1]);
            $("#EndDate").val(chartDetail[2]);
        }
        chartType = chartDetail[3];
        if (chartType == "ProjectWise") {
            $("#btnproject").addClass("activeChart");
        }
        else if (chartType == "ServiceWise") {
            $("#btnservice").addClass("activeChart");
        }
        else if (chartType == "PlanWise")
        {
            $("#btnplan").addClass("activeChart");
        }
    }
    if (chartType == undefined) {
        chartType = "ProjectWise";
    }
    GetAccuralListDataSource();
    $("#SearchBy").change(function () {
        if ($(this).val() == "MonthYear") {
            $(".fromdate").hide();
            $(".enddate").hide();
            $(".month").show();
            $(".year").show();
        }
        else if ($(this).val() == "FromToDate") {
            $(".month").hide();
            $(".year").hide();
            $(".fromdate").show();
            $(".enddate").show();
        }
    });
    $("#btnSearch").click(function () {
        GetAccuralListDataSource();
    });

});
function GetAccuralListDataSource() {
    var revenue = ['revenue'];
    var expense = ['expense'];
    var xlabels = ['x'];
    selectedMonth = $("#Month").val();
    selectedYear = $("#Year").val();
    var url;

    if ($("#SearchBy").val() == "MonthYear") {
        if (chartType == "ProjectWise") {
            url = baseURL + "/Reports/GetProjectRevenueExpensebyMonthYear";
        }
        else if (chartType == "ServiceWise") {
            url = baseURL + "/Reports/GetServiceRevenueExpensebyMonthYear"
        }
        else if (chartType == "PlanWise") {
            url = baseURL + "/Reports/GetCustomerRevenueExpensebyMonthYear";
        }
        $.ajax({
            type: "GET",
            url: url,
            data: { month: selectedMonth, year: selectedYear },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
                source = $.parseJSON(data);
            }
        });
        var resultstr = $("#Month option:selected").text() + " - " + selectedYear + " - " + chartType;
        $(".lblSearchContent").html(resultstr);
    }
    else if ($("#SearchBy").val() == "FromToDate") {
        if (chartType == "ProjectWise") {
            url = baseURL + "/Reports/GetProjectRevenueExpensebyDate";
        }
        else if (chartType == "ServiceWise") {
            url = baseURL + "/Reports/GetServiceRevenueExpensebyDate"
        }
        else if (chartType == "PlanWise") {
            url = baseURL + "/Reports/GetCustomerRevenueExpensebyDate";
        }
        var fromDate = $("#FromDate").val();
        var endDate = $("#EndDate").val();
        $.ajax({
            type: "GET",
            url: url,
            //url: baseURL + "/Reports/GetPlanRevenueExpensebyDate",
            data: { fromdate: fromDate, todate: endDate },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
                source = $.parseJSON(data);
            }
        });
        var resultstr = $("#FromDate").val() + " to " + $("#EndDate").val();
        $(".lblSearchContent").html(resultstr);
    }
    if (source != null) {
        source.filter(function (el) { revenue.push(parseFloat(el.Revenue)); expense.push(parseFloat(el.Expense)); xlabels.push(el.ProjectCode) });
    }
    var xlableText;
    if (chartType == "ProjectWise") {
        xlableText = "Project";
    }
    else if (chartType == "ServiceWise") {
        xlableText = "Service Type"
    }
    else if (chartType == "PlanWise")
    {
        xlableText = "Plan";
    }
    chart = c3.generate({
        bindto: '#chart',
        data: {
            x: 'x',
            columns: [
                xlabels,
            revenue,
            expense
            ],
            type: 'bar'
        },
        axis: {
            x: {
                padding: {
                    left: 0
                },
                type: 'categorized',
                label: {
                    text: xlableText,
                    position: 'outer-center'
                    // inner-right : default
                    // inner-center
                    // inner-left
                    // outer-right
                    // outer-center
                    // outer-left
                }
            },
            y: {
                tick: {
                    format: d3.format("$,")
                },
                label: {
                    text: 'Revenue-Expense ($)',
                    position: 'outer-middle'
                    // inner-top : default
                    // inner-middle
                    // inner-bottom
                    // outer-top
                    // outer-middle
                    // outer-bottom
                }
            }
            //y2: {
            //    show: true,
            //    label: {
            //        text: 'Revenue-Expense',
            //        position: 'outer-middle'
            //        // inner-top : default
            //        // inner-middle
            //        // inner-bottom
            //        // outer-top
            //        // outer-middle
            //        // outer-bottom
            //    }
            //}
        }
    });
    if (chartType == "ProjectWise") {
        $("#btnproject").addClass("activeChart");
    }
    else if (chartType == "ServiceWise") {
        $("#btnservice").addClass("activeChart");
    }
    else if (chartType == "PlanWise") {
        $("#btnplan").addClass("activeChart");
    }
}

function SetFilterDataInCookie(charttype)
{
    var strValue = [];
    if ($("#SearchBy").val() == "MonthYear") {
       var selectedMonth = $("#Month").val();
       var  selectedYear = $("#Year").val();
        strValue.push($("#SearchBy").val());
        strValue.push(selectedMonth);
        strValue.push(selectedYear);
        strValue.push(charttype);
        chartType = charttype;
        $.cookie('RevenueExpenseChart', strValue.join(','), { path: "/" });
    }
    else if ($("#SearchBy").val() == "FromToDate") {
        var fromDate = $("#FromDate").val();
        var endDate = $("#EndDate").val();
        strValue.push($("#SearchBy").val());
        strValue.push(fromDate);
        strValue.push(endDate);
        strValue.push(charttype);
        chartType = charttype;
        $.cookie('RevenueExpenseChart', strValue.join(','), { path: "/" });
    }

}