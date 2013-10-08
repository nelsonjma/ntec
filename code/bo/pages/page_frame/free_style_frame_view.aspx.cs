using System;

using Views.BackOffice;

public partial class bo_pages_page_frame_free_style_frame_view : System.Web.UI.Page
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
    }
}