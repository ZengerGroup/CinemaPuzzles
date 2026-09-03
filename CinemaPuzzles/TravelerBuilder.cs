using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PdfSharp;
using PdfSharp.Charting;
using PdfSharp.Drawing;
using PdfSharp.Drawing.BarCodes;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace CinemaPuzzles
{
    internal class TravelerBuilder
    {
        string JobNumber;
        //New constructor.
        public TravelerBuilder(Order[] orders, string jobNumber)
        {
            JobNumber = jobNumber;
            for(int i = 0; i < orders.Length; i++) if (!GenerateIndividualTraveler(orders[i])) Logger.WriteLog("Failed to generate traveler for order # {0}", false, orders[i].OrderNumber);
            if(!AssembleTravelers()) Logger.WriteLog("Failed to assemble pdfs.", false);
            if (!GenerateCoverSheet(orders)) Logger.WriteLog("Failed to generate cover sheet.", false);
        }
        //Top level functionality.
        private bool GenerateIndividualTraveler(Order order)
        {
            PdfDocument document = new PdfDocument();
            //6 line item sections per page.
            int pageCount = order.LineItems.Length / 6;
            if (order.LineItems.Length % 6 != 0) pageCount++;
            for (int page = 0; page < pageCount; page++) PrintPage(document, order, page, pageCount,
                order.LineItems[(page * 6)..(((page + 1) * 6 > order.LineItems.Length) ? order.LineItems.Length :  (page + 1) * 6)]);
            try
            {
                document.Save(Path.Combine(Configurator.TravelerAssembly, String.Format("{0}.pdf",order.OrderNumber)));
                document.Close();
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e.Message, false);
                Logger.GenerateIssueJson(JobNumber, String.Format("Failed to generate traveler: {0}", order.OrderNumber), "Error");
                return false;
            }
        }
        private bool AssembleTravelers()
        {
            string[] pdfPaths = Directory.GetFiles(Configurator.TravelerAssembly);
            PdfDocument AssembledOutput = new PdfDocument();
            try
            {
                for(int i = 0; i < pdfPaths.Length; i++)
                {
                    PdfDocument tempPdf = new PdfDocument();
                    tempPdf = PdfReader.Open(pdfPaths[i], PdfDocumentOpenMode.Import);
                    for (int page = 0; page < tempPdf.Pages.Count; page++) AssembledOutput.AddPage(tempPdf.Pages[page]);
                    if (tempPdf.PageCount % 2 != 0) AssembledOutput.AddPage(new PdfPage());
                    tempPdf.Close();
                }
                AssembledOutput.Save(Path.Combine(Configurator.TravelerOutput, String.Format("CinemaPuzzlesBatch_{0}.pdf", DateTime.Now.ToString("MMddyy"))));
                AssembledOutput.Close();
                for(int i = 0; i < pdfPaths.Length; i++) File.Delete(pdfPaths[i]);
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e.Message, false);
                Logger.GenerateIssueJson(JobNumber, "Failed to combine Travelers", "Error");
                return false;
            }
        }
        private bool GenerateCoverSheet(Order[] orders)
        {
            try
            {
                PdfDocument coverDocument = new PdfDocument();
                coverDocument.AddPage(new PdfPage());
                XGraphics graphics = XGraphics.FromPdfPage(coverDocument.Pages[^1]);
                PrintCoverHeader(graphics);
                PrintCoverData(graphics);
                PrintCoverSummary(graphics, orders);
                coverDocument.Save(Path.Combine(Configurator.TravelerOutput, String.Format("CinemaPuzzlesCover_{0}.pdf", DateTime.Now.ToString("MMddyy"))));
                return true;
            }
            catch
            {
                Logger.GenerateIssueJson(JobNumber, "Failed to generate cover sheet.", "Warning");
                return false;
            }
        }
        //Traveler draw functions
        private void PrintPage(PdfDocument document, Order order, int pageNumber, int pageCount, LineItem[] lineItems)
        {
            document.AddPage(new PdfPage());
            XGraphics graphics = XGraphics.FromPdfPage(document.Pages[^1]);
            PrintHeader(graphics, order, pageNumber, pageCount);
            for (int i = 0; i < lineItems.Length; i++) PrintLineItem(graphics, i, lineItems[i]);
        }
        private void PrintHeader(XGraphics graphics, Order order, int pageNumber, int pageCount)
        {
            graphics.DrawString(String.Format("Job # {0}", JobNumber), new XFont("Verdana", 18), XBrushes.Black, new XRect(25.0d, 45.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString("Cinema Puzzles Batch", new XFont("Verdana", 18), XBrushes.Black, new XRect(25.0d, 70.0d, 160.0d, 0.0d));
            graphics.DrawString(DateTime.Now.ToString("d"), new XFont("Verdana", 18), XBrushes.Black, new XRect(25.0d, 90.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(String.Format("Page {0} of {1}", pageNumber + 1, pageCount), new XFont("Verdana", 18), XBrushes.Black, new XRect(390.0d, 30.0d, 100.0d, 0.0d));
            PrintShippingAddress(graphics, order.LineItems[0].OrderAddress.GenerateAddressBlock());
            PrintOrderBarcode(graphics, order.OrderNumber);
            graphics.DrawLine(new XPen(XColors.Black, 2), 10.0d, 125.0d, 605.0d, 125.0d);
        }
        private void PrintOrderBarcode(XGraphics graphics, string orderNumber)
        {
            Code3of9Standard barcode = new Code3of9Standard(orderNumber.Replace("#", "/C"), new XSize(100, 50), CodeDirection.LeftToRight);
            graphics.DrawString(String.Format("Order {0}", orderNumber), new XFont("Verdana", 18), XBrushes.Black, new XRect(250.0d, 20.0d, 100.0d, 0.0d), XStringFormats.Center);
            graphics.DrawBarCode(barcode, XBrushes.Black, new XPoint(250.0d, 40.0d));
        }
        private void PrintShippingAddress(XGraphics graphics, string[] addressBlock)
        {
            for (int i = 0; i < addressBlock.Length; i++)
                graphics.DrawString(addressBlock[i], new XFont("Verdana", 12), XBrushes.Black, new XRect(390.0d, (50.0d + i * 12.0d), 100.0d, 0.0d));
        }
        private void PrintLineItem(XGraphics graphics, int itemNumber, LineItem item)
        {
            double startPosition = 125.0d + (itemNumber * 100.0d);
            int leftRight = itemNumber % 2;
            if (item.Fulfilled) PrintFulfilled(graphics, item.LineProduct.Quantity, item.LineProduct.FullSku, (leftRight == 0) ? 30.0d : 390.0d, startPosition + 10.0d);
            else PrintSkuBarcode(graphics, item.LineProduct.FullSku, (leftRight == 0) ? 30.0d : 390.0d,  startPosition + 10.0d);
            PrintLineQuantity(graphics, item.LineProduct.Quantity, startPosition, item.Fulfilled);
            PrintLineSummary(graphics, item.LineProduct.FullSku, item.LineProduct.ItemTitle, item.LineProduct.ItemName, item.LineProduct.PuzzleSize[item.LineProduct.Size], 
                (leftRight == 0) ? 390.0d : 30.0d, startPosition);
            graphics.DrawLine(new XPen(XColors.Black, 2), 10.0d, startPosition + 100.0d, 605.0d, startPosition + 100.0d);
        }
        private void PrintFulfilled(XGraphics graphics, int quantity, string sku, double xPos, double yPos)
        {
            graphics.DrawString("FULLFILLED", new XFont("Verdana", 32), XBrushes.Black, new XRect(xPos, yPos + 10.0d, 150.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(String.Format("QTY: {0}", quantity.ToString()), new XFont("Verdana", 24), XBrushes.Black, new XRect(xPos, yPos + 40.0d, 150.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(sku, new XFont("Verdana", 10), XBrushes.Black, new XRect(xPos, yPos + 65.0d, 100.0d, 0.0d), XStringFormats.Center);
        }
        private void PrintSkuBarcode(XGraphics graphics, string sku, double xPos, double yPos)
        {
            sku = sku.ToUpper();
            string SKU = sku.Replace("_", "%O");
            Code3of9Standard barcode = new Code3of9Standard(SKU, new XSize(200, 50), CodeDirection.LeftToRight);
            graphics.DrawBarCode(barcode, XBrushes.Black, new XPoint(xPos, yPos + 15.0d));
            graphics.DrawString(sku, new XFont("Verdana", 10), XBrushes.Black, new XRect(xPos, yPos + 75.0d, 100.0d, 0.0d), XStringFormats.Center);
        }
        private void PrintLineQuantity(XGraphics graphics, int quantity, double startPosition, bool fulfilled)
        {
            graphics.DrawString("Quantity:", new XFont("Verdana", 12), XBrushes.Black, new XRect(250.0d, startPosition + 25.0d, 100.0d, 0.0d), XStringFormats.Center);
            if(fulfilled) graphics.DrawString("0", new XFont("Verdana", 24), XBrushes.Black, new XRect(250.0d, startPosition + 60.0d, 100.0d, 0.0d), XStringFormats.Center);
            else graphics.DrawString(quantity.ToString(), new XFont("Verdana", 24), XBrushes.Black, new XRect(250.0d, startPosition + 60.0d, 100.0d, 0.0d), XStringFormats.Center);
        }
        private void PrintLineSummary(XGraphics graphics, string sku, string itemTitle, string itemName, string sizeString, double xPos, double yPos)
        {
            Logger.WriteLog("Printing summary", false);
            Logger.WriteLog("{0} - {1} - {2} - {3}", false, sku, itemTitle, itemName, sizeString);
            graphics.DrawString(sku, new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, yPos + 30.0d, 100.0d, 0.0d));
            graphics.DrawString(itemTitle, new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, yPos + 45.0d, 100.0d, 0.0d));
            if(itemTitle == itemName) graphics.DrawString(sizeString, new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, yPos + 60.0d, 100.0d, 0.0d));
            else
            {
                graphics.DrawString(itemName, new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, yPos + 60.0d, 100.0d, 0.0d));
                graphics.DrawString(sizeString, new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, yPos + 75.0d, 100.0d, 0.0d));
            }
            
        }
        //Cover sheet draw functions.
        private void PrintCoverHeader(XGraphics graphics) 
        {
            graphics.DrawString(String.Format("Job # {0}", JobNumber), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 50.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString("Cinema Puzzles Batch", new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 80.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(DateTime.Now.ToString("d"), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 110.0d, 160.0d, 0.0d), XStringFormats.Center);
        }
        private void PrintCoverData(XGraphics graphics)
        {
            graphics.DrawString(String.Format("Batch_Puzzles_{0}", DateTime.Now.ToString("MMddyy")), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 250.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(String.Format("Batch_Posters_{0}", DateTime.Now.ToString("MMddyy")), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 280.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(String.Format("Batch_Sleeves_{0}", DateTime.Now.ToString("MMddyy")), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 310.0d, 160.0d, 0.0d), XStringFormats.Center);
        }
        private void PrintCoverSummary(XGraphics graphics, Order[] orders)
        {
            int puzzleCount = GetPuzzleCount(orders);
            graphics.DrawString(String.Format("Total Orders: {0}", orders.Length), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 350.0d, 160.0d, 0.0d), XStringFormats.Center);
            graphics.DrawString(String.Format("Total Puzzles: {0}", puzzleCount), new XFont("Verdana", 32), XBrushes.Black, new XRect(250.0d, 380.0d, 160.0d, 0.0d), XStringFormats.Center);
        }
        private int GetPuzzleCount(Order[] orders)
        {
            int count = 0;
            for (int i = 0; i < orders.Length; i++) for (int ii = 0; ii < orders[i].LineItems.Length; ii++) count += orders[i].LineItems[ii].LineProduct.Quantity;
            return count;
        }
    }
}
