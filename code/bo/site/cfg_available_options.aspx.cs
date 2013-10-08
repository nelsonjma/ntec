using System;
using System.Activities.Statements;
using System.Linq;

using DbConfig;
using Views.BackOffice;

public partial class bo_cfg_available_options : System.Web.UI.Page
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
            loging.Error("Available Options configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
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
        string id = Request.Form["op_id"];
        string type = Request.Form["op_type"];
        string name = Request.Form["op_name"];
        string url = Request.Form["op_url"];
        string options = Request.Form["op_options"];

        SaveExistingOption(id, type, name, url, options);
    }

    protected void New_OnClick(object sender, EventArgs e)
    {
        string type = Request.Form["op_type"];
        string name = Request.Form["op_name"];
        string url = Request.Form["op_url"];
        string options = Request.Form["op_options"];

        AddNewOption(type, name, url, options);
    }

    /************************ Configurations ************************/
    /// <summary>
    /// Update existing option, the id is used has reference.
    /// </summary>
    static private void SaveExistingOption(string opId, string type, string name, string url, string options)
    {
        try
        {
            // if url does not exists then will add a new option
            if (opId == string.Empty) 
                AddNewOption(type, name, url, options);

            db_config_options dco = new db_config_options();
            dco.Open(); // open connection

            int id = Convert.ToInt32(opId);
            Options op = dco.AllOptions.Single(x => x.ID == id);

            if (op.ID != id) return; // if id is diferent probabaly because user pressed save with no option selected

            op.OBJType = type;
            op.Name = name;
            op.URL = url;
            op.Options1 = options;

            dco.Commit();

            dco.Close(); // close connection

            Generic.JavaScriptInjector("alert('Option Saved'); window.location.reload();");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("SaveError", "alert('" + ex.Message + "');");
        }
    }

    static private void AddNewOption(string type, string name, string url, string options)
    {
        try
        {
            db_config_options dco = new db_config_options(type, name);
            dco.Open(); // open connection

            // exist if type of name are empty, you have to define type and url or option can be empty
            if (type == string.Empty || name == string.Empty) { Generic.JavaScriptInjector("SaveError", "alert('type " + type + " or name " + name + " are empty');"); return; }

            // if it returns more then zero then name and type are alredy in use
            if (dco.AllOptions.Count > 0) { Generic.JavaScriptInjector("SaveError", "alert('type " + type + " and name " + name + " already exists');"); return; }

            Options op = new Options { Name = name, OBJType = type, URL = url, Options1 = options };
            dco.Add(op);

            dco.Close(); // close connection

            Generic.JavaScriptInjector("alert('Option added'); window.location.reload();");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("SaveError", "alert('" + ex.Message + "');");
        }
    }

}