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
        public bool Fulfilled;

        public LineItem(string[] splitRow)
        {
            OrderNumber = splitRow[0].Replace("#", "");
            string name = (splitRow[4].Length > 3) ? splitRow[4] : String.Format("{0} {1}", splitRow[2], splitRow[3]);
            string qty = (splitRow[30] == "partial") ? splitRow[31] : splitRow[18];
            Fulfilled = (splitRow[30].ToLower() == "fulfilled");
            LineProduct = new Product(splitRow[17], Int32.Parse(qty), splitRow[19], splitRow[20]);
            OrderAddress = new Address(name, splitRow[5], splitRow[6], splitRow[7], splitRow[8], splitRow[9], splitRow[10], splitRow[11]);
        }
    }
}
//NEW CSV COLUMNS:
//"Name"                        0
//"Created At"                  1
//"Customer: ID"                2
//"Customer: Email"             3
//"Customer: Phone"             4
//"Customer: First Name"        5
//"Customer: Last Name"         6
//"Shipping: First Name"        7
//"Shipping: Last Name"         8
//"Shipping: Name"              9
//"Shipping: Company"           10
//"Shipping: Phone"             11
//"Shipping: Address 1"         12
//"Shipping: Address 2"         13
//"Shipping: Zip"               14
//"Shipping: City"              15
//"Shipping: Province Code"     16
//"Shipping: Country"           17
//"Row #"                       18
//"Top Row"                     19
//"Line: Type"                  20
//"Line: ID"                    21
//"Line: Product ID"            22
//"Line: Title"                 23
//"Line: Name"                  24
//"Line: Variant ID"            25
//"Line: Variant Title"         26
//"Line: SKU"                   27
//"Line: Quantity"              28
//"Transaction: Processed At"   29
//MISSING:
//Line: Fulfillment status
//Line: Fulfillable Quantity