var alternateCode;
var startDate = new Date('01/01/2012');
var FromEndDate = new Date();
var ToEndDate = new Date();
ToEndDate.setDate(ToEndDate.getDate() + 365);
var servicetype;
var projectId;
var service;
var serviceId;
var customerId;
var feestypes;
var feestype;
$(document).ready(function () {
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
    $('form').validate({
        rules: {

            Amount: {
                min: 0,
                max: 99999999.99,
                number: true,
                dollarsscents: true,
                //regex: "/^\d+(?:\.\d\d?)?$/",
            },
            Volume: {
                required: {
                    depends: function (element) {
                        return $("#FeesType").val() == "Transaction";
                    }
                },
                min: 0,
                max: 99999999.99,
                dollarsscents: true,
                number: true,
            },
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
            },
            file: {
                uploadFile: true,
            }
        },
        messages: {
            ContractCode: {
                remote: jQuery.validator.format("{0} Contract Code is already taken.")
            }
        },
        highlight: function (element) {
            $(element).parent().children(".glyphicon-exclamation-sign").remove();
            $(element).parent().removeClass("has-error");
            $(element).parent().removeClass("has-feedback");
            $(element).removeClass("error");
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
        if ($("#Id").val() > 0) {
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
        $("#EndDate").datepicker('update');
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
        $("#Amount").val(parseFloat(totalAmount).toFixed(2));
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
    $("#ProjectId").change(function () {
        if ($("#Id").val() > 0) {
            $(this).val(projectId);
            return false;
        }
        var strservice = "<option value=''>--Select Service--</option>";
        $("#ServiceId").html(strservice);
        //var strfeestype = "<option value=''>--Select Fees Type--</option>";
        //$("#FeesType").html(strfeestype);
        $("#FeesType").val("");
        $(".volume").hide();
        $("#Volume").val("");
        $("#Amount").val("");
        //$("#Amount").removeAttr("readonly");
        $("#feestypeDetail").hide();
        $("#feestypeDetail").html("");
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
                $("#estBalance").html("Estimated Available balance: $" + kendo.toString(balance, "n"));
                bindServiceTypeddl($(this).val());
            }
        }
        else {
            var str = "<option value=''>--Select Service--</option>";
            $("#ServiceId").html(str);
            $("#estBalance").hide();
            $("#estBalance").html("");
        }
    });
    $("#ServiceId").change(function () {
        if ($("#Id").val() > 0) {
            $(this).val(serviceId);
            return false;
        }
        $("#Volume").val("");
        $("#Amount").val("");
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
                //$("#serviceDetail").html("");
            }
            else {
                $(".alert").hide();
                $("#message").html("");
                console.log("Service Active");
                //$("#Volume").val("");
                $("#Amount").val("");
                service = $(this).val();
                servicetype = getServiceTypeByServiceId(service);
                $("#FeesType").val(servicetype.FeesType);
                $("#serviceDetail").html("Fees Type: " + servicetype.FeesType + ", Budget: " + servicetype.Budget);
                //bindFeeTypeddl($(this).val());
                checkforVolume(servicetype.FeesType);
            }
        }
        else {
            var str = "<option value=''>--Select Fees Type--</option>";
            //$("#FeesType").html(str);
            $(".volume").hide();
            //$("#Amount").removeAttr("readonly");
        }
    });

    //$("#FeesType").change(function () {
    //    if ($("#Id").val() > 0) {
    //        $(this).val(feestype);
    //        return false;
    //    }
    //    $("#Volume").val("");
    //    $("#Amount").val("");
    //    if ($(this).val() != "") {
    //        if ($(this).val() == "Transaction") {
    //            $(".volume").show();
    //            //$("#Amount").attr("readonly", "readonly");

    //        }
    //        else {
    //            $(".volume").hide();
    //            //$("#Amount").removeAttr("readonly");
    //        }
    //        var value = $(this).val();
    //        SetFeesTypeDetail(value)
    //    }
    //    else {
    //        $("#feestypeDetail").html("");
    //        $("#feestypeDetail").hide();
    //        $(".volume").hide();
    //        //$("#Amount").removeAttr("readonly");
    //    }
    //});
    //$('#file').change(function () {
    //    var control = $(this);
    //    var name = this.files[0].name;
    //    var type = this.files[0].type;
    //    //var height = this.files[0].height;
    //    var maxfilesize = 2097152;//2MB;
    //    var size = Math.ceil(this.files[0].size / 1024);
    //    var ext = name.substr(name.lastIndexOf('.') + 1);
    //    var files = !!this.files ? this.files : [];
    //    if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support
    //    if (/^image/.test(files[0].type)) { // only image file
    //        // not allow more than 15 kb
    //        if (size < maxfilesize) {
    //            var reader = new FileReader(); // instance of the FileReader
    //            reader.readAsDataURL(files[0]); // read the local file
    //            reader.onloadend = function () { // set image data as background of div
    //                $('#editlogoIframe').contents().find("span").children().attr("src", this.result);
    //            }
    //        }
    //        else {
    //            $('#editlogoIframe').contents().find("#files").val("");
    //            $('#editlogoIframe').contents().find("span").children().attr("src", "/Images/Logos/nologo.png");
    //            $('#editlogoIframe').contents().find(".invalid").text("To large image file");
    //            $('#editlogoIframe').contents().find(".invalid").show();
    //        }
    //    }
    //    else {
    //        $('#editlogoIframe').contents().find("#files").val("");
    //        $('#editlogoIframe').contents().find("span").children().attr("src", "/Images/Logos/nologo.png");
    //        $('#editlogoIframe').contents().find(".invalid").text("Invalid image file");
    //        $('#editlogoIframe').contents().find(".invalid").show();
    //    }

    //    //console.log(this.hight);
    //});
    $("#Volume").change(function () {
        var volumeAmt = $(this).val();
        var currentFeesType = servicetype.FeesType;
        //var feesdata = feestypes.filter(function (el) {
        //    if (el.FeesType == currentFeesType) {
        //        return el;
        //    }
        //});
        var budgetValue = servicetype.Budget;
        var totalAmt = 0;
        if (currentFeesType == "Transaction" || currentFeesType == "Hourly") {
            totalAmt = (parseFloat(volumeAmt) * parseFloat(budgetValue)).toFixed(2);
            $("#Amount").val(totalAmt);
            //var transactionVolume = feesdata[0].Volume;
            //totalAmt = ((parseFloat(volumeAmt) / parseFloat(transactionVolume)) * (parseFloat(budgetValue))).toFixed(2);
        }
    });

    
    
    if ($("#Id").val() > 0) {
        customerId = $("#CustomerId").val();
        projectId = $("#ProjectId").val();
        serviceId = $("#hdnServiceId").val();
        feestype = $("#hdnFeesType").val();
        alternateCode = $("#ContractCode").val();
        //var serviceId = $("#ServiceId").val();
        //servicetype = getServiceTypeByServiceId(serviceId);

        //var projectId = $("#ProjectId").val();
        var balance = getavailablebalanceforproject(projectId);
        $("#estBalance").html("Estimated Available balance: $" + kendo.toString(balance, "n"));
        bindServiceTypeddl(projectId);
        //bindFeeTypeddl(serviceId);
        $("#ServiceId").val(serviceId);
        debugger;
        servicetype = getServiceTypeByServiceId(serviceId);
        $("#serviceDetail").html("Fees Type: " + servicetype.FeesType + ", Budget: " + servicetype.Budget);
        //$("#FeesType").val(feestype);
        //SetFeesTypeDetail(feestype);
        checkforVolume(feestype);
        //if (feestype == "Transaction" || feestype == "Hourly") {
        //    $(".volume").show();
        //}
        //servicetype = getServiceTypeByServiceId(service);
        //$("#serviceDetail").html("Fees Type: " + servicetype.FeesType + ", Budget: " + servicetype.Budget);
    }
    else {
        var contractforproject = $.cookie('contractforproject');
        if (contractforproject != undefined) {
            var removed = $.removeCookie('contractforproject', { path: '/' });
            $("#ProjectId").val(contractforproject);
            var isActive = checkStatusisActive("project", contractforproject);
            if (!isActive) {
                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                     "<strong>Error!</strong>Project is not an active!! " +
                     "</div>");
                $(".alert").show();
                $(".alert").delay(5000).slideUp(200, function () {
                    $(this).alert('close');
                });
                $("#ProjectId").val("");

            }
            else {
                $(".alert").hide();
                $("#message").html("");
                console.log("Project Active");
                var value = contractforproject;
                $("#estBalance").html("");
                var balance = getavailablebalanceforproject(value);
                $("#estBalance").html("Estimated Available balance: $" + kendo.toString(balance, "n"));
                bindServiceTypeddl(contractforproject);
            }
        }
    }
});
function getServiceTypeByServiceId(value) {
    var servicetypes;
    $.ajax({
        url: baseURL + "/Contract/GetServiceTypeByServiceID",
        data: { serviceId: value },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            servicetypes = $.parseJSON(data);
        },
        failure: function (data) {
            servicetypes = undefined;
        }
    });
    return servicetypes;
}
function checkforVolume(FeesType) {
    if (FeesType != undefined) {
        if (FeesType == "Transaction") {
            $(".volume").show();
            //$("#Amount").attr("readonly", "readonly");
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
function bindServiceTypeddl(projectid) {
    $.ajax({
        url: baseURL + "/Contract/GetServiceTypeByProjectID",
        data: { projectId: projectid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            service = $.parseJSON(data);
            var str = "<option value=''>--Select Service--</option>";
            $.each(service, function (index, data) {
                str += "<option value=" + data.Id + ">" + data.Name + "</option>"
            });
            $("#ServiceId").html(str);
        },
        failure: function (data) {
            service = undefined;
        }
    });
}
//function bindFeeTypeddl(serviceid) {
//    $.ajax({
//        url: baseURL + "/Contract/GetFeesTypeByServiceID",
//        data: { serviceId: serviceid },
//        dataType: "json",
//        contentType: "application/json; charset=utf-8",
//        async: false,
//        success: function (data) {
//            feestypes = $.parseJSON(data);
//            //service = $.parseJSON(data);
//            var str = "<option value=''>--Select Fees Type--</option>";
//            $.each(feestypes, function (index, data) {
//                str += "<option value=" + data.FeesType + ">" + data.FeesType + "</option>"
//            });
//            $("#FeesType").html(str);
//        },
//        failure: function (data) {
//            feestypes = undefined;
//        }
//    });
//}

function SetFeesTypeDetail(value) {
    //var feestypedata = feestypes.filter(function (el) { if (el.FeesType == value) { return el; } });
    //feestypes.filter(function (el) {
    //    if (el.FeesType == "OnceOnly") {
    //        console.log("el.FeesType")
    //    }
    //});
    var label = "Amount($)";
    if (value == "Transaction") {
        label = "Rate/Transaction($)";
    }
    else if (value == "Hourly") {
        label = "Hourly Rate($)"
    }
    $("#feestypeDetail").html(label + ": $" + kendo.toString(feestypedata[0].Amount, "n"));
    $("#feestypeDetail").show();
}


