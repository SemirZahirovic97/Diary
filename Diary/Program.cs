namespace Diary
{
    class Program
    {
        private static List<DiaryEntry> entries = new List<DiaryEntry>();
        private static Dictionary<DateTime, DiaryEntry> entriesByDateTime = new Dictionary<DateTime, DiaryEntry>();

        private const string FilePath = "Diary.json";
        static void Main(string[] args)
        {



            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Dagboksapp ===");
                Console.WriteLine("1) Skriv ny anteckning");
                Console.WriteLine("2) Lista alla anteckningar");
                Console.WriteLine("3) Sök anteckning på datum");
                Console.WriteLine("4) Redigera anteckning");
                Console.WriteLine("5) Ta bort anteckning");
                Console.WriteLine("6) Spara till fil");
                Console.WriteLine("7) Läs från fil");
                Console.WriteLine("0) Avsluta");
                Console.Write("Välj: ");

                string choice = Console.ReadLine();

            }
    }
}
