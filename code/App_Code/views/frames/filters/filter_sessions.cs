using System;
using System.Web;

namespace Views.Frames
{
    /// <summary>
    /// Summary description for filter_sessions
    /// </summary>
    public class filter_sessions
    {
        /************** ComboBoxSelectedIndex **************/
        static public int GetComboBoxSelectedIndex(string pageIdHash, string filterTitleHash, string titleHash)
        {
            object selectdIndex = HttpContext.Current.Session["ddl_select_item_" + pageIdHash + "_" + filterTitleHash + "_" + titleHash];

            return (selectdIndex != null) ? Convert.ToInt32(selectdIndex) : -1;
        }

        static public void SetComboBoxSelectedIndex(string pageIdHash, string filterTitleHash, string titleHash, int index)
        {
            HttpContext.Current.Session["ddl_select_item_" + pageIdHash + "_" + filterTitleHash + "_" + titleHash] = index;
        }

        /************** Filter Item **************/
        static public object GetFilterItem(string pageIdHash, string filterTitleHash, string titleHash)
        {
            try
            {
                return HttpContext.Current.Session["filter_item_" + pageIdHash + "_" + filterTitleHash + "_" + titleHash];
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        static public void SetFilterItem(string pageIdHash, string filterTitleHash, string titleHash, combobox_filter cbf)
        {
            HttpContext.Current.Session["filter_item_" + pageIdHash + "_" + filterTitleHash + "_" + titleHash] = cbf;
        }

        /************** Xml File Last Update **************/
        static public object GetDataFileLastUpd(string pageIdHash, string filterTitleHash)
        {
            try
            {
                return HttpContext.Current.Session["filter_xml_lastupd_" + pageIdHash + "_" + filterTitleHash];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public void SetDataFileLastUpd(string pageIdHash, string filterTitleHash, DateTime dt)
        {
            HttpContext.Current.Session["filter_xml_lastupd_" + pageIdHash + "_" + filterTitleHash] = dt;
        }

        /************** Master Filter String **************/
        static public string GetMasterFilterString(string pageIdHash, string filterTitleHash)
        {
            object str = HttpContext.Current.Session["filter_" + pageIdHash + "_" + filterTitleHash];

            return (str != null) ? (string)str : string.Empty;
        }

        static public void SetMasterFilterString(string pageIdHash, string filterTitleHash, string str)
        {
            HttpContext.Current.Session["filter_" + pageIdHash + "_" + filterTitleHash] = str;
        }
    }
}