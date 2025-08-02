    <%@ Page Title="System Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs"
    Inherits="Charts.Admin.Users.CreateUser" %>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
    <div class="row justify-content-center">
    <div class="col-12">
    <!-- Landing Page with Button and Users Table -->
    <asp:Panel ID="pnlLanding" runat="server" Visible="true">
    <div class="d-flex justify-content-between align-items-start mb-4">
    <h4 class="fw-bold text-primary">System Utilities</h4>
    <asp:Button ID="btnShowCreateUserForm" runat="server"
    Text="Create New Utility"
    CssClass="btn btn-success btn-lg px-4"
    OnClick="btnShowCreateUserForm_Click" />
    </div>
    <!-- Users Table -->
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered"
    OnRowCommand="gvUsers_RowCommand" DataKeyNames="UserId">
    <Columns>
    <asp:BoundField DataField="UserId" HeaderText="User ID" ReadOnly="True" />
    <asp:BoundField DataField="FullName" HeaderText="Utility User" />
    <asp:BoundField DataField="Email" HeaderText="Email" />
    <asp:BoundField DataField="RoleName" HeaderText="Utilities" />
    <asp:TemplateField HeaderText="Actions">
    <ItemTemplate>
    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-sm"
    CommandName="EditUser" CommandArgument='<%# Eval("UserId") %>' />
    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
    CommandName="DeleteUser" CommandArgument='<%# Eval("UserId") %>'
    OnClientClick="return confirm('Are you sure you want to delete this utility user?');" />
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
    <div class="text-center">No utilities found.</div>
    </EmptyDataTemplate>
    </asp:GridView>
    </asp:Panel>
    <!-- Create User Form (Hidden Initially) -->
    <asp:Panel ID="pnlCreateUserForm" runat="server" Visible="false">
    <div class="row justify-content-center">
    <div class="col-12 col-md-6 col-lg-5">
    <div class="card shadow border-0 mt-4">
    <div class="card-body p-4">
    <div class="text-center mb-3">
    <h4 class="fw-bold text-primary mb-1">Create Utility</h4>
    <p class="text-muted small">Add a new system utility</p>
    </div>
    <!-- Alert Panel -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show mb-3">
    <asp:Label ID="lblMessage" runat="server"></asp:Label>
    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>
    <!-- Full Name Field -->
    <div class="mb-3">
    <asp:Label ID="lblFullName" runat="server" Text="Full Name" CssClass="form-label fw-semibold"></asp:Label>
    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control w-100" placeholder="Enter full name" MaxLength="100" required="true"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    <!-- Email Field -->
    <div class="mb-3">
    <asp:Label ID="lblEmail" runat="server" Text="Email Address" CssClass="form-label fw-semibold"></asp:Label>
    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control w-100" placeholder="Enter email address" TextMode="Email" required="true"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format" CssClass="text-danger small" Display="Dynamic" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"></asp:RegularExpressionValidator>
    </div>
    <!-- Role Dropdown Field -->
    <div class="mb-3">
    <asp:Label ID="lblRole" runat="server" Text="User Role" CssClass="form-label fw-semibold"></asp:Label>
    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select w-100" AppendDataBoundItems="true">
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole"
    InitialValue="" ErrorMessage="Please select a role"
    CssClass="text-danger small" Display="Dynamic" />
    </div>
    <!-- Action Buttons -->
    <div class="d-flex justify-content-center gap-2">
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-secondary px-4" OnClick="btnCancel_Click" CausesValidation="false" UseSubmitBehavior="false" />
    <asp:Button ID="btnSave" runat="server" Text="Create Utility" CssClass="btn btn-success px-4" OnClick="btnSave_Click" />
    </div>
    </div>
    </div>
    </div>
    </div>
    </asp:Panel>
    </div>
    </div>
    </div>
    </asp:Content>