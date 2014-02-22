using System;
using System.Data;


public class data_handler
{
    static public string AddFiltersToSql(string sql, string filters)
    {
        // if does not contais a from you are fk and this is not my fault :)
        if (sql.IndexOf("from", StringComparison.OrdinalIgnoreCase) == -1 || filters == string.Empty) return sql;

        if (sql.IndexOf("where", StringComparison.OrdinalIgnoreCase) != -1)
            return Generic.CaseInsensitiveReplace(sql, "where", "where " + filters + " ");

        if (sql.IndexOf("group by", StringComparison.OrdinalIgnoreCase) != -1)
            return Generic.CaseInsensitiveReplace(sql, "group by", "where " + filters + " group by");

        if (sql.IndexOf("order by", StringComparison.OrdinalIgnoreCase) != -1)
            return Generic.CaseInsensitiveReplace(sql, "order by", "where " + filters + " order by");

        return sql + " where " + filters;
    }

    static public DataTable DataTableFilter(DataTable dtin, string query)
    {
        if (query == string.Empty) return dtin; // se não existir nada para fazer baza...

        DataTable dtout = dtin.Clone();

        foreach (DataRow row in dtin.Select(query))
            dtout.ImportRow(row);

        return dtout;
    }

    static public object DataTableCompute(DataTable dt, string where, string operation)
    {
        return dt.Compute(operation, where);
    }

    /// <summary>
    /// Loads data from xml file, if default filter not empty will do a linq select 
    /// and if pageId and masterFilterId not empty will apply a filter
    /// </summary>
    static public DataView LoadXmlData(string fileName, string defaultFilter, string pageId, string masterFilterId)
    {
        try
        {
            DataView dataView;

            DataSet ds = new DataSet(); ds.ReadXml(fileName);

            string filters = string.Empty;

            if (pageId != string.Empty && masterFilterId != string.Empty)
            {
                // loads information of session filters to pre filter data.
                filters += Views.Frames.filter_sessions.GetMasterFilterString(Generic.GetHash(pageId), Generic.GetHash(masterFilterId));
            }

            DataTable newDt = DataTableFilter(ds.Tables[0], filters);

            if (defaultFilter != string.Empty)
            {
                dynamic_select dselect = new dynamic_select(defaultFilter);
                dataView = new DataView(dselect.GetDataTable(newDt));
            }
            else
                dataView = new DataView(newDt);

            return dataView;
        }
        catch (Exception ex)
        {
            throw new Exception("error loading xml data " + ex.Message);
        }
    }

    /// <summary>
    /// Loads data from sqlite file, default filter is the select to database
    /// and if pageId and masterFilterId not empty will apply a filter
    /// </summary>
    static public DataView LoadSqliteDataFile(string connStr, string sql, string pageId, string masterFilterId)
    {
        try
        {
            if (sql == string.Empty || connStr == string.Empty) return new DataView();

            string filters = string.Empty;

            if (pageId != string.Empty && masterFilterId != string.Empty)
            {
                // loads information of session filters to pre filter data.
                filters = Views.Frames.filter_sessions.GetMasterFilterString(Generic.GetHash(pageId), Generic.GetHash(masterFilterId));
            }

            // here we can add the filters to the query
            sql = AddFiltersToSql(sql, filters);

            DataSet ds = Generic.GetSqliteData(connStr, sql);

            return ds.Tables.Count > 0 
                    ? ds.Tables[0].AsDataView() 
                    : new DataView();
        }
        catch (Exception ex)
        {
            throw new Exception("error loading datafile " + ex.Message);
        }
    }
}
