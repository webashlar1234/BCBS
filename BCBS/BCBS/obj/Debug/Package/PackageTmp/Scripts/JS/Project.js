var projectId;
var idstoDelete = [];
var projectGrid;
$(document).ready(function () {
    //artistGrid();\

    // $('#exampleModal').modal({
    //     title: "Confirmation",
    //     autoOpen:false
    //});
   
    $("#settingitems").toggle();
    $(".menuProject").addClass("menuactive");
    $("#deleteproject").hide();
    $(".alert").delay(5000).slideUp(200, function () {
        $(this).alert('close');
    });
    $(".dropdown").click(function () {
        $(this).find(".menu-expand").toggle();
    })
    //$('[data-toggle=offcanvas]').click(function () {
    //    $('.row-offcanvas').toggleClass('active');
    //});
    ProjectGridLoad();
    $('#exampleModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var recipient = button.data('whatever') // Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        var modal = $(this)
        modal.find('.modal-title').text('New message to ' + recipient)
        modal.find('.modal-body input').val(recipient)
    });
    $("#addproject").click(function () {
        window.location.href = baseURL + "/project/new";
    })
    $("#editproject").click(function () {
        if (projectId > 0) {
            window.location.href = baseURL + "/project/edit/" + projectId;
        }
    });
    //$("#btnconfirmdelete").click(function () {
    //    $.ajax({
    //        type: "GET",
    //        url: baseURL + "/Project/DeleteProjectById",
    //        data: { id: projectId },
    //        dataType: "json",
    //        contentType: "application/json; charset=utf-8",
    //        async: false,
    //        success: function (data) {
    //            if (data) {
    //                alert("deleted succees fully");
    //                ProjectGridLoad();
    //            }
    //            else {
    //                alert("deleted failed");
    //            }
    //        }
    //    });
    //});
    $("#btnconfirmdelete").click(function () {

        $('#myModal').modal('hide');
        deleteProjectbyid();
    });
    $(document).on("click", ".chkProject", function () {
        var chkbox = $(this);
        if ($(this).is(":checked")) {
            $(this).parents("tr").addClass("k-state-selected");
            //uncheckallchkbox(chkbox.attr("id"));
            //$(".chkProject").each(function () {
            //    if ($(this).attr("id") != chkbox.attr("id")) {
            //        $(this).attr("checked", false);
            //    }
            //});
        }
        else {
            $(this).parents(".k-state-selected").removeClass("k-state-selected");
            projectId = undefined;
            $("#editproject").hide();
            $("#deleteproject").hide();
        }
        getallSelectedRowId();
        //checkDataSource = $("#ProjectGrid").data("kendoGrid").dataSource;
        //var selected = [];
        //$.each(checkDataSource._data, function (i, data) { selected.push(data.Id); });
        //idstoDelete = selected.join(",");
        //if (idstoDelete.split(',').length > 1) {
        //    $("#editproject").hide();
        //    $("#deleteproject").show();
        //}
        //else if (idstoDelete.split(',').length == 1) {
        //    if (idstoDelete == "") {
        //        $("#editproject").hide();
        //        $("#deleteproject").hide();
        //    }
        //    else {
        //        $("#editproject").show();
        //        $("#deleteproject").show();
        //    }
        //}
        //if (chkbox.is(":checked")) {
        //    chkbox.prop('checked', true);
        //    chkbox.attr('checked',true);
        //}

    });
    $(window).resize(function () {
        resizeGrid();
        if ($("#wrapper").hasClass("toggled")) {

            $("#wrapper").removeClass("toggled");
        }
    });
});
function uncheckallchkbox(selectedid) {
    $(".chkProject").each(function () {
        if ($(this).attr("id") != selectedid) {
            $(this).attr("checked", false);
        }
    });
}
function getallSelectedRowId() {
    idstoDelete = [];
    $("#ProjectGrid").find("tr.k-state-selected").each(function (i, data) {
        idstoDelete.push(data.firstChild.textContent);
    });
    if (idstoDelete.length > 1) {
        $("#editproject").hide();
        $("#deleteproject").show();
    }
    else if (idstoDelete.length == 1) {
        if (idstoDelete == "") {
            $("#editproject").hide();
            $("#deleteproject").hide();
        }
        else {
            projectId = idstoDelete[0];
            $("#editproject").show();
            $("#deleteproject").show();
        }
    }
    else {
        $("#editproject").hide();
        $("#deleteproject").hide();
    }
}
function deleteProjectbyid() {
    $.ajax({
        type: "GET",
        url: baseURL + "/Project/DeleteProjectById",
        data: { id: idstoDelete.join(',') },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            if (data == false) {

                $("#message").html("<div class='alert alert-danger fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Error!</strong>project delete failed!! " +
                    "</div>");
            }
            else {
                $("#message").html("<div class='alert alert-success fade in noRadius'><a href='#' class='close' data-dismiss='alert'>&times;</a>" +
                    "<strong>Success!</strong>Project deleted Successfully!! " +
                    "</div>");
            }
            ProjectGridLoad();
            $(".alert").show();
        }
    });
}
function ProjectGridLoad() {
    projectGrid = $('#ProjectGrid').kendoGrid({
        dataSource: GetProjectDetail(),
        //height: '300',
        navigatable: true,
        sortable: true,
        pageable: true,
        resizable: true,
        change: onRowSelectChange,
        columns: [
            //{ template: "<input type='checkbox' class='checkbox' />" },
            {
                field: "Id",
                title: "ID",
                width: 50,
                hidden: true,
            },
            {
                field: "",
                title: "",
                template: "<input class='chkProject' id='#:Id#' type='checkbox'>",
                width: 30,
            },
            {
                field: "Name",
                title: "Name",
            },
            {
                field: "ChargeCode",
                title: "Charge Code",
            },
            {
                field: "HighLevelBudget",
                title: "High Level Budget($)",
                //template: "#=kendo.toString(HighLevelBudget, 'n') # ,
                template: '$#= kendo.toString(HighLevelBudget,"n") #',
            },
            {
                field: "Status",
                title: "Status",
            },
            {
                field: "Description",
                title: "Description",
            },
             {
                 field: "RC",
                 title: "RC",
             },
        ],
        //dataBound: onProjectDatabound,
        selectable: "multiple",
        resizable: true,
        //detailInit: detailInit,
    }).data("kendoGrid");
    //projectGrid.table.on("click", ".checkbox", selectRow);
    resizeGrid();
}

