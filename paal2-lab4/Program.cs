using System;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;


static class StringExtansion 
{
    public static void Print(this object self) => Console.WriteLine(self);
}

static class Task1
{
    public static string CalculateTask1Time(Func<int, string> fun, int count)
    {
        var sw = Stopwatch.StartNew();
        _ = fun(count);
        sw.Stop();
        return sw.Elapsed.ToString();
    }

    public static string Subtask1(int count)
    {
        string text = "";
        foreach (int i in Enumerable.Range(1,count))
            text += i.ToString() + " ";
        return text;
    }    

    public static string Subtask2(int count)
    {
        string text = "";
        foreach (int i in Enumerable.Range(1, count).Reverse())
            text = i.ToString() + " " + text;
        return text;
    }

    public static string Subtask3(int count)
    {
        StringBuilder text = new();
        foreach (int i in Enumerable.Range(1, count))
            text.Append(i.ToString() + " ");
        return text.ToString();
    }

    public static string Subtask4(int count)
    {
        StringBuilder text = new();
        foreach (int i in Enumerable.Range(1, count).Reverse())
            text.Insert(0, i.ToString() + " ");
        return text.ToString();
    }

    public static string Extra1(int count) => string.Join(" ", Enumerable.Range(1, count));
    public static string Extra2(int count) => string.Join(" ", Enumerable.Range(1, count).Reverse());
}

static class Task2
{
    private static readonly string Vowel = "уеїіаоєяиюУЕЇІАОЄЯИЮ";
    private static readonly string Consonants = "бвгґджзйклмнпрстфхцчшщБВГҐДЖЗЙКЛМНПРСТФХЦЧШЩ";


    /// <summary>
    /// Replaces occurrences of specific words based on surrounding characters in a given string.
    /// </summary>
    /// <param name="input">A string containing words that will be analyzed and modified based on specific rules.</param>
    /// <returns>A modified string with certain words replaced according to the defined conditions.</returns>
    public static string Subtask13(string input)
    {
        string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++)
        {
            if ((words[i] == "в" || words[i] == "у" || words[i] == "В" || words[i] == "У") && i < words.Length - 1)
            {
                char nextChar = GetFirstLetter(words[i + 1]);

                bool nextIsVowel = IsVowel(nextChar);
                bool nextIsConsonant = IsConsonant(nextChar);
                bool isUpper = char.IsUpper(words[i][0]);

                string newWord;

                if (nextIsVowel && words[i].ToLower() == "у")
                    newWord = isUpper ? "В" : "в";

                else if (nextIsConsonant && words[i].ToLower() == "в")
                    newWord = isUpper ? "У" : "у";
                else
                    continue;

                words[i] = newWord;
            }
        }

        return string.Join(" ", words);

