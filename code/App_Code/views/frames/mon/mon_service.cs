using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

using Views.Frames;

/// <summary>
/// Mon send data to feed mon html pages.
/// the objects are HTML they are not asp.net objects
/// </summary>

[WebService(Namespace = "http://ntec.mon.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[ScriptService]
public class mon_service : WebService
{
    private readonly string _logRecord = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string FirstCellValue(string xmlFileName, string selectFilter, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        MonData mon = new MonData() { Data = "0" };

        if (ctrl != crlHash)
            return js.Serialize(mon);

        try
        {
            // transform select filter so that it can be read
            selectFilter = HttpUtility.UrlDecode(selectFilter);

            // transform xml file path so that it can be read
            xmlFileName = HttpUtility.UrlDecode(xmlFileName);

            first_cell_value fcv = new first_cell_value(xmlFileName);

            if (selectFilter != string.Empty)
                fcv.BuildMeasure(selectFilter); // get data from file and apply filter
            else
                fcv.BuildMeasure();             // just get data from file 

            mon.Data = fcv.MeasureValue.ToString(); // apply measurement
           
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice User Webservice", "List All Pages ", ex.Message, _logRecord);
        }

        return js.Serialize(mon);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string RowCount(string xmlFileName, string selectFilter, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        MonData mon = new MonData() { Data = "0" };

        if (ctrl != crlHash)
            return js.Serialize(mon);

        try
        {
            // transform select filter so that it can be read
            selectFilter = HttpUtility.UrlDecode(selectFilter);

            // transform xml file path so that it can be read
            xmlFileName = HttpUtility.UrlDecode(xmlFileName);

            rows_count fcv = new rows_count(xmlFileName);

            if (selectFilter != string.Empty)
                fcv.BuildMeasure(selectFilter); // get data from file and apply filter
            else
                fcv.BuildMeasure();             // just get data from file 

            mon.Data = fcv.MeasureValue.ToString(CultureInfo.InvariantCulture); // apply measurement

        }
        catch (Exception ex)
        {
            loging.Error("BackOffice User Webservice", "List All Pages ", ex.Message, _logRecord);
        }

        return js.Serialize(mon);
    }
}

class MonData
{
    public string Data { get; set; }
}
