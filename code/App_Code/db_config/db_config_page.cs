using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbConfig
{
    /// <summary>
    /// Db to linq page configuration
    /// </summary>
    public class db_config_page : db_config
    {
        private List<Page> _allPages;

        private readonly int _masterPageId;
        private readonly string _pageName;
        private readonly int _pageId;

        public db_config_page()
        {
            _allPages = new List<Page>();

            _pageName = string.Empty;
            _masterPageId = -1;
            _pageId = -1;
        }

        /// <summary>
        /// dummy var is not necessary just to use method that accepts masterPageId and pageId
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="dummy"></param>
        public db_config_page(int pageId, bool iamdummy)
        {
            _allPages = new List<Page>();

            _pageName = string.Empty;
            _masterPageId = -1;
            _pageId = pageId;
        }

        public db_config_page(string pageName)
        {
            _allPages = new List<Page>();

            _pageName = pageName;
            _masterPageId = -1;
            _pageId = -1;
        }

        public db_config_page(int masterPageId)
        {
            _allPages = new List<Page>();

            _pageName = string.Empty;
            _masterPageId = masterPageId;
            _pageId = -1;
            
        }

        public db_config_page(NtEcConFigs db, List<Page> pages, int masterPageId)
        {
            _allPages = pages;
            _db = db;

            _pageName = string.Empty;
            _masterPageId = masterPageId;
            _pageId = -1;
        }

        private Page Copy(Page p)
        {
            try
            {
                using (_db)
                {
                    Page pNew = new Page();

                    pNew.Name = p.Name + "CLONE";
                    pNew.IDMasterPage = p.IDMasterPage;
                    pNew.Options = p.Options;
                    pNew.Title = p.Title;
                    pNew.XMLFolderPath = p.XMLFolderPath;
                    pNew.XMLURL = p.XMLURL;

                    return pNew;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Page Copy(Page p, int newMasterPageId)
        {
            try
            {
                using (_db)
                {
                    Page pNew = new Page();

                    pNew.Name = p.Name + "CLONE";
                    pNew.IDMasterPage = newMasterPageId;
                    pNew.Options = p.Options;
                    pNew.Title = p.Title;
                    pNew.XMLFolderPath = p.XMLFolderPath;
                    pNew.XMLURL = p.XMLURL;

                    return pNew;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check if page has got a master page to connect.
        /// </summary>
        /// <param name="p"></param>
        private void CheckMasterPageStatus(Page p)
        {
            if (_masterPageId >= 0)
            {
                p.IDMasterPage = _masterPageId;
                p.MasterPage = _db.MasterPage.Single(x => x.ID == _masterPageId);
            }
            else
            {
                if (p.IDMasterPage >= 0)
                {
                    if (p.MasterPage == null || p.IDMasterPage != p.MasterPage.ID)
                    {
                        p.MasterPage = _db.MasterPage.Single(x => x.ID == p.IDMasterPage);
                    }
                }
                else
                    throw new Exception("no master page selected with the id " + p.IDMasterPage);
            }
        }

        public override void Commit()
        {
            try
            {
                _db.Page.Context.SubmitChanges();
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
                if (_masterPageId >= 0) // inserted master page id
                {
                    _allPages = (from p in _db.Page
                                 where p.IDMasterPage == _masterPageId
                                 select p).ToList();
                }
                else if (_pageId >= 0) // inserted page id
                {
                    _allPages = (from p in _db.Page
                                 where p.ID == _pageId
                                 select p).ToList();
                }
                else if (_pageName!= string.Empty) // inserted page name
                {
                    _allPages = (from p in _db.Page
                                 where p.Name == _pageName
                                 select p).ToList();
                }
                else // did not insert nothing...
                {
                    _allPages = (from p in _db.Page
                                 select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: select object from db - " + ex.Message + " ...");
            }  
        }

        /// <summary>
        /// select just public pages
        /// </summary>
        public void SelectPublicObjectsFromDb()
        {
            try
            {
                _allPages = PublicPages();
            }
            catch (Exception ex)
            {
                throw new Exception("error: select public object from db - " + ex.Message + " ...");
            } 
        }

        /// <summary>
        /// select public and autheticated pages
        /// </summary>
        public void SelectAuthenticatedObjectsFromDb(List<int> userPages)
        {
            try
            {
                _allPages = UserAuthenticatedPages(userPages);
            }
            catch (Exception ex)
            {
                throw new Exception("error: select authenticated object from db - " + ex.Message + " ...");
            } 
        }

        /* Get pages */
        public List<Page> AllPages
        {
            get { return _allPages; }
            set { _allPages = value; }
        }

        /// <summary>
        /// Get just public pages
        /// </summary>
        /// <returns></returns>
        private List<Page> PublicPages()
        {
            try
            {
                return (from p in _allPages
                         where p.PageUsers.Count == 0
                         select p).ToList();
            }
            catch {}
            
            return new List<Page>();
        }

        /// <summary>
        /// Get public pages and user pages
        /// </summary>
        private List<Page> UserAuthenticatedPages(List<int> userPages)
        {
            List<Page> pages = PublicPages();

            try
            {
                pages.AddRange((from p in _allPages
                             where userPages.Contains(p.ID)
                             select p).ToList());
            }
            catch { }

            return pages != null ? pages :   new List<Page>();
        }

        /* Get page */
        public Page Get(string name)
        {
            try
            {
                return _allPages.Single(x => x.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return page with name " + name + " - " + ex.Message + " ...");
            }
        }

        public Page Get(int id)
        {
            try
            {
                return _allPages.Single(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return page with id " + id + " - " + ex.Message + " ...");
            }
        }

        /* Check if page exists*/
        public bool PageExists(string name)
        {
            try
            {
                return _allPages.Exists(x => x.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant check if page " + name + " exists - " + ex.Message + " ...");
            }
        }

        public bool PageExists(int id)
        {
            try
            {
                return _allPages.Exists(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant check if page id" + id + " exists - " + ex.Message + " ...");
            }
        }

        /* Database operations */
        /* Insert */
        public void Add(Page p)
        {
            try
            {
                using (_db)
                {
                    if (p.IDMasterPage != _masterPageId)
                        CheckMasterPageStatus(p);
                    else
                        p.MasterPage = _db.MasterPage.Single(x => x.ID == p.IDMasterPage);

                    _db.Page.InsertOnSubmit(p);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new page with title " + p.Title + " - " + ex.Message + " ...");
            }
        }

        /* Delete */
        public void Delete(int id)
        {
            try
            {
                using (_db)
                {
                    Page p = _masterPageId >= 0
                                ? _db.Page.Single(x => x.ID == id && x.IDMasterPage == _masterPageId)
                                : _db.Page.Single(x => x.ID == id);

                    DeleteAllPageFrames(p.ID);
                    DeleteAllUserPages(p.ID);

                    _db.Page.DeleteOnSubmit(p);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                string masterPageMsg = _masterPageId >= 0 ? " and master page " + _masterPageId : "";

                throw new Exception("error: deleting page with id " + id + " " + masterPageMsg + " - " + ex.Message + " ...");
            }
        }

        public void Delete(string name)
        {
            try
            {
                using (_db)
                {
                    Page p = _masterPageId >= 0
                                ? _db.Page.Single(x => x.Name == name && x.IDMasterPage == _masterPageId)
                                : _db.Page.Single(x => x.Name == name);

                    DeleteAllPageFrames(p.ID);
                    DeleteAllUserPages(p.ID);

                    _db.Page.DeleteOnSubmit(p);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                string masterPageMsg = _masterPageId >= 0 ? " and master page " + _masterPageId : "";

                throw new Exception("error: deleting page with name " + name + " " + masterPageMsg + " - " + ex.Message + " ...");
            }
        }

        private void DeleteAllPageFrames(int pageId)
        {
            try
            {
                List<Frame> frames = (from f in _db.Frame
                                      where f.IDPage == pageId
                                      select f).ToList();

                db_config_frame dcf = new db_config_frame(_db, frames, pageId);

                foreach (Frame frm in dcf.AllFrames)
                {
                    dcf.Delete(frm.ID);
                }
            }
            catch { }
        }

        private void DeleteAllUserPages(int pageId)
        {
            List<PageUsers> pageUsers = (from pu in _db.PageUsers
                                         where pu.IDPage == pageId
                                         select pu).ToList();

            foreach (PageUsers pu in pageUsers)
            {
                _db.PageUsers.DeleteOnSubmit(pu);
                _db.PageUsers.Context.SubmitChanges();
            }
        }

        /* Clone */
        /// <summary>
        /// Clone Page in the same master page
        /// </summary>
        /// <param name="id"></param>
        public void Clone(int id)
        {
            try
            {
                using (_db)
                {
                    Page p = _db.Page.Single(x => x.ID == id);

                    Page pNew = Copy(p);

                    _db.Page.InsertOnSubmit(pNew);

                    Commit();

                    pNew = _db.Page.Single(x => x.Name == pNew.Name);

                    if (p.Frame.Count > 0)
                    {
                        db_config_frame frame = new db_config_frame(_db, p.Frame.ToList(), p.ID);

                        foreach (Frame frm in frame.AllFrames)
                            frame.Clone(frm.ID, pNew.ID);
                    }

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: cloning master page with id " + id + " - " + ex.Message + "...");
            }
        }

        /* Properties that can be comunicated with other ntec objects */
        /* Get Frames */
        public List<Frame> GetAllFrames(string name)
        {
            try
            {
                Page p = _allPages.Single(x => x.Name == name);

                return p.Frame.ToList();
            }
            catch { }

            return new List<Frame>();
        }

        public List<Frame> GetAllFrames(int id)
        {
            try
            {
                Page p = _allPages.Single(x => x.ID == id);

                return p.Frame.ToList();
            }
            catch { }

            return new List<Frame>();
        }
    }
}