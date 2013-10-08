using System;
using System.Collections.Generic;
using System.Web.UI;

using DbConfig;
using Views.Frames;

public partial class text : System.Web.UI.Page
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
            loging.Error("free text", "load page", ex.Message, ViewState["log_file"].ToString());
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading free text</p>\n");

        Server.ClearError();

        loging.Error("free text", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
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

            // set title position with javascript
            SetTitlePosition();
        }
        else
        {
            lbTitleContainer.Style.Add("display", "none");
        }

        CustomJavascript(oi.GetSingle("custom_javascript"));

        CustomHtml(oi.GetSingle("custom_html"));

        CustomCss(oi.GetSingle("custom_css"));
    }

    // html injector
    private LiteralControl CustomCtrl(string ctrlData)
    {
        return new LiteralControl(ctrlData);
    }

    private LiteralControl TextBox(string data)
    {
        LiteralControl ctrl = new LiteralControl();

        ctrl.Text = "<h2>" + data + "</h2>";

        return ctrl;
    }

    /******************** HTML / Javascript / CSS ********************/
    /// <summary>
    /// adds html content using new LiteralControl
    /// </summary>
    private void CustomHtml(string data)
    {
        freeText.Controls.Add(CustomCtrl(data));
    }

    /// <summary>
    /// add javascript using RegisterStartupScript
    /// </summary>
    private void CustomJavascript(string data)
    {
        Generic.JavaScriptInjector("cjs", data);
    }

    /// <summary>
    /// add css by setting the style object has server side
    /// </summary>
    private void CustomCss(string data)
    {
        customStyle.InnerHtml = data;
    }
    /**************************************************/

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
    private void ChangeCss(string cssFileName)
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