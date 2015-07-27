var contractData;
var servicetype;
var lastActivity;
var alternateCode;
$(document).ready(function () {
    Date.prototype.getTimezoneOffset = function () { return +240; };
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed.");
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
    //jQuery.validator.addMethod("activitycode", function (val, element) {
    //    if (val.split('-').length < 2)// checks the file more than 3 MB
    //    {
    //        return false;
    //    } else {
    //        if (val.split('-')[1] == "") {
    //            return false
    //        }
    //        else {
    //            return true;
    //        }
    //    }
    //}, "Please follow contractcode-Actvitycode format");
    $('form').validate({
        rules: {
            EndDate: {
                required: true,
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
            file: {
                uploadFile: true,
            },
            ActivityCode: {
                required: true,
                //activitycode: true,
                remote: {
                    url: baseURL + "/Contract/IsActivityCodeExist",
                    type: "GET",
                    data: {
                        ActivityCode: function () {
                            return $("#ContractCode").val() + "-" + $("#ActivityCode").val();
                        }
                    },
                    dataFilter: function (data) {
                        if ($("#ActivityCode").val() == alternateCode) {
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
            ActivityCode: {
                remote: jQuery.validator.format("{0} activity Code is already taken.")
            }
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") === 'ActivityCode') {
                error.insertAfter(element.parent());
            }
            else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) {
            //$(element).closest('.form-group').removeClass('success').addClass('error');
            $(element).parent().children(".glyphicon-exclamation-sign").remove();
            $(element).parent().removeClass("has-error");
            $(element).parent().removeClass("has-feedback");
            $(element).removeClass("error");
            $(element).addClass("error");
            $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
            $(element).parent().addClass("has-error has-feedback");
        },
        success: function (element) {
            if ($(element).parent().children(".has-error").length > 0) {
                $(element).parent().children(".has-error").children(".glyphicon-exclamation-sign").remove();
                $(element).parent().children(".has-error").removeClass("has-feedback");
                $(element).parent().children(".has-error").removeClass("has-error");
            }
            else {
                $(element).parent().children(".glyphicon-exclamation-sign").remove();
                $(element).parent().removeClass("has-error");
                $(element).parent().removeClass("has-feedback");
                $(element).removeClass("error");
                $(element).remove();
            }
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
                    //servicetype = getServiceTypeByServiceId(contractData.ServiceId);
                    checkforVolume(contractData);
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

    if (!($("#ActivityId").val() > 0)) {
        $("#FromDate").val("");
        $("#EndDate").val("");
    }
    var datefrom = $("#FromDate").val();
    var checkin = $('#FromDate').datepicker({
        //"setDate": new Date(datefrom),
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
            //checkout.setValue(checkout.date);
        }
        checkin.hide();
        $("#EndDate").datepicker('update');
        //$('#EndDate')[0].focus();
    }).data('datepicker');

    var checkout = $('#EndDate').datepicker({
        format: 'mm/dd/yyyy',
        //"setDate": new Date(datefrom),
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
    $("#Volume").change(function () {
        var volumeAmt = $(this).val();
        var budgetValue = contractData.Amount;
        var totalAmt = 0;
        $("#Amount").val();
        if (contractData.FeesType == "Transaction") {
            var transactionVolume = contractData.Volume;
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
    if ($("#ActivityId").val() > 0) {
        alternateCode = $("#ActivityCode").val();
    }
});

function checkforVolume(contractData) {
    if (contractData != undefined) {
        if (contractData.FeesType == "Transaction" || contractData.FeesType == "Hourly") {
            $(".volume").show();
            $("#Amount").attr("readonly", "readonly");
        }
        else {
            $(".volume").hide();
        }
    }
}
function getServiceTypeByServiceId(value) {
    var service;
    $.ajax({
        url: baseURL + "/Contract/GetServiceTypeByServiceID",
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
