using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using DbConnection;

namespace Views.Frames
{
    /// <summary>
    /// Receives the queries and connections and returns the gridviews
    /// </summary>
    public class BuildGridview
    {
        private readonly List<string> _sqlList;
        private readonly List<string> _connList;
        private readonly List<string> _inputData;
        private readonly List<string> _titleList;
        private readonly List<string> _tableWidthList;
        private readonly bool _queryDebug;

        private bool _isReadToRun;

        public BuildGridview(List<string> sqlList, List<string> connList, List<string> inputData, List<string> titleList, List<string> tableWidthList, bool queryDebug)
        {
            _sqlList = sqlList.ToList();
            _connList = connList;
            _inputData = inputData;
            _titleList = titleList;
            _tableWidthList = tableWidthList;
            _queryDebug = queryDebug;

            _isReadToRun = false;
        }
        /***************************** PRIVATE *****************************/
        /// <summary>
        /// replace the value_xxx to real values in queries
        /// </summary>
        private void BuildQueries()
        {
            try
            {
                if (_inputData.Count == 0) return;
                
                for (int i = 0; i < _sqlList.Count; i++)
                {
                    for (int a = 0; a < _inputData.Count; a++)
                    {
                        _sqlList[i] = _sqlList[i].Replace("value_" + a, _inputData[a]);
                    }
                }

                _isReadToRun = true;
            }
            catch (Exception ex)
            {
                throw new Exception("error building queries " + ex.Message);
            }
        }

        /// <summary>
        /// Runs the queries and generate a list of datatable
        /// </summary>
        private List<DataTable> RunQueries()
        {
            List<DataTable> listResults = new List<DataTable>();

            try
            {
                RunQuery runQuery = new RunQuery();

                // returns if the build failed
                if (!_isReadToRun) return new List<DataTable>();

                // returns if not connection is available
                if (_connList.Count == 0) return new List<DataTable>();
                
                for (int i = 0; i < _sqlList.Count; i++)
                {
                    
                        string query = _sqlList[i];
                        string conn = _sqlList.Count == _connList.Count
                            ? _connList[i]
                            : _connList[0];

                        // adds the results to a list so that it can be process in the next step, the try catch to empty space is because we dont want the execution to stop because some type of error
                    try
                    {
                        listResults.Add(runQuery.GetData(conn, query));
                    }
                    catch (Exception ex)
                    {
                        if (_queryDebug)
                            throw new Exception("error running query at position " + i + " - " + ex.Message);
                    }
                }
                
            }
            catch (Exception ex)
            {                
                throw new Exception("error running queries " + ex.Message);
            }

            return listResults;
        }

        /// <summary>
        /// Builds the gridview and returns the data ready to be be insert in html control
        /// </summary>
        private Control BuildHtmlContent(List<DataTable> listResults)
        {
            try
            {
                HtmlObjects container = new HtmlObjects("div");

                for (int i = 0; i < listResults.Count; i++)
                {

                    DataTable result = listResults[i];


                    if (_titleList.Count > i)
                    {
                        HtmlObjects title = new HtmlObjects("span");
                        title.AddAttribute("class", "title");
                        title.Text = _titleList[i];

                        container.AddControls(title.GetObject);
                    }
                    

                    HtmlObjects spacer = new HtmlObjects("div");
                    spacer.AddAttribute("class", "spacer");

                    GridView gv = new GridView
                    {
                        DataSource = result,
                        CssClass = "gridview",
                        
                    };

                    gv.AlternatingRowStyle.CssClass = "alt";
                    gv.PagerStyle.CssClass = "pgr";

                    // Does not have a catch because if the value cant be converted it will just use the default value
                    try {
                        if (_tableWidthList.Count > i)
                            gv.Width = Unit.Pixel(Convert.ToInt32(_tableWidthList[i])); 
                    } catch {}
                    

                    gv.DataBind();

                    container.AddControls(gv);
                    container.AddControls(spacer.GetObject);
                }

                return container.GetObject;
            }
            catch (Exception ex)
            {
                throw new Exception("error building html data " + ex.Message);
            }  
        }

        /***************************** PUBLIC *****************************/
        /// <summary>
        /// Returns the gridviews
        /// </summary>
        public Control GetHtmlControls()
        {            
            BuildQueries();
            
            List<DataTable> listTables = RunQueries();

            return BuildHtmlContent(listTables);
        }
        
    }
}
