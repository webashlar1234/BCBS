$(document).ready(function () {
    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });
    $(window).resize(function () {
        if ($("#wrapper").hasClass("toggled"))
        {
            $("#wrapper").removeClass("toggled");
        }
    });
    $("#menusetting").click(function () {
        $("#settingitems").toggle("200");
    });
    $("#menuforms").click(function () {
        $("#formsitems").toggle("200");
    });
    $("#menureports").click(function () {
        $("#reportsitem").toggle("200");
    });

    $(document).on({
        ajaxStart: function (e) {
            showloadermessage();
        },
        ajaxStop: function (e) {
            hideloadermessage();
        },
        ajaxComplete: function (e) {
            hideloadermessage();
        }
    });
});
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        if (charCode != 46) {
            return false;
        }
    }
    return true;
}
function showloadermessage() {
    $("#LoaderMessage").html("Loading...");
    $("#LoaderMessage").show();
}
function hideloadermessage() {
    $("#LoaderMessage").html();
    $("#LoaderMessage").hide();
}