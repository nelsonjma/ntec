var opEditor;

$(document).ready(function () {


    InitTextArea(); // load codemirror to options editor

    // Select/Delete trigger for table
    $("#opcontainer").on("click", "input", function () {

        var row = $(this).parents("tr");

        if ($(this).val() == "select") {

            $("#op_id").val(row.find("td:eq(2)").html());
            $("#op_type").val(row.find("td:eq(3)").html());
            $("#op_name").val(row.find("td:eq(4)").html());
            $("#op_url").val(row.find("td:eq(5)").html());

            opEditor.setValue(row.find("textarea").text());

        } else if ($(this).val() == "delete") {
            if (confirm("Realy delete option ?") == true) {
                DeleteOption(row.find("td:eq(2)").html());
            }
            
        }

    });

    // Clear editable textbox and text areas
    $("#btcleanop").click(function () {

        if (confirm("Clear textbox ?")) {
            $("#op_id").val("");
            $("#op_type").val("");
            $("#op_name").val("");
            $("#op_url").val("");
            $("#op_url").val("");

            opEditor.setValue("");
        }

    });

    LoadOptionSelector(); // Load list of available options

});

// Get list of available options from server side
function LoadOptionSelector() {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/ListAvailableOptions",
        data: "{'ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {

            var jsonData = $.parseJSON(data.d);

            $('#opcontainer > table').html("");

            if (jsonData.length > 0) {

                var theader = "";
                theader += "<tr>";
                theader += "    <th></th>";
                theader += "    <th></th>";
                theader += "    <th>Id</th>";
                theader += "    <th>Type</th>";
                theader += "    <th>Name</th>";
                theader += "    <th>Url</th>";
                theader += "    <th>Options</th>";
                theader += "</tr>";

                $('#opcontainer > table').append(theader);

                for (var i = 0; i < jsonData.length; i++) {

                    var tbody = "";

                    tbody += "<tr>";
                    tbody += "    <td><input type='button' value='select'/></td>";
                    tbody += "    <td><input type='button' value='delete'/></td>";
                    tbody += "    <td>" + jsonData[i].ID + "</td>";
                    tbody += "    <td>" + jsonData[i].OBJType + "</td>";
                    tbody += "    <td>" + jsonData[i].Name + "</td>";
                    tbody += "    <td>" + jsonData[i].URL + "</td>";
                    tbody += "    <td><textarea id='optextarea_" + i + "'>" + jsonData[i].Options1 + "</textarea></td>";
                    tbody += "</tr>";

                    $('#opcontainer > table').append(tbody);
                }

                for (var i = 0; i < jsonData.length; i++) {
                    LoadCodeMirror('optextarea_' + i, 'text/x-ntec', 'vibrant-ink', false);
                }

            }
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// Send option id throw ajax to server side to drop option
function DeleteOption(opId) {
    $.ajax({
        type: "POST", contentType: "application/json; charset=utf-8",
        url: "../bo_cmds.asmx/DeleteAvailableOption",
        data: "{'opId': '" + opId + "','ctrl': '" + $("#webServiceKey").val() + "'}",
        success: function (data) {
            alert(data.d);

            LoadOptionSelector(); // reload available options
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

// load codemirror to options editor
function InitTextArea() {
    opEditor = LoadCodeMirror('op_options', 'text/x-ntec', 'vibrant-ink', true);
    opEditor.setSize('100%', (getDocHeight() - 300) + "px");
}