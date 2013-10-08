using System;
using System.IO;
using System.Web.UI.WebControls;

using Views.BackOffice;

public partial class bo_site_cfg_template_editor : System.Web.UI.Page
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
            loging.Error("template configuration", "load template page error", ex.Message, ViewState["log_file"].ToString());
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
            LoadTemplateDropDownList(BuildTemplatePath(template));
        }
    }

    protected void SaveTemplate_OnClick(object sender, EventArgs e)
    {
        try
        {
            string cfgData = Request.Form["templatecfg"];
            string selectedFile = Request.Form["selectedTemplateFile"];

            SaveTemplateFile(cfgData, selectedFile);

            Generic.JavaScriptInjector("alert('Css File Saved');");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /************************ Configurations ************************/
    /// <summary>
    /// Load Template Files to dropdownlist
    /// </summary>
    private void LoadTemplateDropDownList(string templatePath)
    {
        string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

        if (sitePath == null) return; // leave if sitepath is not readable

        sitePath = sitePath.EndsWith(@"\") ? sitePath + templatePath
                                           : sitePath + @"\" + templatePath;

        string[] xmlFiles = Directory.GetFiles(sitePath, "*.xml");

        // clear the option list because multiple refresh.
        try { if (templateSelector.Items.Count > 0) templateSelector.Items.Clear(); } catch { }

        foreach (string xmlFile in xmlFiles)
        {
            string auxCssFile = xmlFile.Contains(@"\")
                                    ? xmlFile.Substring(xmlFile.LastIndexOf(@"\", StringComparison.Ordinal) + 1)
                                    : xmlFile;

            templateSelector.Items.Add(new ListItem(auxCssFile, auxCssFile));
        }
    }

    /// <summary>
    /// Build template path and loads the hidden input text box
    /// </summary>
    /// <param name="headerVar"></param>
    /// <returns></returns>
    private string BuildTemplatePath(string headerVar)
    {
        if (headerVar.Trim().ToLower() == "chart_template")
        {
            templateTitle.InnerHtml = "Chart Template Controls";

            templateType.Value = "chart_template";
            return @"template\charts\";
        }

        templateTitle.InnerHtml = "Monitorization Objects Template Controls";

        templateType.Value = "mon_template";
        return @"template\mon\";    
    }

    /// <summary>
    /// Returns the path of the selected template
    /// </summary>
    /// <returns></returns>
    private string SelectTemplatePath()
    {
        return Request.Form["templateType"] == "chart_template" 
                                                    ? @"template\charts\" 
                                                    : @"template\mon\";
    }

    /// <summary>
    /// Write xml data in textarea to template filename
    /// </summary>
    private void SaveTemplateFile(string data, string fileName)
    {
        try
        {
            if (fileName == string.Empty || data == string.Empty) throw new Exception("error: nothing to save");

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return;

            string templatePath = SelectTemplatePath();

            sitePath = sitePath.EndsWith(@"\")
                                            ? sitePath + templatePath
                                            : sitePath + @"\" + templatePath;

            File.WriteAllText(sitePath + fileName, data);
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Template File: " + ex.Message);
        }
    }
}