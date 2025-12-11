using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class Order
    {
        //stuff and or things.
        public string OrderNumber;
        public LineItem[] LineItems;

        public Order(LineItem lineItem)
        {
            LineItems = new LineItem[1];
            LineItems[0] = lineItem;
            OrderNumber = lineItem.OrderNumber;
        }

        public void AddLineItem(LineItem lineItem)
        {
            LineItem[] lineItems = new LineItem[LineItems.Length + 1];
            lineItems[^1] = lineItem;
            LineItems = lineItems;
        }
    }
}
