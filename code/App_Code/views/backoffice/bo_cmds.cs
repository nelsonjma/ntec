using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.IO;

using Views.BackOffice;
using DbConfig;

/// <summary>
/// Backoffice Ajax Commands
/// </summary>
[WebService(Namespace = "http://ntec.bo_cmds.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[ScriptService]
public class bo_cmds : WebService 
{
    private readonly string _logRecord = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string HelloWorld() 
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        return js.Serialize("Hello World");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string PageTree(string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;
        
        PageTree pt = new PageTree();

        return js.Serialize(pt.TreeItems);
    }

    /*********************************** Page Operations ***********************************/
    [WebMethod]
    public string DeletePage(string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_page dcp = null;

        try 
	    {
            dcp = new db_config_page(Convert.ToInt32(pageId), false);
		    dcp.Open();
            dcp.Delete(Convert.ToInt32(pageId));
            return "Operation Done";
	    }
	    catch (Exception ex)
	    {
            return ex.Message;
	    }
        finally
        {
            if (dcp != null) dcp.Close();
        }
    }

    [WebMethod]
    public string ClonePage(string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_page dcp = null;

        try
        {
            dcp = new db_config_page(Convert.ToInt32(pageId), false);

            dcp.Open();
            dcp.Clone(Convert.ToInt32(pageId));
            return "Operation Done";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcp != null) dcp.Close();
        }
    }

    [WebMethod]
    public string GetPageType(string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_page dcp = null;

        try
        {
            int id = Convert.ToInt32(pageId);

            dcp = new db_config_page(id, false);
            dcp.Open();

            Page p = dcp.Get(id);

            OptionItems oi  = new OptionItems(p.Options);

            return oi.GetSingle("page_type").ToLower() == "free_draw" 
                                                            ? "free_draw"
                                                            : "table";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcp != null)
                dcp.Close();
        }
    }

    [WebMethod]
    public string GetPageName(string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_page dcp = null;

        try
        {
            int id = Convert.ToInt32(pageId);

            dcp = new db_config_page(id, false);
            dcp.Open();

            Page p = dcp.Get(id);

            return p.Name;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcp != null)
                dcp.Close();
        }
    }

    [WebMethod]
    public string GetPageTitle(string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_page dcp = null;

        try
        {
            int id = Convert.ToInt32(pageId);

            dcp = new db_config_page(id, false);
            dcp.Open();

            Page p = dcp.Get(id);

            return p.Title;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcp != null)
                dcp.Close();
        }
    }

    /*********************************** Master Page Operations ***********************************/
    [WebMethod]
    public string DeleteMasterPage(string masterPageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_master_page dcmp = null;

        try
        {
            dcmp = new db_config_master_page();
            dcmp.Open();
            dcmp.Delete(Convert.ToInt32(masterPageId));
            return "Operation Done";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcmp != null) dcmp.Close();
        }
    }

    /*********************************** Frame Operations ***********************************/
    /// <summary>
    /// List all frames from one page, needed to be used in the frame viewer
    /// </summary>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ListFrames(string pageId, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_page dcp = null;

        try
        {
            int id = Convert.ToInt32(pageId);

            dcp = new db_config_page(id, true);
            dcp.Open();

            List<Frame> frames = dcp.GetAllFrames(id);

            List<Dictionary<string, object>> rows = frames.Select(f => new Dictionary<string, object>
                                                                    {
                                                                        {"ID", f.ID}, 
                                                                        {"Title", f.Title}, 
                                                                        {"FrameType", f.FrameType},
                                                                        {"X", f.X}, 
                                                                        {"Y", f.Y}, 
                                                                        {"Width", f.Width}, 
                                                                        {"Height", f.Height}, 
                                                                        {"IsActive", f.IsActive}, 
                                                                        {"Columnspan", new OptionItems(f.Options).GetSingle("columnspan")}, 
                                                                        {"Rowspan", new OptionItems(f.Options).GetSingle("rowspan")},
                                                                    }).ToList();

            return js.Serialize(rows);
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice Frames Webservice", "List Frames", ex.Message, _logRecord);
        }
        finally
        {
            if (dcp != null) dcp.Close();
        }

        return js.Serialize("");
        
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetFrame(string frameId, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_frame dcf = null;

        try
        {
            int id = Convert.ToInt32(frameId);

            dcf = new db_config_frame(id, false);
            dcf.Open();

            Frame f = dcf.Get(id);

            return js.Serialize(new Dictionary<string, object>
                                                {
                                                    {"ID", f.ID},
                                                    {"PageId", f.IDPage},
                                                    {"Title", f.Title},
                                                    {"X", f.X},
                                                    {"Y", f.Y},
                                                    {"Width", f.Width},
                                                    {"Height", f.Height},
                                                    {"FrameType", f.FrameType},
                                                    {"Scroll", f.Scroll},
                                                    {"Options", f.Options},
                                                    {"IsActive", f.IsActive},
                                                    {"ScheduleInterval", f.ScheduleInterval},
                                                });
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice Frames Webservice", "Get Frame", ex.Message, _logRecord);
        }
        finally
        {
            if (dcf != null) 
                dcf.Close();
        }

        return js.Serialize("");
    }

    /// <summary>
    /// used to move frame in frame options
    /// </summary>
    [WebMethod]
    public string MoveFrame(string pageId, string frameId, string pageType, int height, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_frame dcf = null;

        try
        {
            int fid = Convert.ToInt32(frameId);
            int pid = Convert.ToInt32(pageId);

            dcf = new db_config_frame(pid);
            dcf.Open();

            int y = dcf.Get(fid).Y;

            List<Frame> frames = (from f in dcf.AllFrames
                                    where f.Y >= y
                                    select f).ToList();

            foreach (Frame f in frames)
            {
                f.Y = f.Y + height;
            }

            dcf.Commit();

            return "Frame moved";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcf != null)
                dcf.Close();
        }
    }

    /*********************************** Load Css ***********************************/
    /// <summary>
    /// Loads css to client
    /// </summary>
    [WebMethod]
    public string LoadCssFile(string fileName, string cssType, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;
        
        try
        {
            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return "cant get website folder path";

            if (cssType == "frames")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\frames\"
                                        : sitePath + @"\css\views\frames\";

                return File.ReadAllText(sitePath + fileName);
            }

            if (cssType == "page")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\page\"
                                        : sitePath + @"\css\views\page\";

                return File.ReadAllText(sitePath + fileName);
            }

            if (cssType == "frontoffice")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\frontoffice\"
                                        : sitePath + @"\css\views\frontoffice\";

                return File.ReadAllText(sitePath + fileName);
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return string.Empty;
    }

    [WebMethod]
    public string CopyCssFile(string oldFileName, string newFileName ,string cssType, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty; 
        
        try
        {
            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return "cant get website folder path";

            if (!newFileName.ToLower().Contains(".css")) newFileName = newFileName + ".css";

            /************************************************************************/
            if (cssType == "frames")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\frames\"
                                        : sitePath + @"\css\views\frames\";

                File.Copy(sitePath + oldFileName, sitePath + newFileName);

                return "File Copied";
            }

            if (cssType == "page")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\page\"
                                        : sitePath + @"\css\views\page\";

                File.Copy(sitePath + oldFileName, sitePath + newFileName);

                return "File Copied";
            }

            if (cssType == "frontoffice")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\frontoffice\"
                                        : sitePath + @"\css\views\frontoffice\";

                File.Copy(sitePath + oldFileName, sitePath + newFileName);

                return "File Copied";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return string.Empty;
        
    }

    [WebMethod]
    public string DeleteCssFile(string fileName, string cssType, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        try
        {
            if (fileName == string.Empty) throw new Exception("error: nothing to delete");

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return "cant get website folder path";

            if (!fileName.ToLower().Contains(".css")) fileName = fileName + ".css";

            /************************************************************************/
            if (cssType == "frames") 
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\frames\"
                                        : sitePath + @"\css\views\frames\";

                File.Delete(sitePath + fileName);

                return "File Deleted";
            }

            if (cssType == "page") 
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\page\"
                                        : sitePath + @"\css\views\page\";

                File.Delete(sitePath + fileName);

                return "File Deleted";
            }

            if (cssType == "frontoffice")
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"css\views\frontoffice\"
                                        : sitePath + @"\css\views\frontoffice\";

                File.Delete(sitePath + fileName);

                return "File Deleted";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return string.Empty;

    }


    /*********************************** Load Templates ***********************************/
    /// <summary>
    /// Loads css to client
    /// </summary>
    [WebMethod]
    public string LoadTemplateFile(string fileName, string templateType, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        try
        {
            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return "cant get website folder path";

            if (templateType == "chart_template") // read chart template folder
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"template\charts\"
                                        : sitePath + @"\template\charts\";

                return File.ReadAllText(sitePath + fileName);
            }

            if (templateType == "mon_template") // read mon template folder
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"template\mon\"
                                        : sitePath + @"\template\mon\";

                return File.ReadAllText(sitePath + fileName);
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return string.Empty;
    }

    [WebMethod]
    public string CopyTemplateFile(string oldFileName, string newFileName, string templateType, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        try
        {
            if (oldFileName == string.Empty && newFileName == string.Empty) throw new Exception("error: nothing to copy");

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return "cant get website folder path";

            if (!newFileName.ToLower().Contains(".xml")) newFileName = newFileName + ".xml";

            /************************************************************************/
            if (templateType == "chart_template") // chart template file copy
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"template\charts\"
                                        : sitePath + @"\template\charts\";

                File.Copy(sitePath + oldFileName, sitePath + newFileName);

                return "File Copied";
            }

            if (templateType == "mon_template") // mon template file copy
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"template\mon\"
                                        : sitePath + @"\template\mon\";

                File.Copy(sitePath + oldFileName, sitePath + newFileName);

                return "File Copied";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return string.Empty;

    }

    [WebMethod]
    public string DeleteTemplateFile(string fileName, string templateType, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        try
        {
            if (fileName == string.Empty) throw new Exception("error: nothing to delete");

            string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            if (sitePath == null) return "cant get website folder path";

            if (!fileName.ToLower().Contains(".xml")) fileName = fileName + ".xml";

            /************************************************************************/
            if (templateType == "chart_template") // chart template file copy
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"template\charts\"
                                        : sitePath + @"\template\charts\";

                File.Delete(sitePath + fileName);

                return "File Deleted";
            }

            if (templateType == "mon_template") // mon template file copy
            {
                sitePath = sitePath.EndsWith(@"\")
                                        ? sitePath + @"template\mon\"
                                        : sitePath + @"\template\mon\";

                File.Delete(sitePath + fileName);

                return "File Deleted";
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

        return string.Empty;

    }

    /*********************************** Frontoffice Options Controls ***********************************/
    [WebMethod]
    public string GetFrontOfficeOption(string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        try
        {
            db_config_frontoffice dcf = new db_config_frontoffice();
            dcf.Open();

            string options = dcf.Get.Options;

            dcf.Close();

            return options;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /*********************************** Available Options Controls ***********************************/
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ListAvailableOptions(string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return js.Serialize("");

        try
        {
            db_config_options dco = new db_config_options();
            dco.Open();

            List <Options> options = dco.AllOptions.OrderBy(x=> x.OBJType).ToList();

            dco.Close();

            return js.Serialize(options);
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice", "Available Options WebService", ex.Message, _logRecord);
        }

        return js.Serialize("");
    }

    [WebMethod]
    public string DeleteAvailableOption(string opId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return "";

        db_config_options dco = null;

        try
        {
            dco = new db_config_options();
            dco.Open(); // open connection

            dco.Delete(Convert.ToInt32(opId));
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dco != null) dco.Close();
        }

        return "Option Deleted";
    }

    /// <summary>
    /// Get Just one option need object type and name
    /// </summary>
    [WebMethod]
    public string GetAvailableOption(string objType, string name, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_options dco = null;

        try
        {
            dco = new db_config_options(objType, name);
            dco.Open();

            Options option = dco.Get(name);



            return option.Options1;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dco != null)
                dco.Close();
        }
    }

    /*********************************** User BackOffice Controls ***********************************/
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ListUsers(string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null;

        try
        {
            dcu = new db_config_users();
            dcu.Open();

            List<JsonUser> users = (from u in dcu.AllUsers
                                    select new JsonUser
                                    {
                                        Id = u.ID,
                                        Name = u.Name,
                                        Pass = u.Pass,
                                        Admin = u.AdMIn == 1 ? 1 : 0,
                                        Description = u.Description,
                                        AdminOptions = u.AdMInOptions,
                                        UserOptions = u.UserOptions,
                                    }).ToList();

            return js.Serialize(users);
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice User Webservice", "Error", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return js.Serialize("");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ListUserPages(string userId, string ctrl)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return js.Serialize("");

        // first validation to garanty that user is graiter then zero
        int auxUserId = Convert.ToInt32(userId);
        if (auxUserId < 0) return js.Serialize("");

        db_config_users dcu = null;

        try
        {
            dcu = new db_config_users(auxUserId);
            dcu.Open();

            List<JsonUserPages> pages = (from p in dcu.GetPages()
                                         select new JsonUserPages { Id = p.ID, Title = p.Title }).ToList();

            return js.Serialize(pages);
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice User Webservice", "User Pages", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return js.Serialize("");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ListAvailablePagesToUser(string userId, string ctrl)
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return js.Serialize("");

        db_config_page dcp = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return js.Serialize("");

            dcp = new db_config_page();
            dcp.Open();

            db_config_users dcu = new db_config_users(dcp.Db, auxUserId);
            dcu.Open();

            // list user pages
            List<int> userPagesId = (from up in dcu.GetPages() select up.ID).ToList();
            
            // --------------------------------
            // list pages that user dont have
            List<JsonUserPages> noAvailableUserPages = userPagesId.Count == 0 ? (from ap in dcp.AllPages
                                                                                 select new JsonUserPages { Id = ap.ID, Title = ap.Title, Name = ap.Name }).ToList()
                                                                            : (from nup in dcp.AllPages 
                                                                                where !userPagesId.Contains(nup.ID)
                                                                                select new JsonUserPages { Id = nup.ID, Title = nup.Title, Name = nup.Name }).ToList();

            return js.Serialize(noAvailableUserPages);
        }
        catch (Exception ex)
        {
            loging.Error("BackOffice User Webservice", "List All Pages ", ex.Message, _logRecord);
        }
        finally
        {
            if (dcp != null) dcp.Close();
        }

        return js.Serialize(""); 
    }

    [WebMethod]
    public string DeleteUser(string userId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_users dcu = null;

        try
        {
            // first validation to garanty that user is graiter then zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return "User ID is less then zero";

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            if (dcu.AllUsers.Count == 0) return "User not available";

            dcu.DeleteUser();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "Operation Done";
    }

    [WebMethod]
    public string DeleteUserPage(string userId, string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_users dcu = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return "User ID is less then zero";

            // first validation to guarantee that page is greater than zero
            int auxPageId = Convert.ToInt32(pageId);
            if (auxPageId < 0) return "Page ID is less then zero";

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            if (dcu.AllUsers.Count != 1) return "User not available";

            dcu.DeleteUserPage(auxPageId);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "Operation Done";
    }

    [WebMethod]
    public string AddPageToUser(string userId, string pageId, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_users dcu = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return "User ID is less then zero";

            // first validation to guarantee that page is greater than zero
            int auxPageId = Convert.ToInt32(pageId);
            if (auxPageId < 0) return "Page ID is less then zero";

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            if (dcu.AllUsers.Count != 1) return "User not available";

            dcu.AddPageToUser(auxPageId);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "Operation Done";
    }

    [WebMethod]
    public string CloneUser(string userId, string newUserName, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        if (ctrl != crlHash) return string.Empty;

        db_config_users dcu = null;

        try
        {
            // first validation to garanty that user is graiter then zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return "User ID is less then zero";

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            if (dcu.AllUsers.Count == 0) return "User not available";

            dcu.Clone(newUserName);

            return "User Cloned";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }
    }

    /*********************************** User FrontOffice Controls ***********************************/
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ListAllUserPages(string userId, string ctrl)
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_page dcp = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return js.Serialize("");

            dcp = new db_config_page();
            dcp.Open();

            db_config_users dcu = new db_config_users(dcp.Db, auxUserId);
            dcu.Open();

            // list user pages
            List<int> userPagesId = (from up in dcu.GetPages() select up.ID).ToList();

            if (userPagesId.Count > 0)
                dcp.SelectAuthenticatedObjectsFromDb(userPagesId); // list public pages and pages that user can have
            else
                dcp.SelectPublicObjectsFromDb();

            // --------------------------------
            // list pages that user dont have
            List<JsonUserPages> userPages = (from ap in dcp.AllPages
                                            select new JsonUserPages {Id = ap.ID, Title = ap.Title, Name = ap.Name}).ToList();


            return js.Serialize(userPages);
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "List All User Pages ", ex.Message, _logRecord);
        }
        finally
        {
            if (dcp != null) dcp.Close();
        }

        return js.Serialize("");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetUserFavorites(string userId, string ctrl)
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null;
        db_config_page dcp = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return js.Serialize("");

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            dcp = new db_config_page();
            dcp.Open();

            object userOptions = dcu.Get(auxUserId).UserOptions;

            if (userOptions == null) return js.Serialize("");


            OptionItems oi = new OptionItems((string)userOptions);

            List<Dictionary<string, string>> favoriteList = new List<Dictionary<string, string>>();

            foreach (string favoritePage in oi.GetList("favorites"))
            {
                try
                {
                    string title = dcp.Get(favoritePage).Title;

                    favoriteList.Add(new Dictionary<string, string>()
                    {
                        {"Name", favoritePage}, 
                        {"Title", title}
                    });
                }
                catch {}
            }

            return js.Serialize(favoriteList);
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "Get Favorites ", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();

            try { if(dcp != null) dcp.Close(); } catch {} // needed to open a new connection (forgot to implement a page method that accepts new  )
        }

        return js.Serialize("");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetUserDefaultPage(string userId, string ctrl)
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null; // user
        db_config_page dcp = null;  // page

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return js.Serialize("");

            dcu = new db_config_users(auxUserId);
            dcu.Open();
            
            object userOptions = dcu.Get(auxUserId).UserOptions;

            // if user does not exist leaves
            if (userOptions == null) return string.Empty;

            OptionItems oi = new OptionItems((string)userOptions);

            string pageName = oi.GetSingle("default_frontoffice_page");

            // if no default page leave
            if (pageName == string.Empty) return string.Empty;

            dcp = new db_config_page(pageName);
            dcp.Open();

            string pageTitle = dcp.Get(pageName).Title;

            Dictionary<string, string> defPage = new Dictionary<string, string>() {{"Name", pageName}, {"Title", pageTitle}};

            return js.Serialize(defPage);
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "Get Default Page ", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();

            try { if(dcp != null) dcp.Close(); } catch { }
        }

        return string.Empty;
    }

    [WebMethod]
    public string AddUserFavoritePage(string userId, string pageName, string ctrl)
    {
        
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return string.Empty;

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            Users u = dcu.Get(auxUserId);

            if (u == null) return string.Empty; // if user does not exists will leave width no return message 

            OptionItems oi = new OptionItems(u.UserOptions);

            List<string> favoriteList = oi.GetList("favorites");

            if (favoriteList.IndexOf(pageName) < 0)
            {
                favoriteList.Add(pageName);
                oi.UpdateOptions("favorites", favoriteList);
                u.UserOptions = oi.GetOptionsString();
                dcu.Commit();

                return "page added to favorites";
            }
            else
                return "page already added to favorite.";
            
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "Add User Favorite Page", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "failed to add page";
    }

    [WebMethod]
    public string RemoveUserFavoritePage(string userId, string pageName, string ctrl)
    {
        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return string.Empty;

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            Users u = dcu.Get(auxUserId);

            if (u == null) return string.Empty; // if user does not exists will leave width no return message

            OptionItems oi = new OptionItems(u.UserOptions);

            List<string> favoriteList = oi.GetList("favorites");

            favoriteList.Remove(pageName);

            oi.UpdateOptions("favorites", favoriteList);

            u.UserOptions = oi.GetOptionsString();

            dcu.Commit();

            return "page removed from favorites";
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "Remove User From Favorites", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "failed to remove page";
    }

    [WebMethod]
    public string SetUserDefaultFrontOfficePage(string userId, string pageName, string ctrl)
    {

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return string.Empty;

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            Users u = dcu.Get(auxUserId);

            if (u == null) return string.Empty; // if user does not exists it will leave with no return messsage

            OptionItems oi = new OptionItems(u.UserOptions);
            oi.UpdateOptions("default_frontoffice_page", new List<string> { pageName });
            
            u.UserOptions = oi.GetOptionsString();

            dcu.Commit();

            return pageName != ""
                                ? "Page " + pageName + " is set as default frontoffice page"
                                : "Default frontoffice page is clean";
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "Add User Favorite Page", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "failed to set page";
    }

    [WebMethod]
    public string ChangeUserPassword(string userId, string newPass, string ctrl)
    {

        JavaScriptSerializer js = new JavaScriptSerializer();

        string crlHash = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));

        //if (ctrl != crlHash) return js.Serialize("");

        db_config_users dcu = null;

        try
        {
            // first validation to guarantee that user is greater than zero
            int auxUserId = Convert.ToInt32(userId);
            if (auxUserId < 0) return js.Serialize("");

            dcu = new db_config_users(auxUserId);
            dcu.Open();

            Users u = dcu.Get(auxUserId);

            if (u == null) return string.Empty; // if user does not exists it will leave with no return messsage

            u.Pass = newPass;
            dcu.Commit();

            return "password changed";
        }
        catch (Exception ex)
        {
            loging.Error("FrontOffice User Webservice", "User Change Password", ex.Message, _logRecord);
        }
        finally
        {
            if (dcu != null) dcu.Close();
        }

        return "password not changed";
    }
}


/* Classes used in backoffice User Ctrls */
/// <summary>
/// to list user pages in json format
/// </summary>
public class JsonUserPages
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
}

/// <summary>
/// To list users in json format
/// </summary>
public class JsonUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Pass { get; set; }
    public int Admin { get; set; }
    public string Description { get; set; }
    public string AdminOptions { get; set; }
    public string UserOptions { get; set; }
}

/******************************************/