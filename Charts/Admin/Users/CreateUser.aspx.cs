using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Charts;
using Charts.chartsapi;

namespace Charts.Admin.Users
{
    public partial class CreateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRoles();
                LoadUsers();
                // Set up the master page for dashboard
                var master = (SiteMaster)this.Master;
                // Show landing page by default
                pnlLanding.Visible = true;
                pnlCreateUserForm.Visible = false;
            }
        }

        protected void btnShowCreateUserForm_Click(object sender, EventArgs e)
        {
            // Hide landing page and show form
            pnlLanding.Visible = false;
            pnlCreateUserForm.Visible = true;
            // Set focus to the first input field
            SetFocusOnLoad();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string fullName = txtFullName.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    int roleId = Convert.ToInt32(ddlRole.SelectedValue);

                    // Validate input
                    if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(ddlRole.SelectedValue))
                    {
                        ShowMessage("Please fill in all required fields.");
                        return;
                    }

                    // Generate system password
                    string plainPassword = GenerateRandomPassword();

                    // Hash password
                    string hashedPassword = HashPassword(plainPassword);

                    // Call API to create user
                    var api = new chartsapi.ChartsApi();
                    var result = api.CreateUser(fullName, email, hashedPassword, roleId);

                    // Send email
                    SendUserWelcomeEmail(email, fullName, plainPassword);

                    // Show success
                    ShowMessage($"User {fullName} created and email sent successfully!", "alert-success");
                    ClearForm();
                    LoadUsers(); // Refresh users table
                    // Keep form visible to show success message
                    pnlLanding.Visible = false;
                    pnlCreateUserForm.Visible = true;
                }
                catch (SqlException ex) when (ex.Number == 50000 && ex.Message.Contains("A user with this email already exists"))
                {
                    ShowMessage("A user with this email already exists.");
                }
                catch (SmtpException)
                {
                    ShowMessage("The email address is invalid or does not exist.");
                }
                catch (Exception ex)
                {
                    ShowMessage($"An error occurred while creating the user: {ex.Message}");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Go back to landing page
            pnlLanding.Visible = true;
            pnlCreateUserForm.Visible = false;
            ClearForm();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditUser")
            {
                ShowMessage($"Edit functionality for User ID {userId} is not implemented yet.", "alert-info");
            }
            else if (e.CommandName == "DeleteUser")
            {
                ShowMessage($"Delete functionality for User ID {userId} is not implemented yet.", "alert-info");
            }
        }

        private void LoadRoles()
        {
            var api = new chartsapi.ChartsApi();
            DataTable dt = api.GetAllRoles();
            ddlRole.DataSource = dt;
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("-- Select Role --", ""));
        }

        private void LoadUsers()
        {
            var api = new chartsapi.ChartsApi();
            DataTable dt = api.GetAllNonAdminUsers();
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }

        // Generate random password
        private string GenerateRandomPassword()
        {
            // Generate a simple random password
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        // Hashing password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected void lnkBackToDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Dashboard.aspx");
        }

        private void ShowMessage(string message, string alertClass = "alert-danger")
        {
            lblMessage.Text = message;
            pnlAlert.CssClass = $"alert {alertClass} alert-dismissible fade show mb-3";
            pnlAlert.Visible = true;
            // Keep form visible to show the message
            pnlLanding.Visible = false;
            pnlCreateUserForm.Visible = true;
            // Scroll to top to show the alert
            ScrollToTop();
        }

        private void ClearForm()
        {
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlRole.SelectedIndex = 0;
            pnlAlert.Visible = false;
        }

        private void SetFocusOnLoad()
        {
            // Set focus to the full name field using JavaScript
            string script = "document.getElementById('" + txtFullName.ClientID + "').focus();";
            ClientScript.RegisterStartupScript(this.GetType(), "SetFocus", script, true);
        }

        private void ScrollToTop()
        {
            // Scroll to top of page to show alert
            string script = "window.scrollTo(0, 0);";
            ClientScript.RegisterStartupScript(this.GetType(), "ScrollToTop", script, true);
        }

        private void SendUserWelcomeEmail(string toEmail, string fullName, string plainPassword)
        {
            string subject = "Welcome to the System";
            string body = $@"
                Hello {fullName},<br/><br/>
                Your user account has been created.<br/>
                <strong>Login Email:</strong> {toEmail}<br/>
                <strong>Temporary Password:</strong> {plainPassword}<br/><br/>
                Regards,<br/>
                Pegasus Technologies Limited Team
            ";

            try
            {
                string fromEmail = ConfigurationManager.AppSettings["EmailAddress"];
                string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(fromEmail);
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(fromEmail, emailPassword);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (SmtpException)
            {
                throw; 
            }
        }
    }
}