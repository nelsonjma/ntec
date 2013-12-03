using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

using DbConfig;
using Views.BackOffice;

public partial class bo_pages_cfg_page : System.Web.UI.Page
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
            loging.Error("page configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
        }
    }

    private void Startup()
    {
        // redirect users if they are HACKERS or NOT
        GenericBackOfficeSettings.RedirectUsersToMainPage();  // desligado para poder trabalhar...

        // definir uma forma mais segura ou webservice mais complexo...
        string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));
        webServiceKey.Attributes.Add("value", hashKey);

        if (!IsPostBack && Request.QueryString["id"] != null)
        {
            LoadPage(Request.QueryString["id"]);
        } 
        else if (Request.QueryString["id"] == null)
        {
            NewPageDefaultValues();
        }

        if (!IsPostBack)
        {
            LoadCssDropDownList();
        }
    }

    protected void Save_OnClick(object sender, EventArgs e)
    {
        try
        {
            int id = -1; try { id = (Request.Form["p_id"] != null && Request.Form["p_id"] != "") ? Convert.ToInt32(Request.Form["p_id"]) : -1; } catch { }

            int mastePageId = Convert.ToInt32(Request.Form["p_masterpageid"]);
            string name = Request.Form["p_name"];
            string title = Request.Form["p_title"];
            string xmlFolder = Request.Form["p_xml_folder"];
            string xmlUrl = Request.Form["p_xml_url"];
            string options = Request.Form["p_options"];
            
            SavePage(id, mastePageId, name, title, xmlFolder, xmlUrl, options);

            Generic.ParentJavascriptMethodInjector("RefreshTree()");

            Generic.JavaScriptInjector("alert('Page Saved');");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
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
    private void SetPageTitle(string title)
    {
        pageTitle.InnerHtml = title + " Configuration";
    }

    /// <summary>
    ///  Build master page selector if ID  is negative then it will select first item
    /// </summary>
    private void BuildPageSelector(int currentMasterPageId)
    {
        db_config_master_page dcm = null;

        // Clear all items
        p_masterpageid.Items.Clear();

        try
        {
            dcm = new db_config_master_page();
            dcm.Open();

            List<MasterPage> allMasterPages = dcm.AllMasterPages.OrderBy(x=>x.Title).ToList();

            for (int i = 0; i < allMasterPages.Count; i++)
            {
                p_masterpageid.Items.Add(new ListItem(
                                                allMasterPages[i].Title,
                                                allMasterPages[i].ID.ToString(CultureInfo.InvariantCulture)
                                                ));

                // Get the page position then set the selector index
                if (allMasterPages[i].ID == currentMasterPageId)
                    p_masterpageid.SelectedIndex = i;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("error: building master page list " + ex.Message);
        }
        finally
        {
            if (dcm != null)
                dcm.Close();
        }
    }
    
    /// <summary>
    /// Load css Files to dropdownlist
    /// </summary>
    private void LoadCssDropDownList()
    {
        string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

        if (sitePath == null) return; // leave if sitepath is not readable

        sitePath = sitePath.EndsWith(@"\") ? sitePath + @"css\views\page\"
                                           : sitePath + @"\css\views\page\";

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

    private void LoadPage(string id)
    {
        try
        {
            int pId = Convert.ToInt32(id);

            db_config_page dcp = new db_config_page(pId, false);
            dcp.Open();

            DbConfig.Page p = dcp.Get(pId);

            p_id.Value = p.ID.ToString(CultureInfo.InvariantCulture);
            p_name.Value = p.Name;
            p_title.Value = p.Title;
            p_xml_folder.Value = p.XMLFolderPath;
            p_xml_url.Value = p.XMLURL;
            p_options.Value = p.Options;

            BuildPageSelector(p.IDMasterPage);
            SetPageTitle(p.Title);

            dcp.Close();
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /// <summary>
    /// Default values necessary for guideline
    /// </summary>
    private void NewPageDefaultValues()
    {
        //string xmlFolder = Generic.GetWebConfigValue("SqlDataFolderPath");
        //string xmlUrl = Generic.GetWebConfigValue("SqlDataVirtualPath");
        //string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

        p_xml_folder.Value = "default";
        p_xml_url.Value = "default";

        BuildPageSelector(-1);
    }

    private static void SavePage(int id, int masterPageID, string name, string title, string xmlFolder, string xmlUrl, string options)
    {
        try
        {

            db_config_page dcp = new db_config_page();
            dcp.Open();

            Page p = id >= 0
                        ? dcp.Get(id)
                        : new Page();

            p.IDMasterPage = masterPageID;
            p.Name = name;
            p.Title = title;
            p.XMLFolderPath = xmlFolder;
            p.XMLURL = xmlUrl;
            p.Options = options;

            if (id >= 0)
                dcp.Commit();
            else
                dcp.Add(p);

            dcp.Close();
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Page: " + ex.Message);
        }
    }

    /// <summary>
    /// Write css data in textarea to css filename
    /// </summary>
    private static void SaveCssFile(string cssData, string cssFileName)
    {
        try
        {
            if (cssFileName == string.Empty || cssData == string.Empty) throw new Exception("error: nothing to save");

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return;

            sitePath = sitePath.EndsWith(@"\")
                ? sitePath + @"css\views\page\"
                : sitePath + @"\css\views\page\";

            File.WriteAllText(sitePath + cssFileName, cssData);
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Css File: " + ex.Message);
        }
    }

}