var description;
var user_op_ta;
var available_user_op_ta;
var admin_op_ta;
var availabel_admin_op_ta;

$(document).ready(function () {

    LoadUserSelector(); // Load list of users

    InitEditTextArea(); // Initialise text areas

    GetUserOption(); // load user option

    GetAdminOption(); // load admin option

    // User - Select / Delete trigger
    $("#user_container").on("click", "input", function () {

        var row = $(this).parents("tr");

        if ($(this).val() == "select") {

            $("#uid").val(row.find("td:eq(2)").html());
            $("#uname").val(row.find("td:eq(3)").html());
            $("#upass").val(row.find("td:eq(4)").children("input").val());
            $("#uadmin").prop('checked', row.find("td:eq(5)").children("input").is(':checked'));

            $("#udesc").val(row.find("td:eq(6)").children("textarea").html());

            user_op_ta.setValue(row.find("td:eq(8)").children("textarea").html());
            admin_op_ta.setValue(row.find("td:eq(7)").children("textarea").html());

            LoadUserPages(row.find("td:eq(2)").html()); // load user pages
            LoadAvailablePagesToUser(row.find("td:eq(2)").html()); // load available pages to user

        } else if ($(this).val() == "delete") {
            if (confirm("Realy delete user ?") == true) {
                DeleteUser(row.find("td:eq(2)").html());
            }
        }
    });

    // User Pages - Select / Delete trigger
    $("#availablepages").on("click", "input", function () {

        var row = $(this).parents("tr");

        if ($(this).val() == "add" && confirm("Realy add page")) {

            var userId = $("#uid").val();
            var pageId = row.find("td:eq(1)").html();

            AddPageToUser(userId, pageId);
        }
    });

    // User Pages - Select / Delete trigger
    $("#userpages").on("click", "input", function () {

        var row = $(this).parents("tr");

        if ($(this).val() == "remove" && confirm("Realy add page")) {

            var userId = $("#uid").val();
            var pageId = row.find("td:eq(1)").html();

            RemoveUserPage(userId, pageId);
        }
    });

    // Clear button
    $("#btcleanop").click(function () {
        if (confirm("Realy clear text boxs?")) {

            $("#uid").val("");
            $("#uname").val("");
            $("#upass").val("");
            $("#uadmin").prop('checked', false);
            $("#udesc").val("");

            user_op_ta.setValue("");
            admin_op_ta.setValue("");

            $("#availablepages").children("table").html("");
            $("#userpages").children("table").html("");

        }

    });
    
    // Clone User
    $('#btClone').click(function() {

        var newUserName = prompt("Please enter new user name please.");

        if (newUserName != null) {
            var userId = $('#uid').val();

            CloneUser(userId, newUserName);
        }
    });

});

// load users table
function LoadUserSelector() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/ListUsers",
        data: "{'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            var userContainer = $('#user_container > table');

            userContainer.html("");

            if (jsonData.length > 0) {

                var theader = "";
                theader += "<tr>";
                theader += "    <th></th>";
                theader += "    <th></th>";
                theader += "    <th>Id</th>";
                theader += "    <th>User</th>";
                theader += "    <th>Password</th>";
                theader += "    <th>Is Admin</th>";
                theader += "    <th>Description</th>";
                theader += "    <th>Admin Options</th>";
                theader += "    <th>User Options</th>";
                theader += "</tr>";

                userContainer.append(theader);

                for (var i = 0; i < jsonData.length; i++) {

                    var tbody = "";

                    tbody += "<tr>";
                    tbody += "    <td><input type='button' value='select'/></td>";
                    tbody += "    <td><input type='button' value='delete'/></td>";
                    tbody += "    <td>" + jsonData[i].Id + "</td>";
                    tbody += "    <td>" + jsonData[i].Name + "</td>";
                    tbody += "    <td><input type='password' value='" + jsonData[i].Pass + "' class='passbox'/></td>";
                    tbody += "    <td><input type='checkbox' value='" + jsonData[i].Admin + "' " + (jsonData[i].Admin == "1" ? "checked" : "") + "/></td>";
                    tbody += "    <td><textarea id='select_desc_textarea_" + i + "'>" + jsonData[i].Description + "</textarea></td>";
                    tbody += "    <td><textarea id='select_adminop_textarea_" + i + "'>" + jsonData[i].AdminOptions + "</textarea></td>";
                    tbody += "    <td><textarea id='select_userop_textarea_" + i + "'>" + jsonData[i].UserOptions + "</textarea></td>";
                    tbody += "</tr>";

                    userContainer.append(tbody);

                    LoadCodeMirror('select_desc_textarea_' + i, 'text/x-ntec', 'vibrant-ink', false);
                    LoadCodeMirror('select_adminop_textarea_' + i, 'text/x-ntec', 'vibrant-ink', false);
                    LoadCodeMirror('select_userop_textarea_' + i, 'text/x-ntec', 'vibrant-ink', false);
                }
            }
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

function DeleteUser(userId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/DeleteUser",
        data: "{'userId': '" + userId + "','ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);

            LoadUserSelector(); // reload page
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

// load user options
function GetUserOption() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'user', 'name':'user', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            available_user_op_ta.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

// load admin options
function GetAdminOption() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetAvailableOption",
        data: "{'objType':'user', 'name':'admin', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            availabel_admin_op_ta.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

// load user selected pages
function LoadUserPages(userId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/ListUserPages",
        data: "{'userId':'" + userId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            var userPages = $('#userpages > table');

            userPages.html("");

            if (jsonData.length > 0) {

                var theader = "";
                theader += "<tr>";
                theader += "    <th></th>";
                theader += "    <th>id</th>";
                theader += "    <th>title</th>";
                theader += "</tr>";

                userPages.append(theader);

                for (var i = 0; i < jsonData.length; i++) {

                    var tbody = "";

                    tbody += "<tr>";
                    tbody += "    <td><input type='button' value='remove'/></td>";
                    tbody += "    <td>" + jsonData[i].Id + "</td>";
                    tbody += "    <td>" + jsonData[i].Title + "</td>";
                    tbody += "</tr>";

                    userPages.append(tbody);
                }

            }
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

// load available options table
function LoadAvailablePagesToUser(userId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/ListAvailablePagesToUser",
        data: "{'userId':'" + userId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            var availablePages = $('#availablepages > table');

            availablePages.html("");

            if (jsonData.length > 0) {

                var theader = "";
                theader += "<tr>";
                theader += "    <th></th>";
                theader += "    <th>id</th>";
                theader += "    <th>title</th>";
                theader += "</tr>";

                availablePages.append(theader);

                for (var i = 0; i < jsonData.length; i++) {

                    var tbody = "";

                    tbody += "<tr>";
                    tbody += "    <td><input type='button' value='add'/></td>";
                    tbody += "    <td>" + jsonData[i].Id + "</td>";
                    tbody += "    <td>" + jsonData[i].Title + "</td>";
                    tbody += "</tr>";

                    availablePages.append(tbody);
                }
                
            }
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

// removes user page
function RemoveUserPage(userId, pageId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/DeleteUserPage",
        data: "{'userId':'" + userId + "', 'pageId':'" + pageId + "','ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);

            LoadAvailablePagesToUser(userId);
            LoadUserPages(userId);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// removes user page
function AddPageToUser(userId, pageId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/AddPageToUser",
        data: "{'userId':'" + userId + "', 'pageId':'" + pageId + "','ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);

            LoadAvailablePagesToUser(userId);
            LoadUserPages(userId);

        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// load codemirror to options editor
function InitEditTextArea() {

    var textAreaWidth = (getDocWidth() / 2) - 14;

    $("#udesc").css("width", $("#udesc").parents("div").css("width"));
    $("#udesc").css("height", "75px");

    user_op_ta = LoadCodeMirror('u_options', 'text/x-ntec', 'vibrant-ink', true);
    user_op_ta.setSize(textAreaWidth, '200px');

    available_user_op_ta = LoadCodeMirror('u_available_op', 'text/x-ntec', 'vibrant-ink', false);
    available_user_op_ta.setSize(textAreaWidth, '200px');

    admin_op_ta = LoadCodeMirror('u_admin_op', 'text/x-ntec', 'vibrant-ink', true);
    admin_op_ta.setSize(textAreaWidth, '200px');

    availabel_admin_op_ta = LoadCodeMirror('u_admin_available_op', 'text/x-ntec', 'vibrant-ink', false);
    availabel_admin_op_ta.setSize(textAreaWidth, '200px');
}

function CloneUser(userId, newUserName) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/CloneUser",
        data: "{'userId': '" + userId + "', 'newUserName':'" + newUserName + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);

            LoadUserSelector(); // reload page
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}