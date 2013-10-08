using System;
using System.Data.SQLite;


namespace DbConfig
{
    /// <summary>
    /// generic class that represents the objects (master page, page, frame)
    /// </summary>
    public abstract class db_config
    {
        protected NtEcConFigs _db;

        protected string _dbPath;

        protected db_config()
        {
            _db = null;
        }

        public NtEcConFigs Db
        {
            get { return _db; }
            set { _db = value; }
        }

        protected void ConnectToDataBase()
        {
            try
            {
                string dbPath;

                // Checked _dbPath if string is empty, if is empty then you are going to use the default site path
                if (_dbPath != string.Empty)
                {   
                    dbPath = _dbPath;
                }
                else
                {
                    dbPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                    dbPath = dbPath.EndsWith(@"\")
                                ? dbPath + @"\App_Data\configs.db"
                                : dbPath + @"App_Data\configs.db";

                    _dbPath = dbPath; // protection measure for reopen and refresh.
                }

                // if you receive the _db connection from anothe class then it does not need to connect again
                if (_db == null) 
                    _db = new NtEcConFigs(new SQLiteConnection(@"Data Source=" + dbPath + ";Version=3;"));
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Open sqlite database width custom path to database
        /// </summary>
        public void Open(string dbPath)
        {
            try
            {
                _dbPath = dbPath;

                ConnectToDataBase();
                SelectObjectsFromDb();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Open width site default path
        /// </summary>
        public void Open()
        {
            try
            {
                // this will indicate that it will be used the website default path.
                _dbPath = string.Empty;

                ConnectToDataBase();
                SelectObjectsFromDb();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Try to Dispose linq object
        /// </summary>
        public void Close()
        {
            try
            {
                _db.Dispose();
                _db = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Refresh list of objects...
        /// </summary>
        public void Refresh()
        {
            SelectObjectsFromDb();
        }

        /// <summary>
        /// Execute all changes to database.
        /// </summary>
        public abstract void Commit();

        /// <summary>
        /// Dispose old linq object and then create new one.
        /// </summary>
        public void ReOpen()
        {
            try
            {
                Close();

                Open(_dbPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Template to get data from database
        /// </summary>
        protected abstract void SelectObjectsFromDb();
    }
}