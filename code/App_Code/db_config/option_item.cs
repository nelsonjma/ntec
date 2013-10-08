using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DbConfig
{
    public class OptionItems
    {
/*
    color=[red];
    alarms=[
            [word a],
            [word b], 
            [word c]
    ];
*/

        List<OptionItem> items;

        public OptionItems(string data)
        {
            items = new List<OptionItem>();

            ProcessItems(data);
        }

        /******************* PRIVATE **************************/
        /// <summary> 
        /// Method that filters config data. 
        /// </summary>
        private void ProcessItems(string data)
        {
            try
            {
                if (data == null) data = string.Empty;

                string[] listOption = data.Split(new string[] { "];", "*/" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string bruteOption in listOption)
                {
                    try
                    {
                        string name = bruteOption.Substring(0, bruteOption.IndexOf("=")).Trim().ToLower();

                        string options = bruteOption.Substring(bruteOption.IndexOf("=") + 1);

                        options = Generic.RemoveUnwantedChars(options);

                        options = options.StartsWith("[") ? options.Substring(1) : options;

                        if (options.Contains("],"))
                        {
                            string[] subOptions = options.Split(new string[] { "]," }, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < subOptions.Length; i++)
                            {
                                subOptions[i] = subOptions[i].Trim();

                                subOptions[i] = subOptions[i].StartsWith("[")
                                                    ? subOptions[i].Substring(1)
                                                    : subOptions[i];

                                subOptions[i] = subOptions[i].EndsWith("]")
                                                    ? subOptions[i].Substring(0, subOptions[i].Length - 1)
                                                    : subOptions[i];
                            }

                            items.Add(new OptionItem { Name = name, Option = subOptions });
                        }
                        else
                        {
                            //check if is a list with just one element
                            if (CheckIfListOneElement(options))
                                options = Generic.RemoveStartEndSquareBrackets(options);

                            items.Add(new OptionItem { Name = name, Option = new string[1] { options } });
                        }
                    }
                    catch { }
                }

                return;
            }
            catch (Exception ex)
            {
                throw new Exception("error: filtering options from frame " + ex.Message + " ...");
            }
        }

        private static bool CheckIfListOneElement(string data)
        {
            data = data.Trim();

            return data.StartsWith("[") && data.EndsWith("]");
        }

        /******************* PUBLIC **************************/
        /// <summary>
        /// Returns list of elements
        /// </summary>
        public List<string> GetList(string name)
        {
            try
            {
                OptionItem item = items.Find(x => x.Name.ToLower() == name.ToLower());

                if (item != null && item.Option.Length > 0)
                    return new List<string>(item.Option);
            }
            catch { }

            return new List<string>();
        }

        /// <summary>
        /// Returns just one element
        /// </summary>
        public string GetSingle(string name)
        {
            try
            {
                OptionItem item = items.Find(x => x.Name.Trim().ToLower() == name.ToLower());

                if (item != null && item.Option.Length > 0)
                    return item.Option[0].Trim();
            }
            catch { }

            return string.Empty;
        }

        /// <summary>
        /// Returns all options in string format
        /// </summary>
        public string GetOptionsString()
        {
            string options = string.Empty;


            foreach (OptionItem oi in items)
            {
                if (oi.Option.Length == 1)
                {
                    options += oi.Name + "=[" + oi.Option[0] + "]; \n";
                }
                else if (oi.Option.Length > 1)
                {
                    options += oi.Name + "=[";

                    for (int i = 0; i < oi.Option.Length; i++)
                    {
                        options += i == oi.Option.Length - 1 
                                                        ? "[" + oi.Option[i] + "] \n" 
                                                        : "[" + oi.Option[i] + "], \n";
                    }

                    options += "];";
                }
            }

            return options;
        }

        public void AddNewOption(string name, List<string> options)
        {
            if (name != String.Empty && options.Count > 0)
                items.Add(new OptionItem() {Name = name, Option = options.ToArray()});
        }

        public void UpdateOptions(string name, List<string> newOptions)
        {
            try
            {
                int index = items.FindIndex(x => x.Name == name);

                if (index >= 0)
                    items[index].Option = newOptions.ToArray();
                else
                    AddNewOption(name, newOptions); // if option does not exists will try to add it.
            }
            catch {}
        }
    }

    public class OptionItem
    {
        public string Name { get; set; }
        public string[] Option { get; set; }
    }
}