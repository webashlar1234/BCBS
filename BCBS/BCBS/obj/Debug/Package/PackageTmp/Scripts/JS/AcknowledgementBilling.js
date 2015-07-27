var currentProjectIndex = 0;
var projects = [];
var removedProjects = [];
var removedServices = [];
var serviceFeesType = [];
var selectedFeesType;
var addServiceinProject;
$(document).ready(function () {
    $("#formsitems").toggle();
    $(".menuBAF").addClass("menuactive");
    $("#deletecustomer").hide();
    //bindmulitiselectService();
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
    $("#CustomerId").change(function () {
        if ($(this).val() > 0) {
            $("#deletecustomer").show();
        }
        else {
            if (!projects.length > 0) {
                $("#deletecustomer").hide();
            }
        }
    });
    $("#Project").change(function () {
        $("#FeesType").multiselect("clearSelection");
        $("#FeesType").multiselect("destroy");
        $(".feestypeddl").hide();
        bindServiceTypeddl($(this).val());
    });
    $('#Service').change(function () {
        if ($(".projectddl").is(":hidden")) {
            if ($(this).val() > 0) {
                bindFeesTypeddl($(this).val());
                var projectid = $("#Project").val();
                var serviceid = $(this).val();
                $(".feestypeddl").show();
                var projectin = addServiceinProject.split("_")[1];
                $("#tblProject_" + projectin + "_" + projectid).find(".ServiceFeesType." + serviceid).each(function (i, data) {
                    $("#FeesType").find("Option").each(function (optionindex, optiondata) {
                        if ($(optiondata).val() == $(data).html() && $(data).parents(".parentServiceDiv").is(":hidden") != true) {
                            $(optiondata).attr("disabled", "disaabled");
                        }
                    });
                    $("#FeesType").multiselect("clearSelection");
                    $("#FeesType").multiselect("destroy");
                    $("#FeesType").multiselect();
                });
            }
        }
        else {
            $('#Service').valid();
            if ($(this).val() > 0) {
                bindFeesTypeddl($(this).val());
                $(".feestypeddl").show();
            }
            else {
                $("#FeesType").multiselect("clearSelection");
                $("#FeesType").multiselect("destroy");
                $(".feestypeddl").hide();
            }
        }
    });
    $("#FeesType").change(function () {
        $('#FeesType').valid();
        selectedFeesType = $(this).val();
    });
    $("#btnAddProject").click(function () {
        if ($("#Project").valid() && $("#Service").valid() && $("#FeesType").valid()) {
            var projectid = $("#Project").val();
            var services = $("#Service").val();
            var isProjectHidden = $("#Project").is(":hidden");
            $('#myModal').modal('hide');
            $.ajax({
                url: baseURL + "/Customer/GenerateProjectServiceList",
                data: { projectId: projectid, serviceIds: services },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (resultdata) {
                    if (resultdata != null) {
                        if (isProjectHidden) {
                            AddProjectServiceTemplate(projectid, resultdata);
                        }
                        else {
                            GenereateProjectTemplate(resultdata);
                        }

                    }
                }
            });
        }
    });
    $('#myModal').on('hidden.bs.modal', function () {
        $(".projectddl").show();
        $("#Service").html("<option value=''>--Select Service--</option>");
        $("#FeesType").multiselect("clearSelection");
        $("#Project").val("");
        $(".has-error").removeClass("has-error");
        $(".has-feedback").removeClass("has-feedback");
        $("label.error").remove();
        $(".error").removeClass("error");
        $(".feestypeddl").hide();
    });
    $(document).on("click", ".removeProject", function () {
        $(this).parents(".parentDiv").hide();
        removedProjects.push($(this).attr("id").split('_')[2] + "@" + $(this).attr("id").split('_')[1]);
        $("#RemovedProjects").val(removedProjects.join(","));
        projects.splice($.inArray(parseInt($(this).attr("id").split('_')[2]), projects), 1);
        $("#Project option[value='" + $(this).attr("id").split('_')[2] + "']").show();
        checkforatleastoneproject();
    });
    //$(document).on("change", ".volume", function () {
    //    var volume = $(this).val();
    //    var projectindex = $(this).attr("id").split("_")[1];
    //    var serviceindex = $(this).attr("id").split("_")[4];
    //    if (volume > 0) {
    //        var budgetValue = $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Budget").val();
    //        //var transactionvolume = $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Volume").val();

    //        var totalamount = ((parseFloat(volume)) * (parseFloat(budgetValue))).toFixed(2);
    //        if (!totalamount > 0) {
    //            totalamount = "";
    //        }
    //        $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Total").val(totalamount);
    //    }
    //    else {
    //        $("#Projects_" + projectindex + "__Services_" + serviceindex + "__Total").val("");
    //    }
    //});
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
    $(document).on("click", ".addProjectService", function () {
        addServiceinProject = $(this).attr("Id");
        var projectId = $(this).attr("Id").split('_')[2];
        $(".projectddl").hide();
        $("#Project").val(projectId);
        bindServiceTypeddl(projectId);
        $('#myModal').modal('show');
    });

});

