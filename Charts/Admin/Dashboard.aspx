<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Charts.Admin.Dashboard" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Fix the tab navigation bar */
        .nav-tabs-fixed {
            position: fixed;
            top: 56px;
            left: 250px;
            right: 0;
            z-index: 1025;
            background-color: #f8f9fa;
            border-radius: 8px;
            padding: 5px;
            margin: 0 20px 0 0;
        }

        /* Adjust tab content to avoid overlap */
        .tab-content-container {
            margin-top: 70px;
            padding: 20px 0;
        }

        /* Responsive adjustments */
        @media (max-width: 767.98px) {
            .nav-tabs-fixed {
                position: static;
                left: 0;
                right: 0;
                margin: 0;
            }
            .tab-content-container {
                margin-top: 0;
            }
        }
    </style>

    <div class="container-fluid py-4">
        <!-- Top-Level Navigation Tabs -->
        <ul class="nav nav-tabs nav-justified mb-4 nav-tabs-fixed" id="mainTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="analytics-tab" data-bs-toggle="tab" data-bs-target="#analytics" type="button" role="tab" aria-controls="analytics" aria-selected="true" style="font-weight: 500; padding: 10px 20px; border: none; border-radius: 6px;">
                    Analytics
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="daily-tab" data-bs-toggle="tab" data-bs-target="#daily" type="button" role="tab" aria-controls="daily" aria-selected="false" style="color: #495057; font-weight: 500; padding: 10px 20px; border: none; border-radius: 6px;">
                    Daily
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="hourly-tab" data-bs-toggle="tab" data-bs-target="#hourly" type="button" role="tab" aria-controls="hourly" aria-selected="false" style="color: #495057; font-weight: 500; padding: 10px 20px; border: none; border-radius: 6px;">
                    Hourly
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="overtime-tab" data-bs-toggle="tab" data-bs-target="#overtime" type="button" role="tab" aria-controls="overtime" aria-selected="false" style="color: #495057; font-weight: 500; padding: 10px 20px; border: none; border-radius: 6px;">
                    Overtime
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="utilities-tab" data-bs-toggle="tab" data-bs-target="#utilities" type="button" role="tab" aria-controls="utilities" aria-selected="false" style="color: #495057; font-weight: 500; padding: 10px 20px; border: none; border-radius: 6px;">
                    Utilities
                </button>
            </li>
        </ul>

        <!-- Tab Content -->
        <div class="tab-content tab-content-container">
            <!-- Analytics Tab (Default Visible) -->
            <div class="tab-pane fade show active" id="analytics" role="tabpanel" aria-labelledby="analytics-tab">
                <div class="d-flex justify-content-between align-items-center mb-4">
                    <div>
                        <h2 class="fw-bold text-dark mb-1">Fort Portal City Marathon Registrations</h2>
                        <p class="text-muted mb-0">Daily transaction count overview</p>
                    </div>         
                </div>
                <asp:HiddenField ID="hfServerTime" runat="server" />

                <!-- Stats Cards -->
                <div class="row mb-4">
                    <div class="col-md-3 mb-3">
                        <div class="card border-0 shadow-sm h-100" style="background: linear-gradient(135deg, #ffffff, #f0f4f8); border-radius: 10px;">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div class="flex-grow-1">
                                        <h6 class="text-muted mb-1">Total Transactions</h6>
                                        <h4 class="fw-bold text-success mb-0">
                                            <asp:Label ID="lblTotalTransactions" runat="server" Text="0" />
                                        </h4>
                                    </div>
                                    <div class="bg-success bg-opacity-10 rounded-circle p-3">
                                        <i class="bi bi-currency-dollar text-success"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 mb-3">
                        <div class="card border-0 shadow-sm h-100" style="background: linear-gradient(135deg, #ffffff, #f0f4f8); border-radius: 10px;">
                            <div class="card-body">
                                <div class="d-flex align-items-center">
                                    <div class="flex-grow-1">
                                        <h6 class="text-muted mb-1">Current Date & Time</h6>
                                        <h4 class="fw-bold text-primary mb-0">
                                            <span id="currentDateTime"></span>
                                        </h4>
                                    </div>
                                    <div class="bg-primary bg-opacity-10 rounded-circle p-3">
                                        <i class="bi bi-clock text-primary"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 offset-md-3 mb-3">
                        <div class="card border-0 shadow-sm h-100" style="background: linear-gradient(135deg, #ffffff, #f0f4f8); border-radius: 10px;">
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

                <!-- Most Recent 10 Transactions -->
                <div class="row mb-4">
                    <div class="col-12">
                        <div class="card border-0 shadow-sm h-100" style="border-radius: 10px;">
                            <div class="card-header bg-white border-bottom">
                                <h5 class="fw-bold mb-0">Most Recent 10 Transactions</h5>
                            </div>
                            <div class="card-body p-3" style="max-height: 300px; overflow-y: auto; overflow-x: auto; background: #f8f9fa;">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvRecentTransactions" runat="server" CssClass="table table-striped table-bordered" 
                                                  AutoGenerateColumns="false" EmptyDataText="No recent transactions found."
                                                  HeaderStyle-CssClass="table-dark" Width="100%" OnRowDataBound="gvRecentTransactions_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="TranId" HeaderText="Transaction ID" />
                                            <asp:BoundField DataField="TransNo" HeaderText="Transaction No" />
                                            <asp:BoundField DataField="CustomerRef" HeaderText="Customer Ref" />
                                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="CustomerType" HeaderText="Customer Type" />
                                            <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                            <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-CssClass="status-column" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Upload CSV File -->
                <div class="row mb-4">
                    <div class="col-12">
                        <div class="card border-0 shadow-sm mt-0" style="border-radius: 10px;">
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
                </div>
            </div>

            <!-- Daily Tab -->
            <div class="tab-pane fade" id="daily" role="tabpanel" aria-labelledby="daily-tab">
                <div class="row">            
                    <div class="col-md-9">
                        <div class="card border-0 shadow-sm mb-4">
                            <div class="card-header bg-white border-bottom">
                                <div class="d-flex justify-content-between align-items-center">
                                    <h5 class="fw-bold mb-0">Daily Transaction Counts</h5>
                                </div>
                            </div>
                            <div class="card-body p-4">
                                <div class="chart-container position-relative">
                                    <canvas id="salesChart" width="1000" height="600"></canvas>
                                    <asp:HiddenField ID="hfSalesChartData" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="card border-0 shadow-sm mb-4">
                            <div class="card-header bg-white border-bottom">
                                <div class="d-flex justify-content-between align-items-center">
                                    <h5 class="fw-bold mb-0">Transaction Distribution by Vendor</h5>
                                </div>
                            </div>
                            <div class="card-body p-4">
                                <div class="chart-container position-relative">
                                    <canvas id="vendorChart" width="1000" height="600"></canvas>
                                    <asp:HiddenField ID="hfVendorChartData" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="card border-0 shadow-sm mb-4">
                            <div class="card-body" style="min-height: 200px;">
                                <h6 class="fw-bold mb-3">Filter by Vendor</h6>
                                <asp:DropDownList ID="ddlVendorCode" runat="server" CssClass="form-control mb-3" AutoPostBack="true" 
                                                  OnSelectedIndexChanged="ddlVendorCode_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Hourly Tab with Transactions Per Hour -->
            <div class="tab-pane fade" id="hourly" role="tabpanel" aria-labelledby="hourly-tab">
                <iframe src="HourlyTransactions.aspx" style="width: 100%; height: 900px; border: none;" frameborder="0"></iframe>
            </div>

            <!-- Overtime Tab with Status Distribution and Customer Types -->
            <div class="tab-pane fade" id="overtime" role="tabpanel" aria-labelledby="overtime-tab">
                <div class="row">
                    <div class="col-md-12">
                        <div class="card border-0 shadow-sm mb-4">
                            <div class="card-header bg-white border-bottom">
                                <div class="d-flex justify-content-between align-items-center">
                                    <h5 class="fw-bold mb-0">Status Distribution</h5>
                                </div>
                            </div>
                            <div class="card-body p-4">
                                <div class="chart-container position-relative">
                                    <iframe src="StatusDistribution.aspx" style="width: 100%; height: 400px; border: none;" frameborder="0"></iframe>
                                </div>
                            </div>
                        </div>

                        <div class="card border-0 shadow-sm mb-4">
                            <div class="card-header bg-white border-bottom">
                                <div class="d-flex justify-content-between align-items-center">
                                    <h5 class="fw-bold mb-0">Customer Types Distribution</h5>
                                </div>
                            </div>
                            <div class="card-body p-4">
                                <div class="chart-container position-relative">
                                    <iframe src="CustomerTypes.aspx" style="width: 100%; height: 400px; border: none;" frameborder="0"></iframe>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Utilities Tab with UtilityCode Distribution -->
           <!-- Inside the Utilities tab -->
