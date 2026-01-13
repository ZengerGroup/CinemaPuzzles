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

namespace CinemaPuzzles
{
    internal class TravelerBuilder
    {
        public TravelerBuilder(Order[] batchOrders)
        {
            //Stuff and things
            PdfDocument document = new PdfDocument();
            PdfPage workingPage = new PdfPage();
            document.AddPage(workingPage);
            var gfx = XGraphics.FromPdfPage(document.Pages[0]);
            PrintHeader(gfx);
            int pos = 0;
            for(int  i = 0; i < batchOrders.Length; i++)
            {
                if(pos > 4)
                {
                    pos = 0;
                    workingPage = new PdfPage();
                    document.AddPage(workingPage);
                    gfx = XGraphics.FromPdfPage(document.Pages[^1]);
                    PrintHeader(gfx);
                }
                pos = PrintOrderSection(batchOrders[i], pos, gfx);
            }
            document.Save(@"C:\Code\TestingFS\output2.pdf");
            document.Close();
        }
        public TravelerBuilder(Order[] batchOrders, bool testing)
        {
            //Stuff and things
            PdfDocument document = new PdfDocument();
            PdfPage workingPage = new PdfPage();
            document.AddPage(workingPage);
            var gfx = XGraphics.FromPdfPage(document.Pages[0]);
            gfx.DrawLine(new XPen(XColors.Red, 2), 0, 150.0d, document.Pages[0].Width, 150.0d);
            gfx.DrawLine(new XPen(XColors.Red, 2), 0, 300.0d, document.Pages[0].Width, 300.0d);
            gfx.DrawLine(new XPen(XColors.Red, 2), 0, 450.0d, document.Pages[0].Width, 450.0d);
            gfx.DrawLine(new XPen(XColors.Red, 2), 0, 600.0d, document.Pages[0].Width, 600.0d);
            gfx.DrawLine(new XPen(XColors.Red, 2), 0, 750.0d, document.Pages[0].Width, 750.0d);
            PrintBarcode(gfx, 0, "123456");
            PrintBarcode(gfx, 1, "123456");
            PrintBarcode(gfx, 2, "123456");
            PrintBarcode(gfx, 3, "123456");
            PrintBarcode(gfx, 4, "123456");
            document.Save(@"C:\Code\TestingFS\output2.pdf");
            document.Close();
        }

        public TravelerBuilder(bool testing)
        {
            PdfDocument document = new PdfDocument();
            PdfPage workingPage = new PdfPage();
            document.AddPage(workingPage);
            var gfx = XGraphics.FromPdfPage(document.Pages[0]);
            gfx.DrawString("First Print", new XFont("Verdana", 12), XBrushes.Black, new XRect(100.0d, 100.0d, 100.0d, 0.0d));
            gfx.DrawString("Second Print", new XFont("Verdana", 12), XBrushes.Black, new XRect(350.0d, 350.0d, 100.0d, 0.0d));
            gfx.DrawString("Third Print", new XFont("Verdana", 12), XBrushes.Black, new XRect(500.0d, 100.0d, 100.0d, 0.0d));
            gfx.DrawString("Fourth Print", new XFont("Verdana", 12), XBrushes.Black, new XRect(100.0d, 500.0d, 100.0d, 0.0d));
            gfx.DrawString("Fifth Print", new XFont("Verdana", 12), XBrushes.Black, new XRect(50.0d, 775.0d, 100.0d, 0.0d));
            gfx.DrawLine(new XPen(XColors.Red, 2),0, 500.0d, document.Pages[0].Width, 500.0d);


            //PrintBarcode(document.Pages[0], 0.0d, true, "123456");
            XPoint position = new XPoint(10.0d, 100.0d);
            Code3of9Standard barcode = new Code3of9Standard("123456", new XSize(100, 50), CodeDirection.LeftToRight);
            barcode.TextLocation = TextLocation.Below;
            barcode.Text = "123456";
            gfx.DrawBarCode(barcode, XBrushes.Black, new XPoint(350.0d, 50.0d));

            document.Save(@"C:\Code\TestingFS\output1.pdf");
            document.Close();
        }

        private int PrintOrderSection(Order order, int pos, XGraphics gfx)
        {
            double[] yPos = [200.0d, 350.0d, 500.0d, 650.0d, 800.0d];
            PrintBarcode(gfx, pos, order.OrderNumber);
            PrintShippingAddress(gfx, pos, order.LineItems[0].OrderAddress.GenerateAddressBlock());
            pos = PrintProductList(gfx, pos, order.LineItems);
            gfx.DrawLine(new XPen(XColors.Black, 2), 10.0d, yPos[pos], 605.0d, yPos[pos]);
            return pos + 1;
        }
        private void PrintBarcode(XGraphics gfx, int pos, string orderNum)
        {
            double[] yPos = [ 100.0d, 250.0d, 400.0d, 550.0d, 700.0d];
            double xPos;
            if (pos % 2 == 0) xPos = 25.0d;
            else xPos = 485.0d;
            Code3of9Standard barcode = new Code3of9Standard(orderNum, new XSize(100, 50), CodeDirection.LeftToRight);
            barcode.TextLocation = TextLocation.Below;
            barcode.Text = orderNum;
            gfx.DrawBarCode(barcode, XBrushes.Black, new XPoint(xPos, yPos[pos]));
        }
        private int PrintProductList(XGraphics gfx, int pos, LineItem[] products)
        {
            double[] yPos = [70.0d, 220.0d, 370.0d, 520.0d, 670.0d];
            double xPos = 250.0d;
            for(int i = 0; i < products.Length; i++)
            {
                string orderLine = String.Format("{0} x {1}", products[i].LineProduct.FullSku, products[i].LineProduct.Quantity);
                gfx.DrawString(orderLine, new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, (yPos[pos] + i * 12.0d), 100.0d, 0.0d));
            }
            return (products.Length > 8) ? pos + 1 : pos;
        }
        private void PrintShippingAddress(XGraphics gfx, int pos, string[] addressBlock)
        {
            double[] yPos = [100.0d, 260.0d, 410.0d, 560.0d, 710.0d];
            double xPos;
            if (pos % 2 == 0) xPos = 450.0d;
            else xPos = 25.0d;
            for (int i = 0; i < addressBlock.Length; i++)
                gfx.DrawString(addressBlock[i], new XFont("Verdana", 12), XBrushes.Black, new XRect(xPos, (yPos[pos] + i * 12.0d), 100.0d, 0.0d));
        }
        private void PrintHeader(XGraphics gfx)
        {
            gfx.DrawString("Cinema Puzzles Batch", new XFont("Verdana", 18), XBrushes.Black, new XRect(50.0d, 25.0d, 100.0d, 0.0d));
            gfx.DrawString(DateTime.Now.ToString("d"), new XFont("Verdana", 18), XBrushes.Black, new XRect(500.0d, 25.0d, 100.0d, 0.0d));
            gfx.DrawLine(new XPen(XColors.Black, 2), 10.0d, 45.0d, 605.0d, 45.0d);
        }
    }
}