function bindServiceTypeddl(projectid) {
    $.ajax({
        url: baseURL + "/Contract/GetServiceTypeByProjectID",
        data: { projectId: projectid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            $("#Service").multiselect("clearSelection");
            $("#Service").multiselect("destroy");
            service = $.parseJSON(data);
            var str = "<option value=''>--Select Service--</option>";
            $.each(service, function (index, data) {
                str += "<option value=" + data.Id + ">" + data.Name + "</option>"
            });
            $("#Service").html(str);
            //bindmulitiselectService();
        },
        failure: function (data) {
            service = undefined;
        }
    });
}
function bindFeesTypeddl(serviceid) {
    $.ajax({
        url: baseURL + "/Customer/GetFeesTypeByServiceID",
        data: { serviceId: serviceid },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            $("#FeesType").multiselect("clearSelection");
            $("#FeesType").multiselect("destroy");
            feestype = $.parseJSON(data);
            serviceFeesType = feestype;
            var str;
            $.each(feestype, function (index, data) {
                str += "<option value=" + data.FeesType + ">" + data.FeesType + "</option>"
            });
            $("#FeesType").html(str);
            bindmulitiselectFeestype();
        },
        failure: function (data) {
            feestype = undefined;
        }
    });
}

function bindmulitiselectFeestype() {
    $('#FeesType').multiselect({
        //includeSelectAllOption: true,
        numberDisplayed: 1,
        nonSelectedText: '--Select Service--',
        maxHeight: 300,
    });
    $(".btn-group").addClass("form-inline");
    $(".btn-group").removeClass("btn-group");
}