<div class="tab-pane fade" id="utilities" role="tabpanel" aria-labelledby="utilities-tab">
    <div class="row">
        <div class="col-md-12">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white border-bottom">
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="fw-bold mb-0">A piechart showing UtilityCode Distribution</h5>
                    </div>
                </div>
                <div class="card-body p-4">
                    <div class="chart-container position-relative">
                        <canvas id="utilityPieChart" width="400" height="400"></canvas>
                        <asp:HiddenField ID="hfUtilityPieChartData" runat="server" />
                    </div>
                </div>
            </div>

            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white border-bottom">
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="fw-bold mb-0">UtilityCode Distribution (Bar Chart)</h5>
                    </div>
                </div>
                <div class="card-body p-4">
                    <div class="chart-container position-relative">
                        <canvas id="utilityBarChart" width="1000" height="400"></canvas>
                        <asp:HiddenField ID="hfUtilityBarChartData" runat="server" />
                    </div>
                </div>
            </div>

            <!-- New Filter and GridView -->
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white border-bottom">
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="fw-bold mb-0">All Transactions by UtilityCode</h5>
                    </div>
                </div>
                <div class="card-body p-4">
                    <div class="mb-3">
                        <asp:DropDownList ID="ddlUtilityCode" runat="server" CssClass="form-control mb-3" AutoPostBack="true" 
                                          OnSelectedIndexChanged="ddlUtilityCode_SelectedIndexChanged">
                            <asp:ListItem Text="All" Value="" />
                        </asp:DropDownList>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvUtilityTransactions" runat="server" CssClass="table table-striped table-bordered" 
                                      AutoGenerateColumns="true" EmptyDataText="No transactions found for the selected UtilityCode."
                                      HeaderStyle-CssClass="table-dark" Width="100%">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </div>
