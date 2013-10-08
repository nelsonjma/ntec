using System;

using DbConfig;
using Views.BackOffice;

public partial class bo_cfg_users : System.Web.UI.Page
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
            loging.Error("user configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
        }
    }

    private void Startup()
    {
        // redirect users if they are HACKERS or NOT
        GenericBackOfficeSettings.RedirectUsersToMainPage();  // desligado para poder trabalhar...

        // definir uma forma mais segura ou webservice mais complexo...
        string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));
        webServiceKey.Attributes.Add("value", hashKey);
    }

    protected void Save_OnClick(object sender, EventArgs e)
    {
        string id = Request.Form["uid"];
        string name = Request.Form["uname"];
        string pass = Request.Form["upass"];
        object admin = Request.Form["uadmin"];
        string desc = Request.Form["udesc"];
        string userOptions = Request.Form["u_options"];
        string adminOptions = Request.Form["u_admin_op"];

        if (admin != null) 
            admin = admin.Equals("on") ? 1 : 0; 
        else 
            admin = 0;

        SaveUser(id, name, pass, (int)admin, desc, userOptions, adminOptions);
    }

    protected void New_OnClick(object sender, EventArgs e)
    {
        string name = Request.Form["uname"];
        string pass = Request.Form["upass"];
        object admin = Request.Form["uadmin"];
        string desc = Request.Form["uname"];
        string userOptions = Request.Form["u_options"];
        string adminOptions = Request.Form["u_admin_op"];

        if (admin != null)
            admin = admin.Equals("on") ? 1 : 0;
        else
            admin = 0;

        AddNewUser(name, pass, (int)admin, desc, userOptions, adminOptions);
    }

    /************************ Configurations ************************/
    /// <summary>
    /// updating existing user data
    /// </summary>
    private static void SaveUser(string uId, string name, string pass, int admin, string desc, string uOptions, string aOptions)
    {
        try
        {
            int id = Convert.ToInt32(uId);

            db_config_users dcu = new db_config_users(id);
            dcu.Open(); // open connection

            Users user = dcu.Get(id);

            if (user.ID != id) return; // if id is diferent probabaly because user pressed save with no user selected

            user.Name = name;
            user.Pass = pass;
            user.AdMIn = admin;
            user.Description = desc;
            user.UserOptions = uOptions;
            user.AdMInOptions = aOptions;

            dcu.Commit();

            dcu.Close(); // close connection

            Generic.JavaScriptInjector("alert('User Saved'); window.location.reload();");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /// <summary>
    /// Add new user id field will be ignored
    /// </summary>
    private static void AddNewUser(string name, string pass, int admin, string desc, string uOptions, string aOptions)
    {
        try
        {
            db_config_users dcu = new db_config_users();
            dcu.Open(); // open connection

            // user with name or pass empty
            if (name == string.Empty || pass == string.Empty) { Generic.JavaScriptInjector("NewUserError", "alert('name or pass are empty');"); return; }
                
            Users user = new Users
            {
                Name = name,
                Pass = pass,
                AdMIn = admin,
                Description = desc,
                UserOptions = uOptions,
                AdMInOptions = aOptions
            };

            dcu.AddUser(user);

            dcu.Close(); // close connection

            Generic.JavaScriptInjector("alert('New user added'); window.location.reload();");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }
}