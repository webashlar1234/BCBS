var alternateCode = "";
var States = [
    {
        "name": "Alabama",
        "abbreviation": "AL"
    },
    {
        "name": "Alaska",
        "abbreviation": "AK"
    },
    {
        "name": "American Samoa",
        "abbreviation": "AS"
    },
    {
        "name": "Arizona",
        "abbreviation": "AZ"
    },
    {
        "name": "Arkansas",
        "abbreviation": "AR"
    },
    {
        "name": "California",
        "abbreviation": "CA"
    },
    {
        "name": "Colorado",
        "abbreviation": "CO"
    },
    {
        "name": "Connecticut",
        "abbreviation": "CT"
    },
    {
        "name": "Delaware",
        "abbreviation": "DE"
    },
    {
        "name": "District Of Columbia",
        "abbreviation": "DC"
    },
    {
        "name": "Federated States Of Micronesia",
        "abbreviation": "FM"
    },
    {
        "name": "Florida",
        "abbreviation": "FL"
    },
    {
        "name": "Georgia",
        "abbreviation": "GA"
    },
    {
        "name": "Guam",
        "abbreviation": "GU"
    },
    {
        "name": "Hawaii",
        "abbreviation": "HI"
    },
    {
        "name": "Idaho",
        "abbreviation": "ID"
    },
    {
        "name": "Illinois",
        "abbreviation": "IL"
    },
    {
        "name": "Indiana",
        "abbreviation": "IN"
    },
    {
        "name": "Iowa",
        "abbreviation": "IA"
    },
    {
        "name": "Kansas",
        "abbreviation": "KS"
    },
    {
        "name": "Kentucky",
        "abbreviation": "KY"
    },
    {
        "name": "Louisiana",
        "abbreviation": "LA"
    },
    {
        "name": "Maine",
        "abbreviation": "ME"
    },
    {
        "name": "Marshall Islands",
        "abbreviation": "MH"
    },
    {
        "name": "Maryland",
        "abbreviation": "MD"
    },
    {
        "name": "Massachusetts",
        "abbreviation": "MA"
    },
    {
        "name": "Michigan",
        "abbreviation": "MI"
    },
    {
        "name": "Minnesota",
        "abbreviation": "MN"
    },
    {
        "name": "Mississippi",
        "abbreviation": "MS"
    },
    {
        "name": "Missouri",
        "abbreviation": "MO"
    },
    {
        "name": "Montana",
        "abbreviation": "MT"
    },
    {
        "name": "Nebraska",
        "abbreviation": "NE"
    },
    {
        "name": "Nevada",
        "abbreviation": "NV"
    },
    {
        "name": "New Hampshire",
        "abbreviation": "NH"
    },
    {
        "name": "New Jersey",
        "abbreviation": "NJ"
    },
    {
        "name": "New Mexico",
        "abbreviation": "NM"
    },
    {
        "name": "New York",
        "abbreviation": "NY"
    },
    {
        "name": "North Carolina",
        "abbreviation": "NC"
    },
    {
        "name": "North Dakota",
        "abbreviation": "ND"
    },
    {
        "name": "Northern Mariana Islands",
        "abbreviation": "MP"
    },
    {
        "name": "Ohio",
        "abbreviation": "OH"
    },
    {
        "name": "Oklahoma",
        "abbreviation": "OK"
    },
    {
        "name": "Oregon",
        "abbreviation": "OR"
    },
    {
        "name": "Palau",
        "abbreviation": "PW"
    },
    {
        "name": "Pennsylvania",
        "abbreviation": "PA"
    },
    {
        "name": "Puerto Rico",
        "abbreviation": "PR"
    },
    {
        "name": "Rhode Island",
        "abbreviation": "RI"
    },
    {
        "name": "South Carolina",
        "abbreviation": "SC"
    },
    {
        "name": "South Dakota",
        "abbreviation": "SD"
    },
    {
        "name": "Tennessee",
        "abbreviation": "TN"
    },
    {
        "name": "Texas",
        "abbreviation": "TX"
    },
    {
        "name": "Utah",
        "abbreviation": "UT"
    },
    {
        "name": "Vermont",
        "abbreviation": "VT"
    },
    {
        "name": "Virgin Islands",
        "abbreviation": "VI"
    },
    {
        "name": "Virginia",
        "abbreviation": "VA"
    },
    {
        "name": "Washington",
        "abbreviation": "WA"
    },
    {
        "name": "West Virginia",
        "abbreviation": "WV"
    },
    {
        "name": "Wisconsin",
        "abbreviation": "WI"
    },
    {
        "name": "Wyoming",
        "abbreviation": "WY"
    }
];
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
    bindState();
    $("#btnCustomerCancel").click(function () {
        window.location.href = baseURL + "/customer/index";
    });
    if ($("#Id").val() > 0) {
        alternateCode = $("#ChargeCode").val();
        var customerState = $("#hdnState").val();
        $("#State").val(customerState);
    }
    //$("#ChargeCode").keyup(function () {
    //    checkChargeCodeExist($(this).val());
    //});
    //$("#ChargeCode").focusout(function () {
    //    checkChargeCodeExist($(this).val());
    //});
});

function bindState() {
    var str = "<option value=''>--Select State--</option>";
    $.each(States, function (i, data) {
        str += "<option value='" + data.abbreviation + "'>" + data.name + "</option>";
    });
    $("#State").html(str);
}
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