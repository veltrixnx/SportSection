using System.Globalization;
using System.Text;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            Console.WriteLine("\n=== СПОРТИВНА СЕКЦІЯ ===");
            Console.WriteLine("1 — Основна програма");
            Console.WriteLine("2 — Довідка і таблиці (лабораторна 7)");
            Console.WriteLine("3 — Довідка і таблиці з методами (лабораторна 8)");
            Console.WriteLine("4 — Вправи з масивом (лабораторна 9)");
            Console.WriteLine("0 — Вихід");
            Console.Write("Ваш вибір: ");
            string? choice = Console.ReadLine();
            if (choice == null || choice.Trim() == "0") return;
            switch (choice.Trim())
            {
                case "1": Run(); break;
                case "2": Lab7_Run(); break;
                case "3": Lab8_Run(); break;
                case "4": Lab9_Run(); break;
                default: Console.WriteLine("Помилка: виберіть наявний пункт меню."); break;
            }
        }
    }


    private const int Lab7_MaxTableDimension = 20;

    public static void Lab7_Run()
    {
        Console.OutputEncoding = Encoding.UTF8;

        bool isRunning = true;
        while (isRunning)
        {
            Lab7_ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    Lab7_ShowAbout();
                    Lab7_PauseAndClear();
                    break;
                case "2":
                    Lab7_ShowTrainingCostTable();
                    Lab7_PauseAndClear();
                    break;
                case "3":
                    Lab7_ShowAthleteSummaryTable();
                    Lab7_PauseAndClear();
                    break;
                case "0":
                    Console.WriteLine("Завершення програми.");
                    Lab7_PauseAndClear();
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Помилка: виберіть пункт 1, 2, 3 або 0.");
                    Lab7_PauseAndClear();
                    break;
            }
        }

        Console.WriteLine("Роботу програми завершено.");
    }

    private static void Lab7_ShowMenu()
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

    private static void Lab7_ShowAbout()
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

    private static void Lab7_ShowTrainingCostTable()
    {
        Console.WriteLine("\nТАБУЛЮВАННЯ ВАРТОСТІ ТРЕНУВАНЬ");
        Console.WriteLine("Вартість = тривалість тренування × ціна за годину.");
        Console.WriteLine("Програма покаже 10 рядків із кроком 0,5 години.");

        if (!Lab7_ReadPositiveDouble("Введіть ціну одного тренування за годину (грн): ", out double hourlyRate))
        {
            return;
        }

        if (!Lab7_ReadNonNegativeDouble("Введіть початкову тривалість тренування (год): ", out double firstDuration))
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

    private static void Lab7_ShowAthleteSummaryTable()
    {
        Console.WriteLine("\nЗВЕДЕНА ТАБЛИЦЯ ВИХОВАНЦІВ");
        Console.WriteLine("Рядки — вікові групи, стовпці — види спорту.");
        Console.WriteLine("У клітинках введіть кількість вихованців; останній стовпець і нижній рядок міститимуть підсумки.");

        if (!Lab7_ReadPositiveInt("Кількість вікових груп (рядків): ", out int rowCount))
        {
            return;
        }

        if (!Lab7_ReadPositiveInt("Кількість видів спорту (стовпців): ", out int columnCount))
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
            ageGroups[row] = Lab7_ReadNonEmptyText($"Назва вікової групи {row + 1}: ");
        }

        for (int column = 0; column < columnCount; column++)
        {
            sports[column] = Lab7_ReadNonEmptyText($"Назва виду спорту {column + 1}: ");
        }

        Console.WriteLine("\nВведіть кількість вихованців для кожної пари «вікова група — вид спорту».");
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if (!Lab7_ReadNonNegativeInt($"{ageGroups[row]} / {sports[column]}: ", out int count))
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

        Lab7_PrintCell("Вікова група", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            Lab7_PrintCell(sports[column], columnWidths[column]);
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

        Lab7_PrintCell("Разом", totalColumnWidth);
        Console.WriteLine();

        Lab7_PrintSeparator(labelWidth, columnWidths, totalColumnWidth);

        for (int row = 0; row < rowCount; row++)
        {
            Lab7_PrintCell(ageGroups[row], labelWidth);
            for (int column = 0; column < columnCount; column++)
            {
                Console.Write(" | ");
                Lab7_PrintCell(athleteCounts[row, column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
            }

            Console.Write(" | ");
            Lab7_PrintCell(rowTotals[row].ToString(CultureInfo.InvariantCulture), totalColumnWidth);
            Console.WriteLine();
        }

        Lab7_PrintSeparator(labelWidth, columnWidths, totalColumnWidth);
        Lab7_PrintCell("Разом", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            Lab7_PrintCell(columnTotals[column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
        }

        Console.Write(" | ");
        Lab7_PrintCell(grandTotal.ToString(CultureInfo.InvariantCulture), totalColumnWidth);
        Console.WriteLine();
    }

    private static void Lab7_PrintCell(string value, int width)
    {
        Console.Write(value.PadRight(width));
    }

    private static void Lab7_PrintSeparator(int labelWidth, int[] columnWidths, int totalColumnWidth)
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

    private static bool Lab7_ReadPositiveInt(string prompt, out int value)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim() ?? string.Empty;
            if (int.TryParse(input, out value) && value > 0 && value <= Lab7_MaxTableDimension)
            {
                return true;
            }

            Console.WriteLine($"Помилка: введіть ціле число від 1 до {Lab7_MaxTableDimension}. Повернення до меню.");
            return false;
        }
    }

    private static bool Lab7_ReadNonNegativeInt(string prompt, out int value)
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

    private static bool Lab7_ReadPositiveDouble(string prompt, out double value)
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

    private static bool Lab7_ReadNonNegativeDouble(string prompt, out double value)
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

    private static string Lab7_ReadNonEmptyText(string prompt)
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

    private static void Lab7_PauseAndClear()
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


    private const int Lab8_MaxTableDimension = 20;
    private const double Lab8_TrainingRatePerHour = 250.0;
    private const double Lab8_MaxTrainingHours = 12.0;

    public static void Lab8_Run()
    {
        Console.OutputEncoding = Encoding.UTF8;
        bool isRunning = true;

        while (isRunning)
        {
            Lab8_ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine();

            switch (choice)
            {
                case "1": Lab8_ShowAbout(); Lab8_Pause(); break;
                case "2": Lab8_ShowTrainingCostTable(); Lab8_Pause(); break;
                case "3": Lab8_ShowAthleteSummaryTable(); Lab8_Pause(); break;
                case "0": isRunning = false; break;
                default: Console.WriteLine("Немає такого пункту меню."); Lab8_Pause(); break;
            }
        }

    }

    private static void Lab8_ShowMenu()
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

    private static void Lab8_ShowAbout()
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
    
    private static void Lab8_ShowTrainingCostTable()
    {
        Console.WriteLine("ТАБУЛЮВАННЯ ВАРТОСТІ ТРЕНУВАНЬ");
        Console.WriteLine($"Ціна за годину тренування в цьому прикладі: {Lab8_TrainingRatePerHour:F2} грн.");

        double step = Lab8_ReadFractionalStep("Дробовий крок у годинах (0,1–1,5): ", 0.1, 1.5);
        double maxStart = Lab8_MaxTrainingHours - 7 * step;
        double start = Lab8_ReadDouble($"Початкова тривалість (0–{Lab8_FormatNumber(maxStart)} год): ", 0, maxStart);
        double minimumEnd = start + 7 * step;
        double end = Lab8_ReadDouble($"Кінцева тривалість ({Lab8_FormatNumber(minimumEnd)}–{Lab8_FormatNumber(Lab8_MaxTrainingHours)} год): ", minimumEnd, Lab8_MaxTrainingHours);

        Lab8_PrintTrainingCostTable(start, end, step);
    }

    private static double Lab8_CalculateTrainingCost(double hours, double rate)
    {
        return hours * rate;
    }

    private static void Lab8_PrintTrainingCostTable(double start, double end, double step)
    {
        Console.WriteLine("\nТривалість, год | Вартість, грн");
        Console.WriteLine("----------------|---------------");

        for (double hours = start; hours <= end + step * 0.0000001; hours += step)
        {
            double cost = Lab8_CalculateTrainingCost(hours, Lab8_TrainingRatePerHour);
            string hoursText = Lab8_FormatNumber(hours);
            string costText = Lab8_FormatNumber(cost);
            Console.WriteLine($"{hoursText,15} | {costText,13}");
        }
    }

    private static double Lab8_ReadFractionalStep(string prompt, double min, double max)
    {
        while (true)
        {
            double step = Lab8_ReadDouble(prompt, min, max);
            if (step != Math.Truncate(step))
            {
                return step;
            }

            Console.WriteLine("Крок має бути дробовим, наприклад 0,5. Спробуйте ще раз.");
        }
    }

    private static void Lab8_ShowAthleteSummaryTable()
    {
        Console.WriteLine("ЗВЕДЕНА ТАБЛИЦЯ ВИХОВАНЦІВ");
        Console.WriteLine("Рядки — вікові групи, стовпці — види спорту.");

        int rowCount = Lab8_ReadInt($"Кількість вікових груп (1–{Lab8_MaxTableDimension}): ", 1, Lab8_MaxTableDimension);
        int columnCount = Lab8_ReadInt($"Кількість видів спорту (1–{Lab8_MaxTableDimension}): ", 1, Lab8_MaxTableDimension);

        string[] ageGroups = new string[rowCount];
        string[] sports = new string[columnCount];
        int[,] athleteCounts = new int[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            ageGroups[row] = Lab8_ReadNonEmptyText($"Назва вікової групи {row + 1}: ");
        }

        for (int column = 0; column < columnCount; column++)
        {
            sports[column] = Lab8_ReadNonEmptyText($"Назва виду спорту {column + 1}: ");
        }

        Console.WriteLine("Введіть кількість вихованців для кожної пари «вікова група — вид спорту».");
        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                athleteCounts[row, column] = Lab8_ReadInt($"{ageGroups[row]} / {sports[column]}: ", 0, int.MaxValue);
            }
        }

        Lab8_PrintAthleteSummaryTable(ageGroups, sports, athleteCounts);
    }

    private static void Lab8_PrintAthleteSummaryTable(string[] ageGroups, string[] sports, int[,] athleteCounts)
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

        Lab8_PrintCell("Вікова група", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            Lab8_PrintCell(sports[column], columnWidths[column]);
        }

        Console.Write(" | ");
        Lab8_PrintCell("Разом", totalWidth);
        Console.WriteLine();
        Lab8_PrintTableSeparator(labelWidth, columnWidths, totalWidth);

        for (int row = 0; row < rowCount; row++)
        {
            Lab8_PrintCell(ageGroups[row], labelWidth);
            for (int column = 0; column < columnCount; column++)
            {
                Console.Write(" | ");
                Lab8_PrintCell(athleteCounts[row, column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
            }

            Console.Write(" | ");
            Lab8_PrintCell(rowTotals[row].ToString(CultureInfo.InvariantCulture), totalWidth);
            Console.WriteLine();
        }

        Lab8_PrintTableSeparator(labelWidth, columnWidths, totalWidth);
        Lab8_PrintCell("Разом", labelWidth);
        for (int column = 0; column < columnCount; column++)
        {
            Console.Write(" | ");
            Lab8_PrintCell(columnTotals[column].ToString(CultureInfo.InvariantCulture), columnWidths[column]);
        }

        Console.Write(" | ");
        Lab8_PrintCell(grandTotal.ToString(CultureInfo.InvariantCulture), totalWidth);
        Console.WriteLine();
    }

    // Запитує число повторно, доки воно не потрапить у вказаний діапазон.
    private static double Lab8_ReadDouble(string prompt, double min, double max)
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
                Console.WriteLine($"Помилка: число має бути в межах від {Lab8_FormatNumber(min)} до {Lab8_FormatNumber(max)}.");
                continue;
            }

            return value;
        }
    }

    private static int Lab8_ReadInt(string prompt, int min, int max)
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

    private static string Lab8_ReadNonEmptyText(string prompt)
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

    private static void Lab8_PrintCell(string value, int width)
    {
        Console.Write(value.PadRight(width));
    }

    private static void Lab8_PrintTableSeparator(int labelWidth, int[] columnWidths, int totalWidth)
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

    private static string Lab8_FormatNumber(double value)
    {
        return value.ToString("0.##", CultureInfo.GetCultureInfo("uk-UA"));
    }

    private static void Lab8_Pause()
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


    public static void Lab9_Run()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Масив і лічильник створені в Main та використовуються всіма пунктами меню.
        double[] values = new double[100];
        int count = 0;

        Console.WriteLine("ОБЛІК ЧИСЕЛ СПОРТИВНОЇ СЕКЦІЇ");
        Console.WriteLine("Масив може зберігати до 100 чисел.");

        int initialAmount;
        while (true)
        {
            Console.Write("Скільки чисел додати на початку (0–100): ");
            string initialAmountText = Console.ReadLine() ?? string.Empty;

            if (int.TryParse(initialAmountText, out initialAmount) &&
                initialAmount >= 0 && initialAmount <= values.Length)
            {
                break;
            }

            Console.WriteLine($"Помилка: введіть ціле число від 0 до {values.Length}.");
        }

        double[] initialValues = new double[initialAmount];
        for (int i = 0; i < initialAmount; i++)
        {
            while (true)
            {
                Console.Write($"Введіть число №{i + 1}: ");
                string valueText = Console.ReadLine() ?? string.Empty;

                if (Lab9_TryParseDouble(valueText, out initialValues[i]))
                {
                    break;
                }

                Console.WriteLine("Помилка: введіть скінченне число, наприклад 12,5 або 12.5.");
            }
        }

        Lab9_AddValues(values, ref count, initialValues);
        Console.WriteLine($"Лічильник у Main після заповнення: {count}.");

        bool isRunning = true;
        while (isRunning)
        {
            Console.WriteLine();
            Console.WriteLine("МЕНЮ");
            Console.WriteLine("1 — Додати значення");
            Console.WriteLine("2 — Видалити значення за номером");
            Console.WriteLine("3 — Показати статистику");
            Console.WriteLine("4 — Показати всі значення");
            Console.WriteLine("5 — Демонстрація типу значення й посилального типу");
            Console.WriteLine("0 — Вихід");
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                {
                    if (count == values.Length)
                    {
                        Console.WriteLine($"Масив уже заповнений: доступно максимум {values.Length} значень.");
                        break;
                    }

                    int amount;
                    while (true)
                    {
                        Console.Write($"Скільки чисел додати (1–{values.Length - count}): ");
                        string amountText = Console.ReadLine() ?? string.Empty;

                        if (int.TryParse(amountText, out amount) &&
                            amount >= 1 && amount <= values.Length - count)
                        {
                            break;
                        }

                        Console.WriteLine("Помилка: кількість має бути цілим числом у вказаних межах.");
                    }

                    double[] valuesToAdd = new double[amount];
                    for (int i = 0; i < amount; i++)
                    {
                        while (true)
                        {
                            Console.Write($"Введіть нове число №{i + 1}: ");
                            string valueText = Console.ReadLine() ?? string.Empty;

                            if (Lab9_TryParseDouble(valueText, out valuesToAdd[i]))
                            {
                                break;
                            }

                            Console.WriteLine("Помилка: введіть скінченне число, наприклад 12,5 або 12.5.");
                        }
                    }

                    Lab9_AddValues(values, ref count, valuesToAdd);
                    Console.WriteLine($"Додано {amount} значень. Тепер у масиві: {count}.");
                    break;
                }

                case "2":
                {
                    Console.Write("Номер значення для видалення (нумерація починається з 1): ");
                    string indexText = Console.ReadLine() ?? string.Empty;

                    if (int.TryParse(indexText, out int number))
                    {
                        // У меню номер починається з 1, а індекс масиву — з 0.
                        Lab9_RemoveAt(values, ref count, number - 1);
                    }
                    else
                    {
                        Console.WriteLine("Помилка: введіть цілий номер.");
                    }

                    break;
                }

                case "3":
                    Lab9_ShowStatistics(values, count);
                    break;

                case "4":
                    Lab9_PrintValues(values, count);
                    break;

                case "5":
                {
                    int originalNumber = 10;
                    Console.WriteLine("Демонстрація типу значення (int):");
                    Console.WriteLine($"До виклику методу: {originalNumber}");
                    Lab9_TryChangeInteger(originalNumber);
                    Console.WriteLine($"Після виклику методу в Main: {originalNumber}");

                    double[] sampleArray = { 5.0 };
                    Console.WriteLine();
                    Console.WriteLine("Демонстрація масиву (посилального типу):");
                    Console.WriteLine($"До виклику методу: {sampleArray[0]:0.##}");
                    Lab9_ChangeFirstArrayElement(sampleArray);
                    Console.WriteLine($"Після виклику методу в Main: {sampleArray[0]:0.##}");
                    break;
                }

                case "0":
                    isRunning = false;
                    Console.WriteLine("Програму завершено.");
                    continue;

                default:
                    Console.WriteLine("Помилка: виберіть пункт 1, 2, 3, 4, 5 або 0.");
                    break;
            }

            Console.WriteLine();
            Console.Write("Натисніть Enter, щоб повернутися до меню...");
            Console.ReadLine();

            if (!Console.IsInputRedirected)
            {
                Console.Clear();
            }
        }
    }

    // Додає підготовлені в Main числа й змінює лічильник через ref.
    private static void Lab9_AddValues(double[] values, ref int count, double[] newValues)
    {
        if (newValues.Length > values.Length - count)
        {
            Console.WriteLine("Помилка: у масиві недостатньо вільних місць.");
            return;
        }

        for (int i = 0; i < newValues.Length; i++)
        {
            values[count] = newValues[i];
            count++;
        }
    }

    // Друкує лише заповнену частину масиву. Клавіатуру цей метод не читає.
    private static void Lab9_PrintValues(double[] values, int count)
    {
        if (count == 0)
        {
            Console.WriteLine("Масив порожній.");
            return;
        }

        Console.WriteLine("┌────────┬──────────────┐");
        Console.WriteLine("│ Номер  │ Значення     │");
        Console.WriteLine("├────────┼──────────────┤");

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"│ {i + 1,6} │ {values[i],12:0.####} │");
        }

        Console.WriteLine("└────────┴──────────────┘");
    }


    private static bool Lab9_FindMaximum(double[] values, int count, out double maximum, out int number)
    {
        maximum = 0;
        number = 0;

        if (count == 0)
        {
            return false;
        }

        maximum = values[0];
        number = 1;

        for (int i = 1; i < count; i++)
        {
            if (values[i] > maximum)
            {
                maximum = values[i];
                number = i + 1;
            }
        }

        return true;
    }

    // Видаляє елемент за індексом масиву, зсуває наступні значення вліво.
    private static void Lab9_RemoveAt(double[] values, ref int count, int index)
    {
        if (index < 0 || index >= count)
        {
            Console.WriteLine($"Помилка: такого номера немає. Введіть номер від 1 до {count}.");
            return;
        }

        for (int i = index; i < count - 1; i++)
        {
            values[i] = values[i + 1];
        }

        count--;
        values[count] = 0;
        Console.WriteLine($"Значення з номером {index + 1} видалено.");
    }

    private static void Lab9_ShowStatistics(double[] values, int count)
    {
        if (!Lab9_FindMaximum(values, count, out double maximum, out int number))
        {
            Console.WriteLine("Статистики немає: масив порожній.");
            return;
        }

        double sum = 0;
        for (int i = 0; i < count; i++)
        {
            sum += values[i];
        }

        Console.WriteLine($"Кількість значень: {count}");
        Console.WriteLine($"Сума: {sum:0.####}");
        Console.WriteLine($"Середнє значення: {sum / count:0.####}");
        Console.WriteLine($"Найбільше значення: {maximum:0.####}");
        Console.WriteLine($"Номер найбільшого значення: {number}");
    }

    // int передано за значенням: зміна локальної копії не змінює змінну в Main.
    private static void Lab9_TryChangeInteger(int number)
    {
        number = 999;
        Console.WriteLine($"Усередині методу локальна копія дорівнює: {number}");
    }

    // Масив передається як посилання: зміна його елемента видима в Main.
    private static void Lab9_ChangeFirstArrayElement(double[] array)
    {
        array[0] = 99;
    }

    private static bool Lab9_TryParseDouble(string text, out double value)
    {
        string normalizedText = text.Trim().Replace(',', '.');
        return double.TryParse(
                   normalizedText,
                   NumberStyles.Float,
                   CultureInfo.InvariantCulture,
                   out value) &&
               double.IsFinite(value);
    }


    private const int Capacity = 100;
    private const int SportCapacity = 20;
    // Довідник видів спорту: один індекс відповідає одній позиції.
    private static int[] categoryIds = new int[SportCapacity];
    private static string[] categoryNames = new string[SportCapacity];
    private static bool[] categoryActive = new bool[SportCapacity];
    private static int categoryCount;
    private static int nextCategoryId;

    public static void Run()
    {
        Console.OutputEncoding = Encoding.UTF8;
        categoryCount = 0;
        nextCategoryId = 1;
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
                case "6":
                    ShowCrossTable(sportIds, joinedDates, trainings, count);
                    break;
                case "10":
                    AddExamples(ids, names, birthYears, sportIds, ranks, joinedDates, trainings, ref count, ref nextId);
                    break;
                case "12":
                    ShowSummary(ids, birthYears, trainings, count);
                    break;
                case "11":
                    ShowCategoriesMenu();
                    break;
                case "7": case "8": case "9":
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

    // Позицію довідника шукаємо за кодом, а не за її індексом у масивах.
    private static int CategoryIndex(int id)
    {
        for (int i = 0; i < categoryCount; i++)
        {
            if (categoryIds[i] == id) return i;
        }
        return -1;
    }

    private static string CategoryName(int id)
    {
        int index = CategoryIndex(id);
        return index < 0 ? "(невідома)" : categoryNames[index];
    }

    private static bool HasActiveCategory()
    {
        for (int i = 0; i < categoryCount; i++)
        {
            if (categoryActive[i]) return true;
        }
        return false;
    }

    private static void ShowCategoriesMenu()
    {
        while (true)
        {
            Console.WriteLine("\nДОВІДНИК ВИДІВ СПОРТУ");
            Console.WriteLine("1 — Показати всі види спорту");
            Console.WriteLine("2 — Додати вид спорту");
            Console.WriteLine("3 — Вимкнути вид спорту");
            Console.WriteLine("0 — Повернутися до основного меню");
            Console.Write("Ваш вибір: ");
            string? choice = Console.ReadLine()?.Trim();
            if (choice == null || choice == "0") return;
            switch (choice)
            {
                case "1": ShowCategories(false); break;
                case "2": AddCategory(); break;
                case "3": DeactivateCategory(); break;
                default: Console.WriteLine("Помилка: виберіть 1, 2, 3 або 0."); break;
            }
        }
    }

    private static void ShowCategories(bool onlyActive)
    {
        if (categoryCount == 0)
        {
            Console.WriteLine("Довідник видів спорту порожній.");
            return;
        }
        bool found = false;
        for (int i = 0; i < categoryCount; i++)
        {
            if (onlyActive && !categoryActive[i]) continue;
            if (!found) Console.WriteLine("Код  Назва  Стан");
            string state = categoryActive[i] ? "активний" : "вимкнений";
            Console.WriteLine($"{categoryIds[i],3}  {categoryNames[i]}  ({state})");
            found = true;
        }
        if (!found) Console.WriteLine("Активних видів спорту немає.");
    }

    private static void AddCategory()
    {
        if (categoryCount >= SportCapacity)
        {
            Console.WriteLine("Довідник заповнений.");
            return;
        }
        if (!ReadText("Назва виду спорту: ", out string name)) return;
        // Порівнюємо без урахування регістру, зокрема з вимкненими позиціями.
        for (int i = 0; i < categoryCount; i++)
        {
            if (string.Equals(categoryNames[i], name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Такий вид спорту вже є в довіднику.");
                return;
            }
        }
        categoryIds[categoryCount] = nextCategoryId;
        categoryNames[categoryCount] = name;
        categoryActive[categoryCount] = true;
        categoryCount++;
        nextCategoryId++;
        Console.WriteLine($"Додано вид спорту «{name}», код: {nextCategoryId - 1}.");
    }

    private static void DeactivateCategory()
    {
        if (categoryCount == 0)
        {
            Console.WriteLine("Довідник видів спорту порожній.");
            return;
        }
        ShowCategories(false);
        if (!ReadInt("Код виду спорту для вимкнення: ", 1, int.MaxValue, out int id)) return;
        int index = CategoryIndex(id);
        if (index < 0) { Console.WriteLine("Вид спорту не знайдено."); return; }
        if (!categoryActive[index]) { Console.WriteLine("Цей вид спорту вже вимкнений."); return; }
        // Назва залишається для записів, які вже посилаються на цей код.
        categoryActive[index] = false;
        Console.WriteLine($"Вид спорту «{categoryNames[index]}» вимкнено.");
    }

    private static bool ReadCategory(bool optional, int currentId, out int value)
    {
        value = currentId;
        Console.WriteLine("Оберіть активний вид спорту:");
        ShowCategories(true);
        if (!optional && !HasActiveCategory()) return false;
        while (true)
        {
            if (optional)
                Console.Write($"Код виду спорту [{CategoryName(currentId)}; Enter — залишити]: ");
            else Console.Write("Код виду спорту: ");
            string? input = Console.ReadLine();
            if (input == null) return false;
            input = input.Trim();
            if (optional && input.Length == 0) return true;
            if (int.TryParse(input, out int id))
            {
                int index = CategoryIndex(id);
                if (index >= 0 && categoryActive[index]) { value = id; return true; }
            }
            Console.WriteLine("Помилка: виберіть код наявного активного виду спорту.");
        }
    }

    // Один метод будує обидва режими та не виводить нічого на екран.
    private static int[,] BuildGrid(int[] sportIds, DateTime[] joinedDates, int[] trainings,
        int count, bool sumTrainings)
    {
        int[,] grid = new int[categoryCount, 12];
        for (int k = 0; k < count; k++)
        {
            int row = CategoryIndex(sportIds[k]);
            int col = joinedDates[k].Month - 1;
            if (row >= 0) grid[row, col] += sumTrainings ? trainings[k] : 1;
        }
        return grid;
    }

    private static void ShowCrossTable(int[] sportIds, DateTime[] joinedDates, int[] trainings, int count)
    {
        if (categoryCount == 0)
        {
            Console.WriteLine("Довідник видів спорту порожній: таблицю побудувати неможливо.");
            return;
        }
        if (count == 0)
        {
            Console.WriteLine("Сховище вихованців порожнє: таблицю побудувати неможливо.");
            return;
        }
        Console.WriteLine("1 — Кількість вихованців");
        Console.WriteLine("2 — Сума тренувань на тиждень");
        Console.WriteLine("0 — Повернутися до меню");
        string? choice;
        while (true)
        {
            Console.Write("Ваш вибір: ");
            choice = Console.ReadLine()?.Trim();
            if (choice == null || choice == "0") return;
            if (choice == "1" || choice == "2") break;
            Console.WriteLine("Помилка: виберіть 1, 2 або 0.");
        }
        bool sumTrainings = choice == "2";
        int[,] grid = BuildGrid(sportIds, joinedDates, trainings, count, sumTrainings);
        PrintCrossTable(grid, sumTrainings);
        int unknown = 0;
        for (int i = 0; i < count; i++)
        {
            if (CategoryIndex(sportIds[i]) < 0) unknown++;
        }
        if (unknown > 0)
            Console.WriteLine($"Не враховано записів із невідомим видом спорту: {unknown}.");
    }

    private static void PrintCrossTable(int[,] grid, bool sumTrainings)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int labelWidth = "Вид спорту".Length;
        for (int i = 0; i < rows; i++)
        {
            int width = categoryNames[i].Length + (categoryActive[i] ? 0 : " (вимк.)".Length);
            if (width > labelWidth) labelWidth = width;
        }
        Console.WriteLine(sumTrainings ? "\nСУМА ТРЕНУВАНЬ ЗА ВИДАМИ СПОРТУ ТА МІСЯЦЯМИ ВСТУПУ" :
            "\nКІЛЬКІСТЬ ВИХОВАНЦІВ ЗА ВИДАМИ СПОРТУ ТА МІСЯЦЯМИ ВСТУПУ");
        Console.Write("Вид спорту".PadRight(labelWidth));
        for (int j = 0; j < cols; j++) Console.Write($" {j + 1,5}");
        Console.WriteLine($" {"Разом",6}");
        int[] colTotals = new int[cols];
        int grandTotal = 0;
        for (int i = 0; i < rows; i++)
        {
            string label = categoryNames[i] + (categoryActive[i] ? "" : " (вимк.)");
            Console.Write(label.PadRight(labelWidth));
            int rowTotal = 0;
            for (int j = 0; j < cols; j++)
            {
                Console.Write($" {grid[i, j],5}");
                rowTotal += grid[i, j];
                colTotals[j] += grid[i, j];
            }
            Console.WriteLine($" {rowTotal,6}");
            grandTotal += rowTotal;
        }
        Console.Write("Разом".PadRight(labelWidth));
        int columnsTotal = 0;
        for (int j = 0; j < cols; j++)
        {
            Console.Write($" {colTotals[j],5}");
            columnsTotal += colTotals[j];
        }
        Console.WriteLine($" {grandTotal,6}");
        Console.WriteLine($"Сума підсумків рядків: {grandTotal}; сума підсумків стовпців: {columnsTotal}.");
        Console.WriteLine(grandTotal == columnsTotal ? "Підсумки збігаються." : "Помилка: підсумки не збігаються.");
    }

    private static void AddAthlete(int[] ids, string[] names, int[] birthYears, int[] sportIds,
        string[] ranks, DateTime[] joinedDates, int[] trainings, ref int count, ref int nextId)
    {
        if (count >= Capacity)
        {
            Console.WriteLine("Сховище заповнене: більше записів додати не можна.");
            return;
        }

        if (!HasActiveCategory())
        {
            Console.WriteLine("Немає активних видів спорту. Спочатку додайте вид спорту в довіднику (пункт 11).");
            return;
        }

        Console.WriteLine("\nДОДАВАННЯ ВИХОВАНЦЯ");
        if (!ReadText("Прізвище та ім'я: ", out string name) ||
            !ReadInt("Рік народження: ", 1900, DateTime.Today.Year, out int birthYear) ||
            !ReadCategory(false, 0, out int sportId) ||
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
        int[] activeSports = new int[categoryCount];
        int activeCount = 0;
        for (int i = 0; i < categoryCount; i++)
        {
            if (categoryActive[i]) activeSports[activeCount++] = categoryIds[i];
        }
        if (activeCount == 0)
        {
            Console.WriteLine("Немає активних видів спорту. Спочатку додайте вид спорту в довіднику (пункт 11).");
            return;
        }
        string[] exampleRanks = { "початковий", "III юнацький", "II юнацький", "I юнацький", "III дорослий", "II дорослий" };
        int[] exampleTrainings = { 2, 3, 3, 4, 4, 5 };
        int year = DateTime.Today.Year;
        for (int i = 0; i < exampleCount; i++)
        {
            StoreAthlete(ids, names, birthYears, sportIds, ranks, joinedDates, trainings,
                ref count, ref nextId, exampleNames[i], year - ages[i], activeSports[i % activeCount],
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
        int sportWidth = "Вид спорту".Length;
        for (int i = 0; i < count; i++)
        {
            if (names[i].Length > nameWidth) nameWidth = names[i].Length;
            if (ranks[i].Length > rankWidth) rankWidth = ranks[i].Length;
            if (CategoryName(sportIds[i]).Length > sportWidth) sportWidth = CategoryName(sportIds[i]).Length;
        }

        Console.WriteLine($"\n{ "№",4}  {"Прізвище та ім'я".PadRight(nameWidth)}  {"Рік",5}  {"Вид спорту".PadRight(sportWidth)}  {"Розряд".PadRight(rankWidth)}  {"Вступ",10}  {"Трен./тиж.",10}");
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"{ids[i],4}  {names[i].PadRight(nameWidth)}  {birthYears[i],5}  {CategoryName(sportIds[i]).PadRight(sportWidth)}  {ranks[i].PadRight(rankWidth)}  {joinedDates[i]:dd.MM.yyyy}  {trainings[i],10}");
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
        int sportWidth = "Вид спорту".Length;
        for (int i = 0; i < count; i++)
        {
            if (names[i].Length > nameWidth) nameWidth = names[i].Length;
            if (ranks[i].Length > rankWidth) rankWidth = ranks[i].Length;
            if (CategoryName(sportIds[i]).Length > sportWidth) sportWidth = CategoryName(sportIds[i]).Length;
        }
        int found = 0;
        for (int i = 0; i < count; i++)
        {
            // Contains у цій роботі чутливий до регістру.
            bool matches = byName ? names[i].Contains(query) : birthYears[i] >= from && birthYears[i] <= to;
            if (!matches) continue;
            if (found == 0)
                Console.WriteLine($"\n{"№",4}  {"Прізвище та ім'я".PadRight(nameWidth)}  {"Рік",5}  {"Вид спорту".PadRight(sportWidth)}  {"Розряд".PadRight(rankWidth)}  {"Вступ",10}  {"Трен./тиж.",10}");
            Console.WriteLine($"{ids[i],4}  {names[i].PadRight(nameWidth)}  {birthYears[i],5}  {CategoryName(sportIds[i]).PadRight(sportWidth)}  {ranks[i].PadRight(rankWidth)}  {joinedDates[i]:dd.MM.yyyy}  {trainings[i],10}");
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
            !ReadCategory(true, sportIds[index], out int sportId) ||
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
