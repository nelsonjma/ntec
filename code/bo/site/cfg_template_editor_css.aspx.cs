using System;
using System.IO;
using System.Web.UI.WebControls;

using Views.BackOffice;

public partial class bo_site_cfg_page_css : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["log_file"] = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

        try
        {
            string template = Request.QueryString.Get("tmp");

            Startup(template);
        }
        catch (Exception ex)
        {
            loging.Error("css page configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
        }
    }

    private void Startup(string template)
    {
        // redirect users if they are HACKERS or NOT
        GenericBackOfficeSettings.RedirectUsersToMainPage();  // desligado para poder trabalhar...

        // definir uma forma mais segura ou webservice mais complexo...
        string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));
        webServiceKey.Attributes.Add("value", hashKey);

        if (!IsPostBack && template != null)
        {
            LoadCssDropDownList(BuildTemplatePath(template));
        }
    }

    protected void SaveCss_OnClick(object sender, EventArgs e)
    {
        try
        {
            string csscfgData = Request.Form["csscfg"];
            string selectCssFile = Request.Form["selectedCssFile"];

            SaveCssFile(csscfgData, selectCssFile);

            Generic.JavaScriptInjector("alert('Css File Saved');");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /************************ Configurations ************************/
    /// <summary>
    /// Load css Files to dropdownlist
    /// </summary>
    private void LoadCssDropDownList(string templatePath)
    {
        string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

        if (sitePath == null) return; // leave if sitepath is not readable

        sitePath = sitePath.EndsWith(@"\") ? sitePath + templatePath
                                                   : sitePath + @"\" + templatePath;

        string[] cssFiles = Directory.GetFiles(sitePath, "*.css");

        try { if (cssSelector.Items.Count > 0) cssSelector.Items.Clear(); } catch { } // clear the option list because multiple refresh.

        foreach (string cssFile in cssFiles)
        {

            string auxCssFile = cssFile.Contains(@"\")
                                    ? cssFile.Substring(cssFile.LastIndexOf(@"\", StringComparison.Ordinal) + 1)
                                    : cssFile;

            cssSelector.Items.Add(new ListItem(auxCssFile, auxCssFile));
        }
    }

    /// <summary>
    /// Write css data in textarea to css filename
    /// </summary>
    private void SaveCssFile(string cssData, string cssFileName)
    {
        try
        {
            if (cssFileName == string.Empty || cssData == string.Empty) throw new Exception("error: nothing to save");

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return;

            string templatePath = SelectTemplatePath();

            sitePath = sitePath.EndsWith(@"\")
                                            ? sitePath + templatePath
                                            : sitePath + @"\" + templatePath;

            File.WriteAllText(sitePath + cssFileName, cssData);
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Css File: " + ex.Message);
        }
    }

    /// <summary>
    /// Build template path and loads the hidden input text box
    /// </summary>
    /// <param name="headerVar"></param>
    /// <returns></returns>
    private string BuildTemplatePath(string headerVar)
    {
        switch (headerVar.Trim().ToLower())
        {
            case "frames":
                cssTemplateTitle.InnerHtml = "Frames CSS Template Controls";
                cssTemplateType.Value = "frames";
                return @"css\views\frames\";

            case "page":
                cssTemplateTitle.InnerHtml = "Page CSS Template Controls";
                cssTemplateType.Value = "page";
                return @"css\views\page\";

            case "frontoffice":
                cssTemplateTitle.InnerHtml = "Frontoffice CSS Template Controls";
                cssTemplateType.Value = "frontoffice";
                return @"css\views\frontoffice\";
        }

        return string.Empty;
    }

    /// <summary>
    /// Returns the path of the selected template
    /// </summary>
    /// <returns></returns>
    private string SelectTemplatePath()
    {
        switch (Request.Form["cssTemplateType"])
        {
            case "frames":
                return @"css\views\frames\";

            case "page":
                return @"css\views\page\";

            case "frontoffice":
                return @"css\views\frontoffice\";
        }

        return string.Empty;
    }


}