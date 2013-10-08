using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DbConfig;

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

        /******************** Xml Data ********************/
        static public string BuildXmlFilePath(string fileName, int pageId)
        {
            db_config_page page = null;

            try
            {
                // if contains a fixed path then there is no need to add page or default path
                if (fileName.StartsWith("http://") || fileName.Contains(@":\"))
                    return fileName;

                // if the filename is not added then there is no reason to be here
                if (string.IsNullOrEmpty(fileName)) throw new Exception("filename is empty");

                // page fisical path
                page = new db_config_page(pageId, false);
                page.Open();
                string pageXmlFolder = page.Get(pageId).XMLFolderPath;

                // webconfig fisical path
                string sitePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
                string xmlFile = Generic.GetWebConfigValue("XmlQueryFolderPath");

                // if sitePath is null then WTF and if pageXmlFolder is empty well in this case you have Murphy's Law problems
                if ( string.IsNullOrEmpty(sitePath) && (string.IsNullOrEmpty(pageXmlFolder) || pageXmlFolder.StartsWith("default")))
                    throw new Exception("no fisical path to build");

                // if path is null then set to empty ele remove any space that it was added in the end
                sitePath = (sitePath == null) ? string.Empty : sitePath.TrimEnd();
                xmlFile = (xmlFile == null) ? string.Empty : xmlFile.TrimEnd();

                if (!string.IsNullOrEmpty(pageXmlFolder) && !pageXmlFolder.StartsWith("default", StringComparison.CurrentCultureIgnoreCase))
                {
                    return pageXmlFolder.EndsWith(@"\")
                                                    ? pageXmlFolder + fileName
                                                    : pageXmlFolder + @"\" + fileName;
                }

                return xmlFile != string.Empty
                                        ? xmlFile.EndsWith(@"\")    // if not empty then it contains another path then the default
                                                            ? xmlFile + fileName
                                                            : xmlFile + @"\" + fileName
                                        : sitePath.EndsWith(@"\")   // if empty then user whants to use default path 
                                                            ? sitePath + @"xml\" + fileName
                                                            : sitePath + @"\xml\" + fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("build fisical file path: " + fileName + " - " + ex.Message + " ...");
            }
            finally
            {
                if (page != null) page.Close();
            }
        }

        static public string BuildXmlFileVirtualPath(string fileName, int pageId)
        {
            db_config_page page = null;

            try
            {
                // if contains a fixed path then there is no need to add page or default path
                if (fileName.StartsWith("http://"))
                    return fileName;

                // if the filename is not added then there is no reason to be here
                if (string.IsNullOrEmpty(fileName)) throw new Exception("filename is empty");

                // page virtual path
                page = new db_config_page(pageId, false);
                page.Open();
                string pageXmlUrl = page.Get(pageId).XMLURL;

                // webconfig virtual path
                string virtualFolder = Generic.GetWebConfigValue("XmlQueryVirtualPath");
                if (string.IsNullOrEmpty(virtualFolder)) virtualFolder = "~/xml/";

                if (!string.IsNullOrEmpty(pageXmlUrl) && !pageXmlUrl.StartsWith("default", StringComparison.CurrentCultureIgnoreCase))
                {
                    return pageXmlUrl.EndsWith("/")
                                                ? pageXmlUrl + fileName
                                                : pageXmlUrl + @"/" + fileName;    
                }

                return virtualFolder.EndsWith("/")
                                                ? virtualFolder + fileName
                                                : virtualFolder + @"/" + fileName; 
            }
            catch (Exception ex)
            {
                throw new Exception("build vistual path: " + fileName + " - " + ex.Message + " ...");
            }
            finally
            {
                if (page != null) page.Close();
            }
        }

        static public DataView LoadXmlData(string fileName, string defaultFilter, string pageId, string masterFilterId)
        {
            try
            {
                return data_handler.LoadXmlData(fileName, defaultFilter, pageId, masterFilterId);
            }
            catch (Exception ex)
            {
                throw new Exception("generating dataview error from xml file: " + fileName + " - " + ex.Message + " ...");
            }
        }
        /**************************************************/
    }

    public class FrameHtmlCtrls
    {
        public HtmlGenericControl LabelTitle;
        public HtmlGenericControl TitleContainer;
    }
}