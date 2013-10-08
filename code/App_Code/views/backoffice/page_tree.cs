using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DbConfig;

namespace Views.BackOffice
{
    /// <summary>
    /// Lista master pages e pages do site
    /// </summary>
    public class PageTree
    {
        List<TreeItem> _treeItems;

        public PageTree()
	    {
            _treeItems = new List<TreeItem>();

            BuildTree();
	    }

        private void BuildTree()
        {
            db_config_master_page dcmp = new db_config_master_page();
            dcmp.Open();

            foreach (MasterPage mp in dcmp.AllMasterPages.OrderBy(x => x.Title))
            {
                TreeItem ti = new TreeItem
                                    {
                                        Text = mp.Title, 
                                        ObjId = mp.ID.ToString()
                                    };

                List<TreeItem> sti = new List<TreeItem>();

                foreach (DbConfig.Page page in dcmp.GetAllPages(mp.ID).OrderBy(x => x.Title))
                {
                    sti.Add(new TreeItem() { Text = page.Title, ObjId = page.ID.ToString() });
                }

                ti.SubTreeItems = sti;

                _treeItems.Add(ti);
            }

            dcmp.Close();
        }

        public List<TreeItem> TreeItems
        {
            get { return _treeItems; }
        }
    }

    public class TreeItem
    {
        public string Text { get; set; }
        public string ObjId { get; set; }
        public List<TreeItem> SubTreeItems { get; set; }
    }
}