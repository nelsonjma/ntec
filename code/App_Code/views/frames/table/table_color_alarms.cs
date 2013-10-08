using System;
using System.Collections.Generic;
using System.Drawing;

namespace Views.Frames
{
    [Serializable()]
    public class table_color_alarms
    {
        private List<string> _listWarning;
        private List<string> _listCritical;

        private Color _clrWarning;
        private Color _clrCritical;

        public table_color_alarms(List<string> listWarning, List<string> listCritical)
        {
            _clrWarning = Color.LightYellow;
            _clrCritical = Color.LightSalmon;

            _listWarning = listWarning;
            _listCritical = listCritical;
        }

        public Color WarningColor
        {
            get { return _clrWarning; }
            set { _clrWarning = value; }
        }

        public Color CriticalColor
        {
            get { return _clrCritical; }
            set { _clrCritical = value; }
        }

        public bool CheckIfWarning(string data)
        {
            try
            {
                foreach (string warning in _listWarning)
                {
                    if (data.ToLower() == warning.ToLower())
                        return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        public bool CheckIfCritical(string data)
        {
            try
            {
                foreach (string critical in _listCritical)
                {
                    if (data.ToLower() == critical.ToLower())
                        return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

    }
}