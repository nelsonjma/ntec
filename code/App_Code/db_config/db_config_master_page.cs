using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbConfig
{

    /// <summary>
    /// Db to linq master page configuration
    /// </summary>
    public class db_config_master_page : db_config
    {
        private List<MasterPage> _allMasterPages;

        public db_config_master_page()
        {
            _allMasterPages = new List<MasterPage>();
        }

        protected override void SelectObjectsFromDb()
        {
            try
            {
                _allMasterPages = (from mp in _db.MasterPage
                                   select mp).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("error: select object from db - " + ex.Message + " ...");
            }  
        }

        public void SelectPublicObjectsFromDb()
        {
            try
            {
                _allMasterPages = PublicMasterPages;
            }
            catch (Exception ex)
            {
                throw new Exception("error: select public object from db - " + ex.Message + " ...");
            }  
        }

        public void SelectAuthenticatedObjectsFromDb(List<int> userPages)
        {
            try
            {
                _allMasterPages = AuthenticatedMasterPages(userPages);
            }
            catch (Exception ex)
            {
                throw new Exception("error: select authenticated object from db - " + ex.Message + " ...");
            } 
            
        }

        public override void Commit()
        {
            try
            {
                _db.MasterPage.Context.SubmitChanges();
            }
            catch (Exception ex)
            {
                ReOpen();

                throw ex;
            }
        }

        /* Get master pages */
        public List<MasterPage> AllMasterPages
        {
            get { return _allMasterPages; }
            set { _allMasterPages = value; }
        }

        /// <summary>
        /// select master pages width just public pages
        /// </summary>
        private List<MasterPage> PublicMasterPages
        {
            get
            {
                return (from mp in _allMasterPages
                        join p in _db.Page on mp.ID equals p.IDMasterPage
                        where p.PageUsers.Count == 0 && mp.Page.Count > 0
                        select mp).Distinct().ToList();
            }
        }

        /// <summary>
        /// select master pages width public and autheticated pages
        /// </summary>
        private List<MasterPage> AuthenticatedMasterPages(List<int> userPages)
        {
            List<MasterPage> masterPages = PublicMasterPages;

            try
            {
                masterPages.AddRange((from mp in _allMasterPages
                                      join p in _db.Page on mp.ID equals p.IDMasterPage
                                      where userPages.Contains(p.ID) && mp.Page.Count > 0
                                      select mp));
            }
            catch { }

            return masterPages.Distinct().ToList();
        }

        /* Get master page */
        public MasterPage Get(string title)
        {
            try
            {
                return _allMasterPages.Single(x => x.Title == title);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return master page with title " + title + " - " + ex.Message + " ...");
            }
        }

        public MasterPage Get(int id)
        {
            try
            {
                return _allMasterPages.Single(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return master page with id " + id + " - " + ex.Message + " ...");
            }
        }

        /* Database operations */
        /* Insert */
        public void Add(MasterPage mp)
        {
            try
            {
                using (_db)
                {
                    _db.MasterPage.InsertOnSubmit(mp);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new master page with title " + mp.Title + " - " + ex.Message + " ...");
            }
        }

        /* Deleting */
        public void Delete(int id)
        {
            try
            {
                using (_db)
                {
                    MasterPage mp = _db.MasterPage.Single(x => x.ID == id);

                    DeleteAllPages(mp.ID);

                    _db.MasterPage.DeleteOnSubmit(mp);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting master page with id " + id + " - " + ex.Message + " ...");
            }
        }

        public void Delete(string title)
        {
            try
            {
                using (_db)
                {
                    MasterPage mp = _db.MasterPage.Single(x => x.Title == title);

                    DeleteAllPages(mp.ID);

                    _db.MasterPage.DeleteOnSubmit(mp);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting master page with title " + title + " - " + ex.Message + " ...");
            }
        }

        private void DeleteAllPages(int masterPageId)
        {
            try
            {
                List<Page> pages = (from p in _db.Page
                                    where p.IDMasterPage == masterPageId
                                    select p).ToList();

                db_config_page dcp = new db_config_page(_db, pages, masterPageId);

                foreach (Page p in dcp.AllPages)
                {
                    dcp.Delete(p.ID);
                }
            }
            catch { }
        }

        /* Properties that can be comunicated with other ntec objects */
        /* Get Master Page Pages */
        public List<Page> GetAllPages(string masterPageTitle)
        {
            try
            {
                MasterPage mp = _allMasterPages.Single(x => x.Title == masterPageTitle);

                return mp.Page.ToList();
            }
            catch { }

            return new List<Page>();
        }

        public List<Page> GetAllPages(int masterPageId)
        {
            try
            {
                MasterPage mp = _allMasterPages.Single(x => x.ID == masterPageId);

                return mp.Page.ToList();
            }
            catch { }

            return new List<Page>();
        }

        /* Public/User Pages Selector */




    }
}