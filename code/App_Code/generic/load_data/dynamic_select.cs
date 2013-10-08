using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using DbLinq.Factory;

public class dynamic_select
{
    private readonly string _query;
    private readonly List<string> _selectColumns;
    private bool _computeFilter;

    public dynamic_select(string query)
    {
        _query = query;
        _selectColumns = new List<string>();
        _computeFilter = true;
    }

    //-------------------------------------------------------------------------------
    /// <summary>
    /// Build Select
    /// </summary>
    private string Select()
    {
        string select = string.Empty;

        string auxStr = _query;

        // check if is a group by
        bool containsGrp = auxStr.ToLower().Contains("group by"); // it´s here to use somewhere down there.

        // remove where
        auxStr = RemoveWhere(auxStr);

        // remove group by
        auxStr = RemoveGroupBy(auxStr);

        // remove order by
        auxStr = RemoveOrderBy(auxStr);

        // remove select word
        auxStr = RemoveSelectTop(auxStr);

        // get columns
        string[] auxLselect = auxStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < auxLselect.Length; i++)
        {
            string auxSelect = auxLselect[i].Trim();
            string operation = GetOperation(auxSelect);

            bool itsOperation = false;

            if (operation != string.Empty)
            {
                // verifica que tem um filtro então vai mais abaixo usar o CreateDataSet diferente.
                if (operation != "count") _computeFilter = true;
                
                _selectColumns.Add(operation);
                itsOperation = true;
            }
            else
            {
                _selectColumns.Add(auxSelect);
            }
                

            if (containsGrp && !itsOperation)
            {
                select += i.Equals(0) || select.Equals(string.Empty) ? "new(" : ",";
                select += "key." + auxSelect + " as " + auxSelect;
            }
            else if (containsGrp && itsOperation && operation.Equals("count"))
            {
                select += ", count() as count";
            }
            else if (!itsOperation)
            {
                select += !select.Contains("new(") ? "new(" : ",";
                select += "it[\"" + auxSelect + "\"] as " + auxSelect;
            }
        }

        if (auxLselect.Length > 0 && select != string.Empty)
            select += ")";

