using System;
using System.Data;

namespace Views.Frames
{
    /// <summary>
    /// Get the value of the first column of the first row
    /// </summary>
    public class first_cell_value : measure_type
    {
        public first_cell_value(string xmlFileName)
        {
            XmlFileName = xmlFileName;
        }

        public override void BuildMeasure()
        {
            try
            {
                if (XmlFileName == string.Empty) return;

                MeasureValue = Convert.ToInt32(GetData().Table.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                loging.Error("mon", "buid measure", ex.Message, Log);
            }
        }

        public override void BuildMeasure(string filter)
        {
            try
            {
                if (XmlFileName == string.Empty) return;

                MeasureValue = Convert.ToInt32(GetData(filter).Table.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                loging.Error("mon", "buid measure", ex.Message, Log);
            }
        }
    }
}