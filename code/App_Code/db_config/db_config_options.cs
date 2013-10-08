using System;
using System.Collections.Generic;
using System.Linq;

namespace DbConfig 
{
    /// <summary>
    /// Summary description for db_config_options
    /// </summary>
    public class db_config_options : db_config
    {
        private List<Options> _allOptions;

        private string _objType;
        private string _name;

        public db_config_options()
        {
            _objType = string.Empty;
            _name = string.Empty;
        }

        public db_config_options(string objType)
        {
            _objType = objType;
            _name = string.Empty;
        }

	    public db_config_options(string objType, string name)
	    {
            _objType = objType;
            _name = name;
	    }

        public override void Commit()
        {
            try
            {
                _db.Options.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                ReOpen();

                throw ex;
            }

        }

        protected override void SelectObjectsFromDb()
        {
            try
            {
                if (_name != string.Empty)
                {
                    _allOptions = (from op in _db.Options
                                   where op.OBJType == _objType && op.Name == _name
                                   select op).ToList();
                }
                else if (_objType != string.Empty)
                {
                    _allOptions = (from op in _db.Options
                                   where op.OBJType == _objType
                                   select op).ToList();
                }
                else
                {
                    _allOptions = (from op in _db.Options
                                   select op).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: select object from db - " + ex.Message + " ...");
            }
        }

        public List<Options> AllOptions
        {
            get { return _allOptions; }
            set { _allOptions = value; }
        }

        public Options Get(string name)
        {
            try
            {
                return _allOptions.Single(x => x.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return option with name " + name + " - " + ex.Message + " ...");
            }
        }

        /* Database operations */
        /* Insert */
        public void Add(Options op)
        {
            try
            {
                using (_db)
                {
                    _db.Options.InsertOnSubmit(op);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new option with name " + op.Name + " - " + ex.Message + " ...");
            }
        }

        /* Delete */
        public void Delete(int id)
        {
            try
            {
                using (_db)
                {
                    Options op = _db.Options.Single(x => x.ID == id);

                    _db.Options.DeleteOnSubmit(op);

                    Commit();

                    _allOptions.Clear();
                    _allOptions = null;

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting option with id " + id + " - " + ex.Message + " ...");
            }
        }

        public void Delete(string name)
        {
            try
            {
                using (_db)
                {
                    Options op = _db.Options.Single(x => x.Name == name);

                    _db.Options.DeleteOnSubmit(op);

                    Commit();

                    _allOptions.Clear();
                    _allOptions = null;

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting option with name " + name + " - " + ex.Message + " ...");
            }
        }
    }
}