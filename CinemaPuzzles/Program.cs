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
            //Generate 'Traveler' type file
            //TravelerBuilder fooBuilder = new TravelerBuilder(false);
            TravelerBuilder barBuilder = new TravelerBuilder(DaysBatch.Orders);
            //Generate report csvs and send email.
            ReportBuilder repBuilder = new ReportBuilder(DaysBatch.Orders, DaysBatch.Products.ToArray() );
        }
    }
}
