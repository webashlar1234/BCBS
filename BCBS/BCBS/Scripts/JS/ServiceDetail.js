var errorelement;
var selectedfeestype = [];
var contractFeesType = [];
$(document).ready(function () {
    $("#settingitems").toggle();
    $(".menuServiceTypes").addClass("menuactive");
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed");
    $('body').tooltip({
        container: 'body',
        selector: '.has-error',
    });
    $('form').validate({
        rules: {

            Budget: {
                min: 0,
                max: 99999999.99,
                number: true,
                dollarsscents: true,
            },
            //Volume: {
            //    required: {
            //        depends: function (element) {
            //            return $("#FeesType").val() == "Transaction";
            //        }
            //    },
            //    min: 0,
            //    max: 99999999.99,
            //    dollarsscents: true,
            //    number: true,
            //}
        },
        messages: {
            Budget: {
                minlength: 'Enter between 0 - 99999999.99',
                maxlength: 'Enter between 0 - 99999999.99',
            }
        },
        highlight: function (element) {
            //$(element).closest('.form-group').removeClass('success').addClass('error');

            $(element).addClass("error");
            if (!($(element).hasClass("feesAmount"))) {
                $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback' data-toggle='tooltip' data-placement='bottom' title='Tooltip on bottom'></span>");
            }
            $(element).parent().addClass("has-error has-feedback");
        },
        //errorPlacement: function (error, element) {
        //    $(element).parent().attr("title", $(error).html());
        //},
        success: function (element) {
            $(element).parent().children(".glyphicon-exclamation-sign").remove();
            $(element).parent().removeClass("has-error");
            $(element).parent().removeClass("has-feedback");
            $(element).removeClass("error");
            $(element).remove();
            //element.addClass('valid').closest('.form-group').removeClass('error').addClass('success');
        }
    });

    $(".volume").hide();
    //bindProjectDDL();
    $("#btnServiceCancel").click(function () {
        window.location.href = baseURL + "/service/index";
    });
    $(".chkFeesType").click(function () {
        var chkbox = $(this);
        $("#Budget").val("");
        if ($(this).find("[type='checkbox']").is(":checked")) {
            $(".chkFeesType").each(function () {
                if ($(this).find("[type='checkbox']").attr("id") != chkbox.find("[type='checkbox']").attr("id")) {
                    $(this).find("[type='checkbox']").attr("checked", false);
                }
            });
            //if (chkbox.attr("for") == "chkTransaction") {

            //    //$(".volume").show();
            //}
            //if (chkbox.attr("for") == "chkOnceOnly") {
            //    $(".onceonlydiv").show();
            //}
            //else if (chkbox.attr("for") == "chkMonthly") {
            //    $(".monthlydiv").show();

            //}
            //else if (chkbox.attr("for") == "chkTransaction") {
            //    $(".transactiondiv").show();
            //}
            //else if (chkbox.attr("for") == "chkAnnual") {
            //    $(".annualdiv").show();

            //}
            //else if (chkbox.attr("for") == "chkHourly") {
            //    $(".hourlydiv").show();

            //}
            //else if (chkbox.attr("for") == "chkSetUp") {
            //    $(".setupdiv").show();
            //}
            //else {
            //    $("#Volume").val("");
            //    $(".volume").hide();
            //}
            $("#FeesType").val(chkbox.find("[type='checkbox']").val());
        }
        else {
            $("#FeesType").val("");
            //var isContractCreated = contractFeesType.filter(function (el) { if (el == type) { return el; } });
            //if (isContractCreated.length > 0)
            //{
            //    debugger;
            //    $(this).find("[type='checkbox']").attr("checked", true);
            //    return false;
            //}
            //if (chkbox.attr("for") == "chkTransaction") {
            //    //$(".volume").hide();
            //    $(".transactiondiv").hide();
            //}
            //if (chkbox.attr("for") == "chkOnceOnly") {
            //    $(".onceonlydiv").hide();
            //    $(".onceonlydiv input").val("");
            //    $(".onceonlydiv").find("label.error").remove()
            //    $(".onceonlydiv").find(".error").removeClass("error")
            //}
            //else if (chkbox.attr("for") == "chkMonthly") {
            //    $(".monthlydiv").hide();
            //    $(".monthlydiv input").val("");
            //    $(".monthlydiv").find("label.error").remove()
            //    $(".monthlydiv").find(".error").removeClass("error")
            //}
            //else if (chkbox.attr("for") == "chkTransaction") {
            //    $(".transactiondiv").hide();
            //    $(".transactiondiv input").val("");
            //    $(".transactiondiv").find("label.error").remove()
            //    $(".transactiondiv").find(".error").removeClass("error")
            //}
            //else if (chkbox.attr("for") == "chkAnnual") {
            //    $(".annualdiv").hide();
            //    $(".annualdiv input").val("");
            //    $(".annualdiv").find("label.error").remove()
            //    $(".annualdiv").find(".error").removeClass("error")
            //}
            //else if (chkbox.attr("for") == "chkHourly") {
            //    $(".hourlydiv").hide();
            //    $(".hourlydiv input").val("");
            //    $(".hourlydiv").find("label.error").remove()
            //    $(".hourlydiv").find(".error").removeClass("error")
            //}
            //else if (chkbox.attr("for") == "chkSetUp") {
            //    $(".setupdiv").hide();
            //    $(".setupdiv input").val("");
            //    $(".setupdiv").find("label.error").remove()
            //    $(".setupdiv").find(".error").removeClass("error")
            //}
            //SetFeesType();
        }

    });
    $(".feesAmount").change(function () {
        selectedfeestype = [];
        SetFeesType();
    });
    if ($("#Id").val() > 0) {
        //selectedfeestype = [];
        //GetContractFeesTypeByServiceId($("#Id").val());
        if ($("#FeesType").val() != null) {
            //var value = $.parseJSON($("#FeesType").val());
            //$.each(value, function (index, data) {
            var type = $("#FeesType").val();
            //var type = $("#FeesType").val();
            //var amount = data.Amount;
            //selectedfeestype.push(type + "=" + amount);
            //var isContractCreated = contractFeesType.filter(function (el) { if (el == type) { return el; } });
            if (type == "OnceOnly") {
                $("#chkOnceOnly").attr("checked", true);
                //$(".onceonlydiv").show();
                //$(".onceonlydiv input").val(amount);
                //if (isContractCreated.length > 0)
                //{
                //    $("#chkOnceOnly").attr("disabled", "disabled");
                //    $(".onceonlydiv input").attr("disabled", "disabled");
                //}
            }
            else if (type == "Monthly") {
                $("#chkMonthly").attr("checked", true);
                //$(".monthlydiv").show();
                //$(".monthlydiv input").val(amount);
                //if (isContractCreated.length > 0) {
                //    $("#chkMonthly").attr("disabled", "disabled");
                //    $(".monthlydiv input").attr("disabled", "disabled");
                //}
            }
            else if (type == "Transaction") {
                $("#chkTransaction").attr("checked", true);
                //$(".transactiondiv").show();
                //$(".transactiondiv input").val(amount);
                //if (isContractCreated.length > 0) {
                //    $("#chkTransaction").attr("disabled", "disabled");
                //    $(".transactiondiv input").attr("disabled", "disabled");
                //}
            }
            else if (type == "Annual") {
                $("#chkAnnual").attr("checked", true);
                //$(".annualdiv").show();
                //$(".annualdiv input").val(amount);
                //if (isContractCreated.length > 0) {
                //    $("#chkAnnual").attr("disabled", "disabled");
                //    $(".annualdiv input").attr("disabled", "disabled");
                //}
            } else if (type == "Hourly") {
                $("#chkHourly").attr("checked", true);
                //$(".hourlydiv").show();
                //$(".hourlydiv input").val(amount);
                //if (isContractCreated.length > 0) {
                //    $("#chkHourly").attr("disabled", "disabled");
                //    $(".hourlydiv input").attr("disabled", "disabled");
                //}
            }
            else if (type == "SetUp") {
                $("#chkSetUp").attr("checked", true);
                //$(".setupdiv").show();
                //$(".setupdiv input").val(amount);
                //if (isContractCreated.length > 0) {
                //    $("#chkSetUp").attr("disabled", "disabled");
                //    $(".setupdiv input").attr("disabled", "disabled");
                //}
            }
            //});
            //$("#FeesType").val(selectedfeestype.join(','));
            //$("#chk" + value).attr("checked", true);
        }
        //$(".chkFeesType").each(function () {
        //    if ($(this).attr("for") == "chkTransaction") {
        //        if ($(this).find("[type='checkbox']").is(":checked")) {
        //            //$(".volume").show();
        //        }
        //    }
        //});
    }
});
function GetContractFeesTypeByServiceId(serviceid) {
    $.ajax({
        url: baseURL + "/Service/GetContractFeesTypeByServiceId",
        data: { serviceId: serviceid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (resultdata) {
            if (resultdata != "") {
                contractFeesType = $.parseJSON(resultdata);
            }

        },
        failure: function (resultdata) { },
    })
}
//function SetFeesType()
//{
//    selectedfeestype = [];
//    $(".chkFeesType").each(function () {
//        if ($(this).find("[type='checkbox']").is(":checked")) {
//            var checked = $(this).attr("for");
//            var value;
//            if (checked == "chkOnceOnly") {
//                var disabled = $(".onceonlydiv").find(':input:disabled').removeAttr('disabled');
//                value = $(".onceonlydiv input").serialize();
//                disabled.attr('disabled', 'disabled');
//            }
//            if (checked == "chkMonthly") {
//                var disabled = $(".monthlydiv").find(':input:disabled').removeAttr('disabled');
//                value = $(".monthlydiv input").serialize();
//                disabled.attr('disabled', 'disabled');
//            }
//            if (checked == "chkTransaction") {
//                var disabled = $(".transactiondiv").find(':input:disabled').removeAttr('disabled');
//                value = $(".transactiondiv input").serialize();
//                disabled.attr('disabled', 'disabled');
//                //value = $(".transactiondiv input").serialize();
//            }
//            if (checked == "chkAnnual") {
//                var disabled = $(".annualdiv").find(':input:disabled').removeAttr('disabled');
//                value = $(".annualdiv input").serialize();
//                disabled.attr('disabled', 'disabled');
//                //value = $(".annualdiv input").serialize();
//            }
//            if (checked == "chkHourly") {
//                var disabled = $(".hourlydiv").find(':input:disabled').removeAttr('disabled');
//                value = $(".hourlydiv input").serialize();
//                disabled.attr('disabled', 'disabled');
//                //value = $(".hourlydiv input").serialize();
//            }
//            if (checked == "chkSetUp") {
//                var disabled = $(".setupdiv").find(':input:disabled').removeAttr('disabled');
//                value = $(".setupdiv input").serialize();
//                disabled.attr('disabled', 'disabled');
//                //value = $(".setupdiv input").serialize();
//            }
//            selectedfeestype.push(value);
//            $("#FeesType").val(selectedfeestype.join(','));
//        }
//    });
//}

//function bindProjectDDL() {
//    $.ajax({
//        type: "GET",
//        url: baseURL + "/Service/GetProjectList",
//        dataType: "json",
//        contentType: "application/json; charset=utf-8",
//        async: false,
//        success: function (result) {
//            if (result != null) {
//                var stroptions = "";
//                $.each(result, function (index, data) {
//                    stroptions += "<option value='" + data.Id + "'>" + data.Name + "</option>";
//                });
//                console.log(stroptions);
//                $("#ProjectId").append(stroptions);
//            }
//        }
//    });
//}
