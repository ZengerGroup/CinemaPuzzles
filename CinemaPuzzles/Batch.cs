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
        List<Order[]> SortedBatches;
        List<string[]> ErrorRows;

        public Batch(string csvPath)
        {
            if (!File.Exists(csvPath)) Logger.ErrorExit(["Unable to access CSV file."], 400);
            Products = new List<Product>();
            Orders = GetOrderArray(csvPath);
            ErrorRows = new List<string[]>();
        }

        private Order[] GetOrderArray(string csvPath)
        {
            StreamReader sReader = new StreamReader(csvPath);
            List<LineItem> lines = new List<LineItem>();
            string[] headers = sReader.ReadLine().Split(",");
            if (headers.Length != 32) Logger.ErrorExit(["Header row not formatted properly.", headers.Length.ToString()], 399);
            while (!sReader.EndOfStream)
            {
                string[] splitRow = sReader.ReadLine().Split(",");
                for (int i = 0; i < splitRow.Length; i++) splitRow[i] = splitRow[i].Replace("\"", "");

                if (splitRow[15] != "Line Item") continue;
                else if (splitRow.Length != 32 || !Int32.TryParse(splitRow[18], out _))
                {
                    Logger.WriteLog("Found bad row.", false);
                    ErrorRows.Add(splitRow);
                    continue;
                }
                LineItem lineItem = new LineItem(splitRow);
                lines.Add(lineItem);
                AddProduct(lineItem.LineProduct);
            }
            sReader.Close();
            return ParseOrders(lines);
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
                    break;
                }
            }
            if (!matched) Products.Add(new Product(product));
        }
    }
}
