var errorelement;
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
            Volume: {
                required: {
                    depends: function(element){
                        return $("#FeesType").val() == "Transaction";
                    }
                },
                min: 0,
                max: 99999999.99,
                dollarsscents: true,
                number: true,
            }
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
            $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback' data-toggle='tooltip' data-placement='bottom' title='Tooltip on bottom'></span>");
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
        if ($(this).find("[type='checkbox']").is(":checked")) {
            $(".chkFeesType").each(function () {
                if ($(this).find("[type='checkbox']").attr("id") != chkbox.find("[type='checkbox']").attr("id")) {
                    $(this).find("[type='checkbox']").attr("checked", false);
                }
            });
            if (chkbox.attr("for") == "chkTransaction") {
                $(".volume").show();
            }
            else {
                $("#Volume").val("");
                $(".volume").hide();
            }
            $("#FeesType").val(chkbox.find("[type='checkbox']").val());
        }
        else {
            if (chkbox.attr("for") == "chkTransaction") {
                $(".volume").hide();
            }
        }

    });
    if ($("#Id").val() > 0) {
        if ($("#FeesType") != null) {
            var value = $("#FeesType").val();
            $("#chk" + value).attr("checked", true);
        }
        $(".chkFeesType").each(function () {
            if ($(this).attr("for") == "chkTransaction") {
                if ($(this).find("[type='checkbox']").is(":checked")) {
                    $(".volume").show();
                }
            }
        });
    }
});

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