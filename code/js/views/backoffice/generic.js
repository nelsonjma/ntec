// Read css files to show in textares
function GetCssFiles(container, filename, type) {
    $.ajax({
        type: "POST",
        url: "../bo_cmds.asmx/LoadCssFile",
        data: "{'fileName': '" + filename + "', 'cssType': '" + type + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            container.setValue(data.d);
        },
        error: function (e) { alert("Error: Something went wrong " + e.status); }
    });
}

/********************************** Load Code Mirror **********************************/
function LoadCodeMirror(objectId, mode, theme, lineNumbers) {
    return CodeMirror.fromTextArea(document.getElementById(objectId), {
        mode: mode,
        indentWithTabs: true,
        smartIndent: true,
        lineNumbers: lineNumbers,
        matchBrackets: true,
        autofocus: false,
        theme: theme,
    });
}

// Generic method to load text area with code mirror
function LoadCodeMirror(objectId, mode, theme, lineNumber, height) {
    return CodeMirror.fromTextArea(document.getElementById(objectId), {
        mode: mode,
        height: height,
        indentWithTabs: true,
        smartIndent: true,
        lineNumbers: lineNumber,
        matchBrackets: true,
        autofocus: false,
        theme: theme,
    });
}


