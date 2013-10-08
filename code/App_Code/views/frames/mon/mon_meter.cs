using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Views.Frames
{
    /// <summary>
    /// Summary description for mon_meter
    /// </summary>
    public class mon_meter : mon_general
    {
        //threshold
        private string _thresholdColors; // meter

        // border
        private string _borderColor; // meter
        private string _border; // meter

        // labels
        private string _unitsPost; // meter
        private string _label; // meter

        // marks
        private string _tickmarksSmallNum;
        private string _tickmarksBigNum;

        public mon_meter()
        {
            MeasureType = string.Empty;

            _thresholdColors = ".Set('colors.ranges', [";
            _thresholdColors += "[0, 60, 'black'],";
            _thresholdColors += "[60, 80, 'yellow'],";
            _thresholdColors += "[80, 100, 'red']";
            _thresholdColors += "])";

            _borderWidth = ".Set('linewidth', '1')";
            _borderColor = ".Set('border.color', 'black')";
            _border = ".Set('border', true)";

            _unitsPost = ".Set('units.post', '')";
            _label = ".Set('labels', true)";

            _tickmarksSmallNum = ".Set('tickmarks.small.num', '100')";
            _tickmarksBigNum = ".Set('tickmarks.big.num', '10')";

            _gutterTop = ".Set('gutter.top', 8)";
            _gutterBottom = ".Set('gutter.bottom', 8)";
            _gutterLeft = ".Set('gutter.left', 8)";
            _gutterRight = ".Set('gutter.right', 8)";

            // by default its disable
            _needleSize = string.Empty;
        }

        /* Private */
        protected override string BuildMonMethod()
        {
            string mon = string.Empty;

            mon += "function Mon() { \n";

            mon += "mon = new RGraph.Meter('cvs', " + _min + ", " + _max + ", " + _defaultValue + "); \n";

            mon += "mon \n";

            // limits
            mon += _thresholdColors + " \n";

            // background
            mon += _backgroundColor + " \n";

            // border
            mon += _borderWidth + " \n";
            mon += _borderColor + " \n";
            mon += _border + " \n";
            mon += _shadow + " \n";

            // label
            mon += _unitsPost + " \n";
            mon += _label + " \n";
            mon += _textSize + " \n";
            mon += _textColor + " \n";

            // marks
            mon += _tickmarksSmallNum + " \n";
            mon += _tickmarksBigNum + " \n";

            mon += _tickmarksSmallColor + "\n";
            mon += _tickmarksBigColor + " \n";

            mon += _gutterTop + " \n";
            mon += _gutterBottom + " \n";
            mon += _gutterLeft + " \n";
            mon += _gutterRight + " \n";

            mon += ".Draw(); \n";

            mon += "} \n";


            /*
            meter = new RGraph.Meter('cvs2', 0, 200, 10);

                        // limits
                        meter
                            .Set('colors.ranges', [
                                    [0, 60, 'black'], [60, 80, 'yellow'], [80, 100, 'red'],
                                    [100, 160, 'green'],[160, 180, 'yellow'],[180, 200, 'red']
                            ]) 

                            // background
                            .Set('background.color', 'transparent') //string

                            // border
                            .Set('linewidth', '1')
                            .Set('border.color', 'black')
                            .Set('border', true)
                            .Set('shadow', false)

                            // labels text colors
                            .Set('units.post', '')
                            .Set('labels', true)
                            .Set('text.size', '7')
                            .Set('text.color', 'red')

                            // marks
                            .Set('tickmarks.small.num', '100')
                            .Set('tickmarks.big.num', '10')

                            .Set('tickmarks.small.color', 'transparent')
                            .Set('tickmarks.big.color', 'black')

                            // margin
                            .Set('gutter.top', 8)
                            .Set('gutter.bottom', 8)
                            .Set('gutter.left', 8)
                            .Set('gutter.right', 8)

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

            string ajaxSelectFilter =  HttpUtility.UrlEncode(_selectFilter);

            ajax += "\n";
            ajax += "   function GetData() { \n";
            ajax += "\n";
            ajax += "   if (IsPageHidden()) return; \n";

            ajax += "\n";
            ajax += "   UpdatingDataInformation(); ";
            ajax += "\n";

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
            ajax += "               RGraph.Effects.Meter.Grow(mon); \n";

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
                    string backgroundColor = FilterData(dsTemplate, "background", "background_color");

                    _backgroundColor = backgroundColor == string.Empty ? _backgroundColor : ".Set('background.color', '" + backgroundColor + "')";

                    // border
                    string borderWidth = FilterData(dsTemplate, "border", "linewidth");
                    string borderColor = FilterData(dsTemplate, "border", "border_color");
                    string border = FilterData(dsTemplate, "border", "border_show");
                    string shadow = FilterData(dsTemplate, "border", "shadow");

                    _borderWidth = borderWidth == string.Empty ? _borderWidth : ".Set('linewidth', '" + borderWidth + "')";
                    _borderColor = borderColor == string.Empty ? _borderColor : ".Set('border.color', '" + borderColor + "')";
                    _border = border == string.Empty ? _border : ".Set('border', " + border + ")";
                    _shadow = shadow == string.Empty ? _shadow : ".Set('shadow', " + shadow + ")";

                    // label
                    string unitsPost = FilterData(dsTemplate, "label", "units_post");
                    string label = FilterData(dsTemplate, "label", "label_show");
                    string textSize = FilterData(dsTemplate, "label", "text_size");
                    string textColor = FilterData(dsTemplate, "label", "text_color");

                    _unitsPost = unitsPost == string.Empty ? _unitsPost : ".Set('units.post', '" + unitsPost + "')";
                    _label = label == string.Empty ? _label : ".Set('labels', " + label + ")";
                    _textSize = textSize == string.Empty ? _textSize : ".Set('text.size', '" + textSize + "')";
                    _textColor = textColor == string.Empty ? _textColor : ".Set('text.color', '" + textColor + "')";

                    // marks
                    string tickmarksSmallNum = FilterData(dsTemplate, "marks", "tickmarks_small_num");
                    string tickmarksBigNum = FilterData(dsTemplate, "marks", "tickmarks_big_num");
                    string tickmarksSmallColor = FilterData(dsTemplate, "marks", "tickmarks_small_color");
                    string tickmarksBigColor = FilterData(dsTemplate, "marks", "tickmarks_big_color");

                    _tickmarksSmallNum = tickmarksSmallNum == string.Empty ? _tickmarksSmallNum : ".Set('tickmarks.small.num', '" + tickmarksSmallNum + "')";
                    _tickmarksBigNum = tickmarksBigNum == string.Empty ? _tickmarksBigNum : ".Set('tickmarks.big.num', '" + tickmarksBigNum + "')";
                    _tickmarksSmallColor = tickmarksSmallColor == string.Empty ? _tickmarksSmallColor : ".Set('tickmarks.small.color', '" + tickmarksSmallColor + "')";
                    _tickmarksBigColor = tickmarksBigColor == string.Empty ? _tickmarksBigColor : ".Set('tickmarks.big.color', '" + tickmarksBigColor + "')";

                    // margin
                    string gutterTop = FilterData(dsTemplate, "margin", "margin_top");
                    string gutterBottom = FilterData(dsTemplate, "margin", "margin_bottom");
                    string gutterLeft = FilterData(dsTemplate, "margin", "margin_left");
                    string gutterRight = FilterData(dsTemplate, "margin", "margin_right");

                    _gutterTop = gutterTop == string.Empty ? _gutterTop : ".Set('gutter.top', " + gutterTop + ")";
                    _gutterBottom = gutterBottom == string.Empty ? _gutterBottom : ".Set('gutter.bottom', " + gutterBottom + ")";
                    _gutterLeft = gutterLeft == string.Empty ? _gutterLeft : ".Set('gutter.left', " + gutterLeft + ")";
                    _gutterRight = gutterRight == string.Empty ? _gutterRight : ".Set('gutter.right', " + gutterRight + ")";

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
        /// <param name="threshold">[0, 60, 'green'],[60, 80 'yellow'],[80, 100, 'red']</param>
        public override void SetThreshold(string threshold)
        {
            _thresholdColors = ".Set('colors.ranges', [" + threshold + "])";
        }

        public override void SetTickMarks(List<string> tickMarksValues, List<string> tickMasksColors)
        {
            if (tickMarksValues.Count != 2 && tickMasksColors.Count != 2) return;

            // colors
            _tickmarksBigColor      = ".Set('tickmarks.big.color', '" + tickMasksColors[0] != string.Empty ? tickMasksColors[0] : "transparent" + "')";
            _tickmarksSmallColor    = ".Set('tickmarks.small.color', '" + tickMasksColors[1] != string.Empty ? tickMasksColors[1] : "transparent" + "')";

            // values
            if (tickMarksValues[0] != string.Empty) _tickmarksBigNum = ".Set('tickmarks.big.num', '" + tickMarksValues[0] + "')";
            if (tickMarksValues[1] != string.Empty) _tickmarksSmallNum = ".Set('tickmarks.small.num', '" + tickMarksValues[1] + "')";

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="needleSize"></param>
        public override void ChangeNeedleSize(string needleSize)
        {
            _needleSize = needleSize == string.Empty
                                                    ? _needleSize
                                                    : ".Set('chart.needle.linewidth', " + needleSize + ")";

        }

    }
}