<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HourlyTransactions.aspx.cs" Inherits="Charts.Admin.HourlyTransactions" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Hourly Transactions</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid py-4">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white border-bottom">
                    <h5 class="fw-bold mb-0">Transactions Per Hour</h5>
                </div>
                <div class="card-body p-4">
                    <div class="chart-container position-relative">
                        <canvas id="hourlyChart" width="1000" height="600"></canvas>
                        <!-- Hidden field to store chart data -->
                        <asp:HiddenField ID="hfHourlyData" runat="server" />
                    </div>
                </div>
            </div>
            <div class="card border-0 shadow-sm">
                <div class="card-body p-4">
                    <h6 class="fw-bold mb-2">Hour with Highest Transactions:</h6>
                    <p class="mb-0" id="highestHour"></p>
                </div>
            </div>
        </div>

        <!-- Chart.js Script -->
        <script src="/Scripts/chart.min.js"></script>
        <script type="text/javascript">
            document.addEventListener('DOMContentLoaded', function () {
                const hfHourlyData = document.getElementById('<%= hfHourlyData.ClientID %>').value;
                const hourlyData = JSON.parse(hfHourlyData || '{}');

                const ctx = document.getElementById('hourlyChart').getContext('2d');
                new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: hourlyData.labels || [],
                        datasets: [{
                            label: 'Transaction Count',
                            data: hourlyData.data || [],
                            backgroundColor: 'rgba(54, 162, 235, 0.6)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Transactions Per Hour (0-23)',
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
                                    text: 'Hour of Day',
                                    font: { weight: 'bold' }
                                },
                                ticks: {
                                    autoSkip: false // Ensures all hour labels (0-23) are displayed
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

                // Display the hour with the highest number of transactions
                const highestHour = hourlyData.highestHour || 'N/A';
                document.getElementById('highestHour').textContent = highestHour;
            });
        </script>
    </form>
</body>
</html>