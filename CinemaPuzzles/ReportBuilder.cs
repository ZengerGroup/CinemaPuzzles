using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class ReportBuilder
    {
        public string JobNumber;
        public ReportBuilder(Order[] orders, Product[] products, string jobNumber)
        {
            JobNumber = jobNumber;
            BuildOrderReport(orders);
            BuildProductReport(products);
        }
        private void BuildOrderReport(Order[] orders)
        {
            string headers = "Order Number,Sku,Size,Quantity" + Environment.NewLine;
            string ReportPath = Path.Combine(Configurator.ReportPath, String.Format("{0}_CinemaPuzzleOrders_{1}.csv", JobNumber, DateTime.Now.ToString("MMddyy")));
            File.AppendAllText(ReportPath, headers);
            for(int i = 0; i < orders.Length; i++)
            {
                for(int j = 0; j < orders[i].LineItems.Length; j++)
                {
                    string size = PuzzleSize[orders[i].LineItems[j].LineProduct.Size];
                    string row = String.Format("{0},{1},{2},{3}", orders[i].LineItems[j].OrderNumber, orders[i].LineItems[j].LineProduct.ShortSku, size, orders[i].LineItems[j].LineProduct.Quantity);
                    File.AppendAllText(ReportPath, row + Environment.NewLine);
                }
            }
        }
        private void BuildProductReport(Product[] products)
        {
            string headers = "Sku,Quantity" + Environment.NewLine;
            string ReportPath = Path.Combine(Configurator.ReportPath, String.Format("{0}_CinemaPuzzleProducts_{1}.csv", JobNumber, DateTime.Now.ToString("MMddyy")));
            File.AppendAllText(ReportPath, headers);
            for (int i = 0; i < products.Length; i++)
                File.AppendAllText(ReportPath, String.Format("{0},{1}{2}", products[i].FullSku, products[i].Quantity, Environment.NewLine));
        }
        Dictionary<string, string> PuzzleSize = new Dictionary<string, string>()
        {
            {"S", "20 x 16" },
            {"s", "20 x 16" },
            {"L", "27 x 12" },
            {"l", "27 x 12" },
            {"B", "24 x 18" },
            {"b", "24 x 18" }
        };
    }
}
