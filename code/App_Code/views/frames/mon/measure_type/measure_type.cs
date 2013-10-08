using System;
using System.Data;

namespace Views.Frames
{
    /// <summary>
    /// Generic definition of measurement methods
    /// </summary>
    public abstract class measure_type
    {
        protected string _log;
        protected string _fileName;
        protected int _measureValue;

        protected measure_type()
        {
            _fileName = string.Empty;
            _measureValue = 0;

            _log = Generic.GetWebConfigValue("LogFilePath");
        }

        protected string Log
        {
            get { return _log; }
            set { _log = value; }
        }

        protected string XmlFileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        protected DataView GetData()
        {
            try
            {
                if (_fileName != string.Empty)
                {
                    DataSet ds = new DataSet();

                    ds.ReadXml(_fileName);

                    return ds.Tables.Count > 0 
                            ? ds.Tables[0].AsDataView()
                            : new DataView();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: get data " + ex.Message + " ...");
            }

            return new DataView();
        }

        protected DataView GetData(string filter)
        {
            try
            {
                if (_fileName != string.Empty)
                {
                    return filter == string.Empty 
                                            ? GetData()
                                            : data_handler.LoadXmlData(_fileName, filter, string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: get data " + ex.Message + " ...");
            }

            return new DataView();
        }

        public abstract void BuildMeasure();

        public abstract void BuildMeasure(string filter);

        public int MeasureValue
        {
            get { return _measureValue; }
            set { _measureValue = value; }
        }
    }
}