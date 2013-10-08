using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections.Generic;

using DbConfig;

namespace Views.Page
{
    /// <summary>
    /// Summary description for draw_frames
    /// </summary>
    public class DrawFrames
    {
        private readonly string _pageName;
        private readonly string _pageType;

        readonly db_config_options _options;

        public DrawFrames(string pageName, string pageType)
        {
            _pageName = pageName;
            _pageType = pageType;

            _options = new db_config_options("frame");
            _options.Open();
        }

        public Control GetFrames()
        {
            try
            {
                // by default will use table style;
                return _pageType.ToLower().Trim() == "free_draw" 
                                                    ? BuildFreeDrawFrames()
                                                    : BuildTableFrames();
            }
            catch (Exception ex)
            {
                throw new Exception("error: in build frames - " + ex.Message + " ...");
            }
        }

        /**************** Table of Free Draw ****************/
        /// <summary>
        /// Build the page width frames using X, Y.
        /// </summary>
        /// <returns></returns>
        private Control BuildFreeDrawFrames()
        {
            Panel pn = new Panel();
            pn.Style.Add("margin", "0");
            pn.Style.Add("pading", "0");
            pn.BackColor = Color.Transparent;

            try
            {
                db_config_frame frames = new db_config_frame(_pageName);
                frames.Open();

                foreach (Frame frame in frames.AllFrames)
                {
                    if (frame.IsActive != 1) continue; // if is != jumps the code below

                    string frameIdHash = Generic.GetHash(frame.ID.ToString());

                    pn.Controls.Add(BuildFrameCtrl(frame, frameIdHash)); // build page frame

                    SetSessionFrame(frameIdHash, frame);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error: in build frames - " + ex.Message + " ...");
            }

            return pn;
        }

        /// <summary>
        /// Build the page width table style.
        /// </summary>
        /// <returns></returns>
        private Control BuildTableFrames()
        {
            Table tb = new Table();
            TableRow row = new TableRow() { HorizontalAlign = HorizontalAlign.Center, VerticalAlign = VerticalAlign.Top };
            TableCell cell = null;

            try
            {
                db_config_frame frames = new db_config_frame(_pageName);
                OptionItems oi = null;

                frames.Open();
                
                frames.AllFrames.Sort(delegate(Frame a, Frame b) { int ydiff = a.Y.CompareTo(b.Y); return (ydiff != 0) ? ydiff : a.X.CompareTo(b.X); });

                foreach (Frame frm in frames.AllFrames)
                {
                    if (frm.IsActive != 1) continue; // if is != jumps the code below

                    // adds new rows if frame Y position is bigger then the number of rows 
                    while (tb.Rows.Count <= frm.Y) tb.Rows.Add(new TableRow() { HorizontalAlign = HorizontalAlign.Center, VerticalAlign = VerticalAlign.Top });

                    oi = new OptionItems(frm.Options);

                    cell = new TableCell()
                    {
                        HorizontalAlign = HorizontalAlign.Center,
                        VerticalAlign = VerticalAlign.Top,
                        RowSpan = SetRowSpan(oi.GetSingle("rowspan")),
                        ColumnSpan = SetColmnSpan(oi.GetSingle("columnspan")),
                    };

                    string frameIdHash = Generic.GetHash(frm.ID.ToString());

                    cell.Controls.Add(BuildFrameCtrl(frm, frameIdHash));

                    SetSessionFrame(frameIdHash, frm);

                    // add new cell if frame X position is bigger then number of cells
                    while (tb.Rows[frm.Y].Cells.Count <= frm.X) tb.Rows[frm.Y].Cells.Add(new TableCell());

                    tb.Rows[frm.Y].Cells.AddAt(frm.X, cell);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tb;
        }
        /**************** Iframe build ****************/
        /// <summary>
        /// Create the frame width a panel and a iframe
        /// </summary>
        private Control BuildFrameCtrl(Frame f, string frameIdHash)
        {
            Panel pn = new Panel();

            OptionItems oi = new OptionItems(f.Options);

            if (_pageType.ToLower().Trim() == "free_draw")
            {
                pn.Style.Add("position", "absolute");
                pn.Style.Add("top", f.Y + "px");
                pn.Style.Add("left", f.X + "px");
            }

            pn.Style.Add("margin", "0");
            pn.Style.Add("padding", "0");
            pn.Style.Add("border", "0");
            
            pn.Width = Unit.Pixel(f.Width);
            pn.Height = Unit.Pixel(f.Height);
            pn.BackColor = Color.Transparent;

            // ------ Border ------
            pn = Views.Frames.GenericFrameSettings.FrameBorderStyle(
                                                                    pn, 
                                                                    oi.GetList("frame_style")
                                                                    );
            // --------------------

            HtmlObjects iframe = new HtmlObjects("iframe");

            iframe.Id = frameIdHash;

            iframe.AddStyle("border", "0");

            iframe.AddAttribute("scrolling", f.Scroll);
            iframe.AddAttribute("width", f.Width + "px");
            iframe.AddAttribute("height", f.Height + "px");
            iframe.AddAttribute("src", _options.Get(f.FrameType).URL + "?fm=" + frameIdHash);

            pn.Controls.Add(iframe.GetObject);

            // for now its just to tell client that this frame is slave of some filter.
            LoadFrameInfoToClient(
                                    f.ID.ToString(CultureInfo.InvariantCulture), 
                                    frameIdHash, 
                                    oi.GetSingle("master_filter")
                                ); 

            return pn;
        }

        /**************** Send data from page to frame ****************/
        /// <summary>
        /// Load the session variable with the frame to be used in the frame...
        /// </summary>
        /// <param name="frameId"></param>
        /// <param name="frame"></param>
        private void SetSessionFrame(string frameId, Frame frame)
        {
            Views.Frames.frame_sessions.SetFrame(frameId, frame); // set session variable to be used in frame page
        }

        /**************************** Table Options ****************************/
        int SetRowSpan(string op)
        {
            try
            {
                if (op != string.Empty)
                    return Convert.ToInt32(op);
            }
            catch {}
            
            return 0;
        }

        int SetColmnSpan(string op)
        {
            try
            {
                if (op != string.Empty)
                    return Convert.ToInt32(op);
            }
            catch { }

            return 0;
        }

        /**************************** Client Side Operations  ****************************/
        /// <summary>
        /// Used to get properties to client side to be handled by javascript
        /// </summary>
        private static void LoadFrameInfoToClient(string fid, string frameHashId, string masterFilterId)
        {
            string js = string.Empty;

            js += "var f" + fid + " = new Frame(); ";
            js += "f" + fid + ".SetHashId('" + frameHashId + "'); ";
            js += "f" + fid + ".SetMasterTitle('" + masterFilterId + "'); ";
            js += "f" + fid + ".SetIsSlave(" + (masterFilterId != string.Empty ? 1 : 0) + "); ";
            js += "AddFrame(f" + fid + "); ";

            Generic.JavaScriptInjector(frameHashId, js);
        }

    }
}