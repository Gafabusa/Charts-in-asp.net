using Charts.chartsapi;
using System;
using System.Data;
using System.Web.UI;

namespace Charts.Admin
{
    public partial class Filters : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize page
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ddlFilterField.SelectedValue))
                {
                    lblMessage.Text = "Please select a filter field.";
                    lblMessage.Visible = true;
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();
                    return;
                }

                ChartsApi api = new ChartsApi();
                string transNo = null, customerRef = null, customerName = null, customerType = null,
                       customerTel = null, vendorTranId = null, receiptNo = null, vendorCode = null, teller = null;

                switch (ddlFilterField.SelectedValue)
                {
                    case "TransNo":
                        transNo = txtFilterValue.Text;
                        break;
                    case "CustomerRef":
                        customerRef = txtFilterValue.Text;
                        break;
                    case "CustomerName":
                        customerName = txtFilterValue.Text;
                        break;
                    case "CustomerType":
                        customerType = txtFilterValue.Text;
                        break;
                    case "CustomerTel":
                        customerTel = txtFilterValue.Text;
                        break;
                    case "VendorTranId":
                        vendorTranId = txtFilterValue.Text;
                        break;
                    case "ReceiptNo":
                        receiptNo = txtFilterValue.Text;
                        break;
                    case "VendorCode":
                        vendorCode = txtFilterValue.Text;
                        break;
                    case "Teller":
                        teller = txtFilterValue.Text;
                        break;
                }

                DataTable dt = api.FilterReceivedTransactions(
                    transNo, customerRef, customerName, customerType, customerTel,
                    vendorTranId, receiptNo, vendorCode, teller);

                if (dt.Rows.Count > 0)
                {
                    gvTransactions.DataSource = dt;
                    gvTransactions.DataBind();
                    lblMessage.Visible = false;
                }
                else
                {
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();
                    lblMessage.Text = "No transactions found matching the criteria.";
                    lblMessage.Visible = true;
                }

                // Clear fields after filtering
                ddlFilterField.SelectedIndex = 0;
                txtFilterValue.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.Visible = true;
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
            }
        }
    }
}