function GetProjectDetail() {
    var source = new kendo.data.DataSource({
        //data: GetProjectListDataSource(),
        autoSync: true,
        transport: {
            read: {
                type: "GET",
                url: baseURL + "/Project/GetProjectList",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async:true,
                cache: false,
            },
        },
        pageSize: 10,
        //requestStart: function () {
        //    $("#ProjectGrid").append("<div id='loader' class='load' style='display: none;'></div>");
        //    $("#ProjectGrid").find("#loader").show();
        //},
        //requestEnd: function () {
        //    $("#ProjectGrid").find("#loader").remove();
        //},
        schema: {
            model: {
                fields: {
                    Id: { type: "int" },
                    Name: { type: "string" },
                    ChargeCode: { type: "string" },
                    HighLevelBudget: { type: "number" },
                    Status: { type: "string" },
                    Description: { type: "string" },
                    RC: { type: "string" }
                }
            }
        },

    });
    return source;
}
function GetProjectListDataSource() {
    var source;
    $.ajax({
        type: "GET",
        url: baseURL + "/Project/GetProjectList",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (data) {
            source = data;
        },
        failure: function (data) {
            console.log(data);
        }
    });
    return source;
}
function onRowSelectChange(arg) {
    var grid = arg.sender;
    var currentDataItem = grid.dataItem(this.select());
    if (currentDataItem != undefined) {
        //$("#editproject").show();
        //$("#deleteproject").show();
        projectId = currentDataItem.Id;
        uncheckallchkbox(projectId);
        //$(".chkProject").each(function () {
        //    if ($(this).attr("id") != projectId) {
        //        $(this).attr("checked", true);
        //    }
        //});
    }
    getallSelectedRowId();
}
function resizeGrid() {
    var docheight = $(window).height();
    var gridElement = $("#ProjectGrid"),
        dataArea = gridElement.find(".k-grid-content"),
        gridHeight = gridElement.innerHeight(),
        otherElements = gridElement.children().not(".k-grid-content"),
        otherElementsHeight = 200;
    otherElements.each(function () {
        otherElementsHeight += $(this).outerHeight();
    });
    dataArea.height(docheight - otherElementsHeight);
}