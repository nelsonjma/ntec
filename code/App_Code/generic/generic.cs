using System.Web;
using System.Web.UI;
using System;
using System.Collections;
using System.Xml;

/// <summary>
/// Methods that can be used in all pages.
/// </summary>
public class Generic
{
    /// <summary>
    /// Creates hash from string
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    static public string GetHash(string message)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        data = x.ComputeHash(data);
        string ret = string.Empty;
        for (int i = 0; i < data.Length; i++)
            ret += data[i].ToString("x2").ToLower();
        // -------------------------------------------------
        return ret;
    }

    /// <summary>
    /// Makes the parent page of the iframe to refresh
    /// </summary>
    /// <param name="ctrl"></param>
    static public void ParentPageRefresh()
    {
        //Old method => Control ctrl
        //System.Web.UI.ScriptManager.RegisterStartupScript(ctrl, typeof(string), "script", "<script type='text/javascript'> parent.location.href = parent.location.href;</script>", false);

        JavaScriptInjector("refresh_page", "parent.location.href = parent.location.href;");
    }

    /// <summary>
    /// Execute parent page methods from frame
    /// </summary>
    static public void ParentJavascriptMethodInjector(string methodName)
    {
        if (!methodName.Contains(";")) 
            methodName = methodName + ";";

        JavaScriptInjector("window.parent." + methodName.Trim());
    }

    /// <summary>
    /// Redirect to another page
    /// </summary>
    static public void PageRedirect(string url)
    {
        HttpContext.Current.Response.Redirect(url);
    }

    /// <summary>
    /// Refresh de the page or iframe
    /// </summary>
    static public void PageRefresh()
    {
        HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
    }

    /// <summary>
    /// Inserts dinamic javascript in the page.
    /// </summary>
    /// <param name="csName">name that represents the script</param>
    /// <param name="script">javascript script widthout the script type=text/javascript </param>
    static public void JavaScriptInjector(string csName, string script)
    {
        Type cstype = typeof(string); // Define type of the client scripts on the page.

        ClientScriptManager cs = ((Page)HttpContext.Current.Handler).ClientScript;

        if (cs.IsStartupScriptRegistered(cstype, csName)) return;

        cs.RegisterStartupScript(cstype, csName, "<script type='text/javascript'> " + RemoveUnwantedChars(script) + " </script>");
    }

    /// <summary>
    /// Inserts dinamic javascript in the page.
    /// </summary>
    /// <param name="csName">name that represents the script</param>
    /// <param name="script">javascript script widthout the script type=text/javascript </param>
    /// <param name="removeUnwantedChars">will remove special characters or not</param>
    static public void JavaScriptInjector(string csName, string script, bool removeUnwantedChars)
    {
        Type cstype = typeof(string); // Define type of the client scripts on the page.

        ClientScriptManager cs = ((Page)HttpContext.Current.Handler).ClientScript;

        if (cs.IsStartupScriptRegistered(cstype, csName)) return;

        cs.RegisterStartupScript(
                                    cstype, 
                                    csName, 
                                    "<script type='text/javascript'> " + (removeUnwantedChars ? RemoveUnwantedChars(script) : script) +" </script>"
                                );
    }

    /// <summary>
    /// Inserts dinamic javascript in the page.
    /// </summary>
    /// <param name="script">javascript script widthout the script type=text/javascript </param>
    static public void JavaScriptInjector(string script)
    {
        ((Page)HttpContext.Current.Handler).ClientScript.RegisterClientScriptBlock(
                                                                                    ((Page)HttpContext.Current.Handler).GetType(), 
                                                                                    "alertMessage", 
                                                                                    RemoveUnwantedChars(script), true
                                                                                );
    }

    /// <summary>
    /// Inserts dinamic javascript in the page, with remove special char selector.
    /// </summary>
    static public void JavaScriptInjector(string script, bool removeUnwantedChars)
    {
        ((Page)HttpContext.Current.Handler).ClientScript.RegisterClientScriptBlock(
                                                                                    ((Page)HttpContext.Current.Handler).GetType(),
                                                                                    "alertMessage",
                                                                                    removeUnwantedChars ? RemoveUnwantedChars(script) : script, 
                                                                                    true
                                                                                );
    }

    /// <summary>
    ///  Get a value from the web config file
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    static public string GetWebConfigValue(string key)
    {
        try
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(key);
        }
        catch {}

        return string.Empty;
    }

    /// <summary>
    /// Remove spaces and tabs from string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    static public string RemoveUnwantedChars(string data)
    {
        data = data.Replace("\r\n", " ");
        data = data.Replace("\r\n", " ");
        data = data.Replace("\t", " ");
        data = data.Replace("\n", " ");

        return data.Trim();
    }

    static public string RemoveStartEndSquareBrackets(string data)
    {
        try 
	    {	        
            data = data.Trim();

            if (data.StartsWith("[")) 
                data = data.Substring(1);

            if (data.EndsWith("]"))
                data = data.Substring(0, data.Length - 1);

	    }
	    catch { }

        return data.Trim();
    }

    /// <summary>
    /// Get just one xml tag
    /// </summary>
    /// <param name="path"></param>
    /// <param name="colum"></param>
    /// <returns></returns>
    static public ArrayList ReadXml(string path, string colum)
    {
        ArrayList listOfElements = new ArrayList();

        XmlTextReader textReader = new XmlTextReader(path);

        int foundName = 0;

        while (textReader.Read())
        {
            if (foundName > 0 && textReader.Name != "" && textReader.Name.ToLower() != colum.ToLower())
                foundName = 0;

            if (foundName == 2 && textReader.Name.ToLower() == colum.ToLower())
            {
                foundName = 0;
                goto ToEnd;
            }

            if (foundName == 0 && textReader.Name.ToLower() == colum.ToLower())
            {
                foundName = 1;
                goto ToEnd;
            }

            if (foundName == 1 && textReader.Name == "")
            {
                listOfElements.Add(textReader.Value);
                foundName = 2;
            }

        ToEnd:

            byte do_not_remove_please;
        }

        textReader.Close();
        return listOfElements;
    }

    /// <summary>
    /// Equal to method above but put object in a reader that can be 
    /// used more than once without reading file again
    /// </summary>
    /// <param name="textReader"></param>
    /// <param name="colum"></param>
    /// <returns></returns>
    static public ArrayList ReadXml(XmlTextReader textReader, string colum)
    {
        ArrayList listOfElements = new ArrayList();

        int foundName = 0;

        while (textReader.Read())
        {
            if (foundName > 0 && textReader.Name != "" && textReader.Name.ToLower() != colum.ToLower())
                foundName = 0;

            if (foundName == 2 && textReader.Name.ToLower() == colum.ToLower())
            {
                foundName = 0;
                goto ToEnd;
            }

            if (foundName == 0 && textReader.Name.ToLower() == colum.ToLower())
            {
                foundName = 1;
                goto ToEnd;
            }

            if (foundName == 1 && textReader.Name == "")
            {
                listOfElements.Add(textReader.Value);
                foundName = 2;
            }

        ToEnd:

            byte do_not_remove_please;
        }

        textReader.Close();
        return listOfElements;
    }

    /// <summary>
    /// Permits replace the same char multiple times ' => ''
    /// </summary>
    public static string MultiCharReplace(string data, string inChar, string outChar)
    {
        if (!data.Contains(inChar)) return data;

        string[] listDataChars = data.Split(new string[] { inChar }, StringSplitOptions.None);

        string newdata = string.Empty;

        for (int i = 0; i < listDataChars.Length; i++)
        {
            newdata += listDataChars[i];

            if (i != listDataChars.Length - 1) newdata += outChar;
        }


        return newdata;
    }
}