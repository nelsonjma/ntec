using System;
using System.Data;

namespace Views.Frames
{
    /// <summary>
    /// Summary description for rows_count
    /// </summary>
    public class rows_count : measure_type
    {

        public override void BuildMeasure()
        {
            try
            {
                if (LoadData == null) return;

                MeasureValue = GetData().Count;
            }
            catch (Exception ex)
            {
                loging.Error("mon", "buid measure", ex.Message, Log);
            }
        }

        /*
        public override void BuildMeasure(string filter)
        {
            try
            {
                if (XmlFileName == string.Empty) return;

                MeasureValue = GetData(filter).Count;
            }
            catch (Exception ex)
            {
                loging.Error("mon", "buid measure", ex.Message, Log);
            }
        }
        */
    }
}