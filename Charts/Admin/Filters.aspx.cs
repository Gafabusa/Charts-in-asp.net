using Charts.chartsapi;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Charts.Admin
{
    public partial class Filters : Page
    {
        private int totalTransactions = 0; // Store total transactions

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllTransactions();
            }
            UpdatePageInfo();
        }

        private void LoadAllTransactions()
        {
            try
            {
                chartsapi.ChartsApi api = new chartsapi.ChartsApi();
                DataTable dt = api.GetAllReceivedTransactions();

                if (dt.Rows.Count > 0)
                {
                    totalTransactions = dt.Rows.Count; // Store total rows
                    gvTransactions.DataSource = dt;
                    gvTransactions.DataBind();
                    lblMessage.Visible = false;
                }
                else
                {
                    gvTransactions.DataSource = null;
                    gvTransactions.DataBind();
                    lblMessage.Text = "No transactions found.";
                    lblMessage.Visible = true;
                    totalTransactions = 0; // Reset total
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.Visible = true;
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                totalTransactions = 0; // Reset total
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
                    totalTransactions = 0; // Reset total
                    return;
                }

                chartsapi.ChartsApi api = new chartsapi.ChartsApi();
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
                    totalTransactions = dt.Rows.Count; // Store total rows
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
                    totalTransactions = 0; // Reset total
                }

                // Clear fields after filtering
                ddlFilterField.SelectedIndex = 0;
                txtFilterValue.Text = string.Empty;
                UpdatePageInfo();
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.Visible = true;
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                totalTransactions = 0; // Reset total
            }
        }

        protected void gvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTransactions.PageIndex = e.NewPageIndex;
                if (string.IsNullOrWhiteSpace(ddlFilterField.SelectedValue))
                {
                    // Load all transactions if no filter is applied
                    LoadAllTransactions();
                }
                else
                {
                    // Reapply the filter for the new page
                    chartsapi.ChartsApi api = new chartsapi.ChartsApi();
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
                        totalTransactions = dt.Rows.Count; // Store total rows
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
                        totalTransactions = 0; // Reset total
                    }
                }
                UpdatePageInfo();
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error: {ex.Message}";
                lblMessage.Visible = true;
                gvTransactions.DataSource = null;
                gvTransactions.DataBind();
                totalTransactions = 0; // Reset total
            }
        }

        private void UpdatePageInfo()
        {
            int pageSize = gvTransactions.PageSize;
            int currentPage = gvTransactions.PageIndex + 1;
            int startRow = (currentPage - 1) * pageSize + 1;
            int endRow = Math.Min(currentPage * pageSize, totalTransactions);
            string pageInfo = $"Showing {startRow}-{endRow} of {totalTransactions:N0} transactions";

            lblPageInfoTop.Text = pageInfo;
            lblPageInfoBottom.Text = pageInfo;
        }
    }
}