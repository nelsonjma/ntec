// Three Show / Hide
$(document).ready(function () {
    $("#treeCtrl a:first").click(function () {
        var thisObj = $(this);

        if (thisObj.text() == "Hide Tree") {
            thisObj.text("Show Tree");
            $("#left").css("display", "none");
        } else {
            thisObj.text("Hide Tree");
            $("#left").css("display", "table-cell");
        }
    });
});

// Tree Objects Control
$(document).ready(function () {

    RefreshTree(); // get tree objects...

    // stop click event
    $("#tree").on("click", "a.treemasterpage", function (e) {
        e.preventDefault();
    });

    $("#tree").on("click", "a.treepage", function (e) {
        e.preventDefault();
    });

    // open option box
    $("#tree").on("mouseover", "a.treemasterpage", function (e) {

        OpBoxSelect("#treeMasterPageOpBox", "#treePageOpBox", $(this));

        e.preventDefault();
    });

    $("#tree").on("mouseover", "a.treepage", function (e) {

        OpBoxSelect("#treePageOpBox", "#treeMasterPageOpBox", $(this));

        e.preventDefault();
    });

    // option box operations
    $("#treePageOpBox").on("click", "a", function (e) {

        OpBoxClick("#treePageOpBox", $(this));

        e.preventDefault();
    });

    $("#treeMasterPageOpBox").on("click", "a", function (e) {

        OpBoxClick("#treeMasterPageOpBox", $(this));

        e.preventDefault();
    });

    // open option box
    $("#left").mouseleave(function () {

        HideOpBox("#treePageOpBox");
        HideOpBox("#treeMasterPageOpBox");

        e.preventDefault();
    });

});

/*************** Ajax Operations ********************/
// Ajax get tree objects
function RefreshTree() {
    $.ajax({
        type: "POST",
        url: "bo_cmds.asmx/PageTree",
        data: "{'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            var tree = "<ol>";

            var jsonData = $.parseJSON(data.d);

            for (var i = 0; i < jsonData.length; i++) {

                tree += "<li><a class='treemasterpage' href='" + jsonData[i].ObjId + "'>" + jsonData[i].Text + "</a><ol>";

                var subItems = jsonData[i].SubTreeItems;

                for (var a = 0; a < subItems.length; a++) {
                    tree += "<li><a class='treepage' href='" + subItems[a].ObjId + "'>" + subItems[a].Text + "</a></li>";
                }

                tree += "</ol></li>";
            }

            $("#tree").html(tree);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

function DeletePage(pageId) {
    $.ajax({
        type: "POST",
        url: "bo_cmds.asmx/DeletePage",
        data: "{'pageId': '" + pageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            alert(data.d);

            RefreshTree();
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

function ClonePage(pageId) {
    $.ajax({
        type: "POST",
        url: "bo_cmds.asmx/ClonePage",
        data: "{'pageId': '" + pageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            alert(data.d);

            RefreshTree();
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

function DeleteMasterPage(masterPageId) {
    $.ajax({
        type: "POST",
        url: "bo_cmds.asmx/DeleteMasterPage",
        data: "{'masterPageId': '" + masterPageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            alert(data.d);

            RefreshTree();
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}
/***********************************/

// Option Box Control
function OpBox(opBox, x, y, show) {
    $(opBox).css("display", show == true ? "block" : "none");
    $(opBox).css("left", x);
    $(opBox).css("top", y);
}

// Sub Operation Of OpBox
function HideOpBox(opBox) {
    OpBox(opBox, 0, 0, false);
}

// Move the invisible div then show the div in the position that its needed
function OpBoxSelect(opBoxToShow, opBoxToHide, aObjSelected) {

    $(opBoxToShow + " input:first").val(aObjSelected.attr('href'));

    HideOpBox(opBoxToHide);

    OpBox(
            opBoxToShow,
            aObjSelected.offset().left + parseInt(aObjSelected.css("width")) + 5,
            aObjSelected.offset().top,
            true
        );
}

// Option box click operation
function OpBoxClick(opBox, aObjSelected) {
    var objId = $(opBox + " input:first").val();

    OpBoxExecuteOperation(opBox, aObjSelected, objId);

    HideOpBox(opBox);
}

// Execute the operation
function OpBoxExecuteOperation(opBox, aObjSelected, objId) {

    var aText = aObjSelected.text();

    if (opBox == "#treePageOpBox") {

        if (aText == "Edit Page Frames") {
            OpenFrame("pages/cfg_frame.aspx?pageid=" + objId, "Do you want to edit page frames", false);
        } else if (aText == "Edit Page") {
            OpenFrame("pages/cfg_page.aspx?id=" + objId, "Do you want to edit page", false);
        } else {
            RunWebService("p", aText, true, objId);
        }

    } else if (opBox == "#treeMasterPageOpBox") {

        if (aText == "Edit Master Page") {
            OpenFrame("pages/cfg_master_page.aspx?id=" + objId, "Do you want to edit master page", false);
        } else {
            RunWebService("mp", aText, true, objId);
        }
    }

}

// Hask if you are certain of what you want to do
function OpenFrame(url, checkMsg, refresh) {
    if (confirm(checkMsg) == true) {
        $("#objframe").attr("src", url);
    }
}

// GetInformation from WebService.
function RunWebService(objType, operationType, refresh, objId) {

    if (objType == "p") {

        switch (operationType) {
            case "Delete":
                if (confirm("Delete Page ?") == true) {
                    DeletePage(objId);
                }
                break;
            case "Clone":
                if (confirm("Clone Page ?") == true) {
                    ClonePage(objId);
                }
                break;
            default:
                break;
        }

    } else if (objType == "mp") {

        switch (operationType) {
            case "Delete":
                if (confirm("Delete Master Page ?") == true) {
                    DeleteMasterPage(objId);
                }
                break;
            default:
                break;
        }

    }

    if (refresh == true)
        RefreshTree();
}