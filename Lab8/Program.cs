using System.Globalization;
using System.Text;

internal static class Program
{
    private const int MaxTableDimension = 20;
    private const double TrainingRatePerHour = 250.0;
    private const double MaxTrainingHours = 12.0;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        bool isRunning = true;

        while (isRunning)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine();

            switch (choice)
            {
                case "1": ShowAbout(); Pause(); break;
                case "2": ShowTrainingCostTable(); Pause(); break;
                case "3": ShowAthleteSummaryTable(); Pause(); break;
                case "0": isRunning = false; break;
                default: Console.WriteLine("Немає такого пункту меню."); Pause(); break;
            }
        }

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
        Console.WriteLine("ДОВІДКА ПРО ПРОЄКТ");
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
    }
    
    private static void ShowTrainingCostTable()
    {
        Console.WriteLine("ТАБУЛЮВАННЯ ВАРТОСТІ ТРЕНУВАНЬ");
        Console.WriteLine($"Ціна за годину тренування в цьому прикладі: {TrainingRatePerHour:F2} грн.");

        double step = ReadFractionalStep("Дробовий крок у годинах (0,1–1,5): ", 0.1, 1.5);
        double maxStart = MaxTrainingHours - 7 * step;
        double start = ReadDouble($"Початкова тривалість (0–{FormatNumber(maxStart)} год): ", 0, maxStart);
        double minimumEnd = start + 7 * step;
        double end = ReadDouble($"Кінцева тривалість ({FormatNumber(minimumEnd)}–{FormatNumber(MaxTrainingHours)} год): ", minimumEnd, MaxTrainingHours);

        PrintTrainingCostTable(start, end, step);
    }

    private static double CalculateTrainingCost(double hours, double rate)
    {
        return hours * rate;
    }

    private static void PrintTrainingCostTable(double start, double end, double step)
    {
        Console.WriteLine("\nТривалість, год | Вартість, грн");
        Console.WriteLine("----------------|---------------");

        for (double hours = start; hours <= end + step * 0.0000001; hours += step)
        {
            double cost = CalculateTrainingCost(hours, TrainingRatePerHour);
            string hoursText = FormatNumber(hours);
            string costText = FormatNumber(cost);
            Console.WriteLine($"{hoursText,15} | {costText,13}");
        }
    }

    private static double ReadFractionalStep(string prompt, double min, double max)
    {
        while (true)
        {
            double step = ReadDouble(prompt, min, max);
            if (step != Math.Truncate(step))
            {
                return step;
            }

            Console.WriteLine("Крок має бути дробовим, наприклад 0,5. Спробуйте ще раз.");
        }
    }

    private static void ShowAthleteSummaryTable()
    {
        Console.WriteLine("ЗВЕДЕНА ТАБЛИЦЯ ВИХОВАНЦІВ");
        Console.WriteLine("Рядки — вікові групи, стовпці — види спорту.");

        int rowCount = ReadInt($"Кількість вікових груп (1–{MaxTableDimension}): ", 1, MaxTableDimension);
        int columnCount = ReadInt($"Кількість видів спорту (1–{MaxTableDimension}): ", 1, MaxTableDimension);

        string[] ageGroups = new string[rowCount];
        string[] sports = new string[columnCount];
        int[,] athleteCounts = new int[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            ageGroups[row] = ReadNonEmptyText($"Назва вікової групи {row + 1}: ");
        }

        for (int column = 0; column < columnCount; column++)
        {
            sports[column] = ReadNonEmptyText($"Назва виду спорту {column + 1}: ");
        }

        Console.WriteLine("Введіть кількість вихованців для кожної пари «вікова група — вид спорту».");
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                athleteCounts[row, column] = ReadInt($"{ageGroups[row]} / {sports[column]}: ", 0, int.MaxValue);
            }
        }

        PrintAthleteSummaryTable(ageGroups, sports, athleteCounts);
    }

    private static void PrintAthleteSummaryTable(string[] ageGroups, string[] sports, int[,] athleteCounts)
    {
        int rowCount = ageGroups.Length;
        int columnCount = sports.Length;
        long[] rowTotals = new long[rowCount];
        long[] columnTotals = new long[columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                rowTotals[row] += athleteCounts[row, column];
                columnTotals[column] += athleteCounts[row, column];
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
            labelWidth = Math.Max(labelWidth, ageGroups[row].Length);
        }

        int[] columnWidths = new int[columnCount];
        for (int column = 0; column < columnCount; column++)
        {
            columnWidths[column] = Math.Max(sports[column].Length, "Разом".Length);
            columnWidths[column] = Math.Max(columnWidths[column], columnTotals[column].ToString(CultureInfo.InvariantCulture).Length);

            for (int row = 0; row < rowCount; row++)
            {
                columnWidths[column] = Math.Max(
                    columnWidths[column],
                    athleteCounts[row, column].ToString(CultureInfo.InvariantCulture).Length);
            }
        }

        int totalWidth = "Разом".Length;
        for (int row = 0; row < rowCount; row++)
        {
            totalWidth = Math.Max(totalWidth, rowTotals[row].ToString(CultureInfo.InvariantCulture).Length);
        }

        totalWidth = Math.Max(totalWidth, grandTotal.ToString(CultureInfo.InvariantCulture).Length);

        PrintCell("Вікова група", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            PrintCell(sports[column], columnWidths[column]);
        }

        Console.Write(" | ");
        PrintCell("Разом", totalWidth);
        Console.WriteLine();
        PrintTableSeparator(labelWidth, columnWidths, totalWidth);

        for (int row = 0; row < rowCount; row++)
        {
            PrintCell(ageGroups[row], labelWidth);
            for (int column = 0; column < columnCount; column++)
            {
                Console.Write(" | ");
                PrintCell(athleteCounts[row, column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
            }

            Console.Write(" | ");
            PrintCell(rowTotals[row].ToString(CultureInfo.InvariantCulture), totalWidth);
            Console.WriteLine();
        }

        PrintTableSeparator(labelWidth, columnWidths, totalWidth);
        PrintCell("Разом", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            PrintCell(columnTotals[column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
        }

        Console.Write(" | ");
        PrintCell(grandTotal.ToString(CultureInfo.InvariantCulture), totalWidth);
        Console.WriteLine();
    }

    // Запитує число повторно, доки воно не потрапить у вказаний діапазон.
    private static double ReadDouble(string prompt, double min, double max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? string.Empty).Trim().Replace(',', '.');

            if (!double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double value)
                || double.IsNaN(value)
                || double.IsInfinity(value))
            {
                Console.WriteLine("Помилка: введене значення не є числом.");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Помилка: число має бути в межах від {FormatNumber(min)} до {FormatNumber(max)}.");
                continue;
            }

            return value;
        }
    }

    private static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(input, out int value))
            {
                Console.WriteLine("Помилка: введене значення не є цілим числом.");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Помилка: ціле число має бути в межах від {min} до {max}.");
                continue;
            }

            return value;
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

    private static void PrintCell(string value, int width)
    {
        Console.Write(value.PadRight(width));
    }

    private static void PrintTableSeparator(int labelWidth, int[] columnWidths, int totalWidth)
    {
        Console.Write(new string('-', labelWidth));
        for (int column = 0; column < columnWidths.Length; column++)
        {
            Console.Write("-+-");
            Console.Write(new string('-', columnWidths[column]));
        }

        Console.Write("-+-");
        Console.WriteLine(new string('-', totalWidth));
    }

    private static string FormatNumber(double value)
    {
        return value.ToString("0.##", CultureInfo.GetCultureInfo("uk-UA"));
    }

    private static void Pause()
    {
        Console.WriteLine("\nНатисніть Enter, щоб повернутися до меню...");
        Console.ReadLine();

        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Деякі консолі не підтримують очищення екрана.
        }
        catch (InvalidOperationException)
        {
            // Очищення недоступне для перенаправленого виводу.
        }
    }
}
