using System;
using System.IO;
using System.Web.UI.WebControls;

using DbConfig;

using Views.BackOffice;

public partial class bo_cfg_frontoffice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["log_file"] = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

        try
        {
            Startup();
        }
        catch (Exception ex)
        {
            loging.Error("frontoffice configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
        }   
    }

    private void Startup()
    {
        // redirect users if they are HACKERS or NOT
        GenericBackOfficeSettings.RedirectUsersToMainPage();  // desligado para poder trabalhar...

        // definir uma forma mais segura ou webservice mais complexo...
        string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));
        webServiceKey.Attributes.Add("value", hashKey);

        if (!IsPostBack)
        {
            LoadCssDropDownList();
        }
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        try
        {
            string csscfgData = Request.Form["csscfg"];
            string selectCssFile = Request.Form["selectedCssFile"];
            string optionsData = Request.Form["options"];

            SaveCssFile(csscfgData, selectCssFile);
            SaveOptions(optionsData);

            Generic.JavaScriptInjector("alert('Options/Css Saved'); window.location.reload();");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
        
    }

    protected void Load_Click(object sender, EventArgs e)
    {
        Generic.PageRefresh();
    }

    /************************ Configurations ************************/
    /// <summary>
    /// Load css Files to dropdownlist
    /// </summary>
    private void LoadCssDropDownList()
    {
        string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath; 
        
        if (sitePath == null) return; // leave if sitepath is not readable

        sitePath = sitePath.EndsWith(@"\") ? sitePath + @"css\views\frontoffice\"
                                           : sitePath + @"\css\views\frontoffice\";

        string[] cssFiles = Directory.GetFiles(sitePath, "*.css");

        try { if (cssSelector.Items.Count > 0) cssSelector.Items.Clear(); } catch { } // clear the option list because multiple refresh.

        foreach (string cssFile in cssFiles)
        {

            string auxCssFile = cssFile.Contains(@"\")
                                    ? cssFile.Substring(cssFile.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1)
                                    : cssFile;

            cssSelector.Items.Add(new ListItem(auxCssFile, auxCssFile));
        }
    }

    /// <summary>
    /// Write css data in textarea to css filename
    /// </summary>
    private static void SaveCssFile(string cssData, string cssFileName)
    {
        try
        {
            if (cssFileName == string.Empty || cssData == string.Empty) return;

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                
            if (sitePath == null) return;

            sitePath = sitePath.EndsWith(@"\") 
                ? sitePath + @"css\views\frontoffice\"
                : sitePath + @"\css\views\frontoffice\";

            File.WriteAllText(sitePath + cssFileName, cssData);
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Css File: " + ex.Message);
        }
    }

    /// <summary>
    /// Save information of options textarea
    /// </summary>
    private static void SaveOptions(string optionsData)
    {
        try
        {
            db_config_frontoffice dcf = new db_config_frontoffice();
            dcf.Open();

            dcf.Get.Options = optionsData;
            dcf.Commit();

            dcf.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Options: " + ex.Message);
        }
    }
}