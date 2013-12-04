using System;
using System.Data;
using System.Globalization;
using System.Text;
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
    public string FirstCellValue(string datatype, string datafile, string selectFilter, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        MonData mon = new MonData() { Data = "0" };

        if (ctrl != crlHash || datatype == string.Empty)
            return js.Serialize(mon);

        try
        {
            LoadData ld = new LoadData
            {
                // Decoder select filter so that it can be read
                DefaultFilter = HttpUtility.UrlDecode(selectFilter) 
            };

            // Decoder datafile information
            ld.SetDataType(datatype, HttpUtility.UrlDecode(datafile));


            measure_type fcv = new first_cell_value();
            fcv.LoadData = ld;

            // just get data from file 
            fcv.BuildMeasure();

            // apply measurement
            mon.Data = fcv.MeasureValue.ToString(CultureInfo.InvariantCulture); 
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice User Webservice", "List All Pages ", ex.Message, _logRecord);
        }

        return js.Serialize(mon);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string RowCount(string datatype, string datafile, string selectFilter, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        MonData mon = new MonData() { Data = "0" };

        if (ctrl != crlHash || datatype == string.Empty)
            return js.Serialize(mon);

        try
        {
            LoadData ld = new LoadData
            {
                // Decoder select filter so that it can be read
                DefaultFilter = HttpUtility.UrlDecode(selectFilter)
            };

            // Decoder datafile information and the load it to a sqlite/xml selector
            ld.SetDataType(datatype, HttpUtility.UrlDecode(datafile));

            measure_type fcv = new rows_count();
            fcv.LoadData = ld;
            
            // just get data from file 
            fcv.BuildMeasure();

            // apply measurement
            mon.Data = fcv.MeasureValue.ToString(CultureInfo.InvariantCulture);
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
