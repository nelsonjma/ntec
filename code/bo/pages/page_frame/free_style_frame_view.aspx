<%@ Page Language="C#" AutoEventWireup="true" CodeFile="free_style_frame_view.aspx.cs" Inherits="bo_pages_page_frame_free_style_frame_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <!-- JQuery -->
    <script src="../../../js/jquery-1.9.0.js"></script>
    
    <!-- JQuery Ui -->
    <link rel="stylesheet" href="../../../js/jquery-ui/themes/base/jquery-ui.css" />
    <script src="../../../js/jquery-ui/ui/jquery-ui.js"></script>
    
    <!-- Site -->
    <script src="../../../js/generic.js"></script>
  
    <script type="text/javascript">
        
        $(document).ready(function () {

            LoadFrames(GetURLParameter("pageid"));

            // frame click event
            $('body').on("click", 'div', function () {
                
                var frame = $(this);
                if (frame.draggable("option", "disabled")) {

                    SelectFrame(frame.attr('id')); // we dont need to validate, because if page save it will be refreshed.

                    ClearAllSelection();

                    frame.draggable({ disabled: false });
                } else {

                    frame.draggable({ disabled: true });
                }
            });

        });

        function BuildFrame(id, type, title, y, x, width, height, active) {

            $('body').append("<div id='" + id + "'>" + type + "<br>" + title + "<br>" + x + "," + y + "<br>" + width + "," + height + "</div>");

            $('#' + id).css('width', width + "px");
            $('#' + id).css('height', height + "px");

            $('#' + id).css('top', y + "px");
            $('#' + id).css('left', x + "px");

            $('#' + id).css('background-color', active == 1 ?'greenyellow' : 'lightsalmon');

            $('#' + id).draggable({ disabled: true });
            
            // frame move event, it will update parent method with x, y, and frame id
            $('#' + id).draggable({ drag: function () {
                    var frame = $(this);
                    ChangeCoord(frame.css('left'), frame.css('top'));
            }});
        }

        function ChangeCoord(x, y) {

            window.parent.ChangeCoordOfFrame(x, y);
        }

        function ClearAllSelection() {
            $('div').each(function () {
                $(this).draggable({ disabled: true });
            });
        }

        // load frames from server by webservice
        function LoadFrames(pageId) {
            $.ajax({
                type: "POST", contentType: "application/json; charset=utf-8",
                url: "../../bo_cmds.asmx/ListFrames",
                data: "{'pageId':'" + pageId + "', 'ctrl': '" + $("#webServiceKey").val() + "'}",
                success: function (data) {

                    var jsonData = $.parseJSON(data.d);

                    for (var i = 0; i < jsonData.length; i++) {
                        
                        BuildFrame(
                                jsonData[i].ID,
                                jsonData[i].FrameType,
                                jsonData[i].Title,
                                jsonData[i].Y,
                                jsonData[i].X,
                                jsonData[i].Width,
                                jsonData[i].Height,
                                jsonData[i].IsActive
                            );
                    }

                    ClearAllSelection();
                },
                error: function (e) { alert("Error: Something went wrong " + e.status); }
            });
        }

        // parent methods
        function SelectFrame(id) {

            window.parent.SelectFrame(id);
        }

    </script>

    <style>

         body {
             background-color: grey;
             margin: 0;
             padding: 0;
         }

        div {
            position: absolute;
            font: 10px verdana;
            background-color: white;
        }

        div.dragable {
            border: 1px solid black;
        }

    </style>

    <title>free style frame</title>
</head>
<body>
    <input id="webServiceKey" type="hidden" runat="server" />    
</body>
</html>
