using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using DbConfig;


/// <summary>
/// Controls data file and xml file input information
/// </summary>
public class LoadData
{
    public LoadData()
	{
        Datatable = new List<string>();
        FileName = string.Empty;
        Datafile = string.Empty;
        PageId = -1;
        MasterFilterId = string.Empty;
        DefaultFilter = string.Empty;
	}

    /***************** Properties *****************/
    public string FileName { get; set; }

    public string Datafile { get; set; }

    public int PageId { get; set; }

    public string MasterFilterId { get; set; }

    public List<string> Datatable { get; set; }

    public string DefaultFilter { get; set; }

    /***************** Public *****************/
    public DataView GetData()
    {
        if (FileName != string.Empty)
            return LoadXmlData();
        
        
        if (Datafile != string.Empty)
            return LoadSqliteDataFile();
        
        
        return new DataView();
    }

    public string GetFilePath()
    {
        if (FileName != string.Empty)
            return BuildFilePath(FileName);

        if (Datafile != string.Empty)
            return BuildFilePath(Datafile);

        return string.Empty;
    }

    public string GetVirtualFilePath()
    {
        if (FileName != string.Empty)
            return BuildXmlFileVirtualPath(FileName);

        if (Datafile != string.Empty)
            return BuildXmlFileVirtualPath(Datafile);

        return string.Empty;
    }

    public string GetDataFileName()
    {
        if (FileName != string.Empty)
            return FileName;

        if (Datafile != string.Empty)
        {
            return Datafile;
        }
            
        return string.Empty;
    }

    public string GetProcessedDataFileName()
    {
        if (FileName != string.Empty)
            return BuildFilePath(FileName);

        if (Datafile != string.Empty)
        {
            return BuildSqliteDataFileConnection();
        }

        return string.Empty;
    }

    public string GetProcessedDefaultFilter()
    {
        if (Datafile == string.Empty)
            return DefaultFilter;
        
        // if datafile is not empty
        string sql = DefaultFilter;

        string table = Datafile;

        if (DefaultFilter == string.Empty)
        {
            if (Datatable.Count > 0) 
                table = Datatable[0];

            sql = "select * from " + table;
        }

        return sql;
    }

    public string GetDataType()
    {
        if (FileName != string.Empty)
            return "xml";

        if (Datafile != string.Empty)
            return "sqlite";
            
        return string.Empty;
    }

    public void SetDataType(string datatype, string datafile)
    {
        if (datatype == "xml")
            FileName = datafile;

        if (datatype == "sqlite")
            Datafile = datafile;
    }

    /***************** Private *****************/
    /* Get Data */
    private DataView LoadXmlData()
    {
        try
        {
            string pageId = PageId != -1 ? PageId.ToString() : string.Empty;

            string filename = BuildFilePath(FileName);

            return data_handler.LoadXmlData(filename, DefaultFilter, pageId, MasterFilterId);
        }
        catch (Exception ex)
        {
            throw new Exception("generating dataview error from xml file: " + FileName + " - " + ex.Message + " ...");
        }
    }

    private DataView LoadSqliteDataFile()
    {
        try
        {
            string pageId = PageId != -1 ? PageId.ToString() : string.Empty;

            string connStr = BuildSqliteDataFileConnection();

            string sql = GetProcessedDefaultFilter();

            return data_handler.LoadSqliteDataFile(connStr, sql, pageId, MasterFilterId);
        }
        catch (Exception ex)
        {
            throw new Exception("generating dataview error from sqlite file: " + Datafile + " - " + ex.Message + " ...");
        }
    }

