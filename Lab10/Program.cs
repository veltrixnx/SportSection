using System.Globalization;
using System.Text;

internal static class Program
{
    private const int Capacity = 100;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int[] ids = new int[Capacity];
        string[] names = new string[Capacity];
        int[] birthYears = new int[Capacity];
        int[] sportIds = new int[Capacity];
        string[] ranks = new string[Capacity];
        DateTime[] joinedDates = new DateTime[Capacity];
        int[] trainings = new int[Capacity];
        int count = 0;
        int nextId = 1;

        while (true)
        {
            ShowMenu();
            string? choice = Console.ReadLine();
            if (choice == null || choice.Trim() == "0")
            {
                Console.WriteLine("Роботу програми завершено.");
                return;
            }

            switch (choice.Trim())
            {
                case "1":
                    ShowAthletes(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, count);
                    break;
                case "2":
                    AddAthlete(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, ref count, ref nextId);
                    break;
                case "10":
                    AddExamples(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, ref count, ref nextId);
                    break;
                case "3": case "4": case "5": case "6": case "7": case "8": case "9": case "11":
                    Console.WriteLine("Цей пункт поки не реалізовано.");
                    break;
                default:
                    Console.WriteLine("Помилка: виберіть наявний пункт меню.");
                    break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("\n=== СПОРТИВНА СЕКЦІЯ ===");
        Console.WriteLine("1 — Показати всіх вихованців");
        Console.WriteLine("2 — Додати вихованця");
        Console.WriteLine("3 — Знайти запис          4 — Змінити запис");
        Console.WriteLine("5 — Видалити запис        6 — Зведена таблиця");
        Console.WriteLine("7 — Зберегти у файл       8 — Завантажити з файлу");
        Console.WriteLine("9 — Експорт у CSV        10 — Додати 6 готових записів");
        Console.WriteLine("11 — Довідник видів спорту");
        Console.WriteLine("0 — Вихід");
        Console.Write("Ваш вибір: ");
    }

    private static void AddAthlete(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, ref int count, ref int nextId)
    {
        if (count >= Capacity)
        {
            Console.WriteLine("Сховище заповнене: більше записів додати не можна.");
            return;
        }

        Console.WriteLine("\nДОДАВАННЯ ВИХОВАНЦЯ");
        if (!ReadText("Прізвище та ім'я: ", out string name) ||
            !ReadInt("Рік народження: ", 1900, DateTime.Today.Year, out int birthYear) ||
            !ReadInt("Код виду спорту (додатне число): ", 1, int.MaxValue, out int sportId) ||
            !ReadText("Спортивний розряд: ", out string rank) ||
            !ReadDate("Дата вступу (дд.мм.рррр): ", birthYear, out DateTime joinedDate) ||
            !ReadInt("Кількість тренувань на тиждень (0–28): ", 0, 28, out int trainingCount))
        {
            Console.WriteLine("Введення перервано; запис не додано.");
            return;
        }

        StoreAthlete(ids, names, birthYears, sportIds, ranks, joinedDates, trainings,
            ref count, ref nextId, name, birthYear, sportId, rank, joinedDate, trainingCount);
        Console.WriteLine($"Додано вихованця №{ids[count - 1]}.");
    }

    private static void StoreAthlete(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, ref int count, ref int nextId,
        string name, int birthYear, int sportId, string rank, DateTime joinedDate, int trainingCount)
    {
        // Однаковий індекс у всіх масивах відповідає одному вихованцю.
        ids[count] = nextId;
        names[count] = name;
        birthYears[count] = birthYear;
        sportIds[count] = sportId;
        ranks[count] = rank;
        joinedDates[count] = joinedDate;
        trainings[count] = trainingCount;
        count++;
        nextId++;
    }

    private static void AddExamples(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, ref int count, ref int nextId)
    {
        const int exampleCount = 6;
        if (Capacity - count < exampleCount)
        {
            Console.WriteLine("Для шести готових записів недостатньо місця у сховищі.");
            return;
        }

        string[] exampleNames = { "Іван Петренко", "Марія Коваль", "Олег Мельник", "Анна Бойко", "Андрій Шевченко", "Софія Ткач" };
        int[] ages = { 9, 11, 13, 16, 19, 23 };
        int[] exampleSports = { 1, 2, 1, 3, 2, 3 };
        string[] exampleRanks = { "початковий", "III юнацький", "II юнацький", "I юнацький", "III дорослий", "II дорослий" };
        int[] exampleTrainings = { 2, 3, 3, 4, 4, 5 };
        int year = DateTime.Today.Year;
        for (int i = 0; i < exampleCount; i++)
        {
            StoreAthlete(ids, names, birthYears, sportIds, ranks, joinedDates, trainings,
                ref count, ref nextId, exampleNames[i], year - ages[i], exampleSports[i],
                exampleRanks[i], DateTime.Today.AddYears(-2), exampleTrainings[i]);
        }
        Console.WriteLine("Додано 6 готових записів.");
    }

    private static void ShowAthletes(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Записів поки немає.");
            return;
        }

        int nameWidth = "Прізвище та ім'я".Length;
        int rankWidth = "Розряд".Length;
        for (int i = 0; i < count; i++)
        {
            if (names[i].Length > nameWidth) nameWidth = names[i].Length;
            if (ranks[i].Length > rankWidth) rankWidth = ranks[i].Length;
        }

        Console.WriteLine($"\n{ "№",4}  {"Прізвище та ім'я".PadRight(nameWidth)}  {"Рік",5}  {"Спорт",10}  {"Розряд".PadRight(rankWidth)}  {"Вступ",10}  {"Трен./тиж.",10}");
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"{ids[i],4}  {names[i].PadRight(nameWidth)}  {birthYears[i],5}  {sportIds[i],10}  {ranks[i].PadRight(rankWidth)}  {joinedDates[i]:dd.MM.yyyy}  {trainings[i],10}");
        }
        Console.WriteLine($"Записів: {count}.");
    }


    private static bool ReadText(string prompt, out string value)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input == null) { value = string.Empty; return false; }
            value = input.Trim();
            if (value.Length >= 2) return true;
            Console.WriteLine("Помилка: введіть щонайменше два символи.");
        }
    }

    private static bool ReadInt(string prompt, int min, int max, out int value)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input == null) { value = 0; return false; }
            if (int.TryParse(input.Trim(), out value) && value >= min && value <= max) return true;
            Console.WriteLine($"Помилка: введіть ціле число від {min} до {max}.");
        }
    }

    private static bool ReadDate(string prompt, int birthYear, out DateTime value)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input == null) { value = default; return false; }
            if (DateTime.TryParseExact(input.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out value) && value.Date <= DateTime.Today && value.Year >= birthYear)
                return true;
            Console.WriteLine("Помилка: введіть дійсну дату дд.мм.рррр, не пізніше сьогодні і не раніше року народження.");
        }
    }
}