</div>

        <!-- Chart.js Script -->
        <script src="/Scripts/chart.min.js"></script>
        <script type="text/javascript">
            document.addEventListener('DOMContentLoaded', function () {
                // Real-time Date and Time Update
                function updateDateTime() {
                    const serverTimeStr = document.getElementById('<%= hfServerTime.ClientID %>').value;
                    const [datePart, timePart] = serverTimeStr.split(',');
                    const [day, month, year] = datePart.split('/').map(Number);
                    const [hours, minutes, seconds] = timePart.split(':').map(Number);
                    let serverTime = new Date(year, month - 1, day, hours, minutes, seconds);
                    const now = new Date();
                    const elapsedSeconds = Math.floor((now - serverTime) / 1000);
                    serverTime.setSeconds(serverTime.getSeconds() + elapsedSeconds);
                    const formattedDay = String(serverTime.getDate()).padStart(2, '0');
                    const formattedMonth = String(serverTime.getMonth() + 1).padStart(2, '0');
                    const formattedYear = serverTime.getFullYear();
                    const formattedHours = String(serverTime.getHours()).padStart(2, '0');
                    const formattedMinutes = String(serverTime.getMinutes()).padStart(2, '0');
                    const dateTimeString = `${formattedDay}/${formattedMonth}/${formattedYear}, ${formattedHours}:${formattedMinutes}`;
                    document.getElementById('currentDateTime').innerHTML = dateTimeString;
                }
                updateDateTime();
                setInterval(updateDateTime, 1000);

                // Sales Chart
                const hfSalesChartData = document.getElementById('<%= hfSalesChartData.ClientID %>').value;
                const salesChartData = JSON.parse(hfSalesChartData || '{}');
                const salesCtx = document.getElementById('salesChart').getContext('2d');
                new Chart(salesCtx, {
                    type: 'bar',
                    data: {
                        labels: salesChartData.labels || [],
                        datasets: [{
                            label: 'Transaction Count',
                            data: salesChartData.data || [],
                            backgroundColor: 'rgba(13, 110, 253, 0.6)',
                            borderColor: 'rgba(13, 110, 253, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: salesChartData.title || 'Daily Transaction Counts',
                                font: { size: 20 }
                            },
                            legend: {
                                display: true,
                                position: 'bottom'
                            }
                        },
                        scales: {
                            x: {
                                title: {
                                    display: true,
                                    text: 'Record Date',
                                    font: { weight: 'bold' }
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Number of Transactions',
                                    font: { weight: 'bold' }
                                },
                                ticks: {
                                    stepSize: 1
                                }
                            }
                        }
                    }
                });

                // Vendor Chart
                const hfVendorChartData = document.getElementById('<%= hfVendorChartData.ClientID %>').value;
                const vendorChartData = JSON.parse(hfVendorChartData || '{}');
                const vendorCtx = document.getElementById('vendorChart').getContext('2d');
                new Chart(vendorCtx, {
                    type: 'bar',
                    data: {
                        labels: vendorChartData.labels || [],
                        datasets: [{
                            label: 'Transaction Count',
                            data: vendorChartData.data || [],
                            backgroundColor: 'rgba(255, 214, 186, 0.6)',
                            borderColor: 'rgba(255, 214, 186, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: vendorChartData.title || 'Transactions by Vendor Code',
                                font: { size: 20 }
                            },
                            legend: {
                                display: false
                            }
                        },
                        scales: {
                            x: {
                                title: {
                                    display: true,
                                    text: 'Vendor Code',
                                    font: { weight: 'bold' }
                                },
                                ticks: {
                                    autoSkip: false
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Number of Transactions',
                                    font: { weight: 'bold' }
                                },
                                ticks: {
                                    stepSize: 1
                                }
                            }
                        }
                    }
                });

                // Utility Pie Chart
                const hfUtilityPieChartData = document.getElementById('<%= hfUtilityPieChartData.ClientID %>').value;
                const utilityPieChartData = JSON.parse(hfUtilityPieChartData || '{}');
                const utilityPieCtx = document.getElementById('utilityPieChart').getContext('2d');
                new Chart(utilityPieCtx, {
                    type: 'pie',
                    data: {
                        labels: utilityPieChartData.labels || [],
                        datasets: [{
                            data: utilityPieChartData.data || [],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.6)', // Red
                                'rgba(54, 162, 235, 0.6)', // Blue
                                'rgba(255, 206, 86, 0.6)', // Yellow
                                'rgba(75, 192, 192, 0.6)', // Teal
                                'rgba(153, 102, 255, 0.6)' // Purple
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: utilityPieChartData.title || 'UtilityCode Distribution',
                                font: { size: 20 }
                            },
                            legend: {
                                position: 'bottom'
                            }
                        }
                    }
                });

                // Utility Bar Chart
                const hfUtilityBarChartData = document.getElementById('<%= hfUtilityBarChartData.ClientID %>').value;
                const utilityBarChartData = JSON.parse(hfUtilityBarChartData || '{}');
                const utilityBarCtx = document.getElementById('utilityBarChart').getContext('2d');
                new Chart(utilityBarCtx, {
                    type: 'bar',
                    data: {
                        labels: utilityBarChartData.labels || [],
                        datasets: [{
                            label: 'Transaction Count',
                            data: utilityBarChartData.data || [],
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.6)', // Red
                                'rgba(54, 162, 235, 0.6)', // Blue
                                'rgba(255, 206, 86, 0.6)', // Yellow
                                'rgba(75, 192, 192, 0.6)', // Teal
                                'rgba(153, 102, 255, 0.6)' // Purple
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: utilityBarChartData.title || 'UtilityCode Distribution',
                                font: { size: 20 }
                            },
                            legend: {
                                display: false
                            }
                        },
                        scales: {
                            x: {
                                title: {
                                    display: true,
                                    text: 'Utility Code',
                                    font: { weight: 'bold' }
                                },
                                ticks: {
                                    autoSkip: false
                                }
                            },
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Number of Transactions',
                                    font: { weight: 'bold' }
                                },
                                ticks: {
                                    stepSize: 1
                                }
                            }
                        }
                    }
                });
            });
        </script>
    </div>
</asp:Content>