using System;
using System.Collections.Generic;
using System.Drawing;

namespace Views.Frames
{
    [Serializable()]
    public class color_markers
    {
        private readonly List<Dictionary<string, object>> _columnColorMarker;

        /// <summary>
        /// Build color marker from option
        /// </summary>
        /// <param name="colorMarkers"></param>
        public color_markers(List<string> colorMarkers)
        {
            _columnColorMarker = new List<Dictionary<string, object>>();

            BuildColorMarkers(colorMarkers);
        }

        /// <summary>
        /// Add new color marker
        /// </summary>
        public color_markers()
        {
            _columnColorMarker = new List<Dictionary<string, object>>();
        }

        private void BuildColorMarkers(List<string> colorMarkers)
        {
            foreach (string color in colorMarkers)
            {
                try
                {
                    string auxColor = color.Trim();

                    string[] colorMarter = color.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

                    // leave if structure is not [column name, color]
                    if (colorMarter.Length != 2)  continue;

                    // create the dictionary
                    Dictionary<string, object> column = new Dictionary<string, object>
                                                        {
                                                            {"name", colorMarter[0].Trim()},
                                                            {"color", ColorTranslator.FromHtml(colorMarter[1].Trim())}
                                                        };

                    _columnColorMarker.Add(column);

                }
                catch (Exception ex)
                {
                    throw new Exception("error building color markers " + ex.Message);
                }
            }
        }

        public int Count
        {
            get { return _columnColorMarker.Count; }
        }

        public Color GetColor(int index)
        {
            try
            {
                return (Color)_columnColorMarker[index]["color"];
            }
            catch (Exception ex)
            {
                throw new Exception("error getting Column Color " + ex.Message);
            }
        }

        public string GetName(int index)
        {
            try
            {
                return (string)_columnColorMarker[index]["name"];
            }
            catch (Exception ex)
            {
                throw new Exception("error getting Column Name " + ex.Message);
            }
        }

        public int FindColumnName(string name)
        {
            try
            {
                name = name.ToLower();

                for (int i = 0; i < _columnColorMarker.Count; i++)
                    if (((string)_columnColorMarker[i]["name"]).ToLower() == name)
                        return i;
            }
            catch (Exception ex)
            {
                throw new Exception("error finding Column Name " + ex.Message);
            }

            return -1;
        }

        public void AddColorMarker(string name, Color clr)
        {
            int index = FindColumnName(name);

            if (index >= 0)
                _columnColorMarker[index]["color"] = clr;
            else
            {
                _columnColorMarker.Add(
                                        new Dictionary<string, object>
                                        {
                                            {"name", name}, 
                                            {"color", clr}
                                        });
            }
        }

        public void AddColorMarker(string name, Color clr, bool addColorIfColumnNameNotNew)
        {
            int index = FindColumnName(name);

            
            if (index >= 0 && addColorIfColumnNameNotNew)
                _columnColorMarker[index]["color"] = clr;
            else if (index < 0)
            {
                _columnColorMarker.Add(
                                        new Dictionary<string, object>
                                        {
                                            {"name", name}, 
                                            {"color", clr}
                                        });
            }
        }
    }
}