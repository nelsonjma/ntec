using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;

using Views.Frames;
using DbConfig;

public partial class frames_filters : System.Web.UI.Page
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
            loging.Error("filters", "load page", ex.Message, ViewState["log_file"].ToString());
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading filter</p>\n");

        Server.ClearError();

        loging.Error("filters", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
    }

    private void Startup(Frame frame)
    {
        // replace log file so that is one frame to one log
        ReplaceLogFile(frame.IDPage, frame.ID);
        
        OptionItems oi = new OptionItems(frame.Options);

        // ------ Title ------
        headerTitle.Text = frame.Title;

        
        if (!oi.GetSingle("title_is_hidden").Equals("true")) // hide title if value is true, else will always show the title
        {
            lbTitle.InnerText = frame.Title;
            TitleStyle(oi.GetList("title_style")); // title style
        }
        else
        {
            lbTitleContainer.Style.Add("display", "none");
        }

        string filterTitle = frame.Title;
        int pageId = frame.IDPage;

        ViewState["filter_title"] = filterTitle;
        ViewState["filter_pageid"] = pageId.ToString();

        // ------ Load Javascript ------
        SetTitlePosition();

        /******************/

        List<string> listColummns = oi.GetList("columns_to_filter");

/*
        List<string> listColummns = new List<string>();

        listColummns.Add("SUBESTADO");
        listColummns.Add("ALT_DIV");
        listColummns.Add("CENARIO");
        listColummns.Add("SERVICODESC");
*/
        /******************/

        if (ViewState["list_columns_" + Generic.GetHash(filterTitle)] == null)
            ViewState["list_columns_" + Generic.GetHash(filterTitle)] = listColummns;

        bool isVertical = oi.GetSingle("vertical_filtes").Equals("true", StringComparison.CurrentCultureIgnoreCase);

        LoadData ld = new LoadData
        {
            PageId = pageId,
            Datafile = oi.GetSingle("datafile"),
            Datatable = oi.GetList("datatable"),
            DefaultFilter = oi.GetSingle("default_filter"),
            FileName = oi.GetSingle("xml_file"),
            MasterFilterId = oi.GetSingle("master_filter").Trim()
        };

        if (ld.FileName != string.Empty || ld.Datafile != string.Empty)
            BuildFilters(
                        ld,
                        listColummns, 
                        filterTitle,
                        isVertical
                    );
    }

    /********* Filter *********/
    //private void BuildFilters(string xmlFile, List<string> listColummns, string pageId, string filterTitle, bool isVertical)
    private void BuildFilters(LoadData ld, List<string> listColummns, string filterTitle, bool isVertical)
    {
        Table t = new Table();
        TableRow tr = new TableRow();

        string pageId = ld.PageId.ToString();

        // transform the ids into hash ids to send the column to memory or to get them from memory
        string filterTitleHash  = Generic.GetHash(filterTitle);
        string pageIdHash = Generic.GetHash(pageId);

        bool isDataFileChanged = IsDataFileChanged(ld.GetFilePath(), pageIdHash, filterTitleHash);
        bool areFiltersNull     = AreFiltersNull(listColummns, pageIdHash, filterTitleHash);

        if (isDataFileChanged || areFiltersNull) // if first time that the page loads or xml file has changed
        {
            DataView dv = new DataView();

            try
            {
                dv = ld.GetData();
            }
            catch (Exception ex)
            {
                loging.Error("filter", "load xml file", ex.Message, ViewState["log_file"].ToString());
            }

            if (dv.Count > 0)
            {
                foreach (string column in listColummns)
                {
                    try
                    {
                        string columnHash = Generic.GetHash(column);
                        // ----------------------------
                        combobox_filter cbf = new combobox_filter(pageId, filterTitle, column, dv);   // generate combobox

                        filter_sessions.SetFilterItem(pageIdHash, filterTitleHash, columnHash, cbf); // send the columns to memory

                        tr.Cells.Add(cbf.GetComboBox());

                        // set vertical filter alignment
                        if (isVertical) { t.Rows.Add(tr); tr = new TableRow(); }
                    }
                    catch (Exception ex)
                    {
                        loging.Error("filter", "create column", ex.Message, ViewState["log_file"].ToString());
                    }
                }
            }
        }
        else // else will load the data from session variables to have faster access.
        {
            foreach (string column in listColummns)
            {
                try
                {
                    string columnHash = Generic.GetHash(column);
                    // ----------------------------
                    combobox_filter cbf = (combobox_filter)filter_sessions.GetFilterItem(pageIdHash, filterTitleHash, columnHash); // get the columns from memory 

                    tr.Cells.Add(cbf.GetComboBox());

                    // set vertical filter alignment
                    if (isVertical) { t.Rows.Add(tr); tr = new TableRow(); }
                }
                catch (Exception ex)
                {
                    loging.Error("filter", "load column", ex.Message, ViewState["log_file"].ToString());
                }
            }
        }

        // by default is horizontal
        if (!isVertical) { t.Rows.Add(tr); }

        filterPanel.Controls.Add(t); 
    }

    private bool IsDataFileChanged(string file, string pageIdHash, string filterTitleHash)
    {
        try
        {
            DateTime dtFile = File.GetLastWriteTime(file);

            object fileDate = filter_sessions.GetDataFileLastUpd(pageIdHash, filterTitleHash);

            if (fileDate != null)
            {
                DateTime sessionDt = (DateTime)fileDate;

                if (sessionDt.ToFileTimeUtc() == dtFile.ToFileTimeUtc())
                    return false;
            }

            filter_sessions.SetDataFileLastUpd(pageIdHash, filterTitleHash, dtFile);
        }
        catch (Exception ex)
        {
            loging.Error("filter", "is xml file changed", ex.Message, ViewState["log_file"].ToString());
        }

        return true;
    }

    private bool AreFiltersNull(List<string> listColummns, string pageIdHash, string filterTitleHash)
    {
        foreach (string colummn in listColummns)
        {
            try
            {
                string columnHash = Generic.GetHash(colummn);

                object filterItem = filter_sessions.GetFilterItem(pageIdHash, filterTitleHash, columnHash);

                if (filterItem == null)
                    return true;
            }
            catch (Exception ex)
            {
                loging.Error("filter", "is session filter null", ex.Message, ViewState["log_file"].ToString());
            }
        }

        return false;
    }

    private string GetFilters()
    {
        try
        {
            string filters = string.Empty;

            string pageId = (string)ViewState["filter_pageid"];
            string pageIdHash = Generic.GetHash(pageId);

            string filterTitle = (string)ViewState["filter_title"];
            string filterTitleHash = Generic.GetHash(filterTitle);

            List<string> listColumns = (List<string>)ViewState["list_columns_" + filterTitleHash];

            // build query of master filter
            foreach (string column in listColumns)
            {
                try
                {
                    string columnHash = Generic.GetHash(column);

                    object filterItem = filter_sessions.GetFilterItem(pageIdHash, filterTitleHash, columnHash);

                    // if notting to do leave
                    if (filterItem == null) continue;

                    string filter = ((combobox_filter)filterItem).GetSelectedFilter();

                    filters += !filter.Equals(string.Empty) ? ((!filters.Equals(string.Empty)) ? " and " : string.Empty) + filter : string.Empty;
                }
                catch (Exception ex)
                {
                    loging.Error("filter", "run filters - get filter", ex.Message, ViewState["log_file"].ToString());
                }
            }

            filter_sessions.SetMasterFilterString(pageIdHash, filterTitleHash, filters);

            return filterTitle;
        }
        catch (Exception ex)
        {
            loging.Error("filter", "run filters", ex.Message, ViewState["log_file"].ToString());
        }

        return string.Empty;
    }

    private string ClearFilters()
    {
        try
        {
            string pageId = (string)ViewState["filter_pageid"];
            string pageIdHash = Generic.GetHash(pageId);

            string filterTitle = (string)ViewState["filter_title"];
            string filterTitleHash = Generic.GetHash(filterTitle);

            List<string> listColumns = (List<string>)ViewState["list_columns_" + filterTitleHash];

            foreach (string colummn in listColumns)
            {
                try
                {
                    string columnHash = Generic.GetHash(colummn);

                    object filterItem = filter_sessions.GetFilterItem(pageIdHash, filterTitleHash, columnHash);

                    if (filterItem != null)
                    {
                        ((combobox_filter)filterItem).ClearFilter();
                    }
                }
                catch {}
            }

            filter_sessions.SetMasterFilterString(pageIdHash, filterTitleHash, string.Empty);

            return filterTitle;
        }
        catch (Exception ex)
        {
            loging.Error("filter", "clear filters", ex.Message, ViewState["log_file"].ToString());
        }

        return string.Empty;
    }

    /********* Buttons *********/
    protected void btFilter_Click(object sender, EventArgs e)
    {
        string masterFilterTitle = GetFilters();

        Generic.ParentJavascriptMethodInjector("RefreshSlaves('" + masterFilterTitle + "')"); // refresh slaves
    }

    protected void btClear_Click(object sender, EventArgs e)
    {
        string masterFilterTitle = ClearFilters();

        Generic.ParentJavascriptMethodInjector("RefreshSlaves('" + masterFilterTitle + "')"); // refresh slaves
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