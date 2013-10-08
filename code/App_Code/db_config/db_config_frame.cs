using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;

namespace DbConfig
{
    /// <summary>
    /// Db to linq frame configuration
    /// </summary>
    public class db_config_frame : db_config
    {
        private List<Frame> _allFrames;

        private int _frameId;
        private int _pageId;
        private string _pageName;

        public db_config_frame(int frameId, bool iamdummy)
        {
            _allFrames = new List<Frame>();

            _pageName = string.Empty;
            _pageId = -1;
            _frameId = frameId;
        }

        public db_config_frame(string pageName)
        {
            _allFrames = new List<Frame>();

            _pageName = pageName;
            _pageId = -1;
            _frameId = -1;
        }

        public db_config_frame(int pageId)
        {
            _allFrames = new List<Frame>();

            _pageName = string.Empty;
            _pageId = pageId;
            _frameId = -1;
        }

        public db_config_frame(NtEcConFigs db, List<Frame> frames, int pageId)
        {
            _allFrames = frames;
            _db = db;
            _pageName = string.Empty;
            _pageId = pageId;
            _frameId = -1;
        }

        public db_config_frame(NtEcConFigs db, List<Frame> frames, string pageName)
        {
            _allFrames = frames;
            _db = db;
            _pageName = pageName;
            _pageId = -1;
            _frameId = -1;
        }

        private Frame Copy(Frame f)
        {
            try
            {
                using (_db)
                {
                    Frame fNew = new Frame();

                    fNew.IDPage = f.IDPage;
                    fNew.FrameType = f.FrameType;
                    fNew.Height = f.Height;
                    fNew.Width = f.Width;
                    fNew.LastUpd = f.LastUpd;
                    fNew.Options = f.Options;
                    fNew.Scroll = f.Scroll;
                    fNew.Title = f.Title;
                    fNew.X = f.X;
                    fNew.Y = f.Y;
                    fNew.IsActive = f.IsActive;
                    fNew.ScheduleInterval = f.ScheduleInterval;

                    return fNew;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Frame Copy(Frame f, int newPageId)
        {
            try
            {
                using (_db)
                {
                    Frame fNew = new Frame();

                    fNew.IDPage = newPageId;
                    fNew.FrameType = f.FrameType;
                    fNew.Height = f.Height;
                    fNew.Width = f.Width;
                    fNew.LastUpd = f.LastUpd;
                    fNew.Options = f.Options;
                    fNew.Scroll = f.Scroll;
                    fNew.Title = f.Title;
                    fNew.X = f.X;
                    fNew.Y = f.Y;
                    fNew.IsActive = f.IsActive;
                    fNew.ScheduleInterval = f.ScheduleInterval;

                    return fNew;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckPageStatus(Frame f)
        {
            try
            {
                if (f.Page == null && f.IDPage >= 0)
                {
                    f.Page = _db.Page.Single(x => x.ID == f.IDPage);
                }
                else if (f.IDPage < 0)
                {
                    throw new Exception("no page selected");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error in page id " + f.IDPage + " - " + ex.Message);
            }
        }

        public override void Commit()
        {
            try
            {
                _db.Frame.Context.SubmitChanges();
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
                if (_pageId != -1)
                {
                    _allFrames = (from f in _db.Frame
                                  where f.IDPage == _pageId
                                  select f).ToList();
                }
                else if (_frameId != -1)
                {
                    _allFrames = (from f in _db.Frame
                                  where f.ID == _frameId
                                  select f).ToList();
                }
                else if (_pageName != string.Empty)
                {
                    _allFrames = (from f in _db.Frame
                        join p in _db.Page
                            on f.IDPage equals p.ID
                        where p.Name == _pageName
                        select f).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: select object from db - " + ex.Message + " ...");
            }
        }

        /* Get frames */
        public List<Frame> AllFrames
        {
            get { return _allFrames; }
            set { _allFrames = value; }
        }

        public Frame Get(string title)
        {
            try
            {
                return _allFrames.Single(x => x.Title == title);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return frame with title " + title + " - " + ex.Message + " ...");
            }
        }

        public Frame Get(int id)
        {
            try
            {
                return _allFrames.Single(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return frame with id " + id + " - " + ex.Message + " ...");
            }
        }

        /* Database operations */
        /* Insert */
        public void Add(Frame f)
        {
            try
            {
                using (_db)
                {
                    CheckPageStatus(f);

                    _db.Frame.InsertOnSubmit(f);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: inserting new frame with title " + f.Title + " - " + ex.Message + " ...");
            }
        }

        /* Delete */
        public void Delete(int id)
        {
            try
            {
                using (_db)
                {
                    Frame f = _db.Frame.Single(x => x.ID == id);

                    _db.Frame.DeleteOnSubmit(f);

                    Commit();

                    _allFrames.Clear();
                    _allFrames = null;

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting frame with id " + id + " - " + ex.Message + " ...");
            }
        }

        public void Delete(string title)
        {
            try
            {
                using (_db)
                {
                    Frame f = _db.Frame.Single(x => x.Title == title);

                    _db.Frame.DeleteOnSubmit(f);

                    Commit();

                    _allFrames.Clear();
                    _allFrames = null;

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: deleting frame with title " + title + " - " + ex.Message + " ...");
            }
        }

        /* Clone */
        public void Clone(int id)
        {
            try
            {
                using (_db)
                {
                    Frame f = _db.Frame.Single(x => x.ID == id);

                    Frame fNew = Copy(f);

                    fNew.Title = fNew.Title + "CLONE";

                    _db.Frame.InsertOnSubmit(fNew);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: cloning master page with id " + id + " - " + ex.Message + "...");
            }
        }

        public void Clone(int id, int newPageId)
        {
            try
            {
                using (_db)
                {
                    Frame f = _db.Frame.Single(x => x.ID == id);

                    Frame fNew = Copy(f, newPageId);

                    fNew.Title = fNew.Title + "CLONE";

                    _db.Frame.InsertOnSubmit(fNew);

                    Commit();

                    SelectObjectsFromDb();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: cloning master page with id " + id + " - " + ex.Message + "...");
            }
        }

        /* Frame Options */
        public OptionItems GetFrameOptions(int id)
        {
            try
            {
                return new OptionItems(_allFrames.Single(x => x.ID == id).Options);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return options from frame " + id + " - " + ex.Message + " ...");
            }
        }

        public OptionItems GetFrameOptions(string title)
        {
            try
            {
                return new OptionItems(_allFrames.Single(x => x.Title == title).Options);
            }
            catch (Exception ex)
            {
                throw new Exception("error: cant return options from frame " + title + " - " + ex.Message + " ...");
            }
        }
    }
}