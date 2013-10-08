using System;
using System.Globalization;

using DbConfig;
using Views.BackOffice;

public partial class bo_pages_cfg_master_page : System.Web.UI.Page
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
            loging.Error("master page configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
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
            LoadMasterPage(Request.QueryString["id"]);
    }

    protected void Save_OnClick(object sender, EventArgs e)
    {
        try
        { 
            int id = -1; try { id = (Request.Form["mp_id"] != null && Request.Form["mp_id"] != "") ? Convert.ToInt32(Request.Form["mp_id"]) : -1; } catch { }

            string title = Request.Form["mp_title"];
            string desc = Request.Form["mp_desc"];
            string options = Request.Form["mp_options"];

            SaveMasterPage(id, title, desc, options);
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /************************ Configurations ************************/

    private void SetPageTitle(string title)
    {
        masterPageTitle.InnerHtml = title + " Configuration";
    }

    private void LoadMasterPage(string id)
    {
        try
        {
            int mpId = Convert.ToInt32(id);

            db_config_master_page dcm = new db_config_master_page();
            dcm.Open();

            MasterPage mp = dcm.Get(mpId);

            mp_id.Value = mp.ID.ToString(CultureInfo.InvariantCulture);
            mp_title.Value = mp.Title;
            mp_desc.InnerText = mp.Description;
            mp_options.InnerText = mp.Options;

            SetPageTitle(mp.Title);

            dcm.Close();
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /// <summary>
    /// Save current master page that user is editing
    /// </summary>
    private static void SaveMasterPage(int id, string title, string desc, string options)
    {
        try
        {            
            db_config_master_page dcm = new db_config_master_page();
            dcm.Open();

            MasterPage mp = id >= 0 
                            ? dcm.Get(id) 
                            : new MasterPage();

            mp.Title = title;
            mp.Description = desc;
            mp.Options = options;

            if (id >= 0)
                dcm.Commit();
            else
                dcm.Add(mp);

            dcm.Close();

            Generic.ParentJavascriptMethodInjector("RefreshTree()");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }
}