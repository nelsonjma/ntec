using System;
using System.Collections.Generic;
using System.Globalization;
using DbConfig;

using Views.BackOffice;

public partial class bo_site_cfg_user_configs : System.Web.UI.Page
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

        UserLoginData user = (UserLoginData)db_config_sessions.GetUserAuthentication();

        // redirect user to frontoffice if user is not autenticated
        if (user == null)
        {
            Generic.PageRedirect("~/frontoffice.aspx");
        }
        else if(!IsPostBack)
        {
            // user name set in hidden variable
            userId.Attributes.Add("value", user.User.ID.ToString(CultureInfo.InvariantCulture));

            // definir uma forma mais segura ou webservice mais complexo...
            string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));            
            webServiceKey.Attributes.Add("value", hashKey);
        }
    }


    /************************ Configurations ************************/


}