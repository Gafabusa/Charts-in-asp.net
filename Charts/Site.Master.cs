using Charts.chartsapi;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Charts
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Set active class based on the current page
            string currentPath = Request.Url.AbsolutePath.ToLower();
            if (currentPath.Contains("dashboard.aspx"))
            {
                lnkDashboard.Attributes["class"] = "nav-link text-dark rounded d-flex align-items-center py-2 px-3 active bg-info bg-opacity-25";
            }
            else if (currentPath.Contains("filters.aspx"))
            {
                lnkFilters.Attributes["class"] = "nav-link text-dark rounded d-flex align-items-center py-2 px-3 active bg-warning bg-opacity-25";
            }
            else
            {
                lnkDashboard.Attributes["class"] = "nav-link text-dark rounded d-flex align-items-center py-2 px-3 bg-opacity-10";
                lnkFilters.Attributes["class"] = "nav-link text-dark rounded d-flex align-items-center py-2 px-3 bg-opacity-10";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlExportFormat.SelectedValue))
            {
                // Handle error in child page if needed
                return;
            }

            ChartsApi api = new ChartsApi();
            DataTable dt = api.GetAllReceivedTransactions();

            if (dt.Rows.Count == 0)
            {
                // Handle empty data in child page if needed
                return;
            }

            if (ddlExportFormat.SelectedValue == "pdf")
            {
                ExportToPdf(dt);
            }
            else if (ddlExportFormat.SelectedValue == "csv")
            {
                ExportToCsv(dt);
            }
        }

        private void ExportToPdf(DataTable dt)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape; // Landscape for wide table
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Fonts
                var fontOptions = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.EmbedCompleteFontFile);
                XFont titleFont = new XFont("Arial", 18, XFontStyleEx.Bold, fontOptions);
                XFont headerFont = new XFont("Arial", 10, XFontStyleEx.Bold, fontOptions);
                XFont cellFont = new XFont("Arial", 9, XFontStyleEx.Regular, fontOptions);

                // Title
                gfx.DrawString("Fort Portal City Marathon Registrations", titleFont, XBrushes.Black,
                    new XRect(XUnit.FromPoint(0).Point, XUnit.FromPoint(20).Point, XUnit.FromPoint(page.Width.Point).Point, XUnit.FromPoint(30).Point),
                    XStringFormats.Center);

                // Table setup
                double x = 10, y = 60;
                double cellWidth = 60; // Adjusted for 12 columns
                double cellHeight = 20; // Increased for better readability
                int maxRowsPerPage = (int)((page.Height.Point - y - 20) / cellHeight); // Reserve space for footer

                // Headers
                string[] headers = new string[]
                {
                    "TranId", "TransNo", "CustomerRef", "CustomerName", "CustomerType", "CustomerTel",
                    "PaymentDate", "RecordDate", "VendorTranId", "ReceiptNo", "VendorCode", "Teller"
                };

                // Draw header background
                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawRectangle(XBrushes.LightGray, XUnit.FromPoint(x + i * cellWidth).Point, XUnit.FromPoint(y).Point,
                        XUnit.FromPoint(cellWidth).Point, XUnit.FromPoint(cellHeight).Point);
                    gfx.DrawString(headers[i], headerFont, XBrushes.Black,
                        new XRect(XUnit.FromPoint(x + i * cellWidth + 2).Point, XUnit.FromPoint(y + 2).Point,
                            XUnit.FromPoint(cellWidth - 4).Point, XUnit.FromPoint(cellHeight - 4).Point), XStringFormats.TopCenter);
                    gfx.DrawRectangle(XPens.Black, XUnit.FromPoint(x + i * cellWidth).Point, XUnit.FromPoint(y).Point,
                        XUnit.FromPoint(cellWidth).Point, XUnit.FromPoint(cellHeight).Point);
                }

                // Data Rows
                int rowIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (rowIndex % maxRowsPerPage == 0 && rowIndex > 0)
                    {
                        page = document.AddPage();
                        page.Orientation = PdfSharp.PageOrientation.Landscape;
                        gfx = XGraphics.FromPdfPage(page);
                        y = 20;

                        // Redraw headers on new page
                        for (int i = 0; i < headers.Length; i++)
                        {
                            gfx.DrawRectangle(XBrushes.LightGray, XUnit.FromPoint(x + i * cellWidth).Point, XUnit.FromPoint(y).Point,
                                XUnit.FromPoint(cellWidth).Point, XUnit.FromPoint(cellHeight).Point);
                            gfx.DrawString(headers[i], headerFont, XBrushes.Black,
                                new XRect(XUnit.FromPoint(x + i * cellWidth + 2).Point, XUnit.FromPoint(y + 2).Point,
                                    XUnit.FromPoint(cellWidth - 4).Point, XUnit.FromPoint(cellHeight - 4).Point), XStringFormats.TopCenter);
                            gfx.DrawRectangle(XPens.Black, XUnit.FromPoint(x + i * cellWidth).Point, XUnit.FromPoint(y).Point,
                                XUnit.FromPoint(cellWidth).Point, XUnit.FromPoint(cellHeight).Point);
                        }
                        y += cellHeight;
                    }

                    string[] values = new string[]
                    {
                        row["TranId"].ToString(),
                        row["TransNo"]?.ToString() ?? "",
                        row["CustomerRef"]?.ToString() ?? "",
                        row["CustomerName"]?.ToString() ?? "",
                        row["CustomerType"]?.ToString() ?? "",
                        row["CustomerTel"]?.ToString() ?? "",
                        row["PaymentDate"] is DateTime pd ? pd.ToString("yyyy-MM-dd HH:mm:ss") : "",
                        row["RecordDate"] is DateTime rd ? rd.ToString("yyyy-MM-dd HH:mm:ss") : "",
                        row["VendorTranId"]?.ToString() ?? "",
                        row["ReceiptNo"]?.ToString() ?? "",
                        row["VendorCode"]?.ToString() ?? "",
                        row["Teller"]?.ToString() ?? ""
                    };

                    y += cellHeight;
                    for (int i = 0; i < values.Length; i++)
                    {
                        // Split long text into multiple lines
                        string[] lines = WrapText(values[i], cellFont, cellWidth - 4, gfx);
                        double currentY = y + 2;
                        foreach (string line in lines)
                        {
                            gfx.DrawString(line, cellFont, XBrushes.Black,
                                new XRect(XUnit.FromPoint(x + i * cellWidth + 2).Point, XUnit.FromPoint(currentY).Point,
                                    XUnit.FromPoint(cellWidth - 4).Point, XUnit.FromPoint(cellHeight - 4).Point), XStringFormats.TopLeft);
                            currentY += 10; // Adjust line spacing
                        }
                        gfx.DrawRectangle(XPens.Black, XUnit.FromPoint(x + i * cellWidth).Point, XUnit.FromPoint(y).Point,
                            XUnit.FromPoint(cellWidth).Point, XUnit.FromPoint(cellHeight).Point);
                    }
                    rowIndex++;
                }

                // Save to MemoryStream
                document.Save(ms);
                document.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=ReceivedTransactions.pdf");
                Response.BinaryWrite(ms.ToArray());
                Response.End();
            }
        }

        private string[] WrapText(string text, XFont font, double maxWidth, XGraphics gfx)
        {
            if (string.IsNullOrEmpty(text)) return new[] { "" };

            List<string> lines = new List<string>();
            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                double width = gfx.MeasureString(testLine, font).Width;

                if (width <= maxWidth)
                {
                    currentLine = testLine;
                }
                else
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        lines.Add(currentLine);
                    }
                    currentLine = word;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            return lines.ToArray();
        }

        private void ExportToCsv(DataTable dt)
        {
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("TranId,TransNo,CustomerRef,CustomerName,CustomerType,CustomerTel,Area,Tin,TranAmount," +
                           "PaymentDate,RecordDate,TranType,PaymentType,VendorTranId,ReceiptNo,TranNarration,SmsSent,VendorCode," +
                           "Teller,Reversal,Cancelled,Offline,UtilityCode,UtilityTranRef,SentToUtility,RegionCode,DistrictCode,VendorToken," +
                           "ReconFileProcessed,Status,SentToVendor,UtilitySentDate,QueueTime,Reason");

            foreach (DataRow row in dt.Rows)
            {
                string[] values = new string[]
                {
                    row["TranId"].ToString(),
                    $"\"{EscapeCsv(row["TransNo"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["CustomerRef"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["CustomerName"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["CustomerType"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["CustomerTel"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["Area"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["Tin"]?.ToString())}\"",
                    row["TranAmount"].ToString(),
                    row["PaymentDate"] is DateTime pd ? pd.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    row["RecordDate"] is DateTime rd ? rd.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    $"\"{EscapeCsv(row["TranType"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["PaymentType"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["VendorTranId"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["ReceiptNo"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["TranNarration"]?.ToString())}\"",
                    row["SmsSent"].ToString(),
                    $"\"{EscapeCsv(row["VendorCode"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["Teller"]?.ToString())}\"",
                    row["Reversal"]?.ToString() ?? "",
                    row["Cancelled"]?.ToString() ?? "",
                    row["Offline"]?.ToString() ?? "",
                    $"\"{EscapeCsv(row["UtilityCode"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["UtilityTranRef"]?.ToString())}\"",
                    row["SentToUtility"].ToString(),
                    $"\"{EscapeCsv(row["RegionCode"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["DistrictCode"]?.ToString())}\"",
                    $"\"{EscapeCsv(row["VendorToken"]?.ToString())}\"",
                    row["ReconFileProcessed"]?.ToString() ?? "",
                    $"\"{EscapeCsv(row["Status"]?.ToString())}\"",
                    row["SentToVendor"]?.ToString() ?? "",
                    row["UtilitySentDate"] is DateTime usd ? usd.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    row["QueueTime"] is DateTime qt ? qt.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    $"\"{EscapeCsv(row["Reason"]?.ToString())}\""
                };
                csv.AppendLine(string.Join(",", values));
            }

            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=ReceivedTransactions.csv");
            Response.Write(csv.ToString());
            Response.End();
        }
        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Replace("\"", "\"\"");
        }
    }
}