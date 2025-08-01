using Charts.chartsapi;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Charts.Admin
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateVendorCodes();
                LoadTotalTransactions();
                SetupSalesChart();
                SetupVendorBarChart();
                LoadRecentTransactions();
                gvRecentTransactions.RowDataBound += gvRecentTransactions_RowDataBound;

                TimeZoneInfo eatTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Africa Standard Time"); // Use Windows time zone ID for EAT (UTC+3)
                DateTime eatTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, eatTimeZone);
                hfServerTime.Value = eatTime.ToString("dd/MM/yyyy,HH:mm:ss");
            }
        }

        protected void ddlVendorCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTotalTransactions();
            SetupSalesChart();
            SetupVendorBarChart();
        }

        private void PopulateVendorCodes()
        {
            try
            {
                chartsapi.ChartsApi api = new chartsapi.ChartsApi();
                DataTable dt = api.GetAllReceivedTransactions();
                var vendorCodes = dt.AsEnumerable()
                    .Select(row => row.Field<string>("VendorCode"))
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Distinct()
                    .OrderBy(v => v)
                    .ToList();

                ddlVendorCode.Items.Clear();
                ddlVendorCode.Items.Add(new ListItem("All", ""));
                foreach (var vendorCode in vendorCodes)
                {
                    ddlVendorCode.Items.Add(new ListItem(vendorCode, vendorCode));
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading vendor codes: " + ex.Message;
                lblMessage.CssClass = "text-danger mt-2 d-block";
                lblMessage.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error in PopulateVendorCodes: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }

        private void LoadTotalTransactions()
        {
            try
            {
                chartsapi.ChartsApi api = new chartsapi.ChartsApi();
                string vendorCode = ddlVendorCode.SelectedValue;
                DataTable dt = string.IsNullOrEmpty(vendorCode)
                    ? api.GetAllReceivedTransactions()
                    : api.GetAllReceivedTransactionsByVendor(vendorCode);

                lblTotalTransactions.Text = (dt?.Rows.Count ?? 0).ToString("N0");
            }
            catch (Exception ex)
            {
                lblTotalTransactions.Text = "Error";
                System.Diagnostics.Debug.WriteLine($"Error in LoadTotalTransactions: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }

        private void SetupSalesChart()
        {
            chartsapi.ChartsApi api = new chartsapi.ChartsApi();
            string vendorCode = ddlVendorCode.SelectedValue;
            DataTable dt = string.IsNullOrEmpty(vendorCode)
                ? api.GetTransactionCountsByRecordDate()
                : api.GetTransactionCountsByRecordDateAndVendor(vendorCode);

            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();
            double maxCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                DateTime recordDate = Convert.ToDateTime(row["RecordDate"]);
                int transactionCount = Convert.ToInt32(row["TransactionCount"]);
                labels.Add(recordDate.ToString("MMM dd, yyyy"));
                data.Add(transactionCount);

                if (transactionCount > maxCount)
                    maxCount = transactionCount;
            }

            var chartData = new
            {
                labels = labels,
                data = data,
                title = string.IsNullOrEmpty(vendorCode) ? "Daily Transaction Counts" : $"Daily Transaction Counts ({vendorCode})"
            };

            hfSalesChartData.Value = JsonConvert.SerializeObject(chartData);
        }

        private void SetupVendorBarChart()
        {
            chartsapi.ChartsApi api = new chartsapi.ChartsApi();
            string vendorCode = ddlVendorCode.SelectedValue;
            DataTable dt = string.IsNullOrEmpty(vendorCode)
                ? api.GetTransactionCountsByVendorCode()
                : api.GetTransactionCountsByVendorCode().AsEnumerable()
                    .Where(row => row.Field<string>("VendorCode") == vendorCode)
                    .CopyToDataTable();

            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();
            double maxCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                string vendorCodeValue = row["VendorCode"].ToString();
                int transactionCount = Convert.ToInt32(row["TransactionCount"]);
                labels.Add(vendorCodeValue);
                data.Add(transactionCount);

                if (transactionCount > maxCount)
                    maxCount = transactionCount;
            }

            var chartData = new
            {
                labels = labels,
                data = data,
                title = string.IsNullOrEmpty(vendorCode) ? "Transactions by Vendor Code" : $"Transactions by Vendor Code ({vendorCode})"
            };

            hfVendorChartData.Value = JsonConvert.SerializeObject(chartData);
        }

        private void LoadRecentTransactions()
        {
            try
            {
                chartsapi.ChartsApi api = new chartsapi.ChartsApi();
                DataTable dt = api.GetMostRecentTransactions();

                if (dt.Rows.Count > 0)
                {
                    gvRecentTransactions.DataSource = dt;
                    gvRecentTransactions.DataBind();
                }
                else
                {
                    gvRecentTransactions.DataSource = null;
                    gvRecentTransactions.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading recent transactions: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile && FileUpload1.FileName.EndsWith(".csv"))
            {
                try
                {
                    chartsapi.ChartsApi api = new chartsapi.ChartsApi();
                    DataTable dtCount = api.GetTotalTransactionCount();
                    int totalCount = dtCount != null && dtCount.Rows.Count > 0 ? Convert.ToInt32(dtCount.Rows[0]["TotalCount"]) : 0;

                    if (totalCount > 0)
                    {
                        lblMessage.Text = "A CSV file has already been uploaded. Please clear existing data before uploading a new file.";
                        lblMessage.CssClass = "text-danger mt-2 d-block";
                        lblMessage.Visible = true;
                        return;
                    }

                    string filePath = Server.MapPath("~/Uploads/" + FileUpload1.FileName);
                    FileUpload1.SaveAs(filePath);

                    string[] lines = System.IO.File.ReadAllLines(filePath);
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
                    lblMessage.CssClass = "text-success mt-2 d-block";
                    lblMessage.Visible = true;

                    // Refresh vendor codes, chart, total transactions, and bar chart after upload
                    PopulateVendorCodes();
                    LoadTotalTransactions();
                    SetupSalesChart();
                    SetupVendorBarChart();
                    LoadRecentTransactions();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error uploading file: " + ex.Message;
                    lblMessage.CssClass = "text-danger mt-2 d-block";
                    lblMessage.Visible = true;
                }
            }
            else
            {
                lblMessage.Text = "Please select a valid CSV file.";
                lblMessage.CssClass = "text-danger mt-2 d-block";
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

        protected void gvRecentTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = e.Row.Cells[7].Text.Trim();
                if (string.IsNullOrEmpty(status) || status == "NULL" || DBNull.Value.Equals(e.Row.Cells[7].Text))
                {
                    e.Row.Cells[7].Text = "NULL";
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                }
                else if (status == "SUCCESS" || status == "1")
                {
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}