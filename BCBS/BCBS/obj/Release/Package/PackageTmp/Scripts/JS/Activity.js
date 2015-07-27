$(document).ready(function () {
    $('form').validate({
        rules: {

            Project: {
                required: true,
                //regex: "/^\d+(?:\.\d\d?)?$/",
            },
            Service: {
                required: true,
            },
            Customer: {
                required: true,
            },
            EndMonth: {
                required: true,
            },
            FromMonth: {
                required: true,
            },
            Year: {
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

    $("#Project").change(function () {
        $("#Service").html("");
        $(".serviceddl").hide();
        $("#Customer").html("");
        $(".customerddl").hide();
        $(".monthgroup").hide();
        $("#ContractGrid").hide();
        if ($(this).val().length > 0) {
            bindServiceByProject($(this).val());
        }
    });
    $("#Service").change(function () {
        $("#ContractGrid").hide();
        $("#Customer").html("");
        $("#customerddl").hide();
        $("#ContractGrid").hide();
        if ($(this).val().length > 0) {
            bindCustomerByServiceAndProject($(this).val(), $("#Project").val());
        }
        else {
            $(".customerddl").hide();
            $(".monthgroup").hide();
        }
    });
    $("#Customer").change(function () {
        $("#ContractGrid").hide();
        if ($(this).val().length > 0) {
            $(".monthgroup").show();
        }
        else {
            $("#FromMonth").val("");
            $("#EndMonth").val("");
            $(".monthgroup").hide();
        }
    });
    $("#FromMonth").change(function () {
        var value = $(this).val();
        $("#EndMonth").val("");
        if (value.length > 0) {
            $(".endmonth").show();
            $("#EndMonth option").each(function () {
                if (parseInt($(this).val()) <= value) {
                    $(this).prop('disabled', true);
                }
            });
        }
        else {
            $(".endmonth").hide();
        }
    });
    $("#EndMonth").change(function () {
        var value = $(this).val();
        if (value.length > 0) {
            $(".btngroup").show();
        }
        else {
            $(".btngroup").hide();
        }
    });
    $(".btnGetContract").click(function () {
        if ($('form').valid()) {
            $("#ContractGrid").show();
            ContractGridLoad();
        }
    });
});
function bindServiceByProject(projectid) {
    $.ajax({
        type: "GET",
        url: baseURL + "/Activity/ServiceByProjectID",
        data: { projectId: projectid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            source = $.parseJSON(data);
            var servicesStr = "<option value=''>--Select Service--</option>";
            $.each(source, function (index, data) {
                servicesStr += "<option value='" + data.Id + "'>" + data.Name + "</option>"
            });
            $("#Service").html("");
            $("#Service").html(servicesStr);
            $(".serviceddl").show();

        }
    });
}
function bindCustomerByServiceAndProject(serviceid, projectid) {
    $.ajax({
        type: "GET",
        url: baseURL + "/Activity/CustomerByServiceAndProjectId",
        data: { serviceId: serviceid, projectId: projectid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            source = $.parseJSON(data);
            var customersStr = "<option value=''>--Select Customer--</option>";
            $.each(source, function (index, data) {
                customersStr += "<option value='" + data.Id + "'>" + data.Name + "</option>"
            });
            $("#Customer").html("");
            $("#Customer").html(customersStr);
            $(".customerddl").show();
        }
    });
}
function ContractGridLoad() {
    $('#ContractGrid').kendoGrid({
        dataSource: GetContractDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: false,
        resizable: true,
        //change: onRowSelectChange,
        columns: [
            //{
            //    field: "Id",
            //    title: "ID",
            //    width: 50,
            //    hidden: true,
            //},
            //{
            //    field: "",
            //    title: "",
            //    template: "<input class='chkContract' id='#:Id#' type='checkbox'>",
            //    width: 30,
            //},
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
                field: "ProjectCode",
                title: "Project Code",
            },
            {
                field: "Charges",
                title: "Charges",
                footerTemplate: "<span style='float:right'>Total</span>",
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
        }
    }).data("kendoGrid");

    //resizeGrid();
}
function GetContractDetail() {
    var source = new kendo.data.DataSource({
        //data: GetContractListDataSource(),
        autoSync: true,
        //pageSize: 10,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Activity/GetContractForActivity",
                data: { customerId: $("#Customer").val(), serviceId: $("#Service").val(), projectId: $("#Project").val(), fromMonth: $("#FromMonth").val(), toMonth: $("#EndMonth").val() },
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
                    Charges: { type: 'string' }
                }
            }
        },
        //aggregate: [
        //{ field: "Ammount", aggregate: "sum" }
        //],
    });
    return source;
} 