function GenereateProjectTemplate(projectData) {
    projects.push(projectData[0].ProjectId);
    var str = "<div class='row well parentDiv padd-top-10 noRadius' id='Project_" + projectData[0].ProjectId + "' >";
    str += "<div class='row'>" +
    "<div class='col-lg-12 btn-primary' style='margin-bottom: 10px;'><span style='margin-left: 10px;'>" + projectData[0].ProjectName + "</span><span class='pull-right glyphicon glyphicon-remove removeProject' style='padding-top:2px;' title='Remove project' id='removeProject_" + currentProjectIndex + "_" + projectData[0].ProjectId + "'></span></div>" +
    "<div class='form-inline padd-top-10' style='padding-left: 30px;margin-top: 10px;margin-bottom: 10px;'>" +
    "<div class='form-group'>" +
    "<label class='control-label noRadius' for='Projects_" + currentProjectIndex + "__FromDate' style='margin-right: 20px;'>From Date </label>" +
    "<input class='form-control noRadius projectFromDate_" + currentProjectIndex + "' id='Projects_" + currentProjectIndex + "__FromDate' name='Projects[" + currentProjectIndex + "].FromDate' type='text'>" +
    "</div>" +
    "<div class='form-group' style='margin-left:10px'>" +
    "<label class='control-label noRadius' for='Projects_" + currentProjectIndex + "__EndDate' style='margin-right: 20px;'>End Date </label>" +
    "<input class='form-control noRadius projectToDate_" + currentProjectIndex + "' id='Projects_" + currentProjectIndex + "__EndDate' name='Projects[" + currentProjectIndex + "].EndDate' type='text'>" +
    "</div>" +
    "</div>" +
    "</div>" +
    "<input data-val='true' data-val-number='The field Id must be a number.' data-val-required='The Id field is required.' id='Projects_" + currentProjectIndex + "__Project_Name' name='Projects[" + currentProjectIndex + "].Project.Name' type='hidden' value='" + projectData[0].ProjectName + "'>" +
    "<input data-val='true' data-val-number='The field Id must be a number.' data-val-required='The Id field is required.' id='Projects_" + currentProjectIndex + "__Project_Id' name='Projects[" + currentProjectIndex + "].Project.Id' type='hidden' value='" + projectData[0].ProjectId + "'>";
    str += "<table id='tblProject_" + currentProjectIndex + "_" + projectData[0].ProjectId + "' ><tr>" +
    "<th>Name</th>" +
    "<th>Volume</th>" +
    "<th>Rate</th>" +
    "<th>Total</th>" +
      "<th></th>" +
    "</tr>";
    if (selectedFeesType.length > 0) {

        for (var i = 0 ; i < selectedFeesType.length; i++) {
            var serviceFeesTypeData = serviceFeesType.filter(function (el) { if (el.FeesType == selectedFeesType[i]) { return el }; });

            str += "<tr class='parentServiceDiv'>" +
            "<td>" + projectData[0].ServiceName + "</td>";
            if (selectedFeesType[i] == "Transaction") {

                if (serviceFeesTypeData.length > 0) {
                    str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__NewVolume' name='Projects[" + currentProjectIndex + "].Services[" + i + "].NewVolume' onkeypress = 'return isNumber(event)' class='form-control noRadius volume'  required='required' type='text' value=''></td>";
                    str += "<td>$" + kendo.toString(serviceFeesTypeData[0].Amount, "n") + "  /<span class='ServiceFeesType " + projectData[0].ServiceId + "' id='ServiceFeesType_" + projectData[0].ServiceId + "'>" + serviceFeesTypeData[0].FeesType + "</span></td>";
                    str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Total' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='' required='required'></td>" +
                    "<td><span id='Service_" + currentProjectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span></td>"
                    "</tr>";
                }
            }
            else {
                if (serviceFeesTypeData.length > 0) {
                    str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__NewVolume' name='Projects[" + currentProjectIndex + "].Services[" + i + "].NewVolume' onkeypress = 'return isNumber(event)' class='form-control noRadius' readonly='readonly' type='text' value=''></td>";
                    str += "<td>$" + kendo.toString(serviceFeesTypeData[0].Amount, "n") + "/ <span  class='ServiceFeesType " + projectData[0].ServiceId + "' id='ServiceFeesType_" + projectData[0].ServiceId + "'>" + serviceFeesTypeData[0].FeesType + "</span></td>";
                    str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Total' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='' required='required'></td>" +
                    "<td><span id='Service_" + currentProjectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span></td>"
                    "</tr>";
                }
            }
            //str += "<td><input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Total' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='' required='required'></td>" +
            //      "<td><span id='Service_" + currentProjectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span></td>"
            //"</tr>";
            str += "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Name' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Name' type='hidden' value='" + projectData[0].ServiceName + "'>" +
            "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__FeesType' name='Projects[" + currentProjectIndex + "].Services[" + i + "].FeesType' type='hidden' value='" + serviceFeesTypeData[0].FeesType + "'>" +
            "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Budget' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Budget' type='hidden' value='" + serviceFeesTypeData[0].Amount + "'>" +
            //"<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Volume' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Volume' type='hidden' value='" + projectData[i].Volume + "'>" +
            "<input id='Projects_" + currentProjectIndex + "__Services_" + i + "__Id' name='Projects[" + currentProjectIndex + "].Services[" + i + "].Id' type='hidden' value='" + projectData[0].ServiceId + "'>";
        }
    }
    str += "</table><span class='btn btn-sm btn-primary addProjectService' id='AddService_" + currentProjectIndex + "_" + projectData[0].ProjectId + "' style='margin-left: 15px;'>Add Service</span></div>";
    $("#ProjectService").append(str);
    var checkin = $('.projectFromDate_' + currentProjectIndex).datepicker({
        //"setDate": new Date(datefrom),
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
    var checkout = $('.projectToDate_' + currentProjectIndex).datepicker({
        //"setDate": new Date(datefrom),
        onRender: function (date) {
            return date.valueOf() < checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');
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

function AddProjectServiceTemplate(projectid, projectData) {
    var projectin = addServiceinProject.split("_")[1];
    var lastelement = $("#tblProject_" + projectin + "_" + projectid).find(".dollarsscents").last();
    var lastserviceindex = $(lastelement).attr("id").split('_')[4];
    var projectIndex = $(lastelement).attr("id").split('_')[1];
    var cnt = parseInt(lastserviceindex) + 1 + parseInt(selectedFeesType.length);

    if (selectedFeesType.length > 0) {
        var str;
        var j = 0;
        for (var i = parseInt(lastserviceindex) + 1 ; i < cnt; i++) {
            var serviceFeesTypeData = serviceFeesType.filter(function (el) { if (el.FeesType == selectedFeesType[j]) { return el }; });
            str += "<tr class='parentServiceDiv'>" +
            "<td>" + projectData[0].ServiceName + "</td>";
            if (selectedFeesType[j] == "Transaction") {

                if (serviceFeesTypeData.length > 0) {
                    str += "<td><input id='Projects_" + projectIndex + "__Services_" + i + "__NewVolume' name='Projects[" + projectIndex + "].Services[" + i + "].NewVolume' onkeypress = 'return isNumber(event)' class='form-control noRadius volume'  required='required' type='text' value=''></td>";
                    str += "<td>$" + serviceFeesTypeData[0].Amount + "  /<span class='ServiceFeesType " + projectData[0].ServiceId + "' id='ServiceFeesType_" + projectData[0].ServiceId + "'>"+ serviceFeesTypeData[0].FeesType + "</span></td>";
                    str += "<td><input id='Projects_" + projectIndex + "__Services_" + i + "__Total' name='Projects[" + projectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='' required='required'></td>" +
                    "<td><span id='Service_" + projectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span></td>"
                    "</tr>";
                }

            }
            else {
                if (serviceFeesTypeData.length > 0) {
                    str += "<td><input id='Projects_" + projectIndex + "__Services_" + i + "__NewVolume' name='Projects[" + projectIndex + "].Services[" + i + "].NewVolume' onkeypress = 'return isNumber(event)' class='form-control noRadius' readonly='readonly' type='text' value=''></td>";
                    str += "<td>$" + serviceFeesTypeData[0].Amount + "/ <span  class='ServiceFeesType " + projectData[0].ServiceId + "' id='ServiceFeesType_" + projectData[0].ServiceId + "'>" + serviceFeesTypeData[0].FeesType + "</span></td>";
                    str += "<td><input id='Projects_" + projectIndex + "__Services_" + i + "__Total' name='Projects[" + projectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='' required='required'></td>" +
                    "<td><span id='Service_" + projectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span></td>"
                    "</tr>";
                }
            }
            //str += "<td><input id='Projects_" + projectIndex + "__Services_" + i + "__Total' name='Projects[" + projectIndex + "].Services[" + i + "].Total' onkeypress = 'return isNumber(event)'  class='form-control noRadius dollarsscents' type='text' value='' required='required'></td>" +
            //      "<td><span id='Service_" + projectIndex + "_" + i + "' class='glyphicon glyphicon-remove removeService'></span></td>"
            //"</tr>";
            str += "<input id='Projects_" + projectIndex + "__Services_" + i + "__Name' name='Projects[" + projectIndex + "].Services[" + i + "].Name' type='hidden' value='" + projectData[0].ServiceName + "'>" +
            "<input id='Projects_" + projectIndex + "__Services_" + i + "__FeesType' name='Projects[" + projectIndex + "].Services[" + i + "].FeesType' type='hidden' value='" + serviceFeesTypeData[0].FeesType + "'>" +
            "<input id='Projects_" + projectIndex + "__Services_" + i + "__Budget' name='Projects[" + projectIndex + "].Services[" + i + "].Budget' type='hidden' value='" + serviceFeesTypeData[0].Amount + "'>" +
            //"<input id='Projects_" + projectIndex + "__Services_" + i + "__Volume' name='Projects[" + projectIndex + "].Services[" + i + "].Volume' type='hidden' value='" + projectData[i].Volume + "'>" +
            "<input id='Projects_" + projectIndex + "__Services_" + i + "__Id' name='Projects[" + projectIndex + "].Services[" + i + "].Id' type='hidden' value='" + projectData[0].ServiceId + "'>";
            j++;
        }
    }
    $("#tblProject_" + projectin + "_" + projectid).append(str);
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
        $("#btnsubmit").show();
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