        static char GetFirstLetter(string word)
        {
            foreach (char c in word)
                if (char.IsLetter(c))
                    return c;
            return word[0];
        }
        static bool IsVowel(char ch)     => Vowel.Contains(ch);
        static bool IsConsonant(char ch) => Consonants.Contains(ch);
    }

    /// <summary>
    /// Checks if the parentheses in the given string are balanced.
    /// </summary>
    /// <param name="input">A string containing characters, including parentheses, to be evaluated for balance.</param>
    /// <returns>Returns true if the parentheses are balanced, otherwise false.</returns>
    public static bool Subtask15(string input)
    {
        int balance = 0;

        foreach (char ch in input)
        {
            if (ch == '(') balance++;
            else if (ch == ')')
                if (--balance < 0) return false;
        }

        return balance == 0;
    }

    /// <summary>
    /// Extracts words from a string that match a specified pattern using regular expressions.
    /// </summary>
    /// <param name="input">The string from which words will be extracted based on the matching criteria.</param>
    /// <param name="pattern">The pattern used to define the matching criteria for the words in the input string.</param>
    public static IEnumerator<string> Subtask16(string input, string pattern)
    {
        string regexPattern = "^" + Regex.Escape(pattern)
                                 .Replace(@"\*", ".*")
                                 .Replace(@"\?", ".") + "$";


        string[] words = input.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
            if (Regex.IsMatch(word, regexPattern))
                yield return word;
    }

    private static readonly string[] units = { "", "один", "два", "три", "чотири", "п’ять", "шість", "сім", "вісім", "дев’ять" };
    private static readonly string[] unitsFeminine = { "", "одна", "дві", "три", "чотири", "п’ять", "шість", "сім", "вісім", "дев’ять" };
    private static readonly string[] teens = { "десять", "одинадцять", "дванадцять", "тринадцять", "чотирнадцять", "п’ятнадцять", "шістнадцять", "сімнадцять", "вісімнадцять", "дев’ятнадцять" };
    private static readonly string[] tens = { "", "", "двадцять", "тридцять", "сорок", "п’ятдесят", "шістдесят", "сімдесят", "вісімдесят", "дев’яносто" };
    private static readonly string[] hundreds = { "", "сто", "двісті", "триста", "чотириста", "п’ятсот", "шістсот", "сімсот", "вісімсот", "дев’ятсот" };

    private static string Convert999(int number, bool genderSensitive)
    {
        if (number == 0) return "";

        List<string> parts = [];
        var currentUnits = genderSensitive ? unitsFeminine : units;

        if (number >= 100)
        {
            parts.Add(hundreds[number / 100]);
            number %= 100;
        }

        if (number >= 20)
        {
            parts.Add(tens[number / 10]);
            number %= 10;
            if (number > 0)
            {
                parts.Add(currentUnits[number]);
            }
        }
        else if (number >= 10) // 10-19
        {
            parts.Add(teens[number - 10]);
            number = 0;
        }
        else if (number > 0) // 1-9
        {
            parts.Add(currentUnits[number]);
            number = 0;
        }


        return string.Join(" ", parts.Where(p => !string.IsNullOrEmpty(p)));
    }

    private static string NumberToWordsInternal(int number, bool genderSensitive = false)
    {
        if (number == 0) return "нуль";
        if (number < 0) return "мінус " + NumberToWordsInternal(Math.Abs(number));

        List<string> resultParts = [];

        if (number >= 1_000_000)
        {
            int millionsPart = number / 1_000_000;
            resultParts.Add(Convert999(millionsPart, false));

            int lastDigit = millionsPart % 10;
            int lastTwoDigits = millionsPart % 100;
            string millionWord;

            if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
                millionWord = "мільйонів";
            else if (lastDigit == 1)
                millionWord = "мільйон";
            else if (lastDigit >= 2 && lastDigit <= 4)
                millionWord = "мільйони";
            else
                millionWord = "мільйонів";

            resultParts.Add(millionWord);
            number %= 1_000_000;
        }

        if (number >= 1_000)
        {
            int thousandsPart = number / 1_000;
            resultParts.Add(Convert999(thousandsPart, true));

            int lastDigit = thousandsPart % 10;
            int lastTwoDigits = thousandsPart % 100;
            string thousandWord;
            if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
                thousandWord = "тисяч";
            else if (lastDigit == 1)
                thousandWord = "тисяча";
            else if (lastDigit >= 2 && lastDigit <= 4)
                thousandWord = "тисячі";
            else
                thousandWord = "тисяч";
            resultParts.Add(thousandWord);
            number %= 1_000;
        }

        if (number > 0)
            resultParts.Add(Convert999(number, genderSensitive));

        return string.Join(" ", resultParts.Where(p => !string.IsNullOrEmpty(p)));
    }

    public static string Subtask17(string input)
    {
        return new Regex(@"(\d+)\s*(м|грн)").Replace(input, match =>
        {
            if (!int.TryParse(match.Groups[1].Value, out int number))
                return match.Value;
            string unit = match.Groups[2].Value;

            string unitWord;
            bool useFeminine = false;

            if (unit == "м")
            {
                int lastDigit = number % 10;
                int lastTwoDigits = number % 100;

                if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
                    unitWord = "метрів"; // 11-19 метрів
                else if (lastDigit == 1)
                    unitWord = "метр"; // 1 метр, 21 метр, 101 метр ...
                else if (lastDigit >= 2 && lastDigit <= 4)
                    unitWord = "метри"; // 2, 3, 4 метри, 22, 23, 24 метри ...
                else // 0, 5, 6, 7, 8, 9
                    unitWord = "метрів"; // 0 метрів, 5 метрів, 10 метрів, 20 метрів ...
            }
            else if (unit == "грн")
            {
                useFeminine = true;

                int lastDigit = number % 10;
                int lastTwoDigits = number % 100;

                if (lastTwoDigits >= 11 && lastTwoDigits <= 19)
                    unitWord = "гривень"; // 11-19 гривень
                else if (lastDigit == 1)
                    unitWord = "гривня"; // 1 гривня, 21 гривня, 101 гривня ...
                else if (lastDigit >= 2 && lastDigit <= 4)
                    unitWord = "гривні"; // 2, 3, 4 гривні, 22, 23, 24 гривні ...
                else // 0, 5, 6, 7, 8, 9
                    unitWord = "гривень"; // 0 гривень, 5 гривень, 10 гривень, 20 гривень ...
            }
            else
                return match.Value;

            string numberInWords = NumberToWordsInternal(number, useFeminine);

            return $"{numberInWords} {unitWord}";
        });
    }
}

