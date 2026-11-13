using System.Globalization;
using System.Text;

internal static class Program
{
    private const int MaxTableDimension = 20;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        bool isRunning = true;
        while (isRunning)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    ShowAbout();
                    PauseAndClear();
                    break;
                case "2":
                    ShowTrainingCostTable();
                    PauseAndClear();
                    break;
                case "3":
                    ShowAthleteSummaryTable();
                    PauseAndClear();
                    break;
                case "0":
                    Console.WriteLine("Завершення програми.");
                    PauseAndClear();
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Помилка: виберіть пункт 1, 2, 3 або 0.");
                    PauseAndClear();
                    break;
            }
        }

        Console.WriteLine("Роботу програми завершено.");
    }

    private static void ShowMenu()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("       ОБЛІК СПОРТИВНОЇ СЕКЦІЇ");
        Console.WriteLine("========================================");
        Console.WriteLine("1 — Довідка про проєкт");
        Console.WriteLine("2 — Таблиця вартості тренувань");
        Console.WriteLine("3 — Зведена таблиця вихованців");
        Console.WriteLine("0 — Вихід");
        Console.Write("Ваш вибір: ");
    }

    private static void ShowAbout()
    {
        Console.WriteLine("\nДОВІДКА ПРО ПРОЄКТ");
        Console.WriteLine("Тема: Спортивна секція");
        Console.WriteLine("Головна сутність: вихованець спортивної секції.");
        Console.WriteLine("Поля сутності:");
        Console.WriteLine("  номер — int (призначає програма)");
        Console.WriteLine("  прізвище та ім'я — string");
        Console.WriteLine("  рік народження — int");
        Console.WriteLine("  код виду спорту — int (посилання на довідник)");
        Console.WriteLine("  спортивний розряд — string");
        Console.WriteLine("  дата вступу — DateTime");
        Console.WriteLine("  кількість тренувань на тиждень — int");
        Console.WriteLine("Довідник: види спорту.");
        Console.WriteLine("Підсумкова величина: розподіл вихованців за віковими групами.");
    }

    private static void ShowTrainingCostTable()
    {
        Console.WriteLine("\nТАБУЛЮВАННЯ ВАРТОСТІ ТРЕНУВАНЬ");
        Console.WriteLine("Вартість = тривалість тренування × ціна за годину.");
        Console.WriteLine("Програма покаже 10 рядків із кроком 0,5 години.");

        if (!ReadPositiveDouble("Введіть ціну одного тренування за годину (грн): ", out double hourlyRate))
        {
            return;
        }

        if (!ReadNonNegativeDouble("Введіть початкову тривалість тренування (год): ", out double firstDuration))
        {
            return;
        }

        const int RowCount = 10;
        const double Step = 0.5;
        Console.WriteLine("\nТривалість, год | Вартість, грн");
        Console.WriteLine("----------------|---------------");

        for (int row = 0; row < RowCount; row++)
        {
            double duration = firstDuration + row * Step;
            double cost = duration * hourlyRate;
            string durationText = duration.ToString("F2", CultureInfo.GetCultureInfo("uk-UA"));
            string costText = cost.ToString("F2", CultureInfo.GetCultureInfo("uk-UA"));
            Console.WriteLine($"{durationText,15} | {costText,13}");
        }

        Console.WriteLine($"\nПобудовано {RowCount} рядків. Дробовий крок таблиці: {Step:F1} год.");
    }

    private static void ShowAthleteSummaryTable()
    {
        Console.WriteLine("\nЗВЕДЕНА ТАБЛИЦЯ ВИХОВАНЦІВ");
        Console.WriteLine("Рядки — вікові групи, стовпці — види спорту.");
        Console.WriteLine("У клітинках введіть кількість вихованців; останній стовпець і нижній рядок міститимуть підсумки.");

        if (!ReadPositiveInt("Кількість вікових груп (рядків): ", out int rowCount))
        {
            return;
        }

        if (!ReadPositiveInt("Кількість видів спорту (стовпців): ", out int columnCount))
        {
            return;
        }

        string[] ageGroups = new string[rowCount];
        string[] sports = new string[columnCount];
        int[,] athleteCounts = new int[rowCount, columnCount];
        long[] rowTotals = new long[rowCount];
        long[] columnTotals = new long[columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            ageGroups[row] = ReadNonEmptyText($"Назва вікової групи {row + 1}: ");
        }

        for (int column = 0; column < columnCount; column++)
        {
            sports[column] = ReadNonEmptyText($"Назва виду спорту {column + 1}: ");
        }

        Console.WriteLine("\nВведіть кількість вихованців для кожної пари «вікова група — вид спорту».");
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if (!ReadNonNegativeInt($"{ageGroups[row]} / {sports[column]}: ", out int count))
                {
                    return;
                }

                athleteCounts[row, column] = count;
                rowTotals[row] += count;
                columnTotals[column] += count;
            }
        }

        long grandTotal = 0;
        for (int column = 0; column < columnCount; column++)
        {
            grandTotal += columnTotals[column];
        }

        int labelWidth = "Вікова група".Length;
        for (int row = 0; row < rowCount; row++)
        {
            if (ageGroups[row].Length > labelWidth)
            {
                labelWidth = ageGroups[row].Length;
            }
        }

        int[] columnWidths = new int[columnCount];
        for (int column = 0; column < columnCount; column++)
        {
            columnWidths[column] = sports[column].Length;
            for (int row = 0; row < rowCount; row++)
            {
                int valueWidth = athleteCounts[row, column].ToString(CultureInfo.InvariantCulture).Length;
                if (valueWidth > columnWidths[column])
                {
                    columnWidths[column] = valueWidth;
                }
            }

            int totalWidth = columnTotals[column].ToString(CultureInfo.InvariantCulture).Length;
            if (totalWidth > columnWidths[column])
            {
                columnWidths[column] = totalWidth;
            }

            if (columnWidths[column] < "Разом".Length)
            {
                columnWidths[column] = "Разом".Length;
            }
        }

        PrintCell("Вікова група", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            PrintCell(sports[column], columnWidths[column]);
        }

        Console.Write(" | ");
        int totalColumnWidth = "Разом".Length;
        for (int row = 0; row < rowCount; row++)
        {
            int valueWidth = rowTotals[row].ToString(CultureInfo.InvariantCulture).Length;
            if (valueWidth > totalColumnWidth)
            {
                totalColumnWidth = valueWidth;
            }
        }

        int grandTotalWidth = grandTotal.ToString(CultureInfo.InvariantCulture).Length;
        if (grandTotalWidth > totalColumnWidth)
        {
            totalColumnWidth = grandTotalWidth;
        }

        PrintCell("Разом", totalColumnWidth);
        Console.WriteLine();

        PrintSeparator(labelWidth, columnWidths, totalColumnWidth);

        for (int row = 0; row < rowCount; row++)
        {
            PrintCell(ageGroups[row], labelWidth);
            for (int column = 0; column < columnCount; column++)
            {
                Console.Write(" | ");
                PrintCell(athleteCounts[row, column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
            }

            Console.Write(" | ");
            PrintCell(rowTotals[row].ToString(CultureInfo.InvariantCulture), totalColumnWidth);
            Console.WriteLine();
        }

        PrintSeparator(labelWidth, columnWidths, totalColumnWidth);
        PrintCell("Разом", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            PrintCell(columnTotals[column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
        }

        Console.Write(" | ");
        PrintCell(grandTotal.ToString(CultureInfo.InvariantCulture), totalColumnWidth);
        Console.WriteLine();
    }

    private static void PrintCell(string value, int width)
    {
        Console.Write(value.PadRight(width));
    }

    private static void PrintSeparator(int labelWidth, int[] columnWidths, int totalColumnWidth)
    {
        Console.Write(new string('-', labelWidth));
        for (int column = 0; column < columnWidths.Length; column++)
        {
            Console.Write("-+-");
            Console.Write(new string('-', columnWidths[column]));
        }

        Console.Write("-+-");
        Console.WriteLine(new string('-', totalColumnWidth));
    }

    private static bool ReadPositiveInt(string prompt, out int value)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (int.TryParse(input, out value) && value > 0 && value <= MaxTableDimension)
            {
                return true;
            }

            Console.WriteLine($"Помилка: введіть ціле число від 1 до {MaxTableDimension}. Повернення до меню.");
            return false;
        }
    }

    private static bool ReadNonNegativeInt(string prompt, out int value)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (int.TryParse(input, out value) && value >= 0)
            {
                return true;
            }

            Console.WriteLine("Помилка: введіть ціле невід'ємне число. Повернення до меню.");
            return false;
        }
    }

    private static bool ReadPositiveDouble(string prompt, out double value)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim().Replace(',', '.');
            if (double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value)
                && value > 0 && !double.IsInfinity(value))
            {
                return true;
            }

            Console.WriteLine("Помилка: введіть додатне число (дробову частину можна відокремити комою або крапкою). Повернення до меню.");
            return false;
        }
    }

    private static bool ReadNonNegativeDouble(string prompt, out double value)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim().Replace(',', '.');
            if (double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value)
                && value >= 0 && !double.IsInfinity(value))
            {
                return true;
            }

            Console.WriteLine("Помилка: введіть невід'ємне число (дробову частину можна відокремити комою або крапкою). Повернення до меню.");
            return false;
        }
    }

    private static string ReadNonEmptyText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine()?.Trim() ?? string.Empty;
            if (value.Length > 0)
            {
                return value;
            }

            Console.WriteLine("Значення не може бути порожнім. Спробуйте ще раз.");
        }
    }

    private static void PauseAndClear()
    {
        Console.WriteLine("\nНатисніть Enter, щоб повернутися до меню...");
        Console.ReadLine();

        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Console.Clear може бути недоступним у перенаправленому виводі.
        }
        catch (InvalidOperationException)
        {
            // Деякі консолі Rider не підтримують очищення екрана.
        }
    }
}
