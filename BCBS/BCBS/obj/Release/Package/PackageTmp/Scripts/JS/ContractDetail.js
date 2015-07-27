var alternateCode;
var startDate = new Date('01/01/2012');
var FromEndDate = new Date();
var ToEndDate = new Date();
ToEndDate.setDate(ToEndDate.getDate() + 365);
var servicetype;
var projectId;
var service;
var customerId;
$(document).ready(function () {
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed");
    $('form').validate({
        rules: {

            Ammount: {
                min: 0,
                max: 99999999.99,
                number: true,
                dollarsscents: true,
                //regex: "/^\d+(?:\.\d\d?)?$/",
            },
            //Volume: {
            //    min: 0,
            //    max: 99999999,
            //    number: true,
            //    dollarsscents: true,
            //},
            ContractCode: {
                required: true,
                remote: {
                    url: baseURL + "/Contract/IsContractCodeExist",
                    type: "GET",
                    data: {
                        chargeCode: function () {
                            return $("#ContractCode").val();
                        }
                    },
                    dataFilter: function (data) {
                        if ($("#ContractCode").val() == alternateCode) {
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
            ContractCode: {
                remote: jQuery.validator.format("{0} Contract Code is already taken.")
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
    //
    $('input[type = "radio"]').click(function () {
        if ($("#Id").val() > 0) {
            return false;
        }
    });

    //$(".volume").hide();
    $("#FromDate").keypress(function () {
        return false;
    });
    $("#EndDate").keypress(function () {
        return false;
    });
    var datefrom = $("#FromDate").val();
    var checkin = $('#FromDate').datepicker({
        "setDate": new Date(datefrom),
        onRender: function (date) {
            //return date.valueOf() < now.valueOf() ? 'disabled' : '';
        },
    }).on('show', function (ev, date) {
        if ($("#Id").val() > 0)
        {
            checkin.hide();
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
            //$('#EndDate')[0].focus();
        }).data('datepicker');
    var checkout = $('#EndDate').datepicker({
        "setDate": new Date(datefrom),
        onRender: function (date) {
            return date.valueOf() < checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');
    $("#btnContractCancel").click(function () {
        window.location.href = baseURL + "/contract/index";
    });
    $(".txtamt").change(function () {
        var totalAmount = 0.00;
        $(".txtamt").each(function () {
            totalAmount += parseFloat($(this).val());
        });
        $("#Ammount").val(parseFloat(totalAmount).toFixed(2));
    });
    $("#CustomerId").change(function () {
        if ($("#Id").val() > 0) {
            $(this).val(customerId);
            return false;
        }
        if ($(this).val() > 0) {
            var isActive = checkStatusisActive("customer", $(this).val());
            if (!isActive) {
                $(this).val("");
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                       "<strong>Error!</strong>Customer is not an active!! " +
                       "</div>");
                $(".alert").show();
                $(".alert").delay(5000).slideUp(200, function () {
                    $(this).alert('close');
                });
            }
            else {
                $(".alert").hide();
                $("#message").html("");
                console.log("Customer Active");
            }
        }
    });
    $("#ServiceId").change(function () {
        if ($("#Id").val() > 0) {
            $(this).val(service);
            return false;
        }
        if ($(this).val() > 0) {
            var isActive = checkStatusisActive("servicetype", $(this).val());
            if (!isActive) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                      "<strong>Error!</strong>Service type is not an active!! " +
                      "</div>");
                $(".alert").show();
                $(".alert").delay(5000).slideUp(200, function () {
                    $(this).alert('close');
                });
                $(this).val("");
                $("#serviceDetail").html("");
            }
            else {
                $(".alert").hide();
                $("#message").html("");
                console.log("Service Active");
                //$("#Volume").val("");
                $("#Ammount").val("");
                service = $(this).val();
                servicetype = getServiceTypeByServiceId(service);
                $("#serviceDetail").html("Fees Type: " + servicetype.FeesType + ", Budget: " + servicetype.Budget);
                //checkforVolume(servicetype);
            }
        }
    });
    $("#ProjectId").change(function () {
        if ($("#Id").val() > 0) {
            $(this).val(projectId);
            return false;
        }
        if ($(this).val() > 0) {
            var isActive = checkStatusisActive("project", $(this).val());
            if (!isActive) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                     "<strong>Error!</strong>Project is not an active!! " +
                     "</div>");
                $(".alert").show();
                $(".alert").delay(5000).slideUp(200, function () {
                    $(this).alert('close');
                });
                $(this).val("");
            }
            else {
                $(".alert").hide();
                $("#message").html("");
                console.log("Project Active");
                var value = $(this).val();
                $("#estBalance").html("");
                var balance = getavailablebalanceforproject(value);
                $("#estBalance").html("Estimated Available balance: $" + balance);
            }
        }
    });

    //$("#Volume").change(function () {
    //    var volumeAmt = $(this).val();
    //    var budgetValue = servicetype.Budget;
    //    var totalAmt = 0;
    //    if (servicetype.FeesType == "Transaction") {
    //        var transactionVolume = servicetype.Volume;
    //        totalAmt = ((parseFloat(volumeAmt) / parseFloat(transactionVolume)) * (parseFloat(budgetValue))).toFixed(2);
    //    }
    //    else {
    //        totalAmt = (parseFloat(volumeAmt) * parseFloat(budgetValue)).toFixed(2);
    //    }
    //    $("#Ammount").val(totalAmt);

    //});

    if ($("#Id").val() > 0) {
        customerId = $("#CustomerId").val();
        service = $("#ServiceId").val();
        projectId = $("#ProjectId").val();
        alternateCode = $("#ContractCode").val();
        //var serviceId = $("#ServiceId").val();
        //servicetype = getServiceTypeByServiceId(serviceId);
        //checkforVolume(servicetype);
        //var projectId = $("#ProjectId").val();
        var balance = getavailablebalanceforproject(projectId);
        $("#estBalance").html("Estimated Available balance: $" + balance);
        servicetype = getServiceTypeByServiceId(service);
        $("#serviceDetail").html("Fees Type: " + servicetype.FeesType + ", Budget: " + servicetype.Budget);
    }
});
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
function getavailablebalanceforproject(projectid) {
    var balance;
    $.ajax({
        url: baseURL + "/Contract/GetBalanceForProject",
        data: { projectId: projectid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            balance = $.parseJSON(data);
        },
        failure: function (data) {
            balance = "";
        }
    });
    return balance;

}
function checkStatusisActive(tablename, id) {
    var isActive;
    $.ajax({
        url: baseURL + "/Contract/CheckStatusIsActive",
        data: { tableName: tablename, Id: id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            isActive = data;
        },
        failure: function (data) {
            isActive = "";
        }
    });
    return isActive;
}