class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding  = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("""
            Виберіть задачу
            1 - підрахунка часу побудови рядків
            2 - перевірка та виправлення у/в в реченні
            3 - перевірка на правильність закритих дужок
            4 - перевірка слів по шаблону
            5 - переведення чисел в слова
            """);
            switch (Console.ReadLine())
            {
                case "1":
                start1:
                    Console.WriteLine("Введіть кількість символів для обрахунку:");

                    if (!int.TryParse(Console.ReadLine(), out int count))
                    {
                        Console.WriteLine("Невалідне введення");
                        goto start1;
                    }
                    if (count > 5000000)
                    {
                        Console.WriteLine("Не балуйтесь");
                        goto start1;
                    }

                    Console.WriteLine("Обраховуємо результат");
                    var tasks = new List<Task>
                    {
                        Task.Run(() =>
                            Console.WriteLine("Рядок напряму\n"
                            + Task1.CalculateTask1Time(Task1.Subtask1, count))),
                        Task.Run(() =>
                            Console.WriteLine("Рядок з додаванням на початок\n"
                            + Task1.CalculateTask1Time(Task1.Subtask2, count))),
                        Task.Run(() =>
                            Console.WriteLine("Конструктор рядку\n" 
                            + Task1.CalculateTask1Time(Task1.Subtask3, count))),
                        Task.Run(() =>
                            Console.WriteLine("Конструктор рядку з додаванням на початок\n"
                            + Task1.CalculateTask1Time(Task1.Subtask4, count))),
                        Task.Run(() =>
                            Console.WriteLine("По-людськи\n"
                            + Task1.CalculateTask1Time(Task1.Extra1,   count))),
                        Task.Run(() =>
                            Console.WriteLine("По-людськи з додаванням на початок\n"
                            + Task1.CalculateTask1Time(Task1.Extra2,   count))),
                    };

                    Task.WaitAll([.. tasks]);

                    Console.WriteLine("Усі задачі завершено.");

                    break;
                case "2":
                    Console.WriteLine("Введеіть речення що буде перевірятися на валідність використання у/в");
                    Console.WriteLine("Результат:\n" + Task2.Subtask13(Console.ReadLine()!));
                    break;
                case "3":
                    Console.WriteLine("Введіть вираз з дужками для перевірки");
                    Console.WriteLine(Task2.Subtask15(Console.ReadLine()) ? "Яп" : "Не-а");
                    break;
                case "4":
                    Console.WriteLine("Введіть через пробіл слова що будуть перевірятись шаблоном");
                    string txt = Console.ReadLine();
                    Console.WriteLine("Введіть шаблон(\"?\" для 1 довільного символу та \"*\" для будь якого підрядка)");
                    Console.WriteLine("Результат:\n" + String.Join(" ", Task2.Subtask16(txt, Console.ReadLine())));
                    break;
                case "5":
                    Console.WriteLine("Введіть речення з числами з приставками м/грн для заміни");
                    Console.WriteLine("Результат:\n" + Task2.Subtask17(Console.ReadLine()));
                    break;
                default:
                    Console.WriteLine("Невалідний вибір");
                    break;
            }
        }

    }
}
