using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

using Views.Frames;
using DbConfig;

public partial class frames_chart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) ViewState["log_file"] = Generic.GetWebConfigValue("LogFilePath"); /*** Activar log ***/

        try
        {
            string frameIdHash = Request.QueryString.Get("fm");

            if (frameIdHash != null)
            {
                if (frame_sessions.GetFrame(frameIdHash) != null)
                {
                    Startup((Frame)frame_sessions.GetFrame(frameIdHash));
                }
            }
        }
        catch (Exception ex)
        {
            loging.Error("chart", "load page", ex.Message, ViewState["log_file"].ToString());
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        // Get last error from the server
        Exception ex = Server.GetLastError();

        Response.Write("<p style='font:16px bold verdana;'>Error loading chart</p>\n");

        Server.ClearError();

        loging.Error("chart", "load page error at ending", ex.Message, ViewState["log_file"].ToString());
    }

    private void Startup(Frame frame)
    {
        // replace log file so that is one frame to one log
        ReplaceLogFile(frame.IDPage, frame.ID);

        OptionItems oi = new OptionItems(frame.Options);

        // ------ Frame css ------
        ChangeCss(oi.GetSingle("css"));

        // ------ Title ------
        headerTitle.Text = frame.Title;

        // hide title if value is true, else will always show the title
        if (!oi.GetSingle("title_is_hidden").Equals("true", StringComparison.CurrentCultureIgnoreCase)) 
        {
            lbTitle.InnerText = frame.Title;
            TitleStyle(oi.GetList("title_style")); // title style

            // ------ Load Javascript ------ 
            SetTitlePosition(); // loads javascript that controls title position
        }
        else
        {
            lbTitleContainer.Style.Add("display", "none");
        }

        // ------ Border ------
        // Border is defined in draw_frames class, sets the border of the panel outside de iframe.

        // ------ Chart ------
        int rowsCount = -1;

        try
        {
            string defaultFilter = oi.GetSingle("default_filter"); /*** default filter => string defaultFilter = "select SUBESTADO, count as qtd group by SUBESTADO";  ***/

            string xmlFileName = oi.GetSingle("xml_file");

            string xmlFilePath = GenericFrameSettings.BuildXmlFilePath(xmlFileName, frame.IDPage); // set real folder

            DataView dataView = GenericFrameSettings.LoadXmlData(
                                                                xmlFilePath, 
                                                                defaultFilter, 
                                                                frame.IDPage.ToString(), 
                                                                oi.GetSingle("master_filter").Trim()
                                                                );

            rowsCount = dataView.Count;

            if (rowsCount > -1) WriteDataToChart(dataView);
        }
        catch (Exception ex)
        {
            throw new Exception("error loading data " + ex.Message);
        }

        if (rowsCount >= 0)
        {
            try
            {
                /********** Alterar a area onde se encontra o grafico **********/
                //SetChartArea(new List<string>() { "0", "0", "0", "0" }); // tem de ser dos primeiros para os valores se adaptarem para todos...
                SetChartArea(oi.GetList("chart_area"));

                /********** Trocar o eixo de posição **********/
                //SetAxis(new List<string> { "right", "left", "left", "left" });
                SetAxis(oi.GetList("set_y_axis"));

                /********** template selector **********/
                DesignTemplate(oi.GetSingle("design_template"));

                /********** chart antialiasing is on text antialiasing is controled by this variable **********/
                AntiAliasing(oi.GetSingle("text_antialiasing").Equals("true", StringComparison.CurrentCultureIgnoreCase));

                /********** Width, Height **********/
                string newWidth = oi.GetSingle("width");
                string newHeight = oi.GetSingle("height");

                WidthHeight(
                            newWidth != string.Empty ? Convert.ToInt32(newWidth) : frame.Width - 2,
                            newHeight != string.Empty ? Convert.ToInt32(newHeight) : frame.Height - 30
                            ); 

                /********** Util para grafico de barras e linhas **********/
                ScaleBreak(oi.GetSingle("scalebreak").Equals("true", StringComparison.CurrentCultureIgnoreCase));

                /********** 3D controls **********/
                string inclination      = oi.GetSingle("inclination");
                string rotation         = oi.GetSingle("rotation");
                string point_depth      = oi.GetSingle("point_depth");
                string point_gap_depth  = oi.GetSingle("point_gap_depth");
                string is_clustered     = oi.GetSingle("is_clustered");

                Chart3D(
                        oi.GetSingle("chart3d").Equals("true", StringComparison.CurrentCultureIgnoreCase),
                        inclination != string.Empty ? Convert.ToInt32(inclination) : -1,
                        rotation != string.Empty ? Convert.ToInt32(rotation) : -1,
                        point_depth != string.Empty ? Convert.ToInt32(point_depth) : -1, 
                        point_gap_depth != string.Empty ? Convert.ToInt32(point_gap_depth) : -1,
                        is_clustered.Equals("true", StringComparison.CurrentCultureIgnoreCase)
                        );

                /********** legend control **********/
                Legend(
                        oi.GetSingle("legend").Equals("true", StringComparison.CurrentCultureIgnoreCase),
                        oi.GetSingle("docking"),
                        oi.GetSingle("alignment"),
                        oi.GetSingle("text_font"),
                        oi.GetSingle("text_color"),
                        oi.GetSingle("text_size"),
                        oi.GetSingle("back_color")
                        );

                /********** grid on and off **********/
                ChartGrid(oi.GetSingle("chart_grid").Equals("true", StringComparison.CurrentCultureIgnoreCase));

                /********** Cores Manuais **********/
                ChartSeriesColors(oi.GetList("chart_colors"));

                /********** chart type **********/
                ChartSeriesType(oi.GetList("chart_type"));

                /********** enable/disable X axis angle and set this axis angle **********/
                SetxAxisStyle(
                                oi.GetSingle("x_label_angle"),
                                oi.GetSingle("disable_x_label").Equals("true", StringComparison.CurrentCultureIgnoreCase),
                                oi.GetSingle("x_axis_interval")
                            );

                /********** value label **********/
                ShowValueAsLabel(
                                oi.GetList("show_value_in_label"),
                                oi.GetSingle("label_value_color"),
                                oi.GetSingle("label_value_back_color")
                            );

                /********** just only Pie and Doughnut property **********/
                PieLabelStyle(oi.GetSingle("pie_label_style").ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception("error loading options " + ex.Message);
            }

            mChart.DataBind(); // in the end do databind...
        }
    }

    /********************************************* CHART STYLE *********************************************/

    /******************** Load Data To Chart ********************/
    private void WriteDataToChart(DataView dataView)
    {
        int numColumns = dataView.Table.Columns.Count;

        for (int i = 1; i < numColumns; i++)
        {
            mChart.Series.Add("S" + i);
            mChart.Series["S" + i].LegendText = dataView.Table.Columns[i].Caption;
        }

        for (int i = 0; i < dataView.Count; i++)
        {
            DataRowView drv = dataView[i];

            string valX = drv[0].ToString();

            for (int a = 1; a < numColumns; a++)
            {
                double valY = 0;

                try { valY = Convert.ToDouble(drv[a].ToString().Replace('.', ',')); } catch { }

                mChart.Series["S" + a].Points.AddXY(valX, valY);

                mChart.Series["S" + a].ChartArea = "mChartArea";
            }   
        }

    }
    /**************************************************/

    /******************** Design ********************/
    private void DesignTemplate(string template)
    {
        if (template != string.Empty)
        {
            if (!template.ToLower().Contains(".xml"))
                template = template + ".xml";

            mChart.Serializer.IsResetWhenLoading = false;
            mChart.LoadTemplate("~/template/charts/" + template);
        }
    }

    private void AntiAliasing(bool isActive)
    {
        mChart.AntiAliasing = AntiAliasingStyles.Graphics;

        if (isActive)
        {
            mChart.AntiAliasing = AntiAliasingStyles.All;
            mChart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
        }
    }

    private void WidthHeight(int width, int height)
    {
        mChart.Width = width;
        mChart.Height = height;
    }

    private void ScaleBreak(bool contains)
    {
        if (!contains) return;

        foreach (ChartArea chartArea in mChart.ChartAreas)
        {
            chartArea.AxisY.ScaleBreakStyle.Enabled = true;                                // Enable scale breaks
            chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;          // Set the scale break type
            chartArea.AxisY.ScaleBreakStyle.Spacing = 1;                                   // Set the spacing gap between the lines of the scale break (as a percentage of y-axis)
            chartArea.AxisY.ScaleBreakStyle.LineWidth = 1;                                 // Set the line width of the scale break
            chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Black;                       // Set the color of the scale break
            chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 20;                // Show scale break if more than 25% of the chart is empty space
        }
    }

    private void Chart3D(bool enable3D, int inclination, int rotation, int pointDepth, int pointGapDepth, bool isClustered)
    {
        if (enable3D)
        {
            inclination = (inclination != -1 && inclination <= 90 && inclination >= -90) ? inclination : 30;  
            rotation = (rotation != -1 && rotation <= 180 && rotation >= -180) ? rotation : 30;

            pointDepth = (pointDepth != -1 && pointDepth <= 1000 && pointDepth >= 0) ? pointDepth : 100;
            pointGapDepth = (pointGapDepth != -1 && pointGapDepth <= 1000 && pointGapDepth >= 0) ? pointGapDepth : 100;

            foreach (ChartArea chartArea in mChart.ChartAreas)
            {

                chartArea.Area3DStyle.IsClustered = isClustered;                                  // Show columns as clustered

                chartArea.Area3DStyle.Enable3D = true;                                            // Enable 3D charts
                chartArea.Area3DStyle.LightStyle = LightStyle.Realistic;

                chartArea.Area3DStyle.Perspective = 10;                                           // Show a 10% perspective
                chartArea.Area3DStyle.Inclination = inclination;                                  // Set the X Angle to XX
                chartArea.Area3DStyle.Rotation = rotation;                                        // Set the Y Angle to YY

                chartArea.Area3DStyle.PointDepth = pointDepth;                                    // Set the Point Depth to 100
                chartArea.Area3DStyle.PointGapDepth = pointGapDepth;                              // Set the Point Gap Width to 0
            }
        }
    }

    private void Legend(bool contains, string docking, string alignment, string fontFamily, string fontColor, string fontSize, string backColor)
    {
        if (!contains) return;

        Docking dk = Docking.Bottom;
        StringAlignment align = StringAlignment.Center;

        docking = docking.ToLower().Trim();
        alignment = alignment.ToLower().Trim();

        switch (docking)
        {
            case "bottom":
                dk = Docking.Bottom; break;
            case "top":
                dk = Docking.Top; break;
            case "right":
                dk = Docking.Right; break;
            case "left":
                dk = Docking.Left; break;
        }

        switch (alignment)
        {
            case "center":
                align = StringAlignment.Center; break;
            case "far":
                align = StringAlignment.Far; break;
            case "near":
                align = StringAlignment.Near; break;
        }

        FontFamily ff = new FontFamily(fontFamily != string.Empty ? fontFamily : "Calibri");

        float fs = 10; try { if (fontSize != string.Empty) fs = float.Parse(fontSize); } catch { }

        mChart.Legends.Add("MainLegend");
        mChart.Legends["MainLegend"].LegendStyle = LegendStyle.Table;
        mChart.Legends["MainLegend"].Docking = dk;
        mChart.Legends["MainLegend"].Alignment = align;
        mChart.Legends["MainLegend"].Font = new Font(ff, fs);
        mChart.Legends["MainLegend"].ForeColor = fontColor != string.Empty
                                                                        ? ColorTranslator.FromHtml(fontColor)
                                                                        : Color.Transparent;

        mChart.Legends["MainLegend"].BackColor = backColor != string.Empty
                                                                        ? ColorTranslator.FromHtml(backColor)
                                                                        : Color.Transparent;
    }

    private void ChartGrid(bool show)
    {
        foreach (ChartArea chartArea in mChart.ChartAreas)
        {
            chartArea.AxisX.MajorGrid.Enabled = show;
            chartArea.AxisY.MajorGrid.Enabled = show;

            chartArea.AxisX.MinorGrid.Enabled = show;
            chartArea.AxisY.MinorGrid.Enabled = show;

            chartArea.AxisX2.MajorGrid.Enabled = show;
            chartArea.AxisY2.MajorGrid.Enabled = show;

            chartArea.AxisX2.MinorGrid.Enabled = show;
            chartArea.AxisY2.MinorGrid.Enabled = show;
        }
    }

    private void ChartSeriesColors(List<string> colors)
    {
        if (colors.Count >= mChart.Series.Count)
            for (int i = 0; i < mChart.Series.Count; i++)
                mChart.Series[i].Color = ColorTranslator.FromHtml(colors[i]);
    }

    /// <summary>
    /// Set chart type, its pussible to use multiple charts
    /// </summary>
    private void ChartSeriesType(List<string> chartType)
    {
        if (chartType.Count < mChart.Series.Count) return;

        for (int i = 0; i < mChart.Series.Count; i++)
        {
            SeriesChartType sct = GetChartType(chartType[i]);

            mChart.Series[i].ChartType = sct;

            if (sct == SeriesChartType.Pie || sct == SeriesChartType.Doughnut) // desactivar a legenda que é colocada por defeito
                mChart.Series[i].LegendText = string.Empty;
        }
    }

    private void ShowValueAsLabel(List<string> show, string labelColor, string backColor)
    {
        Color labelClr = labelColor != string.Empty
                                            ? ColorTranslator.FromHtml(labelColor)
                                            : Color.Black;

        Color bkClr = backColor != string.Empty
                                            ? ColorTranslator.FromHtml(backColor)
                                            : Color.Transparent;

        if (show.Count == 0) return;

        for (int i = 0; i < mChart.Series.Count; i++)
        {
            // if show is bad formated or this serial value is not true
            if (i < show.Count && show[i].Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                mChart.Series[i].IsValueShownAsLabel = true;

                mChart.Series[i].LabelForeColor = labelClr;

                mChart.Series[i].LabelBackColor = bkClr;


                /* When smart labels are enabled, the Chart control first attempts to reposition an overlapping data point label near the data point itself. 
                       If it fails, the Chart control then moves the data point label to a valid free space and draws a line to link the label with the data point.*/
                mChart.Series[i].SmartLabelStyle.Enabled = true;

                //if (mChart.Series[i].ChartType != SeriesChartType.Pie && mChart.Series[i].ChartType != SeriesChartType.Doughnut)
                //    mChart.Series[i]["LabelStyle"] = "Auto";    
            }
        }
    }

    private void SetChartArea(List<string> chartAreas)
    {
        if (chartAreas.Count >= mChart.Series.Count)
            for (int i = 0; i < mChart.Series.Count; i++)
            {
                int areaIndex = Convert.ToInt32(chartAreas[i]);

                AddNewChartArea(areaIndex);

                mChart.Series[i].ChartArea = mChart.ChartAreas[areaIndex].Name;   
            }
    }

    private void SetAxis(List<string> axis)
    {
        if (axis.Count >= mChart.Series.Count)
            for (int i = 0; i < mChart.Series.Count; i++)
            {
                if (axis[i].ToLower().Trim().Equals("right"))
                    mChart.Series[i].YAxisType = AxisType.Secondary;
            }
    }

    private void SetxAxisStyle(string labelAngle, bool disableLabel, string axisInterval)
    {
        int interval; if (!int.TryParse(axisInterval, out interval)) interval = 0;

        int angle; if (!int.TryParse(labelAngle, out angle)) angle = 0;

        foreach (ChartArea chartArea in mChart.ChartAreas)
        {
            chartArea.AxisX.Interval = (interval >= 1) ? interval : 0;

            if (labelAngle != string.Empty)
                chartArea.AxisX.LabelStyle.Angle = angle;
            else
            {
                chartArea.AxisX.IsLabelAutoFit = true;                            // Enable X axis labels automatic fitting
                chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;  // Set X axis automatic fitting style

                chartArea.AxisY.IsLabelAutoFit = true;                            // Enable X axis labels automatic fitting
                chartArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.WordWrap;  // Set X axis automatic fitting style
            }

            chartArea.AxisX.LabelStyle.Enabled = disableLabel.Equals(false);
        }
    }

    /*** Pie Methods ***/
    private void PieLabelStyle(string style) // "Outside" / "Disabled" / "Inside"
    {
        if (style == string.Empty) style = "Inside";

        for (int i = 0; i < mChart.Series.Count; i++)
        {
            if (mChart.Series[i].ChartType == SeriesChartType.Pie || mChart.Series[i].ChartType == SeriesChartType.Doughnut)
                mChart.Series[i]["PieLabelStyle"] = style;
        }
    }

    /**************************************************/

    /******************** Other Methods ********************/
    private void ChartToImageFile(string filePath)
    {
        if (filePath != string.Empty)
            mChart.SaveImage(filePath, ChartImageFormat.Png);
    }

    private SeriesChartType GetChartType(string chartType)
    {
        chartType = chartType.ToLower().Trim();

        switch (chartType)
        {
            case "stacked column 100":
                return SeriesChartType.StackedColumn100;
            case "stacked column":
                return SeriesChartType.StackedColumn;
            case "column":
                return SeriesChartType.Column;
            case "stacked bar 100":
                return SeriesChartType.StackedBar100;
            case "stacked bar":
                return SeriesChartType.StackedBar;
            case "bar":
                return SeriesChartType.Bar;
            case "stacked area 100":
                return SeriesChartType.StackedArea100;
            case "stacked area":
                return SeriesChartType.StackedArea;
            case "spline area":
                return SeriesChartType.SplineArea;
            case "area":
                return SeriesChartType.Area;
            case "stepline":
                return SeriesChartType.StepLine;
            case "spline":
                return SeriesChartType.Spline;
            case "line":
                return SeriesChartType.Line;
            case "fast line":
                return SeriesChartType.FastLine;
            case "fast point":
                return SeriesChartType.FastPoint;
            case "doughnut":
                return SeriesChartType.Doughnut;
            case "pie":
                return SeriesChartType.Pie;
            case "funnel":
                return SeriesChartType.Funnel;
            case "pyramid":
                return SeriesChartType.Pyramid;
            default:
                return SeriesChartType.Line;
        }
    }

    private void AddNewChartArea(int areaIndex)
    {
        if (areaIndex >= mChart.ChartAreas.Count)
        {
            ChartArea ca = new ChartArea("mChartArea" + mChart.ChartAreas.Count);

            mChart.ChartAreas.Add(ca);

            if (areaIndex >= mChart.ChartAreas.Count)
                AddNewChartArea(areaIndex);
            else
                return;
        }
    }

    /**************************************************/

    /********************************************* FRAME STYLE *********************************************/

    /******************** Title Style ********************/
    private void TitleStyle(List<string> cssOptions)
    {
        FrameHtmlCtrls titleObjects = GenericFrameSettings.FrameTitleStyle(lbTitleContainer, lbTitle, cssOptions);

        lbTitle = titleObjects.LabelTitle;
        lbTitleContainer = titleObjects.TitleContainer;
    }
    /**************************************************/

    /******************** Title Position Ctrl ********************/
    /// <summary>
    /// Loads javascript that controls title position
    /// </summary>
    private void SetTitlePosition()
    {
        Generic.JavaScriptInjector("titleposition", GenericFrameSettings.GetJavascriptTitlePosition());
    }
    /**************************************************/

    /******************** Frame CSS ********************/
    /// <summary>
    /// Change CSS 
    /// </summary>
    /// <param name="cssFileName"></param>
    private void ChangeCss(string cssFileName)
    {
        if (cssFileName != string.Empty)
        {
            cssStyle.Href = "~/css/views/frames/" + cssFileName;
        }
    }
    /**************************************************/

    /******************** Change Log File ********************/
    /// <summary>
    /// its best to one log for frame so that is easy to analise possible errors
    /// </summary>
    private void ReplaceLogFile(int pageId, int frameId)
    {
        // one execution, one log
        if (!IsPostBack)
            ViewState["log_file"] = LogingFileFormats.ReplaceLogFrameFile(ViewState["log_file"].ToString(), pageId, frameId);
    }
    /**************************************************/
}