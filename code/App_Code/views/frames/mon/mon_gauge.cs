using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Views.Frames
{
    /// <summary>
    /// Summary description for mon_gauge
    /// </summary>
    public class mon_gauge : mon_general
    {
        //threshold
        private string _greenEnd; // gauge
        private string _redStart;   // gauge

        private string _greenColor; // gauge
        private string _yellowColor;   // gauge
        private string _redColor;   // gauge

        //needle
        private string _needleColor; // gauge
        private string _centerpinColor; // gauge

        // background
        private string _backgroundGradient; // gauge

        // border
        private string _innerBorderColor; // gause
        private string _outerBorderColor; // gause

        // labels
        private string _labelCount; // gauge

        // marks // .Set('tickmarks.big', '20')
        private string _tickmarksSmallNum;
        private string _tickmarksBigNum;
        private string _tickmarksMediumNum;

        private string _tickmarksMediumColor; // gauge

        public mon_gauge()
        {
            MeasureType = string.Empty;

            _greenEnd               = ".Set('green.end', '5')";
            _redStart               = ".Set('red.start', '8')";

            _greenColor             = ".Set('green.color', 'green')";
            _yellowColor            = ".Set('yellow.color', 'yellow')";
            _redColor               = ".Set('red.color', 'red')";

            _needleColor            = ".Set('needle.colors', [RGraph.RadialGradient(mon, 125, 125, 0, 125, 125, 25, 'transparent', '#d66')])";
            _centerpinColor         = ".Set('centerpin.color', 'black')";
            _needleSize             = ".Set('needle.size', 40)";

            _backgroundGradient     = ".Set('background.gradient', false)";

            _borderWidth            = ".Set('border.width', '1')";
            _innerBorderColor       = ".Set('border.inner', 'transparent')";
            _outerBorderColor       = ".Set('border.outer', 'transparent')";

            _labelCount             = ".Set('labels.count', '5')";

            _tickmarksSmallNum      = ".Set('tickmarks.small', '50')";
            _tickmarksBigNum        = ".Set('tickmarks.big', '20')";
            _tickmarksMediumNum     = ".Set('tickmarks.medium', '100')";

            _tickmarksMediumColor   = ".Set('tickmarks.medium.color', 'black')";

            _gutterTop              = ".Set('gutter.top', 8)";
            _gutterBottom           = ".Set('gutter.bottom', 8)";
            _gutterLeft             = ".Set('gutter.left', 8)";
            _gutterRight            = ".Set('gutter.right', 8)";

        }

        /* Private */
        protected override string BuildMonMethod()
        {
            string mon = string.Empty;

            mon += "function Mon() { \n";

            mon += "mon = new RGraph.Gauge('cvs', " + _min + ", " + _max + ", " + _defaultValue + "); \n";
            mon += "mon \n";

            mon += _greenEnd + " \n";
            mon += _redStart + " \n";

            mon += _greenColor + " \n";
            mon += _yellowColor + " \n";
            mon += _redColor + " \n";

            mon += _needleColor + " \n";
            mon += _centerpinColor + " \n";
            mon += _needleSize + " \n";

            mon += _tickmarksSmallNum + " \n";
            mon += _tickmarksBigNum + " \n";
            mon += _tickmarksMediumNum + " \n";

            mon += _tickmarksSmallColor + " \n";
            mon += _tickmarksBigColor + " \n";
            mon += _tickmarksMediumColor + " \n";

            mon += _textSize + " \n";
            mon += _textColor + " \n";
            mon += _labelCount + " \n";

            mon += _shadow + " \n";
            mon += _borderWidth + " \n";
            mon += _innerBorderColor + " \n";
            mon += _outerBorderColor + " \n";

            mon += _backgroundColor + " \n";
            mon += _backgroundGradient + " \n";

            mon += _gutterTop + " \n";
            mon += _gutterBottom + " \n";
            mon += _gutterLeft + " \n";
            mon += _gutterRight + " \n";

            mon += ".Draw(); \n";

            mon += "} \n";

            /*
             gauge = new RGraph.Gauge('cvs', 0, 10, 5);

                        gauge.Set('needle.colors', [RGraph.RadialGradient(gauge, 125, 125, 0, 125, 125, 25, 'transparent', '#d66')])

                            .Set('centerpin.color', 'black')
                            .Set('needle.size', 40)

                            // markers
                            .Set('tickmarks.big.color', 'black')
                            .Set('tickmarks.medium.color', 'black')
                            .Set('tickmarks.small.color', 'black')

                            .Set('tickmarks.big', '10')
                            .Set('tickmarks.medium', '20')
                            .Set('tickmarks.small', '50')

                            // labels
                            .Set('text.color', 'black')
                            .Set('text.size', '8')
                            .Set('labels.count', '5')

                            // border
                            .Set('border.width', '0')
                            .Set('border.outer', 'transparent')
                            .Set('border.inner', 'transparent')
                            .Set('shadow', false)

                            // alarms
                            .Set('green.end', '4')
                            .Set('red.start', '7')

                            .Set('yellow.color', '#d66')
                            .Set('red.color', 'green')
                            .Set('green.color', 'yellow')

                            // background
                            .Set('background.color', 'transparent')
                            .Set('background.gradient', false)

                            // margin
                            .Set('gutter.top', 1)
                            .Set('gutter.bottom', 1)
                            .Set('gutter.left', 1)
                            .Set('gutter.right', 1)

                            .Draw();
            */

            return mon;
        }

        protected override string BuildAjax()
        {
            string ajax = string.Empty;

            string webServiceMethod = MeasureType.ToLower() == "firstcellvalue" ? "FirstCellValue" : "RowCount";

            string crlHash = HashKey;

            string ajaxReadyStr = HttpUtility.UrlEncode(_xmlFileName);

            string ajaxSelectFilter = HttpUtility.UrlEncode(_selectFilter);

            ajax += "\n";
            ajax += "   function GetData() { \n";
            ajax += "\n";
            ajax += "   if (IsPageHidden()) return; \n";

            ajax += "\n";
            ajax += "   UpdatingDataInformation(); ";
            ajax += "\n";

            ajax += "   $.ajax({ \n";
            ajax += "               type: 'POST',  \n";
            ajax += "               url: './mon_service.asmx/" + webServiceMethod + "', \n";
            ajax += "               data: \"{'xmlFileName': '" + ajaxReadyStr + "', 'selectFilter':'" + ajaxSelectFilter + "', 'ctrl': '" + crlHash + "'}\", \n";
            ajax += "               contentType: 'application/json; charset=utf-8', \n";
            ajax += "               dataType: 'json', \n";
            ajax += "               success: function (data) { \n";
            ajax += "\n";
            ajax += "               var jsonData = $.parseJSON(data.d); \n";
            ajax += "               mon.value = jsonData['Data']; \n";
            ajax += "               RGraph.Effects.Gauge.Grow(mon); \n";

            ajax += "\n";
            ajax += "               ConfirmDataInformation(); ";
            ajax += "\n";

            ajax += _showValue ? "               $('#monValue').text(jsonData['Data']); \n" : "";

            ajax += "           } \n";
            ajax += "       }); \n";
            ajax += "   } \n";

            return ajax;
        }

        /* Public */
        public override void LoadTemplate(string filename)
        {
            try
            {
                if (_templateFolder != string.Empty)
                {
                    _templateFolder = _templateFolder.TrimEnd();

                    if (!_templateFolder.EndsWith(@"\"))
                        _templateFolder = _templateFolder + @"\";

                    DataSet dsTemplate = new DataSet();
                    dsTemplate.ReadXml(_templateFolder + filename);

                    //background
                    string backgroundColor      = FilterData(dsTemplate, "background", "background_color");
                    string backgroundGradient   = FilterData(dsTemplate, "background", "background_gradient");

                    _backgroundColor    = backgroundColor       == string.Empty ? _backgroundColor      : ".Set('background.color', '" + backgroundColor + "')";
                    _backgroundGradient = backgroundGradient    == string.Empty ? _backgroundGradient   : ".Set('background.gradient', '" + backgroundGradient + "')";

                    // border
                    string borderWidth      = FilterData(dsTemplate, "border", "border_width");
                    string outerBorderColor = FilterData(dsTemplate, "border", "border_outer");
                    string innerBorderColor = FilterData(dsTemplate, "border", "border_inner");
                    string shadow           = FilterData(dsTemplate, "border", "shadow");

                    _borderWidth        = borderWidth   == string.Empty ? _borderWidth      : ".Set('border.width', '" + borderWidth + "')";
                    _outerBorderColor   = borderWidth   == string.Empty ? _outerBorderColor : ".Set('border_outer', '" + outerBorderColor + "')";
                    _innerBorderColor   = borderWidth   == string.Empty ? _innerBorderColor : ".Set('border.inner', '" + innerBorderColor + "')";
                    _shadow             = shadow        == string.Empty ? _shadow           : ".Set('shadow', " + shadow + ")";

                    // label
                    string textColor    = FilterData(dsTemplate, "label", "text_color");
                    string textSize     = FilterData(dsTemplate, "label", "text_size");
                    string labelCount   = FilterData(dsTemplate, "label", "labels_count");

                    _textSize   = textSize  == string.Empty ? _textSize     : ".Set('text.size', '" + textSize + "')";
                    _textColor  = textColor == string.Empty ? _textColor    : ".Set('text.color', '" + textColor + "')";
                    _labelCount = textColor == string.Empty ? _labelCount   : ".Set('labels.count', '" + labelCount + "')";

                    // marks
                    string tickmarksBigNum      = FilterData(dsTemplate, "marks", "tickmarks_big_num");
                    string tickmarksMediumNum   = FilterData(dsTemplate, "marks", "tickmarks_medium_num");
                    string tickmarksSmallNum    = FilterData(dsTemplate, "marks", "tickmarks_small_num");
                    
                    string tickmarksBigColor    = FilterData(dsTemplate, "marks", "tickmarks_big_color");
                    string tickmarksMediumColor = FilterData(dsTemplate, "marks", "tickmarks_medium_color");
                    string tickmarksSmallColor  = FilterData(dsTemplate, "marks", "tickmarks_small_color");

                    _tickmarksSmallNum  = tickmarksSmallNum     == string.Empty ? _tickmarksSmallNum    : ".Set('tickmarks.small.num', '" + tickmarksSmallNum + "')";
                    _tickmarksMediumNum = tickmarksMediumColor  == string.Empty ? _tickmarksMediumNum   : ".Set('tickmarks.medium', '" + tickmarksMediumNum + "')";
                    _tickmarksBigNum    = tickmarksBigNum       == string.Empty ? _tickmarksBigNum      : ".Set('tickmarks.big.num', '" + tickmarksBigNum + "')";

                    _tickmarksSmallColor    = tickmarksSmallColor   == string.Empty ? _tickmarksSmallColor  : ".Set('tickmarks.small.color', '" + tickmarksSmallColor + "')";
                    _tickmarksMediumColor   = tickmarksMediumColor  == string.Empty ? _tickmarksMediumColor : ".Set('tickmarks.medium.color', '" + tickmarksMediumColor + "')";
                    _tickmarksBigColor      = tickmarksBigColor     == string.Empty ? _tickmarksBigColor    : ".Set('tickmarks.big.color', '" + tickmarksBigColor + "')";

                    // needle
                    string centerpinColor   = FilterData(dsTemplate, "needle", "centerpin_color");
                    string needleColor      = FilterData(dsTemplate, "needle", "needle_color");
                    string needleSize       = FilterData(dsTemplate, "needle", "needle_size");

                    _centerpinColor = centerpinColor    == string.Empty ? _centerpinColor   : ".Set('centerpin.color', '" + centerpinColor + "')";
                    _needleColor    = needleColor       == string.Empty ? _needleColor      : ".Set('needle.colors', [RGraph.RadialGradient(mon, 125, 125, 0, 125, 125, 25, 'transparent', '" + needleColor + "')])";
                    _needleSize     = needleSize        == string.Empty ? _needleSize       : ".Set('needle.size', " + needleSize + ")";

                    // margin
                    string gutterTop    = FilterData(dsTemplate, "margin", "margin_top");
                    string gutterBottom = FilterData(dsTemplate, "margin", "margin_bottom");
                    string gutterLeft   = FilterData(dsTemplate, "margin", "margin_left");
                    string gutterRight  = FilterData(dsTemplate, "margin", "margin_right");

                    _gutterTop      = gutterTop     == string.Empty ? _gutterTop    : ".Set('gutter.top', " + gutterTop + ")";
                    _gutterBottom   = gutterBottom  == string.Empty ? _gutterBottom : ".Set('gutter.bottom', " + gutterBottom + ")";
                    _gutterLeft     = gutterLeft    == string.Empty ? _gutterLeft   : ".Set('gutter.left', " + gutterLeft + ")";
                    _gutterRight    = gutterRight   == string.Empty ? _gutterRight  : ".Set('gutter.right', " + gutterRight + ")";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /**** Mons Properties ****/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="threshold">[green to yellow, yellow to red],[green color, yellow color, red color] \n
        /// [5, 6],[green, blue, #d66]</param>
        public override void SetThreshold(string threshold)
        {
            //  [5, 6],[green, blue, #d66]
            //  5, 6
            //  ,
            //  green, blue, #d66

            if (threshold != string.Empty)
            {
                string[] listThreshold = threshold.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                if (listThreshold.Length >= 2)
                {
                    string[] thresholds = listThreshold[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (thresholds.Length >= 2)
                    {
                        _greenEnd = ".Set('green.end', '" + thresholds[0].Trim() + "')";
                        _redStart = ".Set('red.start', '" + thresholds[1].Trim() + "')";
                    }

                    thresholds = listThreshold[2].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (thresholds.Length >= 3)
                    {
                        _greenColor = ".Set('green.color', '" + thresholds[0].Trim() + "')";
                        _yellowColor = ".Set('yellow.color', '" + thresholds[1].Trim() + "')";
                        _redColor = ".Set('red.color', '" + thresholds[2].Trim() + "')";
                    }
                }
            }
        }

        public override void SetTickMarks(List<string> tickMarksValues, List<string> tickMasksColors)
        {
            if (tickMarksValues.Count != 3 && tickMasksColors.Count != 3) return;

            // colors
            _tickmarksBigColor      = ".Set('tickmarks.big.color', '"       + (tickMasksColors[0] != string.Empty ? tickMasksColors[0] : "transparent") + "')";
            _tickmarksMediumColor   = ".Set('tickmarks.medium.color', '"    + (tickMasksColors[1] != string.Empty ? tickMasksColors[1] : "transparent") + "')";
            _tickmarksSmallColor    = ".Set('tickmarks.small.color', '"     + (tickMasksColors[2] != string.Empty ? tickMasksColors[2] : "transparent") + "')";

            // values
            if (tickMarksValues[0] != string.Empty) _tickmarksBigNum    = ".Set('tickmarks.big.num', '"     + tickMarksValues[0] + "')";
            if (tickMarksValues[1] != string.Empty) _tickmarksMediumNum = ".Set('tickmarks.medium', '"      + tickMarksValues[1] + "')";
            if (tickMarksValues[2] != string.Empty) _tickmarksSmallNum  = ".Set('tickmarks.small.num', '"   + tickMarksValues[2] + "')";

        }

        public override void ChangeNeedleSize(string needleSize)
        {
            _needleSize = needleSize == string.Empty 
                                                    ? _needleSize 
                                                    : ".Set('needle.size', " + needleSize + ")";
        }
    }
}