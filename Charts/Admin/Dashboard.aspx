<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Charts.Admin.Dashboard" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid py-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h2 class="fw-bold text-dark mb-1">Fort Portal City Marathon Registrations</h2>
                <p class="text-muted mb-0">Daily transaction count overview</p>
            </div>         
        </div>

        <!-- Stats Cards -->
        <div class="row mb-4">
            <div class="col-md-3 mb-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="d-flex align-items-center">
                            <div class="flex-grow-1">
                                <h6 class="text-muted mb-1">Total Transactions</h6>
                                <h4 class="fw-bold text-success mb-0">100</h4>
                            </div>
                            <div class="bg-success bg-opacity-10 rounded-circle p-3">
                                <i class="bi bi-currency-dollar text-success"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="d-flex align-items-center">
                            <div class="flex-grow-1">
                                <h6 class="text-muted mb-1">Unique Customers</h6>
                                <h4 class="fw-bold text-primary mb-0">50</h4>
                            </div>
                            <div class="bg-primary bg-opacity-10 rounded-circle p-3">
                                <i class="bi bi-people text-primary"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="d-flex align-items-center">
                            <div class="flex-grow-1">
                                <h6 class="text-muted mb-1">Total Amount</h6>
                                <h4 class="fw-bold text-info mb-0">$5,000,000</h4>
                            </div>
                            <div class="bg-info bg-opacity-10 rounded-circle p-3">
                                <i class="bi bi-wallet text-info"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="d-flex align-items-center">
                            <div class="flex-grow-1">
                                <h6 class="text-muted mb-1">Success Rate</h6>
                                <h4 class="fw-bold text-success mb-0">100%</h4>
                            </div>
                            <div class="bg-success bg-opacity-10 rounded-circle p-3">
                                <i class="bi bi-check-circle text-success"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Chart Section -->
        <div class="row">
            <div class="col-12">
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-white border-bottom">
                        <div class="d-flex justify-content-between align-items-center">
                            <h5 class="fw-bold mb-0">Daily Transaction Counts</h5>
                        </div>
                    </div>
                    <div class="card-body p-4">
                        <div class="chart-container position-relative">
                            <asp:Chart ID="SalesChart" runat="server" Width="800px" Height="400px" 
                                BackColor="White" BorderlineDashStyle="Solid" BorderlineWidth="1" 
                                BorderlineColor="LightGray" Palette="BrightPastel">
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BackColor="White">
                                        <AxisX LineColor="LightGray" MajorGrid-LineColor="LightGray" 
                                               Title="Record Date" TitleFont="Microsoft Sans Serif, 11pt, style=Bold"
                                               LabelStyle-Font="Microsoft Sans Serif, 10pt">
                                            <MajorGrid LineColor="LightGray" LineDashStyle="Dot" />
                                        </AxisX>
                                        <AxisY LineColor="LightGray" MajorGrid-LineColor="LightGray"
                                               Title="Number of Transactions" TitleFont="Microsoft Sans Serif, 11pt, style=Bold"
                                               LabelStyle-Font="Microsoft Sans Serif, 10pt"
                                               LabelStyle-Format="{0:N0}">
                                            <MajorGrid LineColor="LightGray" LineDashStyle="Dot" />
                                        </AxisY>
                                    </asp:ChartArea>
                                </ChartAreas>
                                <Series>
                                    <asp:Series Name="Daily Transactions" ChartType="Column" Color="#0d6efd" 
                                              BorderWidth="0" ShadowOffset="2">
                                    </asp:Series>
                                </Series>
                                <Titles>
                                    <asp:Title Name="MainTitle" Text="Daily Transaction Counts" 
                                             Font="Microsoft Sans Serif, 14pt, style=Bold" 
                                             ForeColor="DarkBlue" />
                                </Titles>
                                <Legends>
                                    <asp:Legend Name="Legend1" Docking="Bottom" Alignment="Center"
                                              Font="Microsoft Sans Serif, 10pt" />
                                </Legends>
                            </asp:Chart>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- CSV Upload Section -->
        <div class="card border-0 shadow-sm mt-4">
            <div class="card-body">
                <h5 class="fw-bold mb-3">Upload CSV File</h5>
                <div class="mb-3">
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                </div>
                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-2 d-block" Visible="false" />
            </div>
        </div>
    </div>
</asp:Content>