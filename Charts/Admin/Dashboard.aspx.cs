using Charts.chartsapi;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;

namespace Charts.Admin
{
    public partial class Dashboard : Page
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
            SalesChart.Series.Clear();

            Series transactionSeries = new Series("Daily Transactions");
            transactionSeries.ChartType = SeriesChartType.Column;
            transactionSeries.Color = Color.FromArgb(13, 110, 253);
            transactionSeries.BorderWidth = 0;
            transactionSeries.ShadowOffset = 2;
            transactionSeries.ShadowColor = Color.FromArgb(128, 0, 0, 0);

            ChartsApi api = new ChartsApi();
            DataTable dt = api.GetTransactionCountsByRecordDate();

            double maxCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                DateTime recordDate = Convert.ToDateTime(row["RecordDate"]);
                int transactionCount = Convert.ToInt32(row["TransactionCount"]);
                int pointIndex = transactionSeries.Points.AddXY(recordDate.ToString("MMM dd, yyyy"), transactionCount);
                DataPoint point = transactionSeries.Points[pointIndex];
                point.BackSecondaryColor = Color.FromArgb(13, 110, 253);
                point.BackGradientStyle = GradientStyle.TopBottom;
                point.BorderColor = Color.FromArgb(13, 110, 253);
                point.LabelForeColor = Color.FromArgb(73, 80, 87);
                point.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                point.IsValueShownAsLabel = true;
                point.Label = transactionCount.ToString("N0");

                if (transactionCount > maxCount)
                    maxCount = transactionCount;
            }

            SalesChart.Series.Add(transactionSeries);

            ChartArea chartArea = SalesChart.ChartAreas[0];
            chartArea.BackColor = Color.White;
            chartArea.BorderColor = Color.LightGray;
            chartArea.BorderWidth = 1;
            chartArea.BorderDashStyle = ChartDashStyle.Solid;

            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisX.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(73, 80, 87);
            chartArea.AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            chartArea.AxisX.Title = "Record Date";
            chartArea.AxisX.TitleFont = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            chartArea.AxisX.TitleForeColor = Color.FromArgb(73, 80, 87);

            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartArea.AxisY.LineColor = Color.LightGray;
            chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(73, 80, 87);
            chartArea.AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Format = "{0:N0}";
            chartArea.AxisY.Title = "Number of Transactions";
            chartArea.AxisY.TitleFont = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            chartArea.AxisY.TitleForeColor = Color.FromArgb(73, 80, 87);

            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Maximum = Math.Ceiling(maxCount * 1.2);
            chartArea.AxisY.Interval = Math.Max(1, Math.Ceiling(maxCount / 5));

            if (SalesChart.Titles.Count == 0)
            {
                Title title = new Title();
                title.Text = "Daily Transaction Counts";
                title.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(33, 37, 41);
                title.Docking = Docking.Top;
                SalesChart.Titles.Add(title);
            }

            if (SalesChart.Legends.Count > 0)
            {
                Legend legend = SalesChart.Legends[0];
                legend.Docking = Docking.Bottom;
                legend.Alignment = StringAlignment.Center;
                legend.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                legend.ForeColor = Color.FromArgb(73, 80, 87);
                legend.BackColor = Color.Transparent;
            }

            if (dt.Rows.Count > 0)
            {
                DataRow bestDay = dt.AsEnumerable().OrderByDescending(r => r.Field<int>("TransactionCount")).First();
                SalesChart.Titles.Add(new Title
                {
                    Text = $"Best Performance: {bestDay["RecordDate"]:MMM dd, yyyy} - {bestDay["TransactionCount"]:N0} Transactions",
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    ForeColor = Color.FromArgb(73, 80, 87),
                    Docking = Docking.Bottom
                });

                double avgCount = dt.AsEnumerable().Average(r => r.Field<int>("TransactionCount"));
                SalesChart.Titles.Add(new Title
                {
                    Text = $"Average Daily Transactions: {avgCount:N0}",
                    Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular),
                    ForeColor = Color.FromArgb(73, 80, 87),
                    Docking = Docking.Bottom
                });
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string filePath = Server.MapPath("~/Uploads/" + FileUpload1.FileName);
                FileUpload1.SaveAs(filePath);

                string[] lines = System.IO.File.ReadAllLines(filePath);
                ChartsApi api = new ChartsApi();

                for (int i = 1; i < lines.Length; i++) // Skip header
                {
                    string[] values = lines[i].Split(',');

                    if (values.Length < 34) continue;

                    ReceivedTransactionDTO data = new ReceivedTransactionDTO
                    {
                        TranId = long.TryParse(values[0], out long tranId) ? tranId : 0,
                        TransNo = values[1],
                        CustomerRef = values[2],
                        CustomerName = values[3],
                        CustomerType = values[4],
                        CustomerTel = values[5],
                        Area = values[6],
                        Tin = values[7],
                        TranAmount = decimal.TryParse(values[8], out decimal amount) ? amount : 0,
                        PaymentDate = DateTime.TryParse(values[9], out DateTime pd) ? pd : DateTime.MinValue,
                        RecordDate = DateTime.TryParse(values[10], out DateTime rd) ? rd : DateTime.MinValue,
                        TranType = values[11],
                        PaymentType = values[12],
                        VendorTranId = values[13],
                        ReceiptNo = values[14],
                        TranNarration = values[15],
                        SmsSent = SafeToBoolean(values[16]) ?? false,
                        VendorCode = values[17],
                        Teller = values[18],
                        Reversal = SafeToBoolean(values[19]),
                        Cancelled = SafeToBoolean(values[20]),
                        Offline = SafeToBoolean(values[21]),
                        UtilityCode = values[22],
                        UtilityTranRef = values[23],
                        SentToUtility = SafeToBoolean(values[24]) ?? false,
                        RegionCode = values[25],
                        DistrictCode = values[26],
                        VendorToken = values[27],
                        ReconFileProcessed = SafeToBoolean(values[28]),
                        Status = values[29],
                        SentToVendor = int.TryParse(values[30], out int sent) ? sent : (int?)null,
                        UtilitySentDate = DateTime.TryParse(values[31], out DateTime usd) ? usd : (DateTime?)null,
                        QueueTime = DateTime.TryParse(values[32], out DateTime qt) ? qt : (DateTime?)null,
                        Reason = values[33]
                    };

                    api.InsertReceivedTransactionFromAPI(data);
                }

                lblMessage.Text = "CSV uploaded and records inserted successfully.";
                lblMessage.Visible = true;

                // Refresh chart after upload
                SetupSalesChart();
            }
            else
            {
                lblMessage.Text = "Please select a CSV file to upload.";
                lblMessage.Visible = true;
            }
        }

        public static bool? SafeToBoolean(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            s = s.Trim().ToLower();
            if (s == "true" || s == "1" || s == "yes")
                return true;
            if (s == "false" || s == "0" || s == "no")
                return false;
            return null;
        }
    }
}