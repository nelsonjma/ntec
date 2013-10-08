using System;
using System.Collections.Generic;
using System.Globalization;
using DbConfig;

namespace Views.Frames
{
    /// <summary>
    /// transforme the hyperlink option in real table hyperlinks
    /// </summary>
    public class DynamicHyperlink
    {
        private readonly List<string> _jsSwith;
        private readonly List<string> _columnNames; // used this var to check if columns have color markers

        public DynamicHyperlink(List<string> hyperlinkList, int thisPageId)
        {


            _columnNames = new List<string>();
            _jsSwith = new List<string>();

            BuildFilters(hyperlinkList, thisPageId);
        }

        /// <summary>
        /// Transforms the list of hyperlink into a list 
        /// of cases that will be used in javascript
        /// </summary>
        /// <param name="listHyperlinks"></param>
        private void BuildFilters(List<string> listHyperlinks, int thisPageId)
        {
            foreach (string link in listHyperlinks)
            {
                string[] lk = link.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (lk.Length != 4) continue;

                string columnName = lk[0].Trim();
                string pageName = lk[1].Trim();
                string masterFilterName = lk[2].Trim();
                string filterCondition = lk[3].Trim();

                db_config_page dcp = null;

                try
                {
                    _columnNames.Add(columnName);

                    /*
                        case 'sin_req':
                    
                            pageIdHash = '123123123123';
                            filterTitleHash = '1asfa2324q23';
                            isCurrentPage = false

                            filter = "sin_req = 'value'".replace('value', value.toString());
                    */

                    dcp = new db_config_page(pageName);
                    dcp.Open();

                    DbConfig.Page p = dcp.Get(pageName);

                    int pageId = p.ID;

                    string pageHashId           = Generic.GetHash(pageId.ToString(CultureInfo.InvariantCulture));
                    string masterFilterNameHash = Generic.GetHash(masterFilterName);

                    string js = string.Empty;
                    js += "case '" + columnName + "': \n";
                    js += " pageName = '" + pageName + "'; \n";
                    js += " pageIdHash = '" + pageHashId + "'; \n";
                    js += " filterTitle = '" + masterFilterName + "'; \n";
                    js += " filterTitleHash = '" + masterFilterNameHash + "'; \n";
                    js += " isCurrentPage = " + (pageId == thisPageId ? "true" : "false") + "; \n";
                    js += " \n";
                    js += " filter = \"" + filterCondition + "\".replace('value', value.toString()); \n";

                    _jsSwith.Add(js);
                }
                finally
                {
                    if (dcp != null)
                        dcp.Close();
                }
            }
        }

        public string BuildJavascript()
        {
            if (_jsSwith.Count == 0) return string.Empty;

            string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

            string js = string.Empty;

            js += " $(document).ready(function () { \n";
            js += "     $('#mGridView').on('click', 'td', function (e) { \n";
            js += "         var cellPos = this.cellIndex; \n";
            js += "         var header = $('#mGridView').find('th:eq(' + cellPos + ')').html(); \n";

                            // remove sort overhead made by gridview
            js += "         if (header.indexOf('</a>') >= 0) { \n";
            js += "             var headerpart = header.toString().split('>'); \n";
            js += "             header = headerpart[1].substring(0, headerpart[1].indexOf('</a')); \n";
            js += "         } \n";

            js += "         var value = $(this).html(); \n";
            js += "         GenerateMasterFilter(header, value); \n";
            js += "     }); \n";
            js += " }); \n";
            js += " \n";

            js += " function GenerateMasterFilter(header, value) { \n";

            js += "     var pageName; \n";
            js += "     var pageIdHash; \n";
            js += "     var filterTitle; \n";
            js += "     var filterTitleHash; \n";
            js += "     var isCurrentPage; \n";
            js += "     var filter; \n";


            js += "     switch (header.toLowerCase()) { \n";

            foreach (string jsCase in _jsSwith)
            {
                js += " \n";

                js += jsCase;

                js += " \n";
                js += " break; \n";
            }

            js += "         default: \n";
            js += "             return; \n";
            js += "     } \n";

            js += "     filter = encodeURI(filter); \n";
            js += "     while (filter.indexOf(\"'\") >= 0) \n";
            js += "         filter = filter.replace(\"'\", \"%27\"); \n";

            js += "     $.ajax({ \n";
            js += "         type: \"POST\", contentType: \"application/json; charset=utf-8\", \n";
            js += "         url: \"./table_service.asmx/SetMasterFilter\", \n";
            js += "         data: \"{'pageIdHash':'\" + pageIdHash + \"', 'filterTitleHash':'\" + filterTitleHash + \"', 'filter':'\" + filter + \"', 'ctrl': '" + hashKey + "'}\", \n";
            js += "         success: function (data) { \n";
            js += "             if (data.d == 'ok') { \n";

            js += "                 if (isCurrentPage == true) \n";
            js += "                     window.parent.RefreshSlaves(filterTitle); \n";
            js += "                 else \n";
            js += "                     OpenInNewTab('../page.aspx?nm=' + pageName); \n";

            js += "             } \n";

            js += "         }, \n";
            js += "         error: function (e) {  } \n";
            js += "     }); \n";
            js += " } \n";

            return js;

        }

        public List<string> ColumnNames
        {
            get { return _columnNames; }
        }
    }
}

