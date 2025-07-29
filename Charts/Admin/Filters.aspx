<%@ Page Title="Filter Transactions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Filters.aspx.cs" 
    Inherits="Charts.Admin.Filters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        <h2 class="fw-bold text-dark mb-4">Filter Transactions</h2>
        <div class="card border-0 shadow-sm mb-4">
            <div class="card-body">
                <div class="row">
                    <div class="col-md-4 mb-3">
                        <label class="form-label">Select Filter Field</label>
                        <asp:DropDownList ID="ddlFilterField" runat="server" CssClass="form-select" AutoPostBack="false">
                            <asp:ListItem Value="" Text="Select a field" />
                            <asp:ListItem Value="TransNo" Text="Transaction No" />
                            <asp:ListItem Value="CustomerRef" Text="Customer Reference" />
                            <asp:ListItem Value="CustomerName" Text="Customer Name" />
                            <asp:ListItem Value="CustomerType" Text="Customer Type" />
                            <asp:ListItem Value="CustomerTel" Text="Customer Telephone" />
                            <asp:ListItem Value="VendorTranId" Text="Vendor Transaction ID" />
                            <asp:ListItem Value="ReceiptNo" Text="Receipt No" />
                            <asp:ListItem Value="VendorCode" Text="Vendor Code" />
                            <asp:ListItem Value="Teller" Text="Teller" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 mb-3">
                        <label class="form-label">Filter Value</label>
                        <asp:TextBox ID="txtFilterValue" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="d-flex justify-content-end">
                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                </div>
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mb-3 d-block" Visible="false" />

        <div class="card border-0 shadow-sm">
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvTransactions" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="true"
                        EmptyDataText="No transactions found." HeaderStyle-CssClass="table-dark">
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>