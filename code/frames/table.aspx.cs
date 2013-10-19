using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using Views.Frames;
using DbConfig;

public partial class frames_table : System.Web.UI.Page
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
            loging.Error("table", "load page", ex.Message, ViewState["log_file"].ToString());
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading table</p>\n");

        Server.ClearError();

        loging.Error("table", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
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

        // ------ Border ------
        // Border is defined in draw_frames class, sets the border of the panel outside de iframe.

        // ------ Table ------
        int rowsCount = -1;
        
        // file name is here to be used outside the try catch
        string xmlFilePath = string.Empty;

        try
        {
            string defaultFilter = oi.GetSingle("default_filter"); /*** default filter => string defaultFilter = "select SUBESTADO, count as qtd group by SUBESTADO";  ***/
            string xmlFileName = oi.GetSingle("xml_file").Trim();

            // set real folder
            xmlFilePath = GenericFrameSettings.BuildXmlFilePath(xmlFileName, frame.IDPage);

            // set virtual folder
            lbDownload.HRef = GenericFrameSettings.BuildXmlFileVirtualPath(xmlFileName, frame.IDPage); 
            
            // load data from xml file
            mGridView.DataSource = GenericFrameSettings.LoadXmlData(
                                                                    xmlFilePath, 
                                                                    defaultFilter, 
                                                                    frame.IDPage.ToString(), 
                                                                    oi.GetSingle("master_filter").Trim()
                                                                );

            rowsCount = ((DataView)mGridView.DataSource).Count;
        }
        catch (Exception ex)
        {
            loging.Error("table", "load data", ex.Message, ViewState["log_file"].ToString());

            /** Hide when error **/
            LabelCountVisible(true);
            LabelDownloadVisible(true);
            FiltersVisibility(true);
        }

        if (rowsCount >= 0)
        {
            try
            {
                /********** Css Style **********/
                mGridView.CssClass = "gridview";
                mGridView.AlternatingRowStyle.CssClass = "alt";
                mGridView.PagerStyle.CssClass = "pgr";

                /********** Sort **********/
                GridView_Sort(oi.GetSingle("table_sort").Trim().ToLower().Equals("false"));

                /********** Paging **********/
                GridView_Paging(oi.GetSingle("table_paging"), rowsCount);

                /********** GridviewHeader **********/
                GridView_Header(oi.GetSingle("table_show_header").Trim().ToLower().Equals("false"));

                /********** GridView Width **********/
                GridView_Width(oi.GetSingle("table_width"));

                /********** Filter **********/
                FiltersVisibility(oi.GetSingle("filters_visible").Trim().ToLower().Equals("false"));

                /********** Color Markers **********/
                AddColorMarkers(oi.GetList("color_markers"));
                //AddColorMarkers(new List<string> { });

                /********** Rows Count **********/
                LoadLabelCount(rowsCount);
                LabelCountVisible(oi.GetSingle("label_count_visible").Trim().ToLower().Equals("false"));

                /********** Show Xml File lastupd **********/
                LabelFileLastUpdVisible(oi.GetSingle("show_xml_file_lastupd").Trim().ToLower().Equals("true"));
                LoadLabelFileLastUpd(xmlFilePath);

                /********** Downloads **********/
                LabelDownloadVisible(oi.GetSingle("label_download_visible").Trim().ToLower().Equals("false"));
                

                /********** Table Color Alarms **********/
                TableColorAlarms(oi.GetList("warning_text"), 
                                oi.GetList("critical_text"), 
                                oi.GetSingle("warning_color"),
                                oi.GetSingle("critical_color"));

                /********** Hyperlinks **********/
                CreateHyperlink(frame.IDPage, oi.GetList("hyperlinks"), oi.GetSingle("hyperlink_color"));

            }
            catch (Exception ex)
            {
                loging.Error("table", "load options", ex.Message, ViewState["log_file"].ToString());
            }

            mGridView.DataBind(); // no fim de correr todas as opções faz o databind...
        }
    }

    /********************************************* TABLE STYLE *********************************************/

    /******************** Label Download ********************/
    private void LabelDownloadVisible(bool show)
    {
        lbDownload.Visible = !show;
    }
    /**************************************************/

    /******************** Label Counter ********************/
    private void LoadLabelCount(int value)
    {
        lbCount.Text = "Count: " + value;
    }

    private void LabelCountVisible(bool show)
    {
        lbCount.Visible = !show;
    }
    /**************************************************/

    /******************** Xml File LastUpd ********************/
    private void LoadLabelFileLastUpd(string fileName)
    {
        try
        {
            if (lbFileLastUpd.Visible)
                lbFileLastUpd.Text = "LastUpd: " + File.GetLastWriteTime(fileName).ToString("yyyy-MM-dd hh:mm:ss");
        }
        catch (Exception ex)
        {
            throw new Exception("Error reading file last modified date - " + ex.Message);
        }
    }

    private void LabelFileLastUpdVisible(bool show)
    {
        lbFileLastUpd.Visible = show;
    }

    /******************** Paging ********************/
    private void GridView_Paging(string numberPages, int rowCount)
    {
        int numPages = 0;


        if (numberPages != string.Empty && numberPages != "-1")
        {
            try { numPages = Convert.ToInt32(numberPages); } catch { }
        }
        else if (numberPages != "-1" && rowCount > 500)
            numPages = 100;

        if (numPages > 0)
        {
            mGridView.AllowPaging = true;
            mGridView.PageSize = numPages;
            mGridView.PageIndexChanging += mGridView_PageIndexChanging;
        }
    }

    private void mGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView gv = (GridView)sender;
        gv.PageIndex = e.NewPageIndex;
        gv.DataBind();
    }
    /**************************************************/

    /******************** Show/Hide GridView Header ********************/
    private void GridView_Header(bool show)
    {
        mGridView.ShowHeader = !show; // if false then want to show if true you want to hide
    }
    /**************************************************/

    /******************** GridView Width ********************/
    private void GridView_Width(string width)
    {
        try
        {
            if (width != string.Empty)
                mGridView.Width = Convert.ToInt32(width);
        }
        catch { }
    }
    /**************************************************/

    /******************** Sort ********************/
    private void GridView_Sort(bool show)
    {
        show = !show;

        mGridView.AllowSorting = show;

        if (show.Equals(true)) 
            mGridView.Sorting += mGridView_Sorting;
    }

    private void mGridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            GridView gv = (GridView)sender;
            DataView dataView = gv.DataSource as DataView;

            if (dataView != null)
            {
                dataView.Sort = getSortDirection(e.SortExpression, e.SortDirection);

                gv.DataSource = dataView;
                gv.DataBind();
            }
        }
        catch (Exception ex)
        {
            loging.Error("table", "sorting", ex.Message, ViewState["log_file"].ToString());
        }
    }

    private string getSortDirection(string SortExpression, SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;

        string direction;

        if (ViewState["gv_sortdirection"] == null)
            ViewState["gv_sortdirection"] = "DESC"; 

        direction = ViewState["gv_sortdirection"].ToString(); 

        if (direction == "DESC")
            direction = "ASC";
        else 
            direction = "DESC";

        ViewState["gv_sortdirection"] = direction;

        return SortExpression + " " + direction;
    }
    /**************************************************/

    /******************** Color Markers ********************/
    private void AddColorMarkers(List<string> listColors)
    {
        if (listColors.Count > 0)
        {
            color_markers cm = new color_markers(listColors);

            ViewState["color_maker"] = cm;
        }
    }

    private void ColorMarkerColumnIndexs(TableCellCollection cells)
    {
        List<Dictionary<int, Color>> columnIndexList = new List<Dictionary<int, Color>>();

        if (ViewState["color_maker"] != null)
        {
            color_markers cm = (color_markers)ViewState["color_maker"];

            for (int i = 0; i < cm.Count; i++)
            {
                string auxColumnName = cm.GetName(i).ToLower();
                Color auxColumnColor = cm.GetColor(i);

                for (int j = 0; j < cells.Count; j++)
                {
                    DataControlFieldCell cell = (DataControlFieldCell)cells[j];

                    if (cell.ContainingField.ToString().ToLower() == auxColumnName)
                    {
                        columnIndexList.Add(new Dictionary<int, Color> { { j, auxColumnColor } });
                    }
                }
            }

            // creates an object that in can be used in the row data bound that is faster 
            ViewState["color_maker_column_index"] = columnIndexList;
        }
    }
    /**************************************************/

    /******************** Table Alarms ********************/
    private void TableColorAlarms(List<string> warning, List<string> critical, string warningColor, string criticalColor)
    {
        if (warning.Count > 0 || critical.Count > 0)
        {
            table_color_alarms tca = new table_color_alarms(warning, critical);

            if (criticalColor != string.Empty)
                tca.CriticalColor = ColorTranslator.FromHtml(criticalColor);
            
            if (warningColor != string.Empty)
                tca.WarningColor = ColorTranslator.FromHtml(warningColor);

            ViewState["table_alarms"] = tca;
        }
    }
    /**************************************************/

    /******************** Filters ********************/
    private string GetFilters()
    {
        string fitlers = string.Empty;

        if (ViewState["gv_filters"] == null)
            ViewState["gv_filters"] = string.Empty;
        else
            fitlers = ViewState["gv_filters"].ToString();

        return fitlers;
    }

    private int SetFilters()
    {
        try
        {
            DataView dataView = mGridView.DataSource as DataView;
            
            if (dataView != null)
            {
                dataView.RowFilter = GetFilters();

                mGridView.DataSource = dataView;
                mGridView.DataBind();

                return dataView.Count;
            }

        }
        catch (Exception ex)
        {
            loging.Error("table", "Set Filters", ex.Message, ViewState["log_file"].ToString());
        }

        return -1;
    }

    private void FiltersVisibility(bool show)
    {
        show = !show;

        lbFilterText.Visible = show;
        tbFilters.Visible = show;
        btFilters.Visible = show;
        btClearFilter.Visible = show;
    }
    /**************************************************/

    /*** filter buttons ***/
    protected void btFilters_Click(object sender, EventArgs e)
    {
        ViewState["gv_filters"] = tbFilters.Text;
        LoadLabelCount(SetFilters());   
    }

    protected void btClearFilters_Click(object sender, EventArgs e)
    {
        ViewState["gv_filters"] = "";

        Generic.PageRefresh();
    }
    /**************************************************/

    /******************** Row Create/DataBound ********************/
    protected void mGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*** Table Alarms ***/
            if (ViewState["table_alarms"] != null)
            {
                if (mGridView.HeaderRow.Cells[0].Visible) 
                    mGridView.HeaderRow.Cells[0].Visible = false;

                e.Row.Cells[0].Visible = false; // hide me

                table_color_alarms tca = (table_color_alarms)ViewState["table_alarms"];

                if (tca.CheckIfCritical(e.Row.Cells[0].Text))
                    e.Row.BackColor = tca.CriticalColor;
                else if (tca.CheckIfWarning(e.Row.Cells[0].Text))
                    e.Row.BackColor = tca.WarningColor;
            }
            /********************/

            /*** Color Markers *** NOTE: does not have code beauty :( */
            if (ViewState["color_maker_column_index"] != null)
            {
                List<Dictionary<int, Color>> columnIndexList = (List<Dictionary<int, Color>>) ViewState["color_maker_column_index"];

                foreach (Dictionary<int, Color> t in columnIndexList)
                {
                    int pos = t.Keys.First();
                    Color clr = t.Values.First();

                    e.Row.Cells[pos].BackColor = clr;
                }
            }
            /**********************************************************/
        }
    }

    protected void mGridView_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            // its firts row then we have to build column index we have to do here because here the datatable is binded to the gridview
            if (ViewState["color_maker_column_index"] == null && ViewState["color_maker"] != null)
                ColorMarkerColumnIndexs(e.Row.Cells);
        }
    }
    /**************************************************/

    /******************** Dynamic Hyperlink ********************/
    /// <summary>
    /// receive list of elements that can be used to create a master filter
    /// 
    /// hyperlink_color     = its the color that is going to be used my the hyperlinks if none selected it will be used #F0F0F0
    /// 
    /// hyperlink=[	
    ///     [column name, page name, master filter name, condition]
    ///
    ///     [req, analise_x, dados externos, request_num='value'],
    ///     [status, analise_x, dados externos, min_status='value'],
    /// ];
    ///  
    /// column name         = table column name
    /// 
    /// page name           = destination page name
    /// 
    /// master filter name  = its the master filter title that is created in 
    ///                       the destination frame like this option in table frame 
    ///                       "master_filter=[master filter name];"
    /// 
    /// filter condition    = address = 'value'
    /// 
    /// </summary>
    private void CreateHyperlink(int thisPageId, List<string> listHyperlinks, string defaultColor)
    {
        DynamicHyperlink dh = new DynamicHyperlink(listHyperlinks, thisPageId);

        string js = dh.BuildJavascript();

        if (js != string.Empty)
        {
            Generic.JavaScriptInjector("dynamic_filters", js, false);

            // if color markers does not exists will create the necessary else will use the color markers that the user created
            color_markers cm = ViewState["color_maker"] != null
                                                        ? (color_markers)ViewState["color_maker"]
                                                        : new color_markers();

            // if user does not define default hyperlink color then it will be define by this function
            if (defaultColor == string.Empty) defaultColor = "#F0F0F0";

            foreach (string column in dh.ColumnNames)
                cm.AddColorMarker(column, ColorTranslator.FromHtml(defaultColor), false);

            ViewState["color_maker"] = cm;
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