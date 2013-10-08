using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using DbConfig;
using Views.Frames;

public partial class frames_sql_query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) ViewState["log_file"] = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

        try
        {
            string frameIdHash = Request.QueryString.Get("fm");

            if (frameIdHash != null)
            {
                if (frame_sessions.GetFrame(frameIdHash) != null)
                {
                    Startup((Frame)frame_sessions.GetFrame(frameIdHash));
                }
            }
        }
        catch (Exception ex)
        {
            loging.Error("sql query", "load page", ex.Message, ViewState["log_file"].ToString());
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading sql tools</p>\n");

        Server.ClearError();

        loging.Error("sql query", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
    }

    private void Startup(Frame frame)
    {
        // replace log file so that is one frame to one log
        ReplaceLogFile(frame.IDPage, frame.ID);

        OptionItems oi = new OptionItems(frame.Options);

        // ------ Frame css ------
        ChangeCss(oi.GetSingle("css"));

        // ------ Title ------
        headerTitle.Text = frame.Title;

        if (!oi.GetSingle("title_is_hidden").Equals("true")) // hide title if value is true, else will always show the title
        {
            lbTitle.InnerText = frame.Title;
            TitleStyle(oi.GetList("title_style")); // title style

            // loads javascript that controls title position
            SetTitlePosition();
        }
        else
        {
            lbTitleContainer.Style.Add("display", "none");
        }

        /****************** GETTING OPTION DATA ******************/
        // master/slave indicator
        bool isMaster = oi.GetSingle("master").ToLower().Equals("yes");
        bool isSlave = oi.GetSingle("slave").ToLower().Equals("yes");

        // master filter title
        string masterFilter = oi.GetSingle("master_filter");

        // list of the labels of the inputboxs
        List<string> listInputData = oi.GetList("input_data");

        // list of connections
        List<string> connList = oi.GetList("conn");

        // list queries
        List<string> sqlList = oi.GetList("sql");

        bool queryDebug = oi.GetSingle("query_debug").ToLower().Equals("yes");

        // list titles
        List<string> titleList = oi.GetList("titles");

        // list gridviews width
        List<string> tableWidthList = oi.GetList("table_width");

        /****************** RUN CLICK EVENT ******************/
        // data to be used in Run Click Event
        LoadTemporaryPageVariables(frame, isSlave, isMaster, listInputData, masterFilter, connList, sqlList, titleList, tableWidthList, queryDebug);

        /****************** INPUT BOXs ******************/
        // if is not slave will build the inputboxs
        if (!isSlave)
        {
            BuildInputBoxs(listInputData);
        }

        /****************** GRIDVIEWs ******************/
        // builds the gridviews if is master/slave and its not a postback
        if (!IsPostBack && (isMaster || isSlave) && masterFilter != string.Empty)
        {
            BuildGridViews(frame, masterFilter, listInputData, sqlList, connList, titleList, tableWidthList, queryDebug);
        } 
    }

    protected void Run_Click(object sender, EventArgs e)
    {
        try
        {
            if ((bool)ViewState["isMaster"])
            {
                // create master filter
                string masterFilterTitle = BuildMasterFilter();

                // send message to parent page to refresh slave and master pages
                Generic.ParentJavascriptMethodInjector("RefreshSlaves('" + masterFilterTitle + "')"); // refresh slaves
            }
            else
            {
                // builds gridview now
                BuildGridViews();
            }
        }
        catch (Exception ex)
        {
            loging.Error("sql query", "click event", ex.Message, ViewState["log_file"].ToString());
        }
        
        
    }

    /******************************************************************************************/

    /****************** RUN CLICK EVENT ******************/
    /// <summary>
    /// Loads the necessary connect to send to run click event
    /// </summary>
    private void LoadTemporaryPageVariables(Frame frame, bool isSlave, bool isMaster, List<string> inputDataList, string masterFilter,
                                            List<string> connList, List<string> sqlList, List<string> titleList, List<string> tableWidthList, bool queryDebug)
    {
        if (!isSlave && !IsPostBack)
        {
            ViewState["isMaster"] = isMaster;
            ViewState["input_name"] = inputDataList;

            // just add Master Filter and Page Id if it's master 
            if (isMaster)
            {
                ViewState["masterFilter"] = masterFilter;
                ViewState["pageId"] = frame.IDPage.ToString(CultureInfo.InvariantCulture);
            }
            else // if not master then the data below will be used in Run Click Event to Build
            {
                ViewState["conn_list"] = connList;
                ViewState["sql_list"] = sqlList;
                ViewState["title_list"] = titleList;
                ViewState["table_width_list"] = tableWidthList;
                ViewState["query_debug"] = queryDebug;

            }
        }
    }

    /******************** Input Box ********************/
    private void BuildInputBoxs(List<string> inputData)
    {
        BuildInputbox ib = new BuildInputbox(inputData);

        Control table = ib.GetHtmlControls();

        Button run = new Button
        {
            Text = "Run", 
            CssClass = "run"
        };

        run.Click += Run_Click;

        inputBoxContainer.Controls.Add(table);
        inputBoxContainer.Controls.Add(run);
    }

    /******************** GridViews ********************/
    /// <summary>
    /// Builds gridviews with data from master filter to create the html
    /// </summary>
    private void BuildGridViews(Frame f, string masterFilter, List<string> inputNames, List<string> sqlList, 
                                List<string> connList, List<string> titleList, List<string> tableWidthList, bool queryDebug)
    {
        /* 
                sql_tool_filter=[orderid = value],[customerid = value];
                OR
                orderid = value, customerid=value
             */
        try
        {
            string pageIdHash = Generic.GetHash(f.IDPage.ToString(CultureInfo.InvariantCulture));
            string masterFilterHash = Generic.GetHash(masterFilter);

            string filter = filter_sessions.GetMasterFilterString(pageIdHash, masterFilterHash);

            if (filter == string.Empty) return;
            
            List<string> inputData = BuildInputDataList(filter, inputNames);
            BuildGridview bgv = new BuildGridview(sqlList, connList, inputData, titleList, tableWidthList, queryDebug);   
            gridviewContainer.Controls.Add(bgv.GetHtmlControls());
        }
        catch (Exception ex)
        {
            throw new Exception("error building gridviews " + ex.Message);
        }
        
    }

    /// <summary>
    /// Builds gridviews with data from textbox
    /// </summary>
    private void BuildGridViews()
    {
        try
        {
            List<string> connList       = (List<string>) ViewState["conn_list"];
            List<string> sqlList        = (List<string>) ViewState["sql_list"];
            List<string> inputNames     = (List<string>) ViewState["input_name"];
            List<string> titleList      = (List<string>) ViewState["title_list"];
            List<string> tableWidthList = (List<string>) ViewState["table_width_list"];
            bool queryDebug             = (bool) ViewState["query_debug"];


            Dictionary<string, object> inputData = BuildInputDataList(inputNames);

            if (!(bool) inputData["hasvalue"]) return;
            
            List<string> listData = (List<string>)inputData["data"];

            BuildGridview bgv = new BuildGridview(sqlList, connList, listData, titleList, tableWidthList, queryDebug);
            gridviewContainer.Controls.Add(bgv.GetHtmlControls());
            
        }
        catch (Exception ex)
        {
            throw new Exception("error building gridviews " + ex.Message);
        }
    }

    /// <summary>
    /// gets the data from master filter
    /// </summary>
    private List<string> BuildInputDataList(string filter, List<string> inputData)
    {
        /* 
            sql_tool_filter=[orderid = value],[customerid = value];
            OR
            orderid = value, customerid=value
         */

        try
        {
            if (filter == string.Empty) return new List<string>();

            List<string> unorderedDataList;
            List<string> orderedDataList = new List<string>();

            if (filter.ToLower().Trim().StartsWith("sql_tool_filter=")) 
            {
                OptionItems oi = new OptionItems(filter);

                unorderedDataList = oi.GetList("sql_tool_filter");
            }
            else 
            {
                unorderedDataList = filter.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            

            foreach (string name in inputData)
            {
                string value = string.Empty;

                try
                {
                    value = unorderedDataList.Single(x => x.ToLower().TrimStart().StartsWith(name.ToLower()));
                    
                    // orderid = value TRANSFORM TO value
                    value = value.Substring(value.IndexOf('=') + 1).Trim();

                } catch { }

                orderedDataList.Add(value ?? string.Empty);
            }

            return orderedDataList;
        }
        catch (Exception ex)
        {
            throw new Exception("error processing filter " + ex.Message);
        }
    }

    /// <summary>
    /// Gets the values from textbox
    /// </summary>
    private Dictionary<string, object> BuildInputDataList(List<string> inputData)
    {
        try
        {
            bool hasValues = false;

            List<string> dataList = new List<string>();

            for (int i = 0; i < inputData.Count; i++)
            {
                string value = string.Empty;

                try { value = Request.Form["value" + i]; } catch { }

                dataList.Add(value ?? string.Empty);

                if (value != null) hasValues = true;
            }

            return new Dictionary<string, object> { { "data", dataList }, { "hasvalue", hasValues } };
        }
        catch (Exception ex)
        {
            throw new Exception("error processing filter " + ex.Message);
        }   
    }

    // builds the master filter to be used in the slaves
    private string BuildMasterFilter()
    {
        try
        {
            List<string> inputNames = (List<string>)ViewState["input_name"];

            if (inputNames.Count == 0) return string.Empty;


            string filter = "sql_tool_filter=[";
            bool hasValues = false;

            for (int i = 0; i < inputNames.Count; i++)
            {
                string value = string.Empty;

                try { value = Request.Form["value" + i]; } catch { }

                filter += "[" + inputNames[i] + "=" + (value ?? string.Empty) + "]";

                if (i != inputNames.Count - 1) filter += ", ";

                if (value != null) hasValues = true;
            }

            filter += "];";

            // if no value added then does notting
            if (!hasValues) return string.Empty;

            string pageIdHash = Generic.GetHash((string)ViewState["pageId"]);
            string masterFilterHash = Generic.GetHash((string)ViewState["masterFilter"]);

            filter_sessions.SetMasterFilterString(pageIdHash, masterFilterHash, filter);

            return (string)ViewState["masterFilter"];
        }
        catch (Exception ex)
        {
            throw new Exception("error building master filter" + ex.Message);
        }
    }

    /********************************************* FRAME STYLE *********************************************/

    /******************** Title Style ********************/
    private void TitleStyle(List<string> cssOptions)
    {
        FrameHtmlCtrls titleObjects = GenericFrameSettings.FrameTitleStyle(lbTitleContainer, lbTitle, cssOptions);

        lbTitle = titleObjects.LabelTitle;
        lbTitleContainer = titleObjects.TitleContainer;
    }
    /**************************************************/

    /******************** Title Position Ctrl ********************/
    /// <summary>
    /// Loads javascript that controls title position
    /// </summary>
    private void SetTitlePosition()
    {
        Generic.JavaScriptInjector("titleposition", GenericFrameSettings.GetJavascriptTitlePosition());
    }
    /**************************************************/

    /******************** Frame CSS ********************/
    /// <summary>
    /// Change CSS 
    /// </summary>
    /// <param name="cssFileName"></param>
    public void ChangeCss(string cssFileName)
    {
        if (cssFileName != string.Empty)
        {
            cssStyle.Href = "~/css/views/frames/" + cssFileName;
        }
    }
    /**************************************************/

    /******************** Change Log File ********************/
    /// <summary>
    /// its best to one log for frame so that is easy to analise possible errors
    /// </summary>
    private void ReplaceLogFile(int pageId, int frameId)
    {
        // one execution, one log
        if (!IsPostBack)
            ViewState["log_file"] = LogingFileFormats.ReplaceLogFrameFile(ViewState["log_file"].ToString(), pageId, frameId);
    }
    /**************************************************/
}