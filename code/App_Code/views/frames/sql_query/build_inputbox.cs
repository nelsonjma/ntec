using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Views.Frames
{
    /// <summary>
    /// builds the input box, this is composed by spans, textboxs and one button
    /// </summary>
    public class BuildInputbox
    {
        private readonly List<string> _inputBox;

        public BuildInputbox(List<string> inputBoxs)
        {
            _inputBox = inputBoxs;
        }

        /***************************** PRIVATE *****************************/
        /// <summary>
        /// Creates the html objects
        /// </summary>
        private Control BuildHtmlContent()
        {
            try
            {
                HtmlObjects table = new HtmlObjects("div");
                table.AddAttribute("class", "table");

                for (int i = 0; i < _inputBox.Count; i++)
                {
                    HtmlObjects row         = new HtmlObjects("div"); row.AddAttribute("class", "row");
                    HtmlObjects cellLabel   = new HtmlObjects("div"); cellLabel.AddAttribute("class", "cell");
                    HtmlObjects cellTextBox = new HtmlObjects("div"); cellTextBox.AddAttribute("class", "cell");

                    HtmlObjects label       = new HtmlObjects("span") { Text = _inputBox[i] + ":" };
                    label.AddAttribute("class", "label");

                    TextBox textbox         = new TextBox {ID = "value" + i, CssClass = "textbox"};

                    cellLabel.AddControls(label.GetObject);
                    cellTextBox.AddControls(textbox);

                    row.AddControls(cellLabel.GetObject);
                    row.AddControls(cellTextBox.GetObject);

                    table.AddControls(row.GetObject);
                }

                return table.GetObject;
            }
            catch (Exception ex)
            {
                throw new Exception("error building html input boxs " + ex.Message);
            }
        }

        /***************************** PUBLIC *****************************/
        /// <summary>
        /// Returns the input box in html format
        /// </summary>
        public Control GetHtmlControls()
        {
            return BuildHtmlContent();
        }
    }
}