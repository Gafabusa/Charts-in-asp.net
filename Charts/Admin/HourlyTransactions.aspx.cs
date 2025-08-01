using System;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;

namespace Charts.Admin
{
    public partial class HourlyTransactions : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadHourlyData();
            }
        }

        private void LoadHourlyData()
        {
            chartsapi.ChartsApi api = new chartsapi.ChartsApi();
            System.Data.DataTable dt = api.GetTransactionsPerHour();
            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();
            int maxTransactions = 0;
            int hourWithMax = 0;

            // Initialize array for 0-23 hours
            int[] hourlyCounts = new int[24];

            // Aggregate transactions by hour across all days
            foreach (System.Data.DataRow row in dt.Rows)
            {
                int hour = Convert.ToInt32(row["HourOfDay"]);
                int count = Convert.ToInt32(row["TransactionCount"]);
                hourlyCounts[hour] += count;
            }

            // Populate labels and data
            for (int i = 0; i < 24; i++)
            {
                labels.Add(i.ToString());
                data.Add(hourlyCounts[i]);
                if (hourlyCounts[i] > maxTransactions)
                {
                    maxTransactions = hourlyCounts[i];
                    hourWithMax = i;
                }
            }

            var chartData = new
            {
                labels = labels,
                data = data,
                highestHour = maxTransactions > 0 ? $"{hourWithMax}:00 - {hourWithMax}:59 ({maxTransactions} transactions)" : "N/A"
            };

            hfHourlyData.Value = JsonConvert.SerializeObject(chartData);
        }
    }
}