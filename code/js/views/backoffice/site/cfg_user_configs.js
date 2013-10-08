var userId;

$(document).ready(function () {

    userId = $('#userId').val();

    LoadUserPages(userId);
    LoadFavorites(userId);
    LoadDefaultPage(userId);

    /************************************* default page ctrls *************************************/
    // set page as default frontoffice page
    $('#default_page').on("click", "input", function () {

        var row = $(this).parents('tr');
        var pageName = row.find("td:eq(0)").find('input').val();

        SetDefaultPage(userId, pageName);
    });

    // "unselect" page as default frontoffice page
    $('#remove_selected_page').click(function () {
        SetDefaultPage(userId, '');
    });

    /************************************* favorites ctrls *************************************/
    // add page to favorites
    $('#available_pages').on("click", "input", function () {

        var row = $(this).parents('tr');
        var pageName = row.find("td:eq(0)").find('input').val();

        AddPageToFavorites(userId, pageName);
    });

    // remove page from favorites
    $('#favorite_pages').on("click", "input", function () {

        var row = $(this).parents('tr');
        var pageName = row.find("td:eq(2)").find('input').val();

        RemovePageFromFavorites(userId, pageName);
    });

    /************************************* password ctrl *************************************/
    $('#change_pass').click(function () {
        ChangePassword(userId, $('#new_pass').val());
    });
});

// load user pages to available_pages and default_page
function LoadUserPages(userId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/ListAllUserPages",
        data: "{'userId':'" + userId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            var availablePages = $('#available_pages > table');
            var defaultPages = $('#default_page > table');

            availablePages.html("");
            defaultPages.html("");

            if (jsonData.length > 0) {

                var aptbody = "";
                var dptbody = "";

                for (var i = 0; i < jsonData.length; i++) {

                    aptbody += "<tr>";
                    aptbody += "    <td><input type='hidden' value='" + jsonData[i].Name + "'/></td>";
                    aptbody += "    <td style='text-align:left;'>" + jsonData[i].Title + "</td>";
                    aptbody += "    <td><input type='button' class='button' value='add'/></td>";
                    aptbody += "</tr>";

                    dptbody += "<tr>";
                    dptbody += "    <td><input type='hidden' value='" + jsonData[i].Name + "'/></td>";
                    dptbody += "    <td style='text-align:left;'>" + jsonData[i].Title + "</td>";
                    dptbody += "    <td><input type='button' class='button' value='select'/></td>";
                    dptbody += "</tr>";
                }

                availablePages.append(aptbody);
                defaultPages.append(dptbody);
            }
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

/***********************************************************************/
// load favorite list of pages
function LoadFavorites(userId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetUserFavorites",
        data: "{'userId':'" + userId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            var favorites = $('#favorite_pages > table');

            favorites.html("");

            if (jsonData.length > 0) {

                var tbody = "";

                for (var i = 0; i < jsonData.length; i++) {

                    tbody += "<tr>";
                    tbody += "    <td><input type='button' class='button' value='remove'/></td>";
                    tbody += "    <td style='text-align:left;'>" + jsonData[i].Title + "</td>";
                    tbody += "    <td><input type='hidden' value='" + jsonData[i].Name + "'/></td>";
                    tbody += "</tr>";
                }

                favorites.append(tbody);
            }
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

function AddPageToFavorites(userId, pageName) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/AddUserFavoritePage",
        data: "{'userId':'" + userId + "', 'pageName':'" + pageName + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            LoadFavorites(userId);

            alert(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// remove one page from favorites
function RemovePageFromFavorites(userId, pageName) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/RemoveUserFavoritePage",
        data: "{'userId':'" + userId + "', 'pageName':'" + pageName + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            LoadFavorites(userId);

            alert(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

/***********************************************************************/
// load default pages
function LoadDefaultPage(userId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/GetUserDefaultPage",
        data: "{'userId':'" + userId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            if (data.d == '') {
                $('#selected_page_name').val('');
                $('#selected_page_title').html('');
            } else {
                var jsonData = $.parseJSON(data.d);

                $('#selected_page_name').val(jsonData.Name);
                $('#selected_page_title').html(jsonData.Title);
            }

        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });

}

// change de fault page
function SetDefaultPage(userId, pageName) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/SetUserDefaultFrontOfficePage",
        data: "{'userId':'" + userId + "', 'pageName':'" + pageName + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            LoadDefaultPage(userId);
            
            alert(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

/***********************************************************************/
// get the new password from textbox and then change it
function ChangePassword(userId, newPass) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/ChangeUserPassword",
        data: "{'userId':'" + userId + "', 'newPass':'" + newPass + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}