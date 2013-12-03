using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Views.Frames
{
    /// <summary>
    /// Generic frame settings
    /// </summary>
    public class GenericFrameSettings
    {
        /* Javascript Header */
        static public string GetJavascriptTitlePosition()
        {
            string js = string.Empty;

            js += "  var repo; ";
            js += "  var defaultPosition; ";

            js += "  $(document).ready(function () { ";

            js += "      var wWidth = $(window).width(); ";            
            js += "      var tWidth = $('.title').width(); ";

            js += "      defaultPosition = (wWidth / 2) - (tWidth / 2); ";

            js += "      $('.title').css('left', defaultPosition); ";

            js += "      repo = setInterval(function () { TitleReposition() }, 2000); ";

            js += "      $('.title').css('width', $('.title').width() + 20); ";
            js += "  }); ";

            js += "  function TitleReposition() {  ";
            js += "      var scrollPos = $(this).scrollLeft(); ";

            js += "      if (defaultPosition + scrollPos != $('.title').css('left')) {  ";
            js += "			$('.title').css('left', defaultPosition + scrollPos); ";
            js += "      } ";
            js += "  } ";

            return js;
        }

        /******************** Frame Style ********************/
        static public HtmlGenericControl FrameBorderStyle(HtmlGenericControl mPanel, List<string> cssOptions)
        {
            foreach (string cssOp in cssOptions)
            {
                string[] op = cssOp.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                mPanel.Style.Add(op[0], op[1]);
            }

            return mPanel;
        }

        static public Panel FrameBorderStyle(Panel mPanel, List<string> cssOptions)
        {
            foreach (string cssOp in cssOptions)
            {
                string[] op = cssOp.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                mPanel.Style.Add(op[0], op[1]);
            }

            return mPanel;
        }

        static public FrameHtmlCtrls FrameTitleStyle(HtmlGenericControl titleContainer, HtmlGenericControl labelTitle, List<string> cssOptions)
        {
            foreach (string cssOp in cssOptions)
            {
                string[] op = cssOp.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (op[0].ToLower().Contains("border")      || 
                    op[0].ToLower().Contains("background")  || 
                    op[0].ToLower().Contains("height")      ||
                    op[0].ToLower().Contains("margin")      ||
                    op[0].ToLower().Contains("padding") 
                )
                    titleContainer.Style.Add(op[0], op[1]);
                else
                    labelTitle.Style.Add(op[0], op[1]);
            }

            return new FrameHtmlCtrls() { LabelTitle = labelTitle, TitleContainer = titleContainer };
        }
    }

    public class FrameHtmlCtrls
    {
        public HtmlGenericControl LabelTitle;
        public HtmlGenericControl TitleContainer;
    }
}