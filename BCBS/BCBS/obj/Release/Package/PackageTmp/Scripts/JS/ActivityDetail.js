var contractData;
var servicetype;
var lastActivity;
$(document).ready(function () {
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed");
    $('form').validate({
        rules: {
            EndDate:{
                required:true,
            },
            FromDate: {
                required: true,
            },
            Amount: {
                min: 0,
                max: 99999999.99,
                number: true,
                dollarsscents: true,
                //regex: "/^\d+(?:\.\d\d?)?$/",
            },
            Volume: {
                min: 0,
                max: 99999999,
                number: true,
                dollarsscents: true,
            },
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
    if ($("#ContractId").val() > 0) {
        $.ajax({
            type: "GET",
            url: baseURL + "/Contract/GetContractDataById",
            data: { contractId: $("#ContractId").val() },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
                if (data != null) {
                    contractData = data;
                    servicetype = getServiceTypeByServiceId(contractData.ServiceId);
                    checkforVolume(servicetype);
                    lastActivity = getLastContractActivity($("#ContractId").val());
                }
            },
            failure: function (data) {
                console.log(data);
            }
        });
    }
    var nowTemp = new Date();
    var now = new Date(nowTemp.getFullYear(), nowTemp.getMonth(), nowTemp.getDate(), 0, 0, 0, 0);
    var datefrom = $("#FromDate").val();
    var checkin = $('#FromDate').datepicker({
        "setDate": new Date(datefrom),
        format: 'mm/dd/yyyy',
        onRender: function (date) {
            if (date.valueOf() < parseInt(contractData.FromDate.substr(6))) {
                return 'disabled'
            } else if (date.valueOf() > parseInt(contractData.EndDate.substr(6))) {
                return 'disabled';
            }
            if (date.valueOf() > now.valueOf()) {
                return 'disabled';
            }
            if (lastActivity != undefined) {
                if (date.valueOf() <= parseInt(lastActivity.EndDate.substr(6))) {
                    return 'disabled';
                }
                else {
                    return '';
                }
            }
            //else {
            //    return ''
            //}
            //return date.valueOf() <= parseInt(contractData.FromDate.substr(6)) ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        if (ev.date.valueOf() > checkout.date.valueOf()) {
            var newDate = new Date(ev.date)
            newDate.setDate(newDate.getDate());
            checkout.setValue(newDate);
        }
        else {
            var newDate = new Date(ev.date)
            newDate.setDate(newDate.getDate());
            checkout.setValue(checkout.date);
        }
        checkin.hide();
        //$('#EndDate')[0].focus();
    }).data('datepicker');
    var checkout = $('#EndDate').datepicker({
        format: 'mm/dd/yyyy',
        "setDate": new Date(datefrom),
        onRender: function (date) {
            if (date.valueOf() < parseInt(contractData.FromDate.substr(6))) {
                return 'disabled';
            }
            if (date.valueOf() > parseInt(contractData.EndDate.substr(6))) {
                return 'disabled';
            }
            if (date.valueOf() < checkin.date.valueOf()) {
                return 'disabled';
            }
            if (date.valueOf() > now.valueOf()) {
                return 'disabled';
            }
            //else {
            //    return ''
            //}
            //return date.valueOf() < checkin.date.valueOf() && ) ? 'disabled' : '';
        },
        beforeShowDay: function (date) {
            if (date.valueOf() < checkin.date.valueOf()) {
                return 'disabled';
            }
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');
    if (!$("#Id").val() > 0) {
        $("#FromDate").val("");
        $("#EndDate").val("");
    }
    $("#Volume").change(function () {
        var volumeAmt = $(this).val();
        var budgetValue = servicetype.Budget;
        var totalAmt = 0;
        if (servicetype.FeesType == "Transaction") {
            var transactionVolume = servicetype.Volume;
            totalAmt = ((parseFloat(volumeAmt) / parseFloat(transactionVolume)) * (parseFloat(budgetValue))).toFixed(2);
        }
        else {
            totalAmt = (parseFloat(volumeAmt) * parseFloat(budgetValue)).toFixed(2);
        }
        $("#Amount").val(totalAmt);

    });
    $(document).on('keypress', "#FromDate", function () {
        return false;
    });
    $(document).on('keypress', "#EndDate", function () {
        return false;
    });
    $(document).on("click", "#btnActivityCancel", function () {
        window.location.href = baseURL + "/contract/activities";
    });
});

function checkforVolume(servicetype) {
    if (servicetype != undefined) {
        if (servicetype.FeesType == "Hourly" || servicetype.FeesType == "Transaction") {
            $(".volume").show();
        }
        else {
            $(".volume").hide();
        }
    }
}
function getServiceTypeByServiceId(value) {
    var service;
    $.ajax({
        url: baseURL + "/Contract/GetFeesTypeByServiceID",
        data: { serviceId: value },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            service = $.parseJSON(data);
        },
        failure: function (data) {
            service = undefined;
        }
    });
    return service;
}

function getLastContractActivity(contractid) {
    var lastactivity;
    $.ajax({
        url: baseURL + "/Contract/GetLastContractActivity",
        data: { contractId: contractid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            lastactivity = data;
        },
        failure: function (data) {
            lastactivity = undefined;
        }
    });
    return lastactivity;
}
