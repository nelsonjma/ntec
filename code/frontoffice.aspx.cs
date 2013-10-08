using System;
using System.Collections.Generic;
using System.Linq;

using Views.Frontoffice;
using DbConfig;

public partial class frontoffice : System.Web.UI.Page
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
            loging.Error("frontoffice", "load frontoffice error", ex.Message, ViewState["log_file"].ToString());
        }        
    }

    private void Startup()
    {
        db_config_frontoffice dcf = new db_config_frontoffice();
        dcf.Open();

        OptionItems oi = new OptionItems(dcf.Get.Options);

        ChangeCss(oi.GetSingle("css"));

        FillMenuBar(oi);
    }

    /// <summary>
    /// Generate header bar
    /// </summary>
    private void FillMenuBar(OptionItems foOptions)
    {
        try
        {
            headerContainer.Controls.Clear();

            UserLoginData user = (UserLoginData)db_config_sessions.GetUserAuthentication();
            OptionItems userOi = null;

            db_config_master_page dcmp = new db_config_master_page(); dcmp.Open();

            List<string> userFavorites = new List<string>();

            // user not autenticated show just public pages
            if (user == null)
            {
                dcmp.SelectPublicObjectsFromDb();
            }
            else 
            {
                // user autenticated show public pages and user pages
                dcmp.SelectAuthenticatedObjectsFromDb(user.UserPages);
                userOi = new OptionItems(user.User.UserOptions);

                userFavorites = userOi.GetList("favorites");
            }

            // set url to use when the page start
            SelectDefaultPage(foOptions.GetSingle("default_page"), user != null
                                                                        ? userOi.GetSingle("default_frontoffice_page")
                                                                        : "");

            MenuBar mb = new MenuBar();
            mb.AddHeader("Home", "frontoffice.aspx");

            // just show data if is not refresh
            if (!IsPostBack)
            {
                mb.AddHeader("Site pages", ""); // site pages
                int headerId = mb.GetHeaderPosition("Site pages");

                db_config_page dcp;

                foreach (DbConfig.MasterPage item in dcmp.AllMasterPages)
                {
                    // just show pages that admin dont want to hide from you :)
                    List<DbConfig.Page> visiblePages = (from p in dcmp.GetAllPages(item.ID)
                                                        where (new OptionItems(p.Options).GetSingle("hidden_from_frontoffice").Equals("true")) == false
                                                        select p).ToList();


                    dcp = new db_config_page(dcmp.Db, visiblePages, item.ID);

                    if (user == null) 
                        dcp.SelectPublicObjectsFromDb();
                    else 
                        dcp.SelectAuthenticatedObjectsFromDb(user.UserPages);

                    // will just add master page if it has visible pages to show,
                    // if the master page does not have pages will not get here. 
                    if (dcp.AllPages.Count > 0)
                    {
                        // add master page to menu
                        mb.AddMenuItem(headerId, item.Title, "");
                        int menuItemId = mb.GetMenuPosition(headerId, item.Title);

                        // add pages to sub menus
                        foreach (DbConfig.Page subItem in dcp.AllPages)
                            mb.AddSubMenuItem(headerId, menuItemId, subItem.Title, "page.aspx?nm=" + subItem.Name);    
                    }

                    dcp.Close();
                }

                if (userFavorites.Count > 0) // favorites
                {
                    mb.AddHeader("Favorites", "");
                    int favItemId = mb.GetHeaderPosition("Favorites");

                    dcp = new db_config_page(dcmp.Db, new List<DbConfig.Page>(), -1); dcp.Refresh();

                    foreach (string favpage in userFavorites)
                    {
                        DbConfig.Page p = dcp.Get(favpage);

                        mb.AddMenuItem(favItemId, p.Title, "page.aspx?nm=" + p.Name);
                    }

                    dcp.Close();
                }
            }

            dcmp.Close();

            headerContainer.Controls.Add(mb.Get());
            Generic.JavaScriptInjector("js", mb.GetLoginJavascript());
        }
        catch (Exception ex)
        {
            throw new Exception("error: fill menu bar - " + ex.Message + " ...");
        }
    }

    /********************************************* Options *********************************************/

    /// <summary>
    /// Change CSS 
    /// </summary>
    /// <param name="cssFileName"></param>
    private void ChangeCss(string cssFileName)
    {
        if (cssFileName != string.Empty)
            cssStyle.Attributes.Add("href", "css/views/frontoffice/" + cssFileName);
    }

    /// <summary>
    /// Select if user default page or default front office page
    /// </summary>
    private void SelectDefaultPage(string frontOfficePage, string userPage)
    {
        OpenDefaultPage(userPage != string.Empty 
                                            ? userPage 
                                            : frontOfficePage
                        );
    }

    /// <summary>
    /// load iframe with frontoffice open url
    /// </summary>
    private void OpenDefaultPage(string data)
    {
        if (data == string.Empty) return; // dont do noting if its empty;

        string url = data.Contains("http://")
                                            ? data
                                            : "page.aspx?nm=" + data;

        pageContainer.Attributes.Add("src", url);
    }
}