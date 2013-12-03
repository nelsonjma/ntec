using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Views.Frames
{
    /// <summary>
    /// Summary description for combox_filter
    /// </summary>
    [Serializable()]
    public class combobox_filter
    {
        private readonly string _pageIdHash;

        private readonly string _filterTitleHash;

        private readonly string _title;
        private readonly string _titleHash;

        SortedList _listFilteredRows;

        private string _cssTitle;
        private string _cssCombox;

        public combobox_filter(string pageId, string filterTitle, string title, DataView originalDv)
        {
            _pageIdHash = pageId;

            _title = title;

            _filterTitleHash = Generic.GetHash(filterTitle);
            _titleHash = Generic.GetHash(_title);

            _cssTitle = string.Empty;
            _cssCombox = string.Empty;

            LoadData(originalDv);
        }

        /************ Load Data ************/
        private void LoadData(DataView originalDv)
        {
            DataTable filteredDt = null;
            DataView dv = new DataView();

            try
            {
                _listFilteredRows = new SortedList();

                dv = originalDv.ToTable(true, _title).DefaultView; dv.Sort = _title;

                filteredDt = dv.ToTable();

                // removed so that when the column is int type we dont have problems when we want to selecting all the records
//                try { DataRow drAll = filteredDt.NewRow(); drAll[0] = "*"; filteredDt.Rows.InsertAt(drAll, 0); } catch { }

                if (filteredDt.Rows.Count > 0) _listFilteredRows.Add("*", "*");


                foreach (DataRow dr in filteredDt.Rows)
                {
                    string data = dr.ItemArray[0].ToString();

                    _listFilteredRows.Add(data, data);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error loading column " + ex.Message);
            }
            finally
            {
                if (filteredDt != null) filteredDt.Dispose();
                if (dv != null) dv.Dispose();
            }
        }

        /************ Css ************/
        public string CssTitle
        {
            get { return _cssTitle; }
            set { _cssTitle = value; }
        }

        public string CssCombox
        {
            get { return _cssCombox; }
            set { _cssCombox = value; }
        }

        /************ DropDownList Item ************/
        public string GetSelectedFilter()
        {
            //int number;
            string value = "*";

            try { value = (string)_listFilteredRows.GetByIndex(GetSelectedIndex()); } catch { }

            if (value.Equals("*")) return string.Empty;

            // Special char replace
            value = SpecialCharReplace(value);
            
            // disable until better understanding dataview filters...
            //if (!int.TryParse(value, out number))
            //{
            value = "'" + value + "'";
            //}

            return _title + " = " + value;
        }

        public void ClearFilter()
        {


            System.Web.UI.Page page = (System.Web.UI.Page)HttpContext.Current.Handler;
            DropDownList ddl = (DropDownList)page.FindControl("ddl_" + _filterTitleHash + "_" + _titleHash);
            int startIndex = GetStartIndex();

            ddl.SelectedIndex = startIndex;
            SetSelectedIndex(startIndex);
        }

        private int GetSelectedIndex()
        {
            int selectdIndex = -1;

            try { selectdIndex = filter_sessions.GetComboBoxSelectedIndex(_pageIdHash, _filterTitleHash, _titleHash); }
            catch { }

            return !selectdIndex.Equals(-1) ? selectdIndex : GetStartIndex();
        }

        private void SetSelectedIndex(int value)
        {
            filter_sessions.SetComboBoxSelectedIndex(_pageIdHash, _filterTitleHash, _titleHash, value);
        }

        private int GetStartIndex()
        {
            try
            {
                for (int i = 0; i < _listFilteredRows.Count; i++)
                {
                    if ((string)_listFilteredRows.GetByIndex(i) == "*")
                        return i;
                }
            }
            catch { }

            return 0;
        }


        /************ Design ************/
        public TableCell GetComboBox()
        {
            TableCell comboTcell = new TableCell();

            Table t = new Table();
            TableRow trHeader = new TableRow();
            TableRow trComboBox = new TableRow();

            trHeader.Cells.Add(BuildLabel());
            trComboBox.Cells.Add(BuildComboBox());

            t.Rows.Add(trHeader);
            t.Rows.Add(trComboBox);

            comboTcell.Controls.Add(t);

            return comboTcell;
        }

        private TableCell BuildLabel()
        {
            TableCell ct = new TableCell
                                    {
                                        //Text = _title,
                                        VerticalAlign = VerticalAlign.Bottom,
                                        HorizontalAlign = HorizontalAlign.Left,
                                    };

            ct.Controls.Add(new HtmlObjects("span") { Text = _title}.GetObject);

            if (!_cssTitle.Equals(string.Empty)) { ct.CssClass = _cssTitle; }

            return ct;
        }

        private TableCell BuildComboBox()
        {
            DropDownList ddl = new DropDownList
                                            {
                                                ID = "ddl_" + _filterTitleHash + "_" + _titleHash,
                                                Enabled = true,
                                                AppendDataBoundItems = true,
                                                AutoPostBack = true,
                                                DataSource = _listFilteredRows,
                                                DataTextField = "Value",
                                                DataValueField = "Key",
                                                SelectedIndex = GetSelectedIndex(), // Vai tentar ler o valor 
                                            };

            ddl.SelectedIndexChanged += ddl_SelectedIndexChanged;

            if (!_cssCombox.Equals(string.Empty)) { ddl.CssClass = _cssCombox; }

            ddl.DataBind();

            TableCell tcDdl = new TableCell
                                        {
                                            BorderWidth = 0,
                                            VerticalAlign = VerticalAlign.Middle,
                                            HorizontalAlign = HorizontalAlign.Left,
                                            BackColor = Color.Transparent,
                                        };

            tcDdl.Controls.Add(ddl);

            return tcDdl;
        }

        /************ Event ************/
        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            SetSelectedIndex(ddl.SelectedIndex);
        }

        /************ Special Characters Replace ************/
        private string SpecialCharReplace(string data)
        {
            data = Generic.MultiCharReplace(data, "'", "''");

            return data;
        }
    }
}