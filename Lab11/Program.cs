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
                case "3":
                    SearchAthletes(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, count);
                    break;
                case "4":
                    EditAthlete(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, count);
                    break;
                case "5":
                    DeleteAthlete(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, ref count);
                    break;
                case "10":
                    AddExamples(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, ref count, ref nextId);
                    break;
                case "12":
                    ShowSummary(ids, birthYears, trainings, count);
                    break;
                case "6": case "7": case "8": case "9": case "11":
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
        Console.WriteLine("12 — Підсумки");
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
        Console.WriteLine("Розподіл за віковими групами (приблизний вік: поточний рік мінус рік народження):");
        Console.WriteLine($"До 12 років: {CountAgeGroup(birthYears, count, DateTime.Today.Year, 0, 11)}");
        Console.WriteLine($"12–17 років: {CountAgeGroup(birthYears, count, DateTime.Today.Year, 12, 17)}");
        Console.WriteLine($"18 років і старші: {CountAgeGroup(birthYears, count, DateTime.Today.Year, 18, int.MaxValue)}");
    }

    // Повертає індекс запису або -1, якщо номера немає.
    private static int FindIndexById(int[] ids, int count, int id)
    {
        for (int i = 0; i < count; i++)
        {
            if (ids[i] == id) return i;
        }
        return -1;
    }

    private static void SearchAthletes(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Сховище порожнє: шукати нічого.");
            return;
        }

        Console.WriteLine("1 — Пошук за частиною імені чи прізвища");
        Console.WriteLine("2 — Пошук за діапазоном років народження");
        Console.WriteLine("0 — Повернутися до меню");
        Console.Write("Ваш вибір: ");
        string? choice = Console.ReadLine()?.Trim();
        if (choice == null || choice == "0") return;

        if (choice == "1")
        {
            string? query;
            while (true)
            {
                Console.Write("Частина імені чи прізвища: ");
                query = Console.ReadLine();
                if (query == null) return;
                query = query.Trim();
                if (query.Length > 0) break;
                Console.WriteLine("Помилка: введіть непорожній текст.");
            }
            PrintSearchResults(ids, names, birthYears, sportIds, ranks, joinedDates, trainings,
                count, query, 0, 0, true);
        }
        else if (choice == "2")
        {
            if (!ReadInt("Рік народження від: ", 1900, DateTime.Today.Year, out int from) ||
                !ReadInt("Рік народження до: ", 1900, DateTime.Today.Year, out int to)) return;
            if (from > to)
            {
                Console.WriteLine("Помилка: нижня межа не може бути більшою за верхню.");
                return;
            }
            PrintSearchResults(ids, names, birthYears, sportIds, ranks, joinedDates, trainings,
                count, string.Empty, from, to, false);
        }
        else Console.WriteLine("Помилка: виберіть 1, 2 або 0.");
    }

    private static void PrintSearchResults(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, int count,
        string query, int from, int to, bool byName)
    {
        int nameWidth = "Прізвище та ім'я".Length;
        int rankWidth = "Розряд".Length;
        for (int i = 0; i < count; i++)
        {
            if (names[i].Length > nameWidth) nameWidth = names[i].Length;
            if (ranks[i].Length > rankWidth) rankWidth = ranks[i].Length;
        }
        int found = 0;
        for (int i = 0; i < count; i++)
        {
            // Contains у цій роботі чутливий до регістру.
            bool matches = byName ? names[i].Contains(query) : birthYears[i] >= from && birthYears[i] <= to;
            if (!matches) continue;
            if (found == 0)
                Console.WriteLine($"\n{"№",4}  {"Прізвище та ім'я".PadRight(nameWidth)}  {"Рік",5}  {"Спорт",10}  {"Розряд".PadRight(rankWidth)}  {"Вступ",10}  {"Трен./тиж.",10}");
            Console.WriteLine($"{ids[i],4}  {names[i].PadRight(nameWidth)}  {birthYears[i],5}  {sportIds[i],10}  {ranks[i].PadRight(rankWidth)}  {joinedDates[i]:dd.MM.yyyy}  {trainings[i],10}");
            found++;
        }
        if (found == 0) Console.WriteLine("Записів за цією умовою не знайдено.");
        else Console.WriteLine($"Знайдено записів: {found}.");
    }

    private static void EditAthlete(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Сховище порожнє: змінювати нічого.");
            return;
        }
        if (!ReadInt("Номер вихованця для зміни: ", 1, int.MaxValue, out int id)) return;
        int index = FindIndexById(ids, count, id);
        if (index == -1)
        {
            Console.WriteLine($"Вихованця №{id} не знайдено.");
            return;
        }

        Console.WriteLine("Нові значення (порожній Enter залишає поточне). Номер змінити не можна.");
        // Спочатку перевіряємо всі поля, щоб переривання вводу не змінило запис частково.
        if (!ReadOptionalText("Прізвище та ім'я", names[index], out string name) ||
            !ReadOptionalInt("Рік народження", birthYears[index], 1900, DateTime.Today.Year, out int year) ||
            !ReadOptionalInt("Код виду спорту", sportIds[index], 1, int.MaxValue, out int sportId) ||
            !ReadOptionalText("Спортивний розряд", ranks[index], out string rank) ||
            !ReadOptionalDate(joinedDates[index], year, out DateTime joined) ||
            !ReadOptionalInt("Тренувань на тиждень", trainings[index], 0, 28, out int trainingCount))
        {
            Console.WriteLine("Введення перервано; запис не змінено.");
            return;
        }
        names[index] = name;
        birthYears[index] = year;
        sportIds[index] = sportId;
        ranks[index] = rank;
        joinedDates[index] = joined;
        trainings[index] = trainingCount;
        Console.WriteLine($"Запис №{id} змінено.");
    }

    private static bool ReadOptionalText(string label, string current, out string value)
    {
        while (true)
        {
            Console.Write($"{label} [{current}]: ");
            string? input = Console.ReadLine();
            if (input == null) { value = current; return false; }
            input = input.Trim();
            if (input.Length == 0) { value = current; return true; }
            if (input.Length >= 2) { value = input; return true; }
            Console.WriteLine("Помилка: введіть щонайменше два символи або натисніть Enter.");
        }
    }

    private static bool ReadOptionalInt(string label, int current, int min, int max, out int value)
    {
        while (true)
        {
            Console.Write($"{label} [{current}]: ");
            string? input = Console.ReadLine();
            if (input == null) { value = current; return false; }
            input = input.Trim();
            if (input.Length == 0) { value = current; return true; }
            if (int.TryParse(input, out value) && value >= min && value <= max) return true;
            Console.WriteLine($"Помилка: введіть ціле число від {min} до {max} або натисніть Enter.");
        }
    }

    private static bool ReadOptionalDate(DateTime current, int birthYear, out DateTime value)
    {
        while (true)
        {
            Console.Write($"Дата вступу [{current:dd.MM.yyyy}]: ");
            string? input = Console.ReadLine();
            if (input == null) { value = current; return false; }
            input = input.Trim();
            if (input.Length == 0)
            {
                if (current.Year >= birthYear) { value = current; return true; }
                Console.WriteLine("Поточна дата вступу раніша за новий рік народження; введіть іншу дату.");
                continue;
            }
            if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out value) && value.Date <= DateTime.Today && value.Year >= birthYear) return true;
            Console.WriteLine("Помилка: введіть дійсну дату дд.мм.рррр, не пізніше сьогодні і не раніше року народження.");
        }
    }

    private static void DeleteAthlete(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, ref int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Сховище порожнє: видаляти нічого.");
            return;
        }
        if (!ReadInt("Номер вихованця для видалення: ", 1, int.MaxValue, out int id)) return;
        int index = FindIndexById(ids, count, id);
        if (index == -1)
        {
            Console.WriteLine($"Вихованця №{id} не знайдено.");
            return;
        }
        Console.Write($"Видалити вихованця №{id} ({names[index]})? (так/ні): ");
        if (!string.Equals(Console.ReadLine()?.Trim(), "так", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Видалення скасовано.");
            return;
        }
        // Усі поля кожного наступного запису зсуваємо на один спільний індекс вліво.
        for (int i = index; i < count - 1; i++)
        {
            ids[i] = ids[i + 1];
            names[i] = names[i + 1];
            birthYears[i] = birthYears[i + 1];
            sportIds[i] = sportIds[i + 1];
            ranks[i] = ranks[i + 1];
            joinedDates[i] = joinedDates[i + 1];
            trainings[i] = trainings[i + 1];
        }
        count--;
        Console.WriteLine($"Вихованця №{id} видалено. Залишилося записів: {count}.");
    }

    private static void ShowSummary(int[] ids, int[] birthYears, int[] trainings, int count)
    {
        if (!CalculateSummary(ids, birthYears, trainings, count, DateTime.Today.Year,
                out int recordCount, out int under12, out int age12To17, out int age18Plus,
                out int minimum, out int minimumId, out int maximum, out int maximumId, out double average))
        {
            Console.WriteLine("Сховище порожнє: підсумків поки немає.");
            return;
        }
        Console.WriteLine($"Кількість записів: {recordCount}.");
        Console.WriteLine("Розподіл за віковими групами (поточний рік мінус рік народження):");
        Console.WriteLine($"До 12 років: {under12}");
        Console.WriteLine($"12–17 років: {age12To17}");
        Console.WriteLine($"18 років і старші: {age18Plus}");
        Console.WriteLine($"Найменше тренувань на тиждень: {minimum}, номер вихованця: {minimumId}.");
        Console.WriteLine($"Найбільше тренувань на тиждень: {maximum}, номер вихованця: {maximumId}.");
        Console.WriteLine($"Середня кількість тренувань на тиждень: {average:0.##}.");
    }

    // Один прохід: усі результати повертаються через out, метод нічого не виводить.
    private static bool CalculateSummary(int[] ids, int[] birthYears, int[] trainings, int count, int currentYear,
        out int recordCount, out int under12, out int age12To17, out int age18Plus,
        out int minimum, out int minimumId, out int maximum, out int maximumId, out double average)
    {
        recordCount = count;
        under12 = age12To17 = age18Plus = 0;
        minimum = minimumId = maximum = maximumId = 0;
        average = 0;
        if (count == 0) return false;

        minimum = maximum = trainings[0];
        minimumId = maximumId = ids[0];
        long sum = 0;
        for (int i = 0; i < count; i++)
        {
            int age = currentYear - birthYears[i];
            if (age < 12) under12++;
            else if (age < 18) age12To17++;
            else age18Plus++;
            if (trainings[i] < minimum) { minimum = trainings[i]; minimumId = ids[i]; }
            if (trainings[i] > maximum) { maximum = trainings[i]; maximumId = ids[i]; }
            sum += trainings[i];
        }
        average = (double)sum / count;
        return true;
    }

    // Повертає кількість для однієї групи, нічого не виводячи на екран.
    private static int CountAgeGroup(int[] birthYears, int count, int currentYear, int minimumAge, int maximumAge)
    {
        int total = 0;
        for (int i = 0; i < count; i++)
        {
            int age = currentYear - birthYears[i];
            if (age >= minimumAge && age <= maximumAge) total++;
        }
        return total;
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
