using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;

namespace CinemaPuzzles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //System Setup
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            GlobalFontSettings.FontResolver = new FailsafeFontResolver();
            //Ingest order CSV
            Batch DaysBatch = new Batch(args[0]);
            for(int i = 0; i < DaysBatch.Orders.Length; i++)
            {
                Console.WriteLine("Order #: {0}", DaysBatch.Orders[i].OrderNumber);
                for(int j = 0; j < DaysBatch.Orders[i].LineItems.Length; j++)
                {
                    Console.WriteLine(DaysBatch.Orders[i].LineItems[j].LineProduct.FullSku);
                }
            }
            //Generate 'Traveler' type file
            //TravelerBuilder fooBuilder = new TravelerBuilder(false);
            TravelerBuilder barBuilder = new TravelerBuilder(DaysBatch.Orders);
            //Generate report csvs and send email.
            Console.ReadLine();
        }
    }
}
