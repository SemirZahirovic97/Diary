using System.Text.Json;

namespace Diary
{
    class Program
    {
        private static List<DiaryEntry> entries = new List<DiaryEntry>();
        private static Dictionary<DateTime, DiaryEntry> entriesByDateTime = new Dictionary<DateTime, DiaryEntry>();

        private const string FilePath = "Diary.json";
        static void Main(string[] args)
        {

            LoadFromFile();

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

                switch (choice)
                {
                    case "1": AddEntry(); break;
                    case "2": ListEntries(); break;
                    case "3": SearchByDate(); break;
                    case "4": EditEntry(); break;
                    case "5": DeleteEntry(); break;
                    case "6": SaveToFile(); break;
                    case "7":
                        Console.WriteLine("Fil sökväg: " + Path.GetFullPath(FilePath));
                        int loaded = LoadFromFile();
                        if (File.Exists(FilePath))
                            Console.WriteLine($"Läst in {loaded} anteckning(ar) från fil.");
                        else
                            Console.WriteLine("Ingen fil hittades.");
                        Pause();
                        break;
                    case "0": return;
                    default:
                        Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att fortsätta...");
                        Console.ReadKey();
                        Pause();
                        break;

                }
            }
        }

        private static void AddEntry()
        {
            Console.Clear();
            Console.WriteLine("--- Ny anteckning ---");
            Console.WriteLine("Skriv din anteckning:");
            string text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Tom text är inte tillåten.");
                Pause();
                return;
            }

            var entry = new DiaryEntry { Date = DateTime.Now, Text = text.Trim() };

            entries.Add(entry);
            entriesByDateTime[entry.Date] = entry;

            Console.WriteLine("Anteckning sparad.");
            Pause();
        }

        private static void ListEntries()
        {
            Console.Clear();
            Console.WriteLine("--- Alla anteckningar ---");

            if (!entries.Any())
            {
                Console.WriteLine("Inga anteckningar sparade.");
            }
            else
            {
                var sorted = entries.OrderBy(e => e.Date).ToList();
                foreach (var e in sorted)
                {
                    Console.WriteLine($"{e.Date:yyyy-MM-dd HH:mm}: {e.Text}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Pause();
        }

        private static void SearchByDate()
        {
            Console.Clear();
            Console.WriteLine("--- Sök anteckning på datum ---");
            Console.Write("Ange datum (yyyy-mm-dd): ");
            string input = Console.ReadLine();

            if (!DateTime.TryParse(input, out DateTime date))
            {
                Console.WriteLine("Ogiltigt datum!");
                Pause();
                return;
            }

            var found = entries.Where(e => e.Date.Date == date.Date).OrderBy(e => e.Date).ToList();

            if (!found.Any())
            {
                Console.WriteLine("Ingen anteckning hittades.");
            }
            else
            {
                foreach (var e in found)
                {
                    Console.WriteLine($"{e.Date:yyyy-MM-dd HH:mm}: {e.Text}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            Pause();

        }

        private static void EditEntry()
        {
            Console.Clear();
            Console.WriteLine("--- Redigera anteckning ---");
            Console.Write("Ange datum (yyyy-mm-dd): ");
            string input = Console.ReadLine();

            if (!DateTime.TryParse(input, out DateTime date))
            {
                Console.WriteLine("Ogiltigt datum!");
                Pause();
                return;
            }

            var found = entries.Where(e => e.Date.Date == date.Date).OrderBy(e => e.Date).ToList();

            if (!found.Any())
            {
                Console.WriteLine("Ingen anteckning hittades.");
                Pause();
                return;
            }

            for (int i = 0; i < found.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {found[i].Date:HH:mm}: {found[i].Text}");
            }

            Console.Write("Ange nummer för vilken anteckning du vill redigera: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= found.Count)
            {
                var entry = found[choice - 1];
                Console.WriteLine("Ny text:");
                string newText = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newText))
                {
                    entry.Text = newText.Trim();
                    entriesByDateTime[entry.Date] = entry;
                    Console.WriteLine("Anteckningen uppdaterad.");
                }
                else
                {
                    Console.WriteLine("Tom text är inte tillåten.");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }

            Pause();
        }

        private static void DeleteEntry()
        {
            Console.Clear();
            Console.WriteLine("--- Ta bort anteckning ---");
            Console.Write("Ange datum (yyyy-mm-dd): ");
            string input = Console.ReadLine();

            if (!DateTime.TryParse(input, out DateTime date))
            {
                Console.WriteLine("Ogiltigt datum!");
                Pause();
                return;
            }

            var found = entries.Where(e => e.Date.Date == date.Date).OrderBy(e => e.Date).ToList();

            if (!found.Any())
            {
                Console.WriteLine("Ingen anteckning hittades.");
                Pause();
                return;
            }

            for (int i = 0; i < found.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {found[i].Date:HH:mm}: {found[i].Text}");
            }

            Console.Write("Ange nummer för anteckningen du vill ta bort: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= found.Count)
            {
                var entry = found[choice - 1];
                entries.Remove(entry);
                entriesByDateTime.Remove(entry.Date);
                Console.WriteLine("Anteckningen borttagen.");
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }
            Pause();

        }

        private static void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(FilePath, json);
                Console.WriteLine($"Sparat {entries.Count} anteckning(ar).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel vid sparande: " + ex.Message);
            }
            Pause();
        }

        private static int LoadFromFile()
        {
            if (!File.Exists(FilePath))
                return 0;

            try
            {
                string json = File.ReadAllText(FilePath);
                var loaded = JsonSerializer.Deserialize<List<DiaryEntry>>(json);

                entries.Clear();
                entriesByDateTime.Clear();

                if (loaded != null)
                {
                    foreach (var e in loaded)
                    {
                        entries.Add(e);
                        entriesByDateTime[e.Date] = e;
                    }
                }
                return entries.Count;
            }
            catch
            {
                Console.WriteLine("Fel vid inläsning av fil.");
                Pause();
                return 0;
            }
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }
}
