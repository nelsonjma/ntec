using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Views.Frames;

/// <summary>
/// Respont to table ajax requests
/// </summary>
[WebService(Namespace = "http://ntec.table.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[ScriptService]
public class table_service : WebService {

    private readonly string _logRecord = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

    /// <summary>
    /// Permites that you in this case, can create a session a web service
    /// </summary>
    [WebMethod(EnableSession = true)]
    public string SetMasterFilter(string pageIdHash, string filterTitleHash, string filter, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        try
        {
            // transform select filter so that it can be read
            string auxFilter = HttpUtility.UrlDecode(filter);

            // create master filter
            filter_sessions.SetMasterFilterString(pageIdHash, filterTitleHash, auxFilter);

            return "ok";
        }
        catch (Exception ex)
        {
            loging.Error("Table User Webservice", "Set Master Filter", ex.Message, _logRecord);
        }

        return string.Empty;
    }

    
}
