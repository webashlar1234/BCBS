var serviceId;
var idstoDelete = [];
$(document).ready(function () {
    $("#settingitems").toggle();
    $(".menuServiceTypes").addClass("menuactive");
    jQuery.validator.addMethod("dollarsscents", function (value, element) {
        return this.optional(element) || /^[0-9]{1,11}(?:\.[0-9]{1,2})?$/i.test(value);
    }, "Only two decimal places allowed");
    //artistGrid();\

    // $('#exampleModal').modal({
    //     title: "Confirmation",
    //     autoOpen:false
    //});
    $("#deleteservice").hide();
    $(".alert").delay(5000).slideUp(200, function () {
        $(this).alert('close');
    });
    $(".dropdown").click(function () {
        $(this).find(".menu-expand").toggle();
    })
    //$('[data-toggle=offcanvas]').click(function () {
    //    $('.row-offcanvas').toggleClass('active');
    //});
    ServiceGridLoad();
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-title').text('New message to ' + recipient)
        modal.find('.modal-body input').val(recipient)
    });
    $("#addservice").click(function () {
        window.location.href = baseURL + "/service/new";
    })
    $("#editservice").click(function () {
        if (serviceId > 0) {
            window.location.href = baseURL + "/service/edit/" + serviceId;
        }
    });
    //$("#btnconfirmdelete").click(function () {
    //    $.ajax({
    //        type: "GET",
    //        url: baseURL + "/Service/DeleteServiceById",
    //        data: { id: serviceId },
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8",
    //        async: false,
    //        success: function (data) {
    //            if (data) {
    //                alert("deleted succees fully");
    //                ServiceGridLoad();
    //            }
    //            else {
    //                alert("deleted failed");
    //            }
    //        }
    //    });
    //});
    $("#btnconfirmdelete").click(function () {
        $('#myModal').modal('hide');
        deleteServicebyid();
    });
    $(document).on("click", ".chkService", function () {
        var chkbox = $(this);
        if ($(this).is(":checked")) {
            $(this).parents("tr").addClass("k-state-selected");
            //uncheckallchkbox(chkbox.attr("id"));
            //$(".chkService").each(function () {
            //    if ($(this).attr("id") != chkbox.attr("id")) {
            //        $(this).attr("checked", false);
            //    }
            //});
        }
        else {
            $(this).parents(".k-state-selected").removeClass("k-state-selected");
            serviceId = undefined;
            $("#editservice").hide();
            $("#deleteservice").hide();
        }
        //if (chkbox.is(":checked")) {
        //    chkbox.prop('checked', true);
        //    chkbox.attr('checked',true);
        //}
        getallSelectedRowId();
    });
    $(window).resize(function () {
        resizeGrid();
        if ($("#wrapper").hasClass("toggled")) {

            $("#wrapper").removeClass("toggled");
        }
    });
});
function uncheckallchkbox(selectedid) {
    $(".chkService").each(function () {
        if ($(this).attr("id") != selectedid) {
            $(this).attr("checked", false);
        }
    });
}
function deleteServicebyid() {
    $.ajax({
        type: "GET",
        url: baseURL + "/Service/DeleteServiceById",
        data: { id: idstoDelete.join(',') },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            if (data == false) {

                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>service delete failed!! " +
                    "</div>");
            }
            else {
                $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Success!</strong>Service deleted Successfully!! " +
                    "</div>");
            }
            ServiceGridLoad();
            $(".alert").show();
        }
    });
}
function ServiceGridLoad() {
    $('#ServiceGrid').kendoGrid({
        dataSource: GetServiceDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: true,
        resizable: true,
        change: onRowSelectChange,
        columns: [
            {
                field: "Id",
                title: "ID",
                width: 50,
                hidden: true,
            },
            {
                field: "",
                title: "",
                template: "<input class='chkService' id='#:Id#' type='checkbox'>",
                width: 30,
            },
            {
                field: "Name",
                title: "Name",
            },
            {
                field: "ProjectName",
                title: "Project",
            },
            {
                field: "FeesType",
                title: "Fees Type",
            },
            {
                field: "Budget",
                title: "Budget($)",
                template: "<span>$#:Budget# </span>",
            },
            {
                field: "Status",
                title: "Status",
            },
        ],
        //dataBound: onServiceDatabound,
        selectable: "multiple",
        resizable: true,
        //detailInit: detailInit,
    }).data("kendoGrid");
    resizeGrid();
}
function GetServiceDetail() {
    var source = new kendo.data.DataSource({
        //data: GetServiceListDataSource(),
        autoSync: true,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Service/GetServiceList",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: false,
            },
        },
        pageSize: 10,
        //requestStart: function () {
        //    $("#ServiceGrid").append("<div id='loader' class='load' style='display: none;'></div>");
        //    $("#ServiceGrid").find("#loader").show();
        //},
        //requestEnd: function () {
        //    $("#ServiceGrid").find("#loader").remove();
        //},
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    Name: { type: "string" },
                    FeesType: { type: "string" },
                    Budget: { type: "number" },
                    Status: { type: "string" },
                    ProjectName: {type:"string"}
                }
            }
        },

    });
    return source;
}
function GetServiceListDataSource() {
    var source;
    $.ajax({
        type: "GET",
        url: baseURL + "/Service/GetServiceList",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            source = data;
        }
    });
    return source;
}
function onRowSelectChange(arg) {
    var grid = arg.sender;
    var currentDataItem = grid.dataItem(this.select());
    if (currentDataItem != undefined) {
        $("#editservice").show();
        $("#deleteservice").show();
        serviceId = currentDataItem.Id;
        //uncheckallchkbox(serviceId);
        //$(".chkService").each(function () {
        //    if ($(this).attr("id") != serviceId) {
        //        $(this).attr("checked", true);
        //    }
        //});
    }
    getallSelectedRowId();
}
function getallSelectedRowId() {
    idstoDelete = [];
    $("#ServiceGrid").find("tr.k-state-selected").each(function (i, data) {
        idstoDelete.push(data.firstChild.textContent);
    });
    if (idstoDelete.length > 1) {
        $("#editservice").hide();
        $("#deleteservice").show();
    }
    else if (idstoDelete.length == 1) {
        if (idstoDelete == "") {
            $("#editservice").hide();
            $("#deleteservice").hide();
        }
        else {
            serviceId = idstoDelete[0];
            $("#editservice").show();
            $("#deleteservice").show();
        }
    }
    else {
        $("#editservice").hide();
        $("#deleteservice").hide();
    }
}
function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#ServiceGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}