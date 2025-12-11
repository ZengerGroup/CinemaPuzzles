using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class LineItem
    {
        public Product LineProduct;
        public string OrderNumber;
        public Address OrderAddress;

        public LineItem(string orderLine) 
        {
            string[] splitLine = orderLine.Split(',');
            LineProduct = new Product(splitLine[2], Int32.Parse(splitLine[8]));
            OrderNumber = splitLine[1];
            OrderAddress = new Address(splitLine[0], splitLine[3], splitLine[4], splitLine[5], splitLine[6], splitLine[7]);
        }
    }
}

//  0 ,      1     ,  2 ,     3     ,   4     ,   5       ,  6    ,        7   ,  8
//NAME, ORDERNUMBER, SKU, ADD line 1, Add City, Add State, Add ZIP, Add country, QTY
