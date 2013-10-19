<%@ Page Language="C#" AutoEventWireup="true" CodeFile="table_frame_view.aspx.cs" Inherits="bo_pages_page_frame_table_frame_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <!-- JQuery -->
    <script src="../../../js/jquery-1.9.0.js"></script>
    
    <!-- Site -->
    <script src="../../../js/generic.js"></script>

    <script type="text/javascript">
        
        var _tableArray = new Array();
        _tableArray[0] = new Array();

        $(document).ready(function () {

            LoadFrames(GetURLParameter("pageid"));

            // frame click event
            $('body').on("click", 'div', function () {

                var frameId = $(this).attr('id');
                var frame = $(this).parent('td');

                //if (frame.css('border') == '1px solid rgb(0, 0, 0)') {
                if (frame.css('border-left-width') == '1px') { // the value had to be changed because frame.css('border')  does not work in firefox and ie8 well I am FK because probably i have more errors like this OMG

                    SelectFrame(frameId); // we dont need to validate, because if page save it will be refreshed.

                    ClearAllSelection();

                    frame.css('border', '4px solid black');

                } else {

                    ClearAllSelection();
                }
            });
        });

        // class table objects
        function TableFrame(id, type, title, x, y, width, height, colspan, rowspan, isactive) {

            var _id = id;
            var _type = type;
            var _title = title;
            var _x = x;
            var _y = y;
            var _width = width;
            var _height = height;
            var _colspan = colspan;
            var _rowspan = rowspan;
            var _isactive = isactive;

            this.Id = function () {
                return _id;
            };

            this.Type = function () {
                return _type;
            };

            this.Title = function () {
                return _title;
            };

            this.X = function () {
                return _x;
            };

            this.Y = function () {
                return _y;
            };

            this.Width = function () {
                return _width;
            };

            this.Height = function () {
                return _height;
            };

            this.Colspan = function () {
                return _colspan;
            };

            this.Rowspan = function () {
                return _rowspan;
            };
            
            this.IsActive = function () {
                return _isactive;
            };
        }

        function BuildTableArray(tbFrame) {

            var x = tbFrame.X();
            var y = tbFrame.Y();

            // move to y pos
            while (_tableArray.length <= y) {
                _tableArray[_tableArray.length] = new Array();
            }

            _tableArray[y][x] = tbFrame;
        }

        function BuildHtmlTable() {

            var tb ="<table>";

            for (var i = 0; i < _tableArray.length; i++) {

                tb += "<tr>";

                for (var a = 0; a < _tableArray[i].length; a++) {

                    if (_tableArray[i][a] != undefined) {

                        var auxColspan = _tableArray[i][a].Colspan() > 1 ? " colspan='" + _tableArray[i][a].Colspan() + "' " : "";
                        var auxRowspan = _tableArray[i][a].Rowspan() > 1 ? " rowspan='" + _tableArray[i][a].Rowspan() + "' " : "";

                        var auxFrame = "<div id='" + _tableArray[i][a].Id() + "' >" +
                                                     _tableArray[i][a].Type() + "<br>" +
                                                     _tableArray[i][a].Title() + "<br>" +
                                                     _tableArray[i][a].X() + ", " +
                                                     _tableArray[i][a].Y() + "</div>";

                        var frameStatus = _tableArray[i][a].IsActive() == 1 ? "class='active'" : "class='inactive'";

                        tb += "<td" + auxColspan + "" + auxRowspan + " " + frameStatus + " >";
                        tb += auxFrame;
                        tb += "</td>";

                        

                    } else {
                        tb += "<td></td>";
                    }

                }

                tb += "</tr>";
            }

            tb += "</table>";

            $('body').append(tb);

        }

        function ClearAllSelection() {

            $('div').each(function () {
                $(this).parent('td').css('border', '1px solid black');
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

                        BuildTableArray(new TableFrame(
                                jsonData[i].ID,
                                jsonData[i].FrameType,
                                jsonData[i].Title,
                                jsonData[i].X,
                                jsonData[i].Y,
                                jsonData[i].Width,
                                jsonData[i].Height,
                                jsonData[i].Columnspan,
                                jsonData[i].Rowspan,
                                jsonData[i].IsActive
                            ));
                    }

                    BuildHtmlTable();

                    //ClearAllSelection();
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
            background-color: transparent;
            margin: 0;
            padding: 0;

            margin-left: auto;
            margin-right: auto;
            width: 99%;
        }

        table tr td {
            border: 1px solid #000000;
            font: 10px verdana;
            margin: 0 auto;
            color: whitesmoke;
        }

        div {
            width: 100%;
            height: 75px;
            text-align: center;
        }

        td.active {
            background-color: #336600;
        }

        td.inactive {
            background-color: #500000;
        }
    </style>
    
    <title>table frame</title>
</head>
<body>
    <input id="webServiceKey" type="hidden" runat="server" />
</body>
</html>
