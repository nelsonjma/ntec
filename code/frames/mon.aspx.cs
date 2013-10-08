using System;
using System.Collections.Generic;
using Views.Frames;
using DbConfig;

public partial class mon : System.Web.UI.Page
{
    mon_general _mon;

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
            loging.Error("mon", "load page", ex.Message, ViewState["log_file"].ToString());
        }   
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading monitorization</p>\n");

        Server.ClearError();

        loging.Error("mon", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
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

        if (!oi.GetSingle("title_is_hidden").Equals("false"))
        {
            lbTitle.InnerText = frame.Title;
            TitleStyle(oi.GetList("title_style")); // title style

            // ------ Load Javascript ------
            SetTitlePosition(); // loads javascript that controls title position
        }
        else
        {
            lbTitleContainer.Style.Add("display", "none");
        }

        /********** Select Mon type **********/
        string monType = oi.GetSingle("mon_type").ToLower();
        if (monType == string.Empty) monType = "meter";

        SelectMon(monType); // meter / gauge

        /********** Select Filter **********/
        SelectFilter(oi.GetSingle("default_filter"));

        /********** Width, Height **********/
        string newWidth = oi.GetSingle("width");
        string newHeight = oi.GetSingle("height");

        CanvasSize(
                    newWidth != string.Empty ? Convert.ToInt32(newWidth) : frame.Width - 1,
                    newHeight != string.Empty ? Convert.ToInt32(newHeight) : frame.Height - 30
                    );

        /********** Min/Max/Default Values **********/
/*
        min_value=[0];
        max_value=[100];
        default_value=[0];
*/
        MinValue(oi.GetSingle("min_value"));
        MaxValue(oi.GetSingle("max_value"));
        DefaultValue(oi.GetSingle("default_value"));

        /********** Refresh Interval **********/
        RefreshInterval(oi.GetSingle("refresh_interval"));

        /********** Templates em Xml **********/
        LoadXmlTemplate(oi.GetSingle("design_template"));

        /********** Load Xml **********/
        string xmlFileName = GenericFrameSettings.BuildXmlFilePath(oi.GetSingle("xml_file"), frame.IDPage);

        AddXmlFile(xmlFileName);

        /********** Measure Type **********/
        MeasureType(oi.GetSingle("measure_type"));

        /********** Tick Marks **********/
        SetTickMarks(
                    oi.GetList("border_marks_values"),
                    oi.GetList("border_marks_colors")
                );

        /********** Set Threshold **********/
/*
        ok_color=[transparent];
        warning_color=[yellow];
        critical_color=[red];

        meter_ok=[0,20];
        meter_warning=[40,60];
        meter_critical=[70,90];


        gauge_warning_start=[70];
        gauge_critical_start=[85];
*/
        BuildThreshold(oi, monType);

        /********** Change the needle size **********/
        ChangeGaugeNeedleSize(oi.GetSingle("needle_size"));

        /********** Show Mon Value **********/
        ShowValueInMon(oi.GetSingle("show_value_in_label").Equals("true", StringComparison.CurrentCultureIgnoreCase));

        GetMon();
    }

    /********************************************* MON STYLE *********************************************/

    /******************** Canvas ********************/
    private void CanvasSize(int width, int height)
    {
        cvs.Attributes.Add("width", Convert.ToString(width));
        cvs.Attributes.Add("height", Convert.ToString(height));
    }

    /******************** Mon ********************/
    private void SelectMon(string monType)
    {
        if (monType.ToLower() == "gauge")
        {
            _mon = new mon_gauge();
        }
        else if (monType.ToLower() == "meter")
        {
            _mon = new mon_meter();
        }
    }

    public void GetMon()
    {
        Generic.JavaScriptInjector(_mon.GetJavaScript(), false);
    }

    /******************** Mons Properties ********************/
    /// <summary>
    /// Builds the string to be userd in meter and gauge
    /// </summary>
    /// <param name="oi"></param>
    /// <param name="monType"></param>
    private void BuildThreshold(OptionItems oi, string monType)
    {
        string color_ok = oi.GetSingle("color_ok"); if (color_ok == string.Empty) color_ok = "transparent";
        string color_warning = oi.GetSingle("color_warning"); if (color_warning == string.Empty) color_warning = "yellow";
        string color_critical = oi.GetSingle("color_critical"); if (color_critical == string.Empty) color_critical = "red";

        if (monType == "meter")
        {
            string meter_ok = oi.GetSingle("meter_ok"); if (meter_ok == string.Empty) meter_ok = "0,20";
            string meter_warning = oi.GetSingle("meter_warning"); if (meter_warning == string.Empty) meter_warning = "21,60";
            string meter_critical = oi.GetSingle("meter_critical"); if (meter_critical == string.Empty) meter_critical = "61,99";

            //SetThreshold("[0,20,'blue'], [40,60,'green'], [70,90,'red']"); // Meter
            SetThreshold("[" + meter_ok + ",'" + color_ok + "'], [" + meter_warning + ",'" + color_warning + "'], [" + meter_critical + ",'" + color_critical + "']"); // Meter
        }
        else if (monType == "gauge")
        {
            string gauge_warning = oi.GetSingle("gauge_warning_start"); if (gauge_warning == string.Empty) gauge_warning = "50";
            string gauge_critical = oi.GetSingle("gauge_critical_start"); if (gauge_critical == string.Empty) gauge_critical = "80";

            //SetThreshold("[50,80],[transparent, yellow, red]"); // Gauge
            SetThreshold("[" + gauge_warning + "," + gauge_critical + "],[" + color_ok + ", " + color_warning + ", " + color_critical + "]"); // Gauge
        }

    }

    /// <summary>
    /// Change the tick marks values and colors
    /// </summary>
    private void SetTickMarks(List<string> tickMarkValues, List<string> tickMarkColors)
    {
        _mon.SetTickMarks(tickMarkValues, tickMarkColors);
    }

    /// <summary>
    /// Sends the threshold to class...
    /// </summary>
    /// <param name="threshold"></param>
    private void SetThreshold(string threshold)
    {
        _mon.SetThreshold(threshold);
    }

    private void MeasureType(string type)
    {
        _mon.MeasureType = type;
    }

    private void AddXmlFile(string filePath)
    {
        _mon.XmlFileName = filePath;
    }

    private void LoadXmlTemplate(string name)
    {
        if (name == string.Empty) return;

        string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

        _mon.TemplateFolder = sitePath + @"\template\mon";

        _mon.LoadTemplate(name.Contains(".xml") ? name : name + ".xml");
    }

    private void RefreshInterval(string value)
    {
        try 
	    {
            if (value != string.Empty)
                _mon.RefreshInterval = Convert.ToInt32(value);
	    }
	    catch { }    
    }

    private void MinValue(string value)
    {
        try
        {
            if (value != string.Empty)
            _mon.MinValue = Convert.ToInt32(value);
        }
        catch {}
    }

    private void MaxValue(string value)
    {
        try
        {
            if (value != string.Empty)
                _mon.MaxValue = Convert.ToInt32(value);
        }
        catch { }
    }

    private void DefaultValue(string value)
    {
        try
        {
            if (value != string.Empty)
                _mon.DefaultValue = Convert.ToInt32(value);
        }
        catch { }
    }

    /// <summary>
    /// Add a select filter to xml data
    /// </summary>
    /// <param name="filter"></param>
    private void SelectFilter(string filter)
    {
        _mon.SelectFilter = filter;
    }

    private void ChangeGaugeNeedleSize(string needlesize)
    {
        _mon.ChangeNeedleSize(needlesize);
    }

    /// <summary>
    /// Show the mon value, not just the needle
    /// </summary>
    /// <param name="show"></param>
    private void ShowValueInMon(bool show)
    {
        _mon.ShowValue(show);
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