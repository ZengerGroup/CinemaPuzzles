namespace CinemaPuzzles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Ingest order CSV
            Batch DaysBatch = new Batch(args[0]);
            Console.WriteLine(DaysBatch.Orders.Length);
            //Parse the orders by type/size (100pc 20x16, 500pc 20x16, 100pc 27x12, 500pc 27x12, 100pc 24x18, 800pc 24x18) into subsets
            //foreach subset:
            //foreach Order object in the subset:
            //Generate 'Traveler' type file
            //Create a copy of all three pieces (puzzle, sleeve, insert) then name and mark those copies with the order and batch sequence #s, respectively
            //Combine all pieces from this subset by type.
            //Save Combined files in output folder ({dateCode}_{puzzleType}.pdf)
            //Generate report csvs and send email.
            Console.ReadLine();
        }
    }
}
