using System;
using System.Web.Services;
using System.Web.UI;
using Newtonsoft.Json;

namespace Charts.Admin
{
    public partial class CustomerTypes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomerTypeData();
            }
        }
        private void LoadCustomerTypeData()
        {
            chartsapi.ChartsApi api = new chartsapi.ChartsApi();
            System.Data.DataTable dt = api.GetCustomerTypeDistribution();
            var labels = new System.Collections.Generic.List<string>();
            var data = new System.Collections.Generic.List<int>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                labels.Add(row["CustomerType"].ToString());
                data.Add(Convert.ToInt32(row["TypeCount"]));
            }

            var chartData = new
            {
                labels = labels,
                data = data
            };

            hfCustomerTypeData.Value = JsonConvert.SerializeObject(chartData);
        }
    }    
    }    