    /* Build Fisical Path */
    private string BuildFilePath(string fileName)
    {
        db_config_page page = null;

        try
        {
            // if contains a fixed path then there is no need to add page or default path
            if (fileName.StartsWith("http://") || fileName.Contains(@":\"))
                return fileName;

            // if the filename is not added then there is no reason to be here
            if (string.IsNullOrEmpty(fileName)) throw new Exception("filename is empty");

            // page fisical path
            string pageDataFolder = string.Empty;

            if (PageId != -1)
            {
                page = new db_config_page(PageId, false);
                page.Open();
                pageDataFolder = page.Get(PageId).XMLFolderPath;    
            }
            
            // webconfig fisical path
            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
            string dataFile = Generic.GetWebConfigValue("DataFolderPath");

            // if sitePath is null then WTF and if pageXmlFolder is empty well in this case you have Murphy's Law problems
            if (string.IsNullOrEmpty(sitePath) && (string.IsNullOrEmpty(pageDataFolder) || pageDataFolder.StartsWith("default")))
                throw new Exception("no fisical path to build");

            // if path is null then set to empty ele remove any space that it was added in the end
            sitePath = (sitePath == null) ? string.Empty : sitePath.TrimEnd();
            dataFile = (dataFile == null) ? string.Empty : dataFile.TrimEnd();

            if (!string.IsNullOrEmpty(pageDataFolder) && !pageDataFolder.StartsWith("default", StringComparison.CurrentCultureIgnoreCase))
            {
                return pageDataFolder.EndsWith(@"\")
                                                ? pageDataFolder + fileName
                                                : pageDataFolder + @"\" + fileName;
            }

            return dataFile != string.Empty
                                    ? dataFile.EndsWith(@"\")    // if not empty then it contains another path then the default
                                                        ? dataFile + fileName
                                                        : dataFile + @"\" + fileName
                                    : sitePath.EndsWith(@"\")   // if empty then user whants to use default path 
                                                        ? sitePath + @"data\" + fileName
                                                        : sitePath + @"\data\" + fileName;
        }
        catch (Exception ex)
        {
            throw new Exception("build fisical file path: " + fileName + " - " + ex.Message + " ...");
        }
        finally
        {
            if (page != null) page.Close();
        }
    }

    /* Build Virtual Path */
    private string BuildXmlFileVirtualPath(string fileName)
    {
        db_config_page page = null;

        try
        {
            // if contains a fixed path then there is no need to add page or default path
            if (fileName.StartsWith("http://"))
                return fileName;

            // if the filename is not added then there is no reason to be here
            if (string.IsNullOrEmpty(fileName)) throw new Exception("filename is empty");

            // page virtual path
            string pageXmlUrl = string.Empty;

            if (PageId != -1)
            {
                page = new db_config_page(PageId, false);
                page.Open();
                pageXmlUrl = page.Get(PageId).XMLURL;
            }

            // webconfig virtual path
            string virtualFolder = Generic.GetWebConfigValue("DataVirtualFolderPath");
            if (string.IsNullOrEmpty(virtualFolder)) virtualFolder = "~/data/";

            if (!string.IsNullOrEmpty(pageXmlUrl) && !pageXmlUrl.StartsWith("default", StringComparison.CurrentCultureIgnoreCase))
            {
                return pageXmlUrl.EndsWith("/")
                                            ? pageXmlUrl + fileName
                                            : pageXmlUrl + @"/" + fileName;
            }

            return virtualFolder.EndsWith("/")
                                            ? virtualFolder + fileName
                                            : virtualFolder + @"/" + fileName;
        }
        catch (Exception ex)
        {
            throw new Exception("build vistual path: " + fileName + " - " + ex.Message + " ...");
        }
        finally
        {
            if (page != null) page.Close();
        }
    }

    /// <summary>
    /// Get the database file path to build the slite connection,
    /// someting like this Data Source=my_database.db;Version=3;Read Only=True;
    /// </summary>
    /// <returns></returns>
    private string BuildSqliteDataFileConnection()
    {
        string datafile = BuildFilePath(Datafile);

        string connStr = datafile.EndsWith(".db")
                        ? "Data Source=" + datafile + ";Version=3;Read Only=True;"
                        : "Data Source=" + datafile + ".db;Version=3;Read Only=True;";

        return connStr;
    }
}