using System;
using System.Collections.Generic;
using System.Globalization;
using DbConfig;
using Views.Frames;

public partial class frames_url : System.Web.UI.Page
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
            loging.Error("url", "load page", ex.Message, ViewState["log_file"].ToString());
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading url</p>\n");

        Server.ClearError();

        loging.Error("url", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
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

            // ------ Load Javascript ------
            SetTitlePosition(); // loads javascript that controls title position
        }
        else
        {
            lbTitleContainer.Style.Add("display", "none");
        }
        
        string url = oi.GetSingle("url");
        
        // if url does not exists does noting
        if (url != string.Empty)
        {
            /********** Url **********/
            AddUrlToIframe(url);

            /********** Width, Height **********/
            string newWidth = oi.GetSingle("width");
            string newHeight = oi.GetSingle("height");

            SetIframeSize(
                    newWidth != string.Empty ? newWidth : (frame.Width).ToString(CultureInfo.InvariantCulture),
                    newHeight != string.Empty ? newHeight : (frame.Height - 10).ToString(CultureInfo.InvariantCulture)
                );
        }
    }

    /******************** Properties ********************/
    private void AddUrlToIframe(string urlStr)
    {
        url.Attributes.Add("src", urlStr);
    }

    private void SetIframeSize(string width, string height)
    {
        if (!width.Contains("%"))
            if (!width.Contains("px"))
                width = width.Trim() + "px";

        if (!height.Contains("%"))
            if (height.Contains("px"))
                height = height.Trim() + "px";

        url.Attributes.Add("width", width);
        url.Attributes.Add("height", height);
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