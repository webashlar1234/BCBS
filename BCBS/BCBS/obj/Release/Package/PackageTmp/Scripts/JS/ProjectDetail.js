var alternateCode = "";
$(document).ready(function () {
    $("#settingitems").toggle();
    $(".menuProject").addClass("menuactive");
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed");
    $('form').validate({
        rules: {

            HighLevelBudget: {
                min: 0,
                max: 99999999.99,
                number: true,
                dollarsscents: true,
            },
            ChargeCode: {
                remote: {
                    url: baseURL + "/Project/IsChargeCodeExist",
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
                remote: jQuery.validator.format("{0} charge Code is already taken.")
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
    $("#btnProjectCancel").click(function () {
        window.location.href = baseURL + "/project/index";
    });
    //$("#ChargeCode").keyup(function () {
    //    checkChargeCodeExist($(this).val());
    //});
    //$("#ChargeCode").focusout(function () {
    //    checkChargeCodeExist($(this).val());
    //});
    if ($("#Id").val() > 0) {
        alternateCode = $("#ChargeCode").val();
    }
});

function checkChargeCodeExist(value) {
    $.ajax({
        url: baseURL + "/Project/IsChargeCodeExist",
        data: { chargeCode: value },
        type: "GET",
        success: function (data) {
            console.log(data);
            if (data) {
                $("#ChargeCode").append("<span class='danger'>" + value + " Code already exist</span>");
                //alert("alreadey exist");
            }
        }
    })
}