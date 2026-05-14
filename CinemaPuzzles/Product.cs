using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class Product
    {
        public string FullSku;
        public string ShortSku;
        public string Pieces;
        public string Size;
        public string ItemTitle;
        public string ItemName;
        public int Quantity;

        public Product(string sku, int qty, string itemTitle, string itemName)
        {
            FullSku = sku;
            string[] splitSku = sku.Split('_');
            ShortSku = splitSku[0];
            Pieces = splitSku[1];
            Size = splitSku[2];
            Quantity = qty;
            ItemTitle = itemTitle;
            ItemName = itemName;
        }
        public Product(Product p)
        {
            FullSku = p.FullSku;
            ShortSku = p.ShortSku;
            Pieces = p.Pieces;
            Size = p.Size;
            Quantity = p.Quantity;
        }
        public Dictionary<string, string> PuzzleSize = new Dictionary<string, string>()
        {
            {"S", "20\" x 16\"" },
            {"s", "20\" x 16\"" },
            {"L", "27\" x 12\"" },
            {"l", "27\" x 12\"" },
            {"B", "24\" x 18\"" },
            {"b", "24\" x 18\"" }
        };
    }
}
