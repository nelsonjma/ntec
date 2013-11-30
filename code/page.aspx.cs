using System;
using System.Globalization;

using Views.Page;
using DbConfig;

public partial class page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["log_file"] = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

        try
        {
            string pageName = Request.QueryString.Get("nm");

            if (pageName != null)
            {
                Startup(pageName);
            }
        }
        catch (Exception ex)
        {
            loging.Error("frontoffice", "load page error", ex.Message, ViewState["log_file"].ToString());
        }   
    }

    private void Startup(string pageName)
    {
        db_config_page dcp = new db_config_page(pageName);
        dcp.Open();

        DbConfig.Page page = dcp.Get(pageName);

         PageAuthentication pa = new PageAuthentication(page.ID);

        if (!pa.IsPageVisible()) Response.Redirect("frontoffice.aspx");

        pageTitle.Text = page.Title;

        /*************** Options ****************/
        OptionItems oi = new OptionItems(page.Options);

        ChangeAlignment(oi.GetSingle("page_alignment"));

        ChangeCss(oi.GetSingle("css"));

        ChangeBackground(oi.GetSingle("background-color"));

        RefreshPage(oi.GetSingle("refresh_page_interval"));

        /*************** draw frames ****************/
        DrawFrames df = new DrawFrames(pageName, oi.GetSingle("page_type"));

        mPage.Controls.Add(df.GetFrames());
    }

    /********************************************* Options *********************************************/
    /******************** Page CSS ********************/
    /// <summary>
    /// Change CSS 
    /// </summary>
    /// <param name="cssFileName"></param>
    public void ChangeCss(string cssFileName)
    {
        if (cssFileName != string.Empty)
            cssStyle.Href = "~/css/views/page/" + cssFileName;
    }

    /******************** Page Background ********************/
    /// <summary>
    /// Change body background color
    /// </summary>
    /// <param name="bkColor"></param>
    public void ChangeBackground(string bkColor)
    {        
        if (bkColor != string.Empty) 
            pageBody.Style.Add("background-color", bkColor);
    }

    /******************** Inner Content Alignment ********************/
    /// <summary>
    /// Change page alignment but the old way using the align property
    /// </summary>
    /// <param name="align"></param>
    public void ChangeAlignment(string align)
    {
        if (align != string.Empty)
            posDiv.Attributes.Add("align", align);
    }
    /**************************************************/

    public void RefreshPage(string interval)
    {
        try
        {
            int res;

            // leave the method if it cant convert to integer.
            if (!int.TryParse(interval, out res)) return;

            int result = Convert.ToInt32(interval) * 60;

            refreshPage.Attributes.Add("content", result.ToString(CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            throw new Exception("error seting refresh page interval " + ex.Message);
        }        
    }
}