
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public class HtmlObjects
{
    HtmlGenericControl _htmlObj;

    public HtmlObjects(string objType)
    {
        _htmlObj = new HtmlGenericControl(objType);
    }

    public string Id
    {
        get { return _htmlObj.ID; }
        set { _htmlObj.ID = value; }
    }

    public string Text
    {
        set { _htmlObj.InnerText = value; }
    }

    public string Html
    {
        set { _htmlObj.InnerHtml = value; }
    }

    public Control GetObject
    {
        get { return _htmlObj; }
    }

    public void AddAttribute(string name, string value)
    {
        _htmlObj.Attributes.Add(name, value);
    }

    public void AddStyle(string name, string value)
    {
        _htmlObj.Style.Add(name, value);
    }

    public void AddControls(Control ctrl)
    {
        _htmlObj.Controls.Add(ctrl);
    }
}
