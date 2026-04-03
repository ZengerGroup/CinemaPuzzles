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
        public int Quantity;

        public Product(string sku, int qty)
        {
            FullSku = sku;
            string[] splitSku = sku.Split('_');
            ShortSku = splitSku[0];
            Pieces = splitSku[1];
            Size = splitSku[2];
            Quantity = qty;
        }
        public Product(Product p)
        {
            FullSku = p.FullSku;
            ShortSku = p.ShortSku;
            Pieces = p.Pieces;
            Size = p.Size;
            Quantity = p.Quantity;
        }
    }
}
