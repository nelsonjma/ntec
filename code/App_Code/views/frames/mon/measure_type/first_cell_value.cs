using System;
using System.Data;

namespace Views.Frames
{
    /// <summary>
    /// Get the value of the first column of the first row
    /// </summary>
    public class first_cell_value : measure_type
    {
        public override void BuildMeasure()
        {
            try
            {
                if (LoadData == null) return;

                MeasureValue = Convert.ToInt32(GetData().Table.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                loging.Error("mon", "buid measure", ex.Message, Log);
            }
        }
    }
}