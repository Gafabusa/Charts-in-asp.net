using System;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;

namespace Charts.Admin
{
    public partial class StatusDistribution : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatusData();
            }
        }

        private void LoadStatusData()
        {
            chartsapi.ChartsApi api = new chartsapi.ChartsApi();
            System.Data.DataTable dt = api.GetStatusDistribution();
            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                labels.Add(row["Status"].ToString());
                data.Add(Convert.ToInt32(row["StatusCount"]));
            }

            var chartData = new
            {
                labels = labels,
                data = data
            };

            hfStatusData.Value = JsonConvert.SerializeObject(chartData);
        }
    }
}