using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Views.Frames
{
    /// <summary>
    /// Summary description for mon_general
    /// </summary>
    public abstract class mon_general
    {
        // mon web service key
        protected string _hashKey;

        // refresh interval
        protected int _refreshInterval;

        // xml file to read
        protected string _xmlFileName;

        // select filter
        protected string _selectFilter;

        // web service running method
        protected string _measureType;

        protected string _templateFolder;

        // init values
        protected int _min; // shared
        protected int _max; // shared
        protected int _defaultValue; // shared

        // background
        protected string _backgroundColor; // shared

        // border
        protected string _borderWidth; // lineWidth in meter = border width in gauge
        protected string _shadow; // shared

        // labels
        protected string _textSize; // shared
        protected string _textColor; // shared

        // marks
        protected string _tickmarksSmallColor; // shared
        protected string _tickmarksBigColor; // shared

        // margin
        protected string _gutterTop; // shared
        protected string _gutterBottom; // shared
        protected string _gutterLeft; // shared
        protected string _gutterRight; // shared

        protected bool _showValue; // shared

        //needle
        protected string _needleSize; // shared

        public mon_general()
        {
            _hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

            _refreshInterval = 60000;

            _xmlFileName = string.Empty;

            _selectFilter = string.Empty;

            _min = 0;
            _max = 100;
            _defaultValue = 0;

            _backgroundColor = ".Set('background.color', 'transparent')";
            _shadow = ".Set('shadow', false)";

            _textSize = ".Set('text.size', '7')";
            _textColor = ".Set('text.color', 'red')";

            _tickmarksSmallColor = ".Set('tickmarks.small.color', 'transparent')";
            _tickmarksBigColor = ".Set('tickmarks.big.color', 'black')";

            _showValue = false;
        }

        /* Private */
        protected string HashKey
        {
            get { return _hashKey; }
        }

        protected abstract string BuildMonMethod();

        protected abstract string BuildAjax();

        protected string BuildJavasScript()
        {
            Random random = new Random();
            int randomRefresh = random.Next(500, 10000) + RefreshInterval;

            string script = string.Empty;

            script += "var mon;  \n";

            script += "window.onload = function () { \n";

            script += "     Mon(); \n";
            
            script += _showValue ? "               $('#monValue').css('display','block'); \n" : "";

            script += "     var timerInterval = self.setInterval(function () { \n";

            script += "     GetData(); \n";

            script += "     }, " + randomRefresh + "); \n";

            script += "     GetData(); \n";

            script += "} \n";

            script += BuildMonMethod();

            script += BuildAjax();

            return script;
        }

        /// <summary> Filter data from template </summary>
        /// <param name="dt"></param>
        /// <param name="column"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        protected string FilterData(DataSet ds, string table, string column)
        {
            try
            {
                int index = ds.Tables[table].Columns[column].Ordinal;

                if (index >= 0)
                    return ds.Tables[table].Rows[0].ItemArray[index].ToString();
            }
            catch { }

            return string.Empty;
        }

        /* Public */
        public string GetJavaScript()
        {
            return _xmlFileName != string.Empty
                    ? BuildJavasScript()
                    : string.Empty;
        }

        /********/
        public abstract void LoadTemplate(string filename);

        public int MinValue
        {
            get { return _min; }
            set { _min = value; }
        }

        public int MaxValue
        {
            get { return _max; }
            set { _max = value; }
        }

        public int DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public string MeasureType
        {
            get { return _measureType; }
            set { _measureType = value; }
        }

        public int RefreshInterval
        {
            get { return _refreshInterval; }
            set { _refreshInterval = value; }
        }

        public string TemplateFolder
        {
            get { return _templateFolder; }
            set { _templateFolder = value; }
        }

        public string XmlFileName
        {
            get { return _xmlFileName; }
            set { _xmlFileName = value; }
        }

        public string SelectFilter
        {
            get { return _selectFilter; }
            set { _selectFilter = value; }
        }

        /// <summary>
        /// Show the value in the moon, not just the needle position
        /// </summary>
        public void ShowValue(bool show)
        {
            _showValue = show;
        }

        /**** Mons Properties ****/
        public abstract void SetThreshold(string threshold);

        public void StartValues(int min, int max, int defaultValue)
        {
            _min = min;
            _max = max;
            _defaultValue = defaultValue;
        }

        public abstract void SetTickMarks(List<string> tickMarksValues, List<string> tickMasksColors);

        public abstract void ChangeNeedleSize(string needleSize);
    }
}