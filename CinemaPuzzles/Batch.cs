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
        List<Order[]> SortedBatches;

        public Batch(string csvPath)
        {
            if (!File.Exists(csvPath)) Logger.ErrorExit(["Unable to access CSV file."], 400);
            Orders = GenerateOrderArray(csvPath);
        }

        private Order[] GenerateOrderArray(string csvPath)
        {
            StreamReader sr = new StreamReader(csvPath);
            List<LineItem> lineItems = new List<LineItem>();
            sr.ReadLine();
            while(!sr.EndOfStream) lineItems.Add(new LineItem(sr.ReadLine()));
            sr.Close();
            return ParseOrders(lineItems);
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
    }
}
