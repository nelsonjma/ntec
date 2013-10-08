using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Util;
using DbConfig;
using Views.BackOffice;

public partial class bo_pages_cfg_frame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["log_file"] = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

        try
        {
            Startup();
        }
        catch (Exception ex)
        {
            loging.Error("page configuration", "load page error", ex.Message, ViewState["log_file"].ToString());
        }
    }

    private void Startup()
    {
        // redirect users if they are HACKERS or NOT
        GenericBackOfficeSettings.RedirectUsersToMainPage();  // desligado para poder trabalhar...

        // definir uma forma mais segura ou webservice mais complexo...
        string hashKey = Generic.GetHash(Generic.GetWebConfigValue("WebServiceKey"));
        webServiceKey.Attributes.Add("value", hashKey);

        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                LoadFrame(Request.QueryString["id"]);
            }
            else if (Request.QueryString["id"] == null)
            {
                NewFrameDefaultValues();
            }
        }
    }

    protected void Save_OnClick(object sender, EventArgs e)
    {
        try
        {
            int id = -1; try { id = (Request.Form["f_id"] != null && Request.Form["f_id"] != "") ? Convert.ToInt32(Request.Form["f_id"]) : -1; } catch { }

            int pageId = Convert.ToInt32(Request.Form["f_pageid"]);

            string title = Request.Form["f_title"];

            int x = Convert.ToInt32(Request.Form["f_x"]);
            int y = Convert.ToInt32(Request.Form["f_y"]);
            int width = Convert.ToInt32(Request.Form["f_width"]);
            int height = Convert.ToInt32(Request.Form["f_height"]);

            int isActive = Convert.ToInt32(Request.Form["f_isactive"]);
            string fType = Request.Form["f_type"];
            string scroll = Request.Form["f_scroll"];
            int interval = Convert.ToInt32(Request.Form["f_schedule_interval"]);

            string options = Request.Form["f_options"];

            SaveFrame(id, pageId, title, x, y, width, height, isActive, scroll, fType, interval, options);

            Generic.ParentJavascriptMethodInjector("RefreshTree()");

            Generic.JavaScriptInjector("alert('Frame Saved');");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    protected void Clone_OnClick(object sender, EventArgs e)
    {
        try
        {
            int id = -1; try { id = (Request.Form["f_id"] != null && Request.Form["f_id"] != "") ? Convert.ToInt32(Request.Form["f_id"]) : -1; } catch { }

            int pageId = Convert.ToInt32(Request.Form["f_pageid"]);

            if (id == -1) Generic.JavaScriptInjector("alert('Cant clone frame');");

            CloneFrame(id, pageId);

            Generic.ParentJavascriptMethodInjector("RefreshTree()");

            Generic.JavaScriptInjector("alert('Frame Cloned');");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    protected void Delete_OnClick(object sender, EventArgs e)
    {
        try
        {
            int id = -1; try { id = (Request.Form["f_id"] != null && Request.Form["f_id"] != "") ? Convert.ToInt32(Request.Form["f_id"]) : -1; } catch { }

            if (id == -1) Generic.JavaScriptInjector("alert('Cant delete frame');");

            DeleteFrame(id);

            Generic.ParentJavascriptMethodInjector("RefreshTree()");

            Generic.JavaScriptInjector("alert('Frame Deleted');");
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /************************ Configurations ************************/
    private void LoadFrame(string id)
    {
        try
        {
            int fId = Convert.ToInt32(id);

            db_config_frame dcf = new db_config_frame(fId, false);
            dcf.Open();

            Frame f = dcf.Get(fId);

            f_id.Value = f.ID.ToString(CultureInfo.InvariantCulture);
            f_title.Value = f.Title;
            f_x.Value = f.X.ToString(CultureInfo.InvariantCulture);
            f_y.Value = f.Y.ToString(CultureInfo.InvariantCulture);
            f_width.Value = f.Width.ToString(CultureInfo.InvariantCulture);
            f_height.Value = f.Height.ToString(CultureInfo.InvariantCulture);
            f_options.InnerHtml = f.Options;

            BuildIsActive(f.IsActive);
            BuildScrollBar(f.Scroll);
            BuildPageSelector(f.IDPage);
            BuildFrameType(f.FrameType);
            BuildScheduleInterval(f.ScheduleInterval);

            LoadFrameTypeToOpTextArea(f.FrameType);
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    private void NewFrameDefaultValues()
    {
        try
        {
            f_x.Value = "0";
            f_y.Value = "0";
            f_width.Value = "0";
            f_height.Value = "0";

            BuildIsActive(1);           // by default is active
            BuildPageSelector(-1);      // add pages first will be selected
            BuildFrameType("");         // no frame selected by default will be the first
            BuildScheduleInterval(0);   // will start disable

            // NOTE: by default will load first frame type to text area.
            LoadFrameTypeToOpTextArea(f_type.Items[0].Value);
        }
        catch (Exception ex)
        {
            Generic.JavaScriptInjector("alert('" + ex.Message + "');");
        }
    }

    /// <summary>
    /// Save the frame content
    /// </summary>
    private static void SaveFrame(int id, int pageId, string title, int x, int y, int width, int height, 
                                  int isActive, string scroll, string frameType, int interval, string options)
    {
        db_config_frame dcf = null;

        try
        {
            dcf = id >= 0
                        ? new db_config_frame(id, false) // frame exists query to frame
                        : new db_config_frame(pageId);  // new frame filter query with just page frames

            dcf.Open();

            Frame f = id >= 0
                        ? dcf.Get(id)
                        : new Frame();

            f.IDPage = pageId;
            f.Title = title;
            f.X = x;
            f.Y = y;
            f.Width = width;
            f.Height = height;
            f.IsActive = isActive;
            f.Scroll = scroll;
            f.FrameType = frameType;
            f.ScheduleInterval = interval;
            f.Options = options;

            if (id >= 0)
                dcf.Commit();
            else
                dcf.Add(f);
        }
        catch (Exception ex)
        {
            throw new Exception("error: Save Frame: " + ex.Message);
        }
        finally
        {
            if (dcf != null)
                dcf.Close();
        }
    }
    
    /// <summary>
    /// Copy the content of current frame to a new frame
    /// </summary>
    private static void CloneFrame(int id, int pageId)
    {
        db_config_frame dcf = null;

        try
        {
            dcf = new db_config_frame(id, false);
            dcf.Open();

            dcf.Clone(id, pageId);
            
        }
        catch (Exception ex)
        {
            throw new Exception("error: Clone Frame: " + ex.Message);
        }
        finally
        {
            if (dcf != null)
                dcf.Close();
        }
    }

    /// <summary>
    /// Delete frame by id
    /// </summary>
    private static void DeleteFrame(int id)
    {
        db_config_frame dcf = null;

        try
        {
            dcf = new db_config_frame(id, false);
            dcf.Open();

            dcf.Delete(id);
        }
        catch (Exception ex)
        {
            throw new Exception("error: Delete Frame: " + ex.Message);
        }
        finally
        {
            if (dcf != null)
                dcf.Close();
        }
    }

    /************************ Selector Box ************************/
    /// <summary>
    ///  Build page selector if ID  is negative then it will select first item
    /// </summary>
    private void BuildPageSelector(int currentPageId)
    {
        db_config_page dcp = null;

        // Clear all items
        f_pageid.Items.Clear();

        try
        {
            dcp = new db_config_page();
            dcp.Open();

            List<Page> allPages = dcp.AllPages.OrderBy(x=>x.Title).ToList();

            for (int i = 0; i < allPages.Count; i++)
            {
                f_pageid.Items.Add(new ListItem(
                                                allPages[i].Title, 
                                                allPages[i].ID.ToString(CultureInfo.InvariantCulture)
                                                ));

                // Get the page position then set the selector index
                if (allPages[i].ID == currentPageId)
                    f_pageid.SelectedIndex = i;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("error: building page list " + ex.Message);
        }
        finally
        {
            if (dcp != null)
                dcp.Close();
        }
    }

    /// <summary>
    /// Build Is Active selector
    /// </summary>
    private void BuildIsActive(int isActive)
    {
        f_isactive.SelectedIndex = isActive;
    }

    /// <summary>
    /// Set the Scroll Bar
    /// </summary>
    private void BuildScrollBar(string scroll)
    {
        switch (scroll)
        {
            case "auto":
                f_scroll.SelectedIndex = 0;
                break;
            case "yes":
                f_scroll.SelectedIndex = 1;
                break;
            case "no":
                f_scroll.SelectedIndex = 2;
                break;
        }
    }

    /// <summary>
    /// Set the FrameType
    /// </summary>
    private void BuildFrameType(string fType)
    {
        db_config_options dco = null;

        f_type.Items.Clear();

        try
        {
            dco = new db_config_options("frame");
            dco.Open();

            List<Options> options = dco.AllOptions;

            options.RemoveAt(options.FindIndex(x=> x.Name == "generic"));

            for (int i = 0; i < options.Count; i++)
            {
                f_type.Items.Add(new ListItem(
                                            options[i].Name,
                                            options[i].Name
                                            ));

                // Get the option position then set the selector index
                if (options[i].Name == fType)
                    f_type.SelectedIndex = i;    
            }
        }
        catch (Exception ex)
        {
            throw new Exception("error: build frame type " + ex.Message);
        }
        finally
        {
            if (dco != null)
                dco.Close();
        }
    }

    /// <summary>
    /// build the schedule interval select box
    /// </summary>
    private void BuildScheduleInterval(int? interval)
    {
        db_config_options dco = null;

        f_schedule_interval.Items.Clear();

        try
        {
            dco = new db_config_options("backoffice", "frame_options");
            dco.Open();

            Options op = dco.Get("frame_options");

            OptionItems oi = new OptionItems(op.Options1);
            
            List<string> scheduleList = oi.GetList("schedule_interval");
            
            for (int i = 0; i < scheduleList.Count; i++)
            {
                string[] schedule = scheduleList[i].Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries);

                f_schedule_interval.Items.Add(new ListItem(
                                                            schedule[1], 
                                                            schedule[0]
                                                            ));

                try
                {
                    if (Convert.ToInt32(schedule[0]) == interval)
                        f_schedule_interval.SelectedIndex = i;
                } catch {}
            }
        }
        catch (Exception ex)
        {
            throw new Exception("error: build frame type " + ex.Message);
        }
        finally
        {
            if (dco != null)
                dco.Close();
        }
    }

    /// <summary>
    /// Load available options from loaded frame type
    /// </summary>
    private void LoadFrameTypeToOpTextArea(string ftype)
    {
        db_config_options dco = null;

        try
        {
            dco = new db_config_options("frame", ftype);
            dco.Open();

            Options op = dco.Get(ftype);

            aval_type_op.InnerHtml = op.Options1;
        }
        catch (Exception ex)
        {
            throw new Exception("error: build default frame type (" + ftype + "): " + ex.Message);
        }
        finally
        {
            if (dco != null)
                dco.Close();
        }
    }
}
