using System;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace Charts
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetupSalesChart();
            }
        }

        private void SetupSalesChart()
        {
            // Clear any existing series
            SalesChart.Series.Clear();

            // Create a new series for sales data
            Series salesSeries = new Series("Daily Sales");
            salesSeries.ChartType = SeriesChartType.Column;
            salesSeries.Color = Color.FromArgb(13, 110, 253); // Bootstrap primary blue
            salesSeries.BorderWidth = 0;
            salesSeries.ShadowOffset = 2;
            salesSeries.ShadowColor = Color.FromArgb(128, 0, 0, 0);

            // Sample data for the four days
            salesSeries.Points.AddXY("Monday", 2800);
            salesSeries.Points.AddXY("Tuesday", 3200);
            salesSeries.Points.AddXY("Wednesday", 2950);
            salesSeries.Points.AddXY("Thursday", 3500);

            // Customize individual points
            foreach (DataPoint point in salesSeries.Points)
            {
                point.BackSecondaryColor = Color.FromArgb(13, 110, 253);
                point.BackGradientStyle = GradientStyle.TopBottom;
                point.BorderColor = Color.FromArgb(13, 110, 253);
                point.LabelForeColor = Color.FromArgb(73, 80, 87);
                point.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);

                // Add value labels on top of bars
                point.IsValueShownAsLabel = true;
                point.Label = "$" + point.YValues[0].ToString("N0");
            }

            // Add series to chart
            SalesChart.Series.Add(salesSeries);

            // Customize chart area
            ChartArea chartArea = SalesChart.ChartAreas[0];
            chartArea.BackColor = Color.White;
            chartArea.BorderColor = Color.LightGray;
            chartArea.BorderWidth = 1;
            chartArea.BorderDashStyle = ChartDashStyle.Solid;

            // Customize X-axis
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisX.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(73, 80, 87);
            chartArea.AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            chartArea.AxisX.Title = "Days of Week";
            chartArea.AxisX.TitleFont = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            chartArea.AxisX.TitleForeColor = Color.FromArgb(73, 80, 87);

            // Customize Y-axis
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.LineColor = Color.LightGray;
            chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(73, 80, 87);
            chartArea.AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Format = "${0:N0}";
            chartArea.AxisY.Title = "Sales Amount ($)";
            chartArea.AxisY.TitleFont = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            chartArea.AxisY.TitleForeColor = Color.FromArgb(73, 80, 87);

            // Set Y-axis range for better visualization
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = 4000;
            chartArea.AxisY.Interval = 500;

            // Customize chart title
            if (SalesChart.Titles.Count == 0)
            {
                Title title = new Title();
                title.Text = "Weekly Sales Performance";
                title.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(33, 37, 41);
                title.Docking = Docking.Top;
                SalesChart.Titles.Add(title);
            }

            // Customize legend
            if (SalesChart.Legends.Count > 0)
            {
                Legend legend = SalesChart.Legends[0];
                legend.Docking = Docking.Bottom;
                legend.Alignment = StringAlignment.Center;
                legend.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                legend.ForeColor = Color.FromArgb(73, 80, 87);
                legend.BackColor = Color.Transparent;
            }
        }

        // Method to update chart data (you can call this when data changes)
        public void UpdateChartData(string[] days, double[] values)
        {
            if (days.Length != values.Length)
                return;

            SalesChart.Series.Clear();

            Series salesSeries = new Series("Daily Sales");
            salesSeries.ChartType = SeriesChartType.Column;
            salesSeries.Color = Color.FromArgb(13, 110, 253);
            salesSeries.BorderWidth = 0;
            salesSeries.ShadowOffset = 2;

            for (int i = 0; i < days.Length; i++)
            {
                int pointIndex = salesSeries.Points.AddXY(days[i], values[i]);
                DataPoint point = salesSeries.Points[pointIndex]; point.IsValueShownAsLabel = true;
                point.Label = "$" + values[i].ToString("N0");
                point.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                point.LabelForeColor = Color.FromArgb(73, 80, 87);
            }

            SalesChart.Series.Add(salesSeries);
        }
    }
}