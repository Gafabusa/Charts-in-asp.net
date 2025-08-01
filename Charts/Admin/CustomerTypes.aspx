<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerTypes.aspx.cs" Inherits="Charts.Admin.CustomerTypes" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Customer Types Distribution</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid py-4">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white border-bottom">
                    <h5 class="fw-bold mb-0">Customer Type Distribution</h5>
                </div>
                <div class="card-body p-4">
                    <div class="chart-container position-relative">
                        <canvas id="customerTypeChart" width="800" height="400"></canvas>
                        <!-- Hidden field to store chart data -->
                        <asp:HiddenField ID="hfCustomerTypeData" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Chart.js Script -->
        <script src="/Scripts/chart.min.js"></script>
        <script type="text/javascript">
            document.addEventListener('DOMContentLoaded', function () {
                const hfCustomerTypeData = document.getElementById('<%= hfCustomerTypeData.ClientID %>').value;
                const customerTypeData = JSON.parse(hfCustomerTypeData || '{}');

                const ctx = document.getElementById('customerTypeChart').getContext('2d');
                new Chart(ctx, {
                    type: 'bar', // Vertical bar chart
                    data: {
                        labels: customerTypeData.labels || [],
                        datasets: [{
                            label: 'Number of Transactions',
                            data: customerTypeData.data || [],
                            backgroundColor: 'rgba(54, 162, 235, 0.6)', // Single color for all bars
                            borderColor: 'rgba(54, 162, 235, 1)', // Single border color
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Customer Type Distribution',
                                font: { size: 20 }
                            },
                            legend: {
                                display: true,
                                position: 'top'
                            }
                        },
                        scales: {
                            y: {
                                beginAtZero: true,
                                title: {
                                    display: true,
                                    text: 'Number of Transactions'
                                }
                            },
                            x: {
                                title: {
                                    display: true,
                                    text: 'Customer Types'
                                }
                            }
                        }
                    }
                });
            });
        </script>
    </form>
</body>
</html>