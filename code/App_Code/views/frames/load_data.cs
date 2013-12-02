using System.Collections.Generic;
using System.Data;
using DbLinq.Data.Linq.SqlClient;


/// <summary>
/// Controls data file and xml file input information
/// </summary>
public class LoadData
{
    private string _datatable;

    public LoadData()
	{
        FileName = string.Empty;
        Datafile = string.Empty;
        _datatable = string.Empty;
        PageId = -1;
        MasterFilterId = string.Empty;
	}

    public string FileName { get; set; }

    public string Datafile { get; set; }

    /***********************************/
    /* datatable is a special case */
    public void Datatable(List<string> data)
    {
        if (data.Count > 0)
            _datatable = data[0];
    }

    public string DataTable()
    {
        return _datatable;
    }
    /***********************************/

    public int PageId { get; set; }

    public string MasterFilterId { get; set; }

    public DataView GetData()
    {
        return new DataView();
    }

    /***************** Private *****************/
    private DataView LoadXmlData()
    {
        return new DataView();
    }

    private DataView LoadDataFile()
    {
        return new DataView();
    }
}