        return select;
    }

    /// <summary>
    /// Build Where
    /// </summary>
    private string Where()
    {
        string where = string.Empty;

        string auxStr = _query;

        if (auxStr.ToLower().Contains("where"))
        {
            // removes group by and order by
            where = RemoveGroupBy(auxStr);

            // remove where word
            where = where.Substring(auxStr.IndexOf("where", StringComparison.CurrentCultureIgnoreCase) + 5).Trim();
        }

        return where;        
    }

    /// <summary>
    /// Build Group by
    /// </summary>
    private string GroupBy()
    {
        string groupby = string.Empty;

        string auxStr = _query;

        if (auxStr.ToLower().Contains("group by"))
        {
            // removes order by string
            auxStr = RemoveOrderBy(auxStr);

            auxStr = auxStr.Substring(auxStr.IndexOf("group by", StringComparison.CurrentCultureIgnoreCase) + 8);

            string[] auxLgrp = auxStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < auxLgrp.Length; i++)
            {
                string auxGrp = auxLgrp[i].Trim();

                groupby += !i.Equals(0) ? "," : "new(";
                groupby += "it[\"" + auxGrp + "\"] as " + auxGrp;
                groupby += i.Equals(auxLgrp.Length - 1) ? ")" : " ";
            }
        }

        return groupby;        
    }

    /// <summary>
    /// Get group by string
    /// </summary>
    private string OrderBy()
    {
        return _query.ToLower().Contains("order by") 
                                                    ?_query.Substring(_query.IndexOf("order by", StringComparison.CurrentCultureIgnoreCase) + 8)
                                                    : string.Empty;      
    }

    private int Top()
    {
        string data = _query.TrimStart();

        if (data.StartsWith("select", StringComparison.CurrentCultureIgnoreCase))
        {
            data = data.Substring(6).TrimStart();

            if (data.StartsWith("top", StringComparison.CurrentCultureIgnoreCase))
            {
                data = data.Remove(0, 3).Trim();
                data = data.Substring(0, data.IndexOf(' '));

                return Convert.ToInt32(data);
            }
        }

        return -1;
    }

    //-------------------------------------------------------------------------------
    private string GetOperation(string data)
    {
        data = data.ToLower().Trim();

        if (data.StartsWith("count "))
            return "count";

        if (data.StartsWith("avg("))
            return data;

        if (data.StartsWith("min("))
            return data;

        if (data.StartsWith("max("))
            return data;

        if (data.StartsWith("sum("))
            return data;

        return string.Empty;
    }

    private string GetOperationStr(string data)
    {
        data = data.ToLower().Trim();

        if (data.StartsWith("avg(", StringComparison.CurrentCultureIgnoreCase) || 
            data.StartsWith("min(", StringComparison.CurrentCultureIgnoreCase) || 
            data.StartsWith("max(", StringComparison.CurrentCultureIgnoreCase) || 
            data.StartsWith("sum(", StringComparison.CurrentCultureIgnoreCase))
        {
            return data.ToLower().Contains(" as") 
                                                ? data.Substring(0, data.IndexOf(" as", StringComparison.CurrentCultureIgnoreCase)).Trim()
                                                : data.Trim();
        }

        return string.Empty;
    }

    private string GetColumnHeader(string data)
    {
        data = data.ToLower();

        if (data.StartsWith("count "))
        {
            if (data.Contains(" as"))
            {
                return data.Substring(data.IndexOf(" as", StringComparison.Ordinal) + 3);
            }

            return "count";
        }

        if (data.StartsWith("avg(") || data.StartsWith("min(") || data.StartsWith("max(") || data.StartsWith("sum("))
        {
            if (data.Contains(" as"))
            {
                return data.Substring(data.IndexOf(" as", StringComparison.Ordinal) + 3);
            }

            return data.Replace("(", "_").Replace(")", " ").Trim().Replace(" ", "_");
        }

        return string.Empty;
    }

    //-------------------------------------------------------------------------------
    /// <summary>
    /// Sort data, and returns a datatable
    /// </summary>
    private DataTable GetOrderedData(DataTable dt, string orderByStr)
    {
        if (orderByStr == string.Empty) 
            return dt;

        DataView dv = dt.DefaultView;
        dv.Sort = orderByStr;

        return dv.ToTable();
    }

    /// <summary>
    /// Top like operation
    /// </summary>
    private DataTable GetTopRows(DataTable dt, int top)
    {
        if (top == -1) return dt;

        List<DataRow> listRows = dt.AsEnumerable().Take(top).ToList();

        DataTable newDt = dt.Clone();

        foreach (DataRow row in listRows)
            newDt.Rows.Add(row.ItemArray);

        return newDt;
    }

    //-------------------------------------------------------------------------------
    private string RemoveGroupBy(string data)
    {
        return data.ToLower().Contains("order by")
                                                ? data.Substring(0, data.IndexOf("group by", StringComparison.CurrentCultureIgnoreCase))
                                                : data;
    }

    private string RemoveOrderBy(string data)
    {
        return data.ToLower().Contains("order by")
                                                ? data.Substring(0, data.IndexOf("order by", StringComparison.CurrentCultureIgnoreCase))
                                                : data;
    }

    private string RemoveWhere(string data)
    {
        return data.ToLower().Contains("where") 
                                                ? data.Substring(0, data.IndexOf("where", StringComparison.CurrentCultureIgnoreCase))
                                                : data;
    }

    private string RemoveSelectTop(string data)
    {
        data = data.TrimStart();

        if (data.StartsWith("select", StringComparison.CurrentCultureIgnoreCase))
        {
            data = data.Substring(6).TrimStart();

            if (data.StartsWith("top", StringComparison.CurrentCultureIgnoreCase))
            {
                data = data.Remove(0, 3).Trim();
                data = data.Remove(0, data.IndexOf(' '));

                return data;
            }
        }

        return data;
    }

    //-------------------------------------------------------------------------------
    private string GetComputeWhere(List<string> columns, string[] listData)
    {
        string where = string.Empty;

        List<string> auxColumns = new List<string>();

        foreach (string column in columns)
        {
            string auxColumn = GetOperation(column);

            if (auxColumn == string.Empty || auxColumn == "count")
            {
                auxColumns.Add(column);
            }
        }

        if (auxColumns.Count == listData.Length)
        {
            for (int i = 0; i < auxColumns.Count; i++)
            {
                if (auxColumns[i] != "count")
                {
                    string column = auxColumns[i];
                    string data = listData[i].Substring(listData[i].IndexOf('=') + 1);

                    if (!i.Equals(0) && auxColumns.Count > 1)
                        where += "and ";

                    where += column + "= '" + data + "' ";
                }
            }
        }

        return where;
    }

    //-------------------------------------------------------------------------------
    private DataTable CreateDataSet(IQueryable items, List<string> columns)
    {
        try
        {
            // definir as colunas do avg, etc, etc e o compute

            DataSet ds = new DataSet();    
            DataTable dt = ds.Tables.Add();

            //-- Add columns to the data table
            if (columns.Count > 0)
            {
                foreach (string column in columns)
                {
                    Type type = typeof(string);
                    string opColumn = GetColumnHeader(column);
                    string auxColumn = column;
                        
                    if (auxColumn != string.Empty)
                    {
                        type = typeof(double);
                        auxColumn = opColumn;
                    }

                    dt.Columns.Add(auxColumn, type);
                }

                foreach (var item in items)
                {
                    DataRow dr = dt.NewRow();

                    // it will split the line, the delimitator is ", " because of doubles like this 6,9
                    string[] listSubItems = item.ToString().Replace("{", "").Replace("}", " ").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < listSubItems.Length; i++)
                    {
                        listSubItems[i] = listSubItems[i].Substring(listSubItems[i].IndexOf('=') + 1);
                    }

                    dr.ItemArray = listSubItems;
                    dt.Rows.Add(dr);
                }
            }

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable CreateDataSet(IQueryable items, List<string> columns, DataTable inDt)
    {
        try
        {
            // definir as colunas do avg, etc, etc e o compute

            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();

            //-- Add columns to the data table
            if (columns.Count > 0)
            {
                foreach (string column in columns)
                {

                    Type type = typeof(string);
                    string opColumn = GetColumnHeader(column);
                    string auxColumn = column;

                    // the value of column may have already been iterated, its a resharp warning message did not know this
                    string tmpColumn = column.ToLower().Trim();

                    foreach (DataColumn dc in inDt.Columns.Cast<DataColumn>().Where(dc => tmpColumn.Contains(dc.Caption.ToLower())))
                        type = dc.DataType;

                    if (opColumn != string.Empty)
                    {
                        type = typeof(double);
                        auxColumn = opColumn.Trim();
                    }

                    dt.Columns.Add(auxColumn, type);
                }

                foreach (var item in items)
                {
                    DataRow dr = dt.NewRow();

                    object[] itemArray = new object[columns.Count];

                    // it will split the line, the delimitator is ", " because of doubles like this 6,9
                    string[] listSubItems = item.ToString().Replace("{", "").Replace("}", " ").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string subItems in listSubItems)
                    {
                        string columHeader = subItems.Substring(0, subItems.IndexOf('='));
                        string value = subItems.Substring(subItems.IndexOf('=') + 1);

                        int index = columns.FindIndex(x => x==columHeader.Trim());

                        itemArray[index] = value;
                    }

                    string where = GetComputeWhere(columns, listSubItems);

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        try
                        {
                            // gets the operation from the column
                            string operation = GetOperationStr(columns[i]);

                            if (operation != string.Empty && dt.Columns[i].ColumnName != "count")
                            {
                                object value = data_handler.DataTableCompute(inDt, where, operation);
                                itemArray[i] = Convert.ToDouble(value);
                            }
                        }
                        catch { } // gets here if the operation does not match the column type something like doing avg of the string column => ITS NOT POSSIBLE
                    }

                    dr.ItemArray = itemArray;
                    dt.Rows.Add(dr);
                }
            }

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //-------------------------------------------------------------------------------
    public DataTable GetDataTable(DataTable dt)
    {
        try
        {
            string select = Select();
            string where = Where();
            string groupBy = GroupBy();
            string orderBy = OrderBy();
            int top = Top();

            DataTable auxDt = where != string.Empty
                                                ? data_handler.DataTableFilter(dt, where)
                                                : dt;

            if (select != string.Empty)
            {
                IQueryable resultA = groupBy != string.Empty
                                                        ? auxDt.AsEnumerable().AsQueryable().GroupBy(groupBy, "it").Select(select)
                                                        : auxDt.AsEnumerable().AsQueryable().Select(select);

                // auxDt.Dispose(); // NOTE: disabled so that the oprations min, max, sum, avg can be used in the filtered dataset

                // THE COMPUTE FILTER IS NOT NECESSARY DISABLE IN THE FUTURE
                //auxDt = _computeFilter.Equals(true)
                //                                //? CreateDataSet(resultA, _selectColumns, dt) // dont delete for reminder see note above
                //                                ? CreateDataSet(resultA, _selectColumns, auxDt)
                //                                : CreateDataSet(resultA, _selectColumns);
                auxDt = CreateDataSet(resultA, _selectColumns, auxDt);
            }

            // order data
            if (orderBy != string.Empty) auxDt = GetOrderedData(auxDt, orderBy);

            // set TOP 
            if (top != -1) auxDt = GetTopRows(auxDt, top);

            return auxDt;
        }
        catch (Exception)
        {
            throw;
        }        
    }
}