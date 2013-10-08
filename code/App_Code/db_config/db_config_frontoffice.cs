using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbConfig
{
    /// <summary>
    /// Summary description for db_config_frontoffice
    /// </summary>
    public class db_config_frontoffice : db_config
    {
        private FrontOffice _cfg;

	    public db_config_frontoffice()
	    {
	    }

        protected override void SelectObjectsFromDb()
        {
            // this method is a little awkward because of loop problem wen after the insert there is an error or someting.
            try
            {
                bool chickenMove = true;

            Chicken:
                _cfg = (from f in _db.FrontOffice select f).FirstOrDefault();

                if (_cfg == null && _db.DatabaseExists() && chickenMove)
                {
                    InsertDefault();
                    chickenMove = false;
                    goto Chicken;

                    //SelectObjectsFromDb(); // risky move, can cause loop needs more tests, dont want to take the risk
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: select object from db - " + ex.Message + " ...");
            }            
        }

        public override void Commit()
        {
            try
            {
                _db.FrontOffice.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                ReOpen();

                throw ex;
            }
        }

        private void InsertDefault()
        {
            try
            {
                using (_db)
                {
                    FrontOffice fo = new FrontOffice();
                    fo.Options = "";

                    _db.FrontOffice.InsertOnSubmit(fo);

                    Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new option - " + ex.Message + " ...");
            }

            

        }

        public FrontOffice Get
        {
            get { return _cfg; }
        }
    }
}