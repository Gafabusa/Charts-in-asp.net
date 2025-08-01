using Charts.chartsapi;
using System;
using System.Data;
using System.Web.UI;

namespace Charts.Login
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing messages
                pnlAlert.Visible = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Call the API with raw password
                var api = new ChartsApi();
                DataTable dt = api.AdminLogin(email, password);

                // Check if valid user data is returned (non-NULL UserId)
                if (dt.Rows.Count > 0 && dt.Rows[0]["UserId"] != DBNull.Value)
                {
                    Session["UserId"] = dt.Rows[0]["UserId"];
                    Session["FullName"] = dt.Rows[0]["FullName"];
                    Session["Email"] = dt.Rows[0]["Email"];
                    Session["RoleId"] = dt.Rows[0]["RoleId"];
                    Response.Redirect("~/Admin/Dashboard.aspx");
                }
                else
                {
                    ShowMessage("Invalid email or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("An error occurred during login: " + ex.Message);
            }
        }
        private void ShowMessage(string message, string alertClass = "alert-danger")
        {
            lblMessage.Text = message;
            pnlAlert.CssClass = $"alert {alertClass} alert-dismissible fade show mb-3";
            pnlAlert.Visible = true;
        }
    }
}