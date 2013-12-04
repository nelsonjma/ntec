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
        
        protected int _measureValue;

        // Sqlite/Xml file
        protected LoadData _loadData;

        protected measure_type()
        {
            _loadData = null;

            _measureValue = 0;

            _log = Generic.GetWebConfigValue("LogFilePath");
        }

        protected string Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public LoadData LoadData
        {
            get { return _loadData; }
            set { _loadData = value; }
        }

        protected DataView GetData()
        {
            try
            {
                if (LoadData != null)
                {
                    return LoadData.GetData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: get data " + ex.Message + " ...");
            }

            return new DataView();
        }

        public abstract void BuildMeasure();

        public int MeasureValue
        {
            get { return _measureValue; }
            set { _measureValue = value; }
        }
    }
}