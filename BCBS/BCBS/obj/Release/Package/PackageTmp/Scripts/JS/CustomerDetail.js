var alternateCode = "";
$(document).ready(function () {
    $("#settingitems").toggle();
    $(".menuCustomers").addClass("menuactive");
    $.validator.addMethod('usPhone', function (value, element) {
        return this.optional(element) || /^[01]?[- .]?\(?[2-9]\d{2}\)?[- .]?\d{3}[- .]?\d{4}$/.test(value);
    }, 'Please enter a valid US phone number.');
    $.validator.addMethod('usFax', function (value, element) {
        return this.optional(element) || /^[01]?[- .]?\(?[2-9]\d{2}\)?[- .]?\d{3}[- .]?\d{4}$/.test(value);
    }, 'Please enter a valid US fax number.');
    $('form').validate({
        rules: {
            Email: {
                required: true,
                email: true,
            },
            Phone: {
                required: true,
                usPhone: true,
            },
            Fax: {
                usFax: true,
            },
            PostalCode: {
                minlength: 5,
                maxlength: 5,
                number: true
            },
            ChargeCode: {
                required: true,
                remote: {
                    url: baseURL + "/Customer/IsChargeCodeExist",
                    type: "GET",
                    data: {
                        chargeCode: function () {
                            return $("#ChargeCode").val();
                        }
                    },
                    dataFilter: function (data) {
                        if ($("#ChargeCode").val() == alternateCode) {
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
            ChargeCode: {
                remote: jQuery.validator.format("{0} Customer Code is already taken.")
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
    $("#btnCustomerCancel").click(function () {
        window.location.href = baseURL + "/customer/index";
    });
    if ($("#Id").val() > 0)
    {
        alternateCode = $("#ChargeCode").val();
    }
    //$("#ChargeCode").keyup(function () {
    //    checkChargeCodeExist($(this).val());
    //});
    //$("#ChargeCode").focusout(function () {
    //    checkChargeCodeExist($(this).val());
    //});
});
//function checkChargeCodeExist(value) {
//    $.ajax({
//        url: baseURL + "/Customer/IsChargeCodeExist",
//        data: { chargeCode: value },
//        type: "GET",
//        success: function (data) {
//            console.log(data);
//            if (data) {
//                $("#ChargeCode").append("<span class='danger'>" + value + " Code already exist</span>");
//            }
//        }
//    })
//}