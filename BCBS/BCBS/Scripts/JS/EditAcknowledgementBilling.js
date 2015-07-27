var currentProjectIndex = 0;
var projects = [];
var removedProjects = [];
var removedServices = [];
$(document).ready(function () {
    $("#formsitems").toggle();
    $(".menuBAF").addClass("menuactive");
    $("#deletecustomer").hide();
    bindmulitiselectService();
    $("#formack").validate({
        highlight: function (element) {
            //$(element).closest('.form-group').removeClass('success').addClass('error');
            $(element).parent().children(".glyphicon-exclamation-sign").remove();
            $(element).parent().removeClass("has-error");
            $(element).parent().removeClass("has-feedback");
            $(element).removeClass("error");
            $(element).addClass("error");
            //$(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
            $(element).parent().addClass("has-error has-feedback");
            //$(element).addClass("error");
            //if ($(element).attr("id") != "Service") {
            //    $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
            //}
            //$(element).parent().addClass("has-error has-feedback");
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
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only up to two decimal places allowed.");
    jQuery.validator.addClassRules("dollarsscents", {
        min: 0,
        max: 99999999.99,
        number: true,
        dollarsscents: true,
    });
    $('#frommodal').validate({
        rules: {
            Service: {
                required: true,
                //regex: "/^\d+(?:\.\d\d?)?$/",
            },
            Project: {
                required: true,
            },
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") === 'Service') {
                error.insertAfter(element.next());
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
            //$(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
            $(element).parent().addClass("has-error has-feedback");
            //$(element).addClass("error");
            //if ($(element).attr("id") != "Service") {
            //    $(element).after("<span class='glyphicon glyphicon-exclamation-sign form-control-feedback'></span>");
            //}
            //$(element).parent().addClass("has-error has-feedback");
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
    var projects = $("#SelectedProjects").val();
    var services = $("#SelectedServices").val();
    projects = $.parseJSON($("#SelectedProjects").val());
    services = $.parseJSON($("#SelectedServices").val());
    $.each(projects, function (index, projectdata) {
        if (projectdata != null) {
            var projectservices = services.filter(function (el) { if (el.ProjectId == projectdata) { return el } });
            var selectedServices = [];
            $.each(projectservices, function (i, data) {
                selectedServices.push(data.ServiceId);
            });
            if (selectedServices.length > 0) {

                $.ajax({
                    url: baseURL + "/Customer/GetAcknowledgementProjectServiceList",
                    data: { projectId: projectdata, serviceIds: selectedServices.join(","), ackId: $("#Id").val() },
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (resultdata) {
                        if (resultdata != null) {
                            GenereateProjectTemplate(resultdata, projectservices);
                        }
                    }
                });
            }
        }
    });


    $("#CustomerId").change(function () {
        $(this).val($("#customer").val());
        return false;
        //if ($(this).val() > 0) {
           
        //    //$("#deletecustomer").show();
        //}
        //else {
        //    if (!projects.length > 0) {
        //        $("#deletecustomer").hide();
        //    }
        //}
    });
    $('#Service').change(function () {
        $('#Service').valid();
    });
    $("#btnAddProject").click(function () {
        if ($("#Project").valid() && $("#Service").valid()) {
            var projectid = $("#Project").val();
            var services = $("#Service").val();
            $('#myModal').modal('hide');
            $.ajax({
                url: baseURL + "/Customer/GenerateProjectServiceList",
                data: { projectId: projectid, serviceIds: services.join(",") },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (resultdata) {
                    if (resultdata != null) {
                        GenereateProjectTemplate(resultdata);
                    }
                }
            });
        }
    });
    $('#myModal').on('hidden.bs.modal', function () {
        $("#Service").multiselect("clearSelection");
        $("#Project").val("");
        $(".has-error").removeClass("has-error");
        $(".has-feedback").removeClass("has-feedback");
        $("label.error").remove();
        $(".error").removeClass("error");
    });
    $(document).on("click", ".removeProject", function () {
        $(this).parents(".parentDiv").hide();
        removedProjects.push($(this).attr("id").split('_')[2] + "@" + $(this).attr("id").split('_')[1]);
        $("#RemovedProjects").val(removedProjects.join(","));
        projects.splice($.inArray(parseInt($(this).attr("id").split('_')[2]), projects), 1);
        $("#Project option[value='" + $(this).attr("id").split('_')[2] + "']").show();
        checkforatleastoneproject();
    });
    $(document).on("change", ".volume", function () {
        var volume = $(this).val();
        var projectindex = $(this).attr("id").split("_")[1];
        var serviceindex = $(this).attr("id").split("_")[4];

        var budgetValue = $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Budget").val();
        var transactionvolume = $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Volume").val();

        var totalamount = ((parseFloat(volume) / parseFloat(transactionvolume)) * (parseFloat(budgetValue))).toFixed(2);
        $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Total").val(totalamount);
    });
    $(document).on("click", ".removeService", function () {
        if ($(this).parents(".parentDiv").find(".parentServiceDiv:visible").length > 1) {
            $(this).parents(".parentServiceDiv").hide();
            removedServices.push($(this).attr("id").split('_')[1] + "_" + $(this).attr("id").split('_')[2]);
            $("#RemovedServices").val(removedServices.join(","));
        }
        else {
            $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                   "<strong>Error!</strong> Atleast one service required in a project..!! " +
                   "</div>");
            $(".alert").show();
            $(".alert").delay(4000).slideUp(200, function () {
                $(this).alert('close');
            });
        }
    });
});
function BindProjectsbyIds() {

}
function bindmulitiselectService() {
    $('#Service').multiselect({
        //includeSelectAllOption: true,
        numberDisplayed: 1,
        nonSelectedText: '--Select Service--',
        maxHeight: 300,
    });
    $(".btn-group").addClass("form-inline");
    $(".btn-group").removeClass("btn-group");
}

function GenereateProjectTemplate(projectData, projectservices) {
    projects.push(projectData[0].ProjectId);
    var str = "<div class='row well parentDiv padd-top-10' >";
    str += "<div class='row'>" +
        //<span class='pull-right glyphicon glyphicon-remove removeProject' style='padding-top:2px;' title='Remove project' id='removeProject_" + currentProjectIndex + "_" + projectData[0].ProjectId + "'></span>
    "<div class='col-lg-12 btn-primary' style='margin-bottom: 10px;'><span style='margin-left: 10px;'>" + projectData[0].ProjectName + "</span></div>" +
    "<div class='form-inline padd-top-10' style='padding-left: 30px;margin-top: 10px;margin-bottom: 10px;'>" +
    "<div class='form-group'>" +
    "<label class='control-label noRadius' for='Projects_" + currentProjectIndex + "__FromDate' style='margin-right: 20px;'>From Date </label>" +
    "<input  class='form-control noRadius projectFromDate_" + currentProjectIndex + "' id='Projects_" + currentProjectIndex + "__FromDate' name='Projects[" + currentProjectIndex + "].FromDate' type='text'  readonly='readonly' >" +
    "</div>" +
    "<div class='form-group' style='margin-left:10px'>" +
    "<label class='control-label noRadius' for='Projects_" + currentProjectIndex + "__EndDate' style='margin-right: 20px;'>End Date </label>" +
    "<input class='form-control noRadius projectToDate_" + currentProjectIndex + "' id='Projects_" + currentProjectIndex + "__EndDate' name='Projects[" + currentProjectIndex + "].EndDate' type='text'  readonly='readonly'>" +
    "</div>" +
    "</div>" +
    "</div>" +
    "<input data-val='true' data-val-number='The field Id must be a number.' data-val-required='The Id field is required.' id='Projects_" + currentProjectIndex + "__Project_Name' name='Projects[" + currentProjectIndex + "].Project.Name' type='hidden' value='" + projectData[0].ProjectName + "'>" +
    "<input data-val='true' data-val-number='The field Id must be a number.' data-val-required='The Id field is required.' id='Projects_" + currentProjectIndex + "__Project_Id' name='Projects[" + currentProjectIndex + "].Project.Id' type='hidden' value='" + projectData[0].ProjectId + "'>";
    str += "<table><tr>" +
    "<th>Name</th>" +
    "<th>Volume</th>" +
    "<th>Rate</th>" +
    "<th>Total</th>" +
      "<th></th>" +
    "</tr>";
    for (var i = 0 ; i < projectData.length; i++) {
        str += "<tr class='parentServiceDiv'>" +

        "<td>" + projectData[i].ServiceName + "</td>";

        if (projectData[i].FeesType == "Transaction") {
            str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__NewVolume' name='Projects[" + currentProjectIndex + "].Services[" + i + "].NewVolume' class='form-control noRadius volume'  required='required' type='text' value='" + projectservices[i].Volume + "'  readonly='readonly'></td>";
            str += "<td>$" + kendo.toString(projectservices[i].Total, "n") + "  @ <span id='transaction_" + currentProjectIndex + "_" + i + "'>" + projectData[i].Volume + "</td>";
        }
        else {
            str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__NewVolume' name='Projects[" + currentProjectIndex + "].Services[" + i + "].NewVolume' class='form-control noRadius' readonly='readonly' type='text' value=''></td>";
            str += "<td>$" + kendo.toString(projectData[i].Budget, "n") + "/" + projectData[i].FeesType + "</td>";
        }
        str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Total' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='" + projectservices[i].Total + "' required='required'  readonly='readonly'></td>" +
              "<td></td>"+ //<span id='Service_" + currentProjectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span>
        "</tr>";
        str += "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Name' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Name' type='hidden' value='" + projectData[i].ServiceName + "'>" +
        "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__FeesType' name='Projects[" + currentProjectIndex + "].Services[" + i + "].FeesType' type='hidden' value='" + projectData[i].FeesType + "'>" +
        "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Budget' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Budget' type='hidden' value='" + projectData[i].Budget + "'>" +
        "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Volume' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Volume' type='hidden' value='" + projectData[i].Volume + "'>" +
        "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Id' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Id' type='hidden' value='" + projectData[i].ServiceId + "'>";
    }
    str += "</table></div>";
    $("#ProjectService").append(str);
    var checkin = $('.projectFromDate_' + currentProjectIndex).datepicker({
        "setDate": new Date(projectservices[0].FromDate),
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
            newDate.setDate(newDate.getDate());
            checkout.setValue(newDate);
        }
        else {
            var newDate = new Date(ev.date)
            newDate.setDate(newDate.getDate());
            checkout.setValue(checkout.date);
        }
        checkin.hide();
        //$("#EndDate").datepicker('update');
        //$('#EndDate')[0].focus();
    }).data('datepicker');
    checkin.setValue(new Date(projectservices[0].FromDate));
    var checkout = $('.projectToDate_' + currentProjectIndex).datepicker({
        "setDate": new Date(projectservices[0].EndDate),
        onRender: function (date) {
            return date.valueOf() < checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).on('show', function (ev, date) {
        if ($("#Id").val() > 0) {
            checkout.hide();
        }
    }).data('datepicker');
    checkout.setValue(new Date(projectservices[0].EndDate));
    currentProjectIndex++;
    hideSlectedProjectOptions();
    checkforatleastoneproject();

    //"<div class='row'>" +
    //"<input data-val='true' data-val-number='The field Id must be a number.' data-val-required='The Id field is required.' id='Projects_0__Services_1__Id' name='Projects[0].Services[1].Id' type='hidden' value='9'>" +
    //"<div class='col-lg-3'>Service101</div>" +
    //"<div class='col-lg-3'><input id='Projects_0__Services_0__Volume' name='Projects[0].Services[0].Volume' readonly='readonly' type='text' value=''></div>" +
    //"<div class='col-lg-3'>100 / OnceOnly</div>" +
    //"<div class='col-lg-3'><input id='Total' name='Total' type='text' value=''></div>" +
    //"</div>" +

    //"<div class='row'>" +
    //"<div class='col-lg-3'>Service3</div>" +
    //"<div class='col-lg-3'><input id='Projects_0__Services_1__Volume' name='Projects[0].Services[1].Volume' readonly='readonly' type='text' value=''></div>" +
    //"<div class='col-lg-3'>100 / Monthly</div>" +
    //"<div class='col-lg-3'><input id='Total' name='Total' type='text' value=''></div>" +
    //"</div>"
}
function hideSlectedProjectOptions() {
    if (projects != null) {
        if (projects.length > 0) {
            $.each(projects, function (index, data) {
                $("#Project option[value='" + data + "']").hide();
            });
        }
    }
}
function checkforatleastoneproject() {
    if (projects.length > 0) {
        if ($("#Status").val() != "Approved") {
            $("#btnsubmit").show();
        }
    }
    else {
        $("#btnsubmit").hide();
        $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                   "<strong>Error!</strong>No Projects available..!!" +
                   "</div>");
        $(".alert").show();
        $(".alert").delay(4000).slideUp(200, function () {
            $(this).alert('close');
        });
    }

}