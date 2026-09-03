using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;

namespace CinemaPuzzles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger.WriteLog("Starting days batch.", true);
            if (args.Length != 2)
            {
                Logger.ErrorExit(["Missing arguments."], 10);
            }
            else Logger.JobNumber = args[1];
            //System Setup
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            GlobalFontSettings.FontResolver = new FailsafeFontResolver();
            //Ingest order CSV
            Batch DaysBatch = new Batch(args[0]);
            //Generate 'Traveler' type file
            //TravelerBuilder fooBuilder = new TravelerBuilder(false);
            TravelerBuilder TBuilder = new TravelerBuilder(DaysBatch.Orders, args[1]);
            //Generate report csvs and send email.
            ReportBuilder RBuilder = new ReportBuilder(DaysBatch.Orders, DaysBatch.Products.ToArray(), args[1]);
        }
    }
}
