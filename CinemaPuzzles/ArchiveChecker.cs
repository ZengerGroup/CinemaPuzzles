using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class ArchiveChecker
    {
        private List<Order> FilteredOrders;
        public ArchiveChecker(Order[] unfilteredOrders)
        {
            FilteredOrders = unfilteredOrders.ToList<Order>();
            CheckArchives();
        }
        public Order[] GetOrders()
        {
            return FilteredOrders.ToArray();
        }
        private void CheckArchives()
        {
            string[] archivedExports = Directory.GetFiles(Configurator.ArchivePath);
            for (int i = 0; i < archivedExports.Length; i++)
            {
                if (IsTodaysBatch(archivedExports[i])) continue;
                string[] orderNumbers = GetOrderNumbers(archivedExports[i]);
                FilterOrders(orderNumbers);
            }
        }
        private bool IsTodaysBatch(string exportPath)
        {
            string dateString = DateTime.Now.ToString("yyyy-MM-dd");
            return exportPath.Contains(dateString);
        }
        private string[] GetOrderNumbers(string exportPath)
        {
            List<string> orderList = new List<string>();
            string[] rows = File.ReadAllLines(exportPath);
            for (int i = 1; i < rows.Length; i++)
            {
                if (rows[i].Split(",").Length > 0)
                {
                    string orderNum = rows[i].Split(",")[0];
                    if(!orderList.Contains(orderNum))orderList.Add(orderNum.Replace("\"",""));
                }
                else continue;
            }
            return orderList.ToArray();
        }
        private void FilterOrders(string[] archivedOrders)
        {
            List<Order> ToRemove = new List<Order>();
            for(int i = 0; i < archivedOrders.Length; i++)
                for (int ii = 0; ii < FilteredOrders.Count; ii++)
                {
                    if (archivedOrders[i] == FilteredOrders[ii].OrderNumber)
                    {
                        ToRemove.Add(FilteredOrders[ii]);
                    }
                }
                    
            for (int i = 0; i < ToRemove.Count; i++) FilteredOrders.Remove(ToRemove[i]);
        }
    }
}
