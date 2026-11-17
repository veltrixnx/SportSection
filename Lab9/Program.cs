using System.Globalization;
using System.Text;

internal static class Program
{
    private static void Main()
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

                if (TryParseDouble(valueText, out initialValues[i]))
                {
                    break;
                }

                Console.WriteLine("Помилка: введіть скінченне число, наприклад 12,5 або 12.5.");
            }
        }

        AddValues(values, ref count, initialValues);
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

                            if (TryParseDouble(valueText, out valuesToAdd[i]))
                            {
                                break;
                            }

                            Console.WriteLine("Помилка: введіть скінченне число, наприклад 12,5 або 12.5.");
                        }
                    }

                    AddValues(values, ref count, valuesToAdd);
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
                        RemoveAt(values, ref count, number - 1);
                    }
                    else
                    {
                        Console.WriteLine("Помилка: введіть цілий номер.");
                    }

                    break;
                }

                case "3":
                    ShowStatistics(values, count);
                    break;

                case "4":
                    PrintValues(values, count);
                    break;

                case "5":
                {
                    int originalNumber = 10;
                    Console.WriteLine("Демонстрація типу значення (int):");
                    Console.WriteLine($"До виклику методу: {originalNumber}");
                    TryChangeInteger(originalNumber);
                    Console.WriteLine($"Після виклику методу в Main: {originalNumber}");

                    double[] sampleArray = { 5.0 };
                    Console.WriteLine();
                    Console.WriteLine("Демонстрація масиву (посилального типу):");
                    Console.WriteLine($"До виклику методу: {sampleArray[0]:0.##}");
                    ChangeFirstArrayElement(sampleArray);
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
    private static void AddValues(double[] values, ref int count, double[] newValues)
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
    private static void PrintValues(double[] values, int count)
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


    private static bool FindMaximum(double[] values, int count, out double maximum, out int number)
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
    private static void RemoveAt(double[] values, ref int count, int index)
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

    private static void ShowStatistics(double[] values, int count)
    {
        if (!FindMaximum(values, count, out double maximum, out int number))
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
    private static void TryChangeInteger(int number)
    {
        number = 999;
        Console.WriteLine($"Усередині методу локальна копія дорівнює: {number}");
    }

    // Масив передається як посилання: зміна його елемента видима в Main.
    private static void ChangeFirstArrayElement(double[] array)
    {
        array[0] = 99;
    }

    private static bool TryParseDouble(string text, out double value)
    {
        string normalizedText = text.Trim().Replace(',', '.');
        return double.TryParse(
                   normalizedText,
                   NumberStyles.Float,
                   CultureInfo.InvariantCulture,
                   out value) &&
               double.IsFinite(value);
    }
}
