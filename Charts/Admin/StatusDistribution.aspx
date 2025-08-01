<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatusDistribution.aspx.cs" Inherits="Charts.Admin.StatusDistribution" %>
<%@ Register Assembly="System.Web.DataVisualization" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Status Distribution</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid py-4">
            <div class="card border-0 shadow-sm mb-4">
                <div class="card-header bg-white border-bottom">
                    <h5 class="fw-bold mb-0">Status Distribution</h5>
                </div>
               <div class="card-body p-4">
    <div class="chart-container position-relative" style="max-width: 350px; margin: 0 auto;">
        <canvas id="statusChart" style="max-height: 350px;"></canvas>
        <asp:HiddenField ID="hfStatusData" runat="server" />
    </div>
</div>

            </div>
        </div>

        <!-- Chart.js Script -->
        <script src="/Scripts/chart.min.js"></script>
        <script type="text/javascript">
            document.addEventListener('DOMContentLoaded', function () {
                const hfStatusData = document.getElementById('<%= hfStatusData.ClientID %>').value;
                const statusData = JSON.parse(hfStatusData || '{}');

                const ctx = document.getElementById('statusChart').getContext('2d');
                new Chart(ctx, {
                    type: 'pie',
                    data: {
                        labels: statusData.labels || [],
                        datasets: [{
                            data: statusData.data || [],
                            backgroundColor: [
                                'rgba(75, 192, 75, 0.6)',     
                                'rgba(255, 99, 132, 0.6)',   
                                'rgba(255, 206, 86, 0.6)',    
                                'rgba(255, 159, 64, 0.6)',   
                                'rgba(153, 102, 255, 0.6)',   
                                'rgba(101, 67, 33, 0.6)'   
                            ],
                            borderColor: [
                                'rgba(75, 192, 75, 1)',    
                                'rgba(255, 99, 132, 1)',      
                                'rgba(255, 206, 86, 1)',     
                                'rgba(255, 159, 64, 1)',      
                                'rgba(153, 102, 255, 1)',      
                                'rgba(101, 67, 33, 1)'   
                            ],

                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            title: {
                                display: true,
                                text: 'Status Distribution',
                                font: { size: 20 }
                            },
                            legend: {
                                position: 'bottom'
                            }
                        }
                    }
                });
            });
        </script>
    </form>
</body>
</html>