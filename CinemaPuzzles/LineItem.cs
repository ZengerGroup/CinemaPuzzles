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

        public LineItem(string[] splitRow)
        {
            LineProduct = new Product(splitRow[17], Int32.Parse(splitRow[18]), splitRow[19], splitRow[20]);
            OrderNumber = splitRow[0].Replace("#", "");
            string name = (splitRow[4].Length > 3) ? splitRow[4] : String.Format("{0} {1}", splitRow[2], splitRow[3]);
            OrderAddress = new Address(name, splitRow[5], splitRow[6], splitRow[7], splitRow[8], splitRow[9], splitRow[10], splitRow[11]);
        }
    }
}
//CSV COLUMNS:
//"Name"                        0
//"Created At"                  1
//"Shipping: First Name"        2
//"Shipping: Last Name"         3
//"Shipping: Name"              4
//"Shipping: Company"           5
//"Shipping: Address 1"         6
//"Shipping: Address 2"         7
//"Shipping: City"              8
//"Shipping: Province Code"     9
//"Shipping: Zip"               10
//"Shipping: Country"           11
//"Shipping: Phone"             12
//"Row #"                       13
//"Top Row"                     14
//"Line: Type"                  15
//"Line: ID"                    16
//"Line: SKU"                   17
//"Line: Quantity"              18
//"Line: Title"                 19
//"Line: Name"                  20
//"Line: Variant ID"            21
//"Line: Variant Title"         22
//"Line: Product ID"            23
//"Customer: ID"                24
//"Customer: Email"             25
//"Customer: Phone"             26
//"Customer: First Name"        27
//"Customer: Last Name"         28
//"ID"                          29