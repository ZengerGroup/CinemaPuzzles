using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class Batch
    {
        public Order[] Orders;
        public List<Product> Products;
        public List<Product> Refunds;
        List<Order[]> SortedBatches;
        List<string[]> ErrorRows;

        public Batch(string csvPath)
        {
            if (!File.Exists(csvPath)) Logger.ErrorExit(["Unable to access CSV file."], 400);
            Products = new List<Product>();
            ErrorRows = new List<string[]>();
            Orders = GetOrderArray(csvPath);
        }

        private Order[] GetOrderArray(string csvPath)
        {
            StreamReader sReader = new StreamReader(csvPath);
            List<LineItem> lines = new List<LineItem>();
            List<LineItem> refunds = new List<LineItem>();
            string[] headers = sReader.ReadLine().Split(",");
            if (headers.Length != 32) Logger.ErrorExit(["Header row not formatted properly.", headers.Length.ToString()], 399);
            Logger.WriteLog("Starting to read export.", false);
            while (!sReader.EndOfStream)
            {
                //string[] splitRow = sReader.ReadLine().Split(",");
                string[] splitRow = SplitRow(sReader.ReadLine());
                for (int i = 0; i < splitRow.Length; i++) splitRow[i] = splitRow[i].Replace("\"", "");
                if (splitRow.Length != 32 || !Int32.TryParse(splitRow[18], out _))
                {
                    Logger.WriteLog("Found bad row. {0} - {1}", false, splitRow[0], splitRow[13]);
                    ErrorRows.Add(splitRow);
                    continue;
                }
                if (splitRow[15] == "Line Item")
                {
                    if (splitRow[17].Split('_').Length != 3) continue;
                    Logger.WriteLog("Found line item", false);
                    LineItem lineItem = new LineItem(splitRow);
                    lines.Add(lineItem);
                    AddProduct(lineItem.LineProduct);
                }
                else if (splitRow[15] == "Refund Line")
                {
                    if (splitRow[18] == "" || splitRow[18] == "0") continue;
                    Logger.WriteLog("Found refund line", false);
                    LineItem refundItem = new LineItem(splitRow);
                    refunds.Add(refundItem);
                    AddProduct(refundItem.LineProduct);
                }
                Logger.WriteLog("Finished reading export.", false);
            }
            sReader.Close();
            return ParseOrders(ProcessRefunds(lines, refunds));
        }
        private Order[] ParseOrders(List<LineItem> unparsedLines)
        {
            List<Order> parsedOrders = new List<Order>();
            for(int i = 0; i < unparsedLines.Count; i++)
            {
                int matchedIndex = -1;
                for (int j = 0; j < parsedOrders.Count; j++) 
                { 
                    if (unparsedLines[i].OrderNumber == parsedOrders[j].OrderNumber) matchedIndex = j;
                }
                if (matchedIndex >= 0) parsedOrders[matchedIndex].AddLineItem(unparsedLines[i]);
                else parsedOrders.Add(new Order(unparsedLines[i]));
            }
            return parsedOrders.ToArray();
        }
        private void AddProduct(Product product)
        {
            bool matched = false;
            for(int i = 0; i < Products.Count; i++)
            {
                if (Products[i].FullSku == product.FullSku)
                {
                    matched = true;
                    Products[i].Quantity += product.Quantity;
                    if (Products[i].Quantity <= 0) Products.RemoveAt(i);
                    break;
                }
            }
            if (!matched) Products.Add(new Product(product));
        }
        private List<LineItem> ProcessRefunds(List<LineItem> lines, List<LineItem> refunds)
        {
            for(int i = 0; i < refunds.Count; i++)
            {
                for(int ii = 0; ii < lines.Count; ii++)
                {
                    if (refunds[i].LineId == lines[ii].LineId)
                    {
                        lines[ii].LineProduct.Quantity += refunds[i].LineProduct.Quantity;
                        if (lines[ii].LineProduct.Quantity <= 0) lines.RemoveAt(ii);
                    }
                }
            }
            return lines;
        }
        private string[] SplitRow(string row)
        {
            List<string> rowValues = new List<string>();
            bool started = false;
            int openedIndex = 0;
            for(int i = 0; i < row.Length; i++)
            {
                if (row[i] == '"')
                {
                    if (!started)
                    {
                        openedIndex = i;
                        started = true;
                    }
                    else
                    {
                        started = false;
                        rowValues.Add(row.Substring(openedIndex, i - openedIndex).Replace(",", ""));
                    }
                }
            }
            return rowValues.ToArray();
        }
    }
}
