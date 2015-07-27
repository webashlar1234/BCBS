var serviceId;
var idstoDelete = [];
var selectedMonth = new Date().getMonth();
var selectedYear = new Date().getFullYear();

var startDate = new Date('01/01/2012');
var FromEndDate = new Date();
var ToEndDate = new Date();
ToEndDate.setDate(ToEndDate.getDate() + 365);
var viewModel;
(function () {
    //var log = (function (el) {
    //    return function (text) {
    //        el.html(el.html() + '<br/>' + text);
    //    };
    //})($('#log'));

    viewModel = kendo.observable({

        saveGridCsv: function () {
            viewModel.exportCsv('AccuralGrid', 'testdata.csv');
        },
        exportCsv: function (gridId, fileName) {
            var grid = $("#" + gridId).data("kendoGrid");
            var originalPageSize = grid.dataSource.pageSize();
            var csv = '';
            fileName = fileName || 'BCBSReport.csv';

            // Increase page size to cover all the data and get a reference to that data
            grid.dataSource.pageSize(grid.dataSource.view().length);
            var data = grid.dataSource.view();
            //add the header row
            for (var i = 0; i < grid.columns.length; i++) {
                var field = grid.columns[i].field;
                var title = grid.columns[i].title || field;

                //NO DATA
                if (!field) continue;

                title = title.replace(/"/g, '""');
                csv += '"' + title + '"';
                if (i < grid.columns.length - 1) {
                    csv += ',';
                }
            }
            csv += '\n';

            //add each row of data
            for (var row in data) {
                for (var i = 0; i < grid.columns.length; i++) {
                    var fieldName = grid.columns[i].field;
                    var template = grid.columns[i].template;
                    var exportFormat = grid.columns[i].exportFormat;

                    //VALIDATE COLUMN
                    if (!fieldName) continue;
                    var value = '';
                    if (fieldName.indexOf('.') >= 0) {
                        var properties = fieldName.split('.');
                        var value = data[row] || '';
                        for (var j = 0; j < properties.length; j++) {
                            var prop = properties[j];
                            value = value[prop] || '';
                        }
                    }
                    else {

                        value = data[row][fieldName] || '';
                    }
                    if (fieldName == "Revenue" || fieldName == "Expense") {
                        if (value == "") {
                            value = "0";
                        }
                    }
                    if (value && template && exportFormat !== false) {
                        if (template.indexOf("Expense") > 0 || template.indexOf("Revenue")) {
                            if (value == "") {
                                value = "0";
                            }
                        }
                        value = '$' + value.toString().replace(/"/g, '""');
                        //value = _.isFunction(template)
                        //    ? template(data[row])
                        //    : kendo.template(template)(data[row]);
                    }
                    value = value.toString().replace(/"/g, '""');
                    csv += '"' + value + '"';
                    if (i < grid.columns.length - 1) {
                        csv += ',';
                    }
                }
                csv += '\n';
            }

            // Reset datasource
            grid.dataSource.pageSize(originalPageSize);

            //EXPORT TO BROWSER
            var blob = new Blob([csv], { type: 'text/csv;charset=utf-8' }); //Blob.js
            saveAs(blob, fileName); //FileSaver.js
        }
    })
    // Kendo MVVM binding    
    kendo.bind('body', viewModel);
})()
$(document).ready(function () {
    $('form').validate({
        rules: {
            FromDate: {
                required: {
                    depends: function (element) {
                        return $("#SearchBy").val() == "FromToDate";
                    }
                },
            },
            EndDate: {
                required: {
                    depends: function (element) {
                        return $("#SearchBy").val() == "FromToDate";
                    }
                },
            }
        },
        highlight: function (element) {
            //$(element).closest('.form-group').removeClass('success').addClass('error');
            $(element).addClass("error");
            //$(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback' data-toggle='tooltip' data-placement='bottom' title='Tooltip on bottom'></span>");
            //$(element).parent().addClass("has-error has-feedback");
        },
        //errorPlacement: function (error, element) {
        //    $(element).parent().attr("title", $(error).html());
        //},
        success: function (element) {
            //$(element).parent().children(".glyphicon-exclamation-sign").remove();
            //$(element).parent().removeClass("has-error");
            //$(element).parent().removeClass("has-feedback");
            $(element).removeClass("error");
            $(element).remove();
            //element.addClass('valid').closest('.form-group').removeClass('error').addClass('success');
        }
    });
    $("#reportsitem").toggle();
    $(".menuAccuralWorksheet").addClass("menuactive");
    //artistGrid();\

    // $('#exampleModal').modal({
    //     title: "Confirmation",
    //     autoOpen:false
    //});

    $("#FromDate").keypress(function () {
        return false;
    });
    $("#EndDate").keypress(function () {
        return false;
    });

    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    var datefrom = $("#FromDate").val();
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
        $('form').valid();
        checkin.hide();
        //$('#EndDate')[0].focus();
    }).data('datepicker');
    var checkout = $('#EndDate').datepicker({
        "setDate": new Date(datefrom),
        onRender: function (date) {
            return date.valueOf() < checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        $('form').valid();
        checkout.hide();
    }).data('datepicker');
    $(".alert").delay(5000).slideUp(200, function () {
        $(this).alert('close');
    });
    $(".dropdown").click(function () {
        $(this).find(".menu-expand").toggle();
    });
    AccuralGridLoad();
    var resultstr = $("#Month option:selected").text() + " - " + selectedYear;
    $(".lblSearchContent").html(resultstr);
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-title').text('New message to ' + recipient)
        modal.find('.modal-body input').val(recipient)
    });

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
        if ($('form').valid()) {
            AccuralGridLoad();
        }
    });
    $("#btnconfirmdelete").click(function () {
        $('#myModal').modal('hide');
        deleteAccuralbyid();
    });
    $(document).on("click", ".chkAccural", function () {
        var chkbox = $(this);
        if ($(this).is(":checked")) {
            $(this).parents("tr").addClass("k-state-selected");
            //uncheckallchkbox(chkbox.attr("id"));
            //$(".chkAccural").each(function () {
            //    if ($(this).attr("id") != chkbox.attr("id")) {
            //        $(this).attr("checked", false);
            //    }
            //});
        }
        else {
            $(this).parents(".k-state-selected").removeClass("k-state-selected");
            serviceId = undefined;
            //$("#editservice").hide();
            //$("#deleteservice").hide();
        }
        //if (chkbox.is(":checked")) {
        //    chkbox.prop('checked', true);
        //    chkbox.attr('checked',true);
        //}
        //getallSelectedRowId();
    });
    $("#btnprint").click(function () {
        printGrid();
    });
    $("#btncsv").click(function () {
        viewModel.exportCsv("AccuralGrid", "AccuralReport.csv");
    });
    $("#ChartType").change(function () {
        if ($('form').valid()) {
            if ($(this).val() != null) {
                if ($(this).val() == "ProjectWise") {
                    SetFilterDataInCookie("ProjectWise");
                }
                else if ($(this).val() == "ServiceWise") {
                    SetFilterDataInCookie("ServiceWise");
                }
                else if ($(this).val() == "PlanWise") {
                    SetFilterDataInCookie("PlanWise");
                }
                window.location.href = baseURL + "/reports/revenueExpense"
            }
        }
        else {
            $(this).val("");
        }
    });
    $(window).resize(function () {
        resizeGrid();
        if ($("#wrapper").hasClass("toggled")) {

            $("#wrapper").removeClass("toggled");
        }
    });
});
function uncheckallchkbox(selectedid) {
    $(".chkAccural").each(function () {
        if ($(this).attr("id") != selectedid) {
            $(this).attr("checked", false);
        }
    });
}
function AccuralGridLoad() {
    $('#AccuralGrid').kendoGrid({
        //toolbar: ["excel"],
        //excel: {
        //    fileName: "Accural.xlsx",
        //    filterable: true,
        //    allPages: true
        //},
        dataSource: GetAccuralDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: true,
        resizable: true,
        change: onRowSelectChange,
        columns: [
            {
                field: "ProjectCode",
                title: "Project Code",
                //width: 50,
                //hidden: true,
            },
            //{
            //    field: "",
            //    title: "",
            //    template: "<input class='chkAccural' id='#:Id#' type='checkbox'>",
            //    width: 30,
            //},
            {
                field: "CustomerType",
                title: "Customer Type",
            },
            {
                field: "ServiceName",
                title: "Service Name",
                footerTemplate: "<span style='float:right'>Total</span>",
            },
            {
                field: "Revenue",
                title: "Revenue($)",
                //template: "$#=Revenue#",
                template: '$#= kendo.toString(Revenue,"n") #',

                exportFormat: "string",
                footerTemplate: "$#= kendo.toString(sum, 'n') #",
            },
            {
                field: "Expense",
                title: "Expense($)",
                //template: "$#=Expense#",
                template: '$#= kendo.toString(Expense,"n") #',
                exportFormat: "string",
                footerTemplate: "$#= kendo.toString(sum, 'n') #",
            },
            {
                field: "Estimate",
                title: "Estimate",
            },
            {
                field: "CustomerName",
                title: "Customer Name",
            },
        ],
        //dataBound: onAccuralDatabound,
        selectable: "multiple",
        resizable: true,
        //detailInit: detailInit,
    }).data("kendoGrid");
    resizeGrid();
}
function GetAccuralDetail() {
    var source;
    if ($("#SearchBy").val() == "MonthYear") {
        source = GetAccuralByMonthYear();
    }
    else if ($("#SearchBy").val() == "FromToDate") {
        source = GetAccuralByDate();
    }
    return source;
}
function onRowSelectChange(arg) {
    var grid = arg.sender;
    var currentDataItem = grid.dataItem(this.select());
    if (currentDataItem != undefined) {
        //$("#editservice").show();
        //$("#deleteservice").show();
        serviceId = currentDataItem.Id;
        //uncheckallchkbox(serviceId);
        //$(".chkAccural").each(function () {
        //    if ($(this).attr("id") != serviceId) {
        //        $(this).attr("checked", true);
        //    }
        //});
    }
    //getallSelectedRowId();
}
function GetAccuralByMonthYear() {
    selectedMonth = $("#Month").val();
    selectedYear = $("#Year").val();
    var source = new kendo.data.DataSource({
        autoSync: true,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Reports/GetAccuralListbyMonthYear",
                data: { month: selectedMonth, year: selectedYear },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
            },
        },
        pageSize: 20,
        aggregate: [
            { field: "Revenue", aggregate: "sum" },
            { field: "Expense", aggregate: "sum" }
        ],
        schema: {
            model: {
                fields: {
                    ProjectCode: { type: "string" },
                    CustomerType: { type: "string" },
                    ServiceName: { type: "string" },
                    Revenue: { type: "number" },
                    Expense: { type: "number" },
                    Estimate: { type: "string" },
                    CustomerName: { type: "string" },
                    FromDate: { type: "string" },
                }
            }
        },

    });
    var resultstr = $("#Month option:selected").text() + " - " + selectedYear;
    $(".lblSearchContent").html(resultstr);
    return source;
}
function GetAccuralByDate() {
    var fromDate = $("#FromDate").val();
    var endDate = $("#EndDate").val();
    var source = new kendo.data.DataSource({
        autoSync: true,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Reports/GetAccuralListbyDate",
                data: { fromdate: fromDate, todate: endDate },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                cache: false,
            },
        },
        pageSize: 20,
        aggregate: [
            { field: "Revenue", aggregate: "sum" },
            { field: "Expense", aggregate: "sum" }
        ],
        schema: {
            model: {
                fields: {
                    ProjectCode: { type: "string" },
                    CustomerType: { type: "string" },
                    ServiceName: { type: "string" },
                    Revenue: { type: "number" },
                    Expense: { type: "number" },
                    Estimate: { type: "string" },
                    CustomerName: { type: "string" },
                    FromDate: { type: "string" },
                }
            }
        },

    });
    var resultstr = $("#FromDate").val() + " to " + $("#EndDate").val();
    $(".lblSearchContent").html(resultstr);
    return source;
}
function printGrid() {
    var gridElement = $('#AccuralGrid'),
        printableContent = '',
        win = window.open('', '', 'width=800, height=500'),
        doc = win.document.open();
    var kendocss = baseURL + "/Content/kendo.common.min.css";
    var printpreviecss = baseURL + "/Content/printpreview.css";
    var htmlStart =
            '<!DOCTYPE html>' +
            '<html>' +
            '<head>' +
            '<meta charset="utf-8" />' +
            '<title style="font-wieght:bold">Accural Report(' + $(".lblSearchContent").html() + ')</title>' +
            '<link href="' + kendocss + '" rel="stylesheet" /> ' +
            '<link href="' + printpreviecss + '" rel="stylesheet" /> ' +
            '</head>' +
            '<body>';

    var htmlEnd =
            '</body>' +
             '<script type="text/javascript">' +
            'window.print();' +
            '</script>' +
            '</html>';

    var gridHeader = gridElement.children('.k-grid-header');
    if (gridHeader[0]) {
        var thead = gridHeader.find('thead').clone().addClass('k-grid-header');
        printableContent = gridElement
            .clone()
                .children('.k-grid-header').remove()
            .end()
                .children('.k-grid-content')
                    .find('table')
                        .first()
                            .children('tbody').before(thead)
                        .end()
                    .end()
                .end()
            .end()[0].outerHTML;
    } else {
        printableContent = gridElement.clone()[0].outerHTML;
    }
    doc.write(htmlStart + printableContent + htmlEnd);
    setTimeout(function () {
        //doc.close();
        //win.print();
    }, 250);
}
function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#AccuralGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}
function SetFilterDataInCookie(charttype) {
    var strValue = [];
    if ($("#SearchBy").val() == "MonthYear") {
        var selectedMonth = $("#Month").val();
        var selectedYear = $("#Year").val();
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

