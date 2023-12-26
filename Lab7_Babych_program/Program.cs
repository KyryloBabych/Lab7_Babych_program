using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

public class Lab7_Babych_program
{
    public List<Comic> comicsList;

    public Lab7_Babych_program()
    {
        comicsList = new List<Comic>();
    }
    class Program
    {
        static void Main()
        {
            Lab7_Babych_program lab7 = new Lab7_Babych_program();
            lab7.Run();
        }
    }
    private static int numberOfDeletedComics; // Счетчик удаленных комиксов

    // Статичне властивість для отримання значення лічильника видалених коміксів
    public static int NumberOfDeletedComics
    {
        get { return numberOfDeletedComics; }
    }

    // Статичний метод для збільшення лічильника видалених коміксів
    public static void IncrementDeletedComics()
    {
        numberOfDeletedComics++;
    }

    public void Run()
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.BackgroundColor = ConsoleColor.White;

        CultureInfo uaCulture = new CultureInfo("uk-UA");
        CultureInfo.DefaultThreadCurrentCulture = uaCulture;
        CultureInfo.DefaultThreadCurrentUICulture = uaCulture;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        int maxComics;

        while (true)
        {
            if (!IsRunningInTestMode())
            {
                Console.Clear();
            };
            Console.Write("Введіть значення N (1 і більше): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out maxComics) && maxComics >= 1)
            {
                break;
            }
            else
            {
                Console.WriteLine("Невірне значення N. N повинно бути 1 і більше.");
            }
        }

        Console.WriteLine("Ви ввели коректне значення N: " + maxComics);
        Lab7_Babych_program lab7 = new Lab7_Babych_program();
        // AutoFillData(maxComics-1);
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1 - Додати об'єкт коміксу");
            Console.WriteLine("2 - Вивести на екран об'єкти коміксів");
            Console.WriteLine("3 - Знайти об'єкт коміксу");
            Console.WriteLine("4 - Видалити об'єкт коміксу");
            Console.WriteLine("5 - Демонстрація поведінки");
            Console.WriteLine("6 – демонстрація роботи static методів");
            Console.WriteLine("7 - Зберегти/зчитати колекцію об'єктів у файлі");
            Console.WriteLine("8 - Очистити колекцію об'єктів");
            Console.WriteLine("0 - Вийти з програми");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    if (comicsList.Count < maxComics)
                    {
                        AddObjectViaInput();
                        //AddComic();
                    }
                    else
                    {
                        Console.WriteLine("Досягнута максимальна кількість об'єктів коміксів.");
                    }
                    break;
                case 2:
                    DisplayComics(comicsList);
                    DisplayObjectCount();
                    break;
                case 3:
                    SearchComic();
                    break;
                case 4:
                    DeleteComic();
                    break;
                case 5:
                    DemonstrateBehavior();
                    break;
                case 6:
                    DemonstrateStaticMethods();
                    break;
                case 7:
                    SaveLoadMenu();
                    break;
                case 8:
                    ClearCollection();
                    Console.WriteLine("Колекцію об'єктів успішно очищено.");
                    break;
                case 9:
                    AutoFillData(maxComics);
                    Console.WriteLine("Заповнено");
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }
    public void ClearCollection()
    {
        comicsList.Clear();
        Console.WriteLine("Колекцію об'єктів успішно очищено.");
    }
    private static bool IsRunningInTestMode()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Any(assembly => assembly.FullName.ToLowerInvariant().StartsWith("microsoft.visualstudio.testtools"));
    }
    // ЧЕТВЕРТА ЛАБА
    public void AddObjectViaInput()
    {
        Console.WriteLine("Додавання об'єкта коміксу через введення рядка з характеристиками:");

        Console.Write("Введіть рядок з характеристиками: ");
        string inputString = Console.ReadLine();

        if (Comic.TryParse(inputString, out Comic newComic))
        {
            comicsList.Add(newComic);
            Console.WriteLine("Об'єкт коміксу успішно додано.");
        }
        else
        {
            Console.WriteLine("Не вдалося створити об'єкт коміксу за введеними характеристиками.");
        }
    }
    public void DisplayObjectCount()
    {
        Console.WriteLine($"Кількість створених об'єктів коміксів: {Comic.NumberOfObjects}");
        Console.WriteLine($"Кількість видалених коміксів: {Lab7_Babych_program.NumberOfDeletedComics}");

    }
    public void DemonstrateStaticMethods()
    {
        Console.WriteLine("\nДемонстрація роботи static методів:");

        // Виклик статичного методу для обчислення статистики по жанрам
        Dictionary<Genre, double> genreStatistics = Comic.CalculateGenreStatistics(comicsList);

        if (genreStatistics != null)
        {
            Console.WriteLine("Статистика по жанрам:");

            foreach (var genrePercentage in genreStatistics)
            {
                Console.WriteLine($"{genrePercentage.Key} - {genrePercentage.Value:F2}%");
            }
        }
        Console.WriteLine($"Кількість створених об'єктів коміксів: {Comic.NumberOfObjects}");
        Console.WriteLine($"Кількість видалених коміксів: {Lab7_Babych_program.NumberOfDeletedComics}");
    }
    public void AutoFillData(int maxComics)
    {
        Random random = new Random();

        for (int i = 0; i < maxComics; i++)
        {
            // Генеруємо дані для коміксу
            string name = GenerateRandomComicTitle();
            Genre genre = (Genre)random.Next(1, Enum.GetValues(typeof(Genre)).Length + 1);
            DateTime yearReleased = GenerateRandomDate();
            string author = GenerateRandomAuthor();
            double price = Math.Round(random.NextDouble() * (100 - 2) + 2, 2);

            // Створюємо екземпляр класу Comic та додаємо його до списку
            Comic comic = new Comic(name, genre, yearReleased, author, price);
            comicsList.Add(comic);
        }
    }

    private string GenerateRandomComicTitle()
    {
        string[] titles = new string[]
        {
        "Хроники Звездного Спасения", "Тень Вечности Путь Героя", "Механизм Времени Зов Пустоты",
         "Легенды Лунного Сияния", "Стражи Первозданного Леса", "Гиперборейская Сага Сияющий Лед",
         "Атомные Войны Последний Рассвет", "Час Заката Герои Забытых Миров", "Хранители Космической Гравитации",
         "Владыки Магии Темные Искусства", "Пустоши Забытых Героев", "Эпоха Механического Восстания",
         "Стражи Подземных Коридоров", "Астральные Хроники Путь К Звездам", "Метаморфозы Реальности",
         "Герои Виртуального Мира", "Судьбы Галактических Путешественников", "Пламя Возмездия Расколенные Души",
         "Скрытые Тени Вселенной", "Звездный Вихрь Возвращение Легенды", "Хранители Кристального Ордена",
         "Древние Заветы Зов Прошлого", "Кодекс Квантовой Реальности", "Герои Затмения Путь К Свету",
         "Экспедиция к Звездным Горизонтам", "Магия Пандоры Тайна Забытых Легенд", "Аномалии Космического Края",
         "Терра Механика Судьба Роботов", "Сага Огненных Душ", "Век Героев Легенды Бессмертных",
         "Путь Ордена Звездного Рассвета", "Адмиралы Космического Флота", "Герои Космической Аномалии",
         "Секреты Звездного Магнетизма", "Эпопея Затерянных Миров", "Кровавый Сумрак Темные Силы Пустоты",
         "Свет Звездного Потока", "Лабиринты Виртуальной Реальности", "Герои Седьмого Неба Вечные Врата",
         "Империя Теней Забытый Артефакт"
        };

        Random random = new Random();
        return titles[random.Next(titles.Length)];
    }

    private DateTime GenerateRandomDate()
    {
        Random random = new Random();
        int range = (DateTime.Today - new DateTime(1960, 1, 1)).Days;
        return new DateTime(1960, 1, 1).AddDays(random.Next(range));
    }

    private string GenerateRandomAuthor()
    {
        string[] authors = new string[]
        {
        "Эларион Стеллархарт","Изабелла Ноктюрналис","Локан Тумблвейв","Сирин Стелларфайр",
        "Феликс Спектрокрафт","Астрид Силентблейд","Эмери Дримвэйв","Ксантус Вейвшадоу",
        "Селестра Лунавейв","Лоран Астраллерн"
        };

        Random random = new Random();
        return authors[random.Next(authors.Length)];
    }


    public void AddComic()
    {
        Console.WriteLine("Додавання об'єкта коміксу:");

        Console.Write("Назва: ");
        string name = Console.ReadLine();
        if (name.Length < 3 || !name.All(c => char.IsLetter(c) || c == ' '))
        {
            Console.WriteLine("Назва занадто коротка або містить символи, що не є літерами або пробілами. Спробуйте ще раз.");
            return;
        }


        Console.WriteLine("Жанри коміксу:");
        foreach (Genre g in Enum.GetValues(typeof(Genre)))
        {
            Console.WriteLine($"{(int)g} - {g}");
        }
        Genre selectedGenre;
        Console.Write("Виберіть жанр (введіть номер): ");
        if (!Enum.TryParse(typeof(Genre), Console.ReadLine(), out object genreObj) || !Enum.IsDefined(typeof(Genre), genreObj))
        {
            Console.WriteLine("Невірний вибір жанру.");
            return;
        }
        selectedGenre = (Genre)genreObj;

        Console.Write("Рік випуску (dd.MM.yyyy): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime yearReleased)
            || yearReleased < new DateTime(1960, 1, 1) || yearReleased > DateTime.Now)
        {
            Console.WriteLine("Невірний формат або діапазон року.");
            return;
        }

        Console.Write("Автор: ");
        string author = Console.ReadLine();

        Console.Write("Ціна (2-100): ");
        if (!double.TryParse(Console.ReadLine(), out double price) || price < 2 || price > 100)
        {
            Console.WriteLine("Невірний діапазон ціни.");
            return;
        }

        Comic comic = new Comic(name, selectedGenre, yearReleased, author, price);
        comicsList.Add(comic);
        Console.WriteLine("Об'єкт коміксу успішно додано.");
    }
    public void DisplayComics(List<Comic> comicsList)
    {
        if (comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній.");
        }
        else
        {
            Console.WriteLine("Список коміксів:");
            int index = 1;
            foreach (Comic comic in comicsList)
            {
                Console.WriteLine($"#{index++}");
                comic.DisplayInfo();
                Console.WriteLine();
            }
        }
    }

    public void SearchComic()
    {
        if (comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній.");
            return;
        }

        Console.WriteLine("Виберіть критерії пошуку:");
        Console.WriteLine("1 - По назві");
        Console.WriteLine("2 - По жанру");
        Console.WriteLine("3 - По автору");
        Console.Write("Введіть номер: ");

        if (!int.TryParse(Console.ReadLine(), out int searchChoice) || (searchChoice < 1 || searchChoice > 3))
        {
            Console.WriteLine("Невірний вибір критерію пошуку.");
            return;
        }

        string searchTerm;
        switch (searchChoice)
        {
            case 1:
                Console.Write("Введіть назву для пошуку: ");
                searchTerm = Console.ReadLine();
                var resultsByName = comicsList.Where(comic => comic.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                DisplayComics(resultsByName);
                break;
            case 2:
                Console.WriteLine("Доступні жанри для пошуку:");
                foreach (Genre genre in Enum.GetValues(typeof(Genre)))
                {
                    Console.WriteLine($"{(int)genre} - {genre}");
                }
                Console.Write("Виберіть жанр для пошуку (введіть номер): ");
                if (!Enum.TryParse(typeof(Genre), Console.ReadLine(), out object genreObj) || !Enum.IsDefined(typeof(Genre), genreObj))
                {
                    Console.WriteLine("Невірний вибір жанру.");
                    return;
                }
                Genre searchGenre = (Genre)genreObj;
                var resultsByGenre = comicsList.Where(comic => comic.Genre == searchGenre).ToList();
                DisplayComics(resultsByGenre);
                break;
            case 3:
                Console.Write("Введіть ім'я автора для пошуку: ");
                searchTerm = Console.ReadLine();
                var resultsByAuthor = comicsList.Where(comic => comic.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                DisplayComics(resultsByAuthor);
                break;
        }
    }

    public void DeleteComic()
    {
        if (comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній.");
            throw new IOException("Список коміксів порожній.");
            return;
        }

        Console.WriteLine("Виберіть метод видалення:");
        Console.WriteLine("1 - Видалити за номером");
        Console.WriteLine("2 - Видалити за назвою");
        Console.Write("Введіть номер: ");

        if (!int.TryParse(Console.ReadLine(), out int deleteChoice) || (deleteChoice < 1 || deleteChoice > 2))
        {
            Console.WriteLine("Невірний вибір методу видалення.");
            return;
        }

        switch (deleteChoice)
        {
            case 1:
                Console.Write("Введіть номер коміксу для видалення: ");
                if (int.TryParse(Console.ReadLine(), out int deleteIndex) && deleteIndex >= 1 && deleteIndex <= comicsList.Count)
                {
                    Comic comicToRemove = comicsList[deleteIndex - 1];
                    comicsList.Remove(comicToRemove);
                    Console.WriteLine("Об'єкт коміксу успішно видалено.");
                    IncrementDeletedComics();
                }
                else
                {
                    Console.WriteLine("Невірний номер коміксу для видалення.");
                }
                break;
            case 2:
                Console.Write("Введіть назву коміксу для видалення: ");
                string deleteName = Console.ReadLine();
                var comicsToRemove = comicsList.Where(comic => comic.Name.Equals(deleteName, StringComparison.OrdinalIgnoreCase)).ToList();
                if (comicsToRemove.Count == 0)
                {
                    Console.WriteLine("Комікс не знайдено для видалення.");
                }
                else
                {
                    foreach (var comicToRemove in comicsToRemove)
                    {
                        comicsList.Remove(comicToRemove);
                    }
                    Console.WriteLine("Об'єкт коміксу успішно видалено.");
                }
                break;
        }
    }
    public void DemonstrateBehavior()
    {
        if (comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній.");
            return;
        }

        Console.WriteLine("Оберіть комікс для демонстрації поведінки:");
        DisplayComics(comicsList);
        Console.Write("Введіть номер коміксу: ");
        if (int.TryParse(Console.ReadLine(), out int demoIndex) && demoIndex >= 1 && demoIndex <= comicsList.Count)
        {
            Comic selectedComic = comicsList[demoIndex - 1];

            Console.WriteLine("\nІнформація про обраний комікс із ціною:");
            selectedComic.DisplayInfo();

            Console.WriteLine("\nІнформація про обраний комікс без ціни:");
            selectedComic.DisplayInfo(false);
        }
        else
        {
            Console.WriteLine("Невірний номер коміксу.");
        }
    }
    //7ЛАБА
    public void SaveToCsv(string filePath)
    {
        if (comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній. Немає даних для збереження у CSV.");
            return;
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Записуємо заголовок CSV
                writer.WriteLine("Name,Genre,YearReleased,Author,Price");

                // Записуємо дані про кожен комікс
                foreach (Comic comic in comicsList)
                {
                    writer.WriteLine($"{comic.Name},{comic.Genre},{comic.YearReleased:dd.MM.yyyy},{comic.Author},{comic.Price:F2}");
                }

                Console.WriteLine("Дані успішно збережено у CSV файл.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні у CSV файл: {ex.Message}");
        }
    }

    private void SaveLoadMenu()
    {
        Console.WriteLine("\nПідменю Збереження/Зчитування:");
        Console.WriteLine("1 - Зберегти у файл *.csv (*.txt)");
        Console.WriteLine("2 - Зберегти у файл *.json");
        Console.WriteLine("3 - Зчитати з файлу *.csv (*.txt)");
        Console.WriteLine("4 - Зчитати з файлу *.json");

        int choice;
        if (!int.TryParse(Console.ReadLine(), out choice))
        {
            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
            return;
        }

        switch (choice)
        {
            case 1:
                SaveToCsvMenu();
                break;
            case 2:
                SaveToJsonMenu();
                break;
                break;
            case 3:
                LoadFromCsvMenu();
                break;
            case 4:
                LoadFromJsonMenu();
                break;
                break;
            default:
                Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                break;
        }
    }

    private void SaveToCsvMenu()
    {
        Console.Write("Введіть шлях для збереження файлу *.csv (*.txt): ");
        string filePath = Console.ReadLine();
        SaveToCsv(filePath);
    }

    private void LoadFromCsvMenu()
    {
        Console.Write("Введіть шлях для зчитування файлу *.csv (*.txt): ");
        string filePath = Console.ReadLine();
        LoadFromCsv(filePath);
    }
    public void LoadFromCsv(string filePath)
    {
        try
        {
            List<Comic> loadedComics = new List<Comic>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Comic comic = Comic.FromCsv(line);
                    if (comic != null)
                    {
                        loadedComics.Add(comic);
                    }
                }
            }
            comicsList.AddRange(loadedComics);
            Console.WriteLine("Дані успішно завантажено з файлу CSV.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завантаженні даних з файлу CSV: {ex.Message}");
        }
    }
    public void SaveToJson(string filePath)
    {
        if (comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній. Немає даних для збереження у JSON.");
            return;
        }

        try
        {
            string jsonData = JsonConvert.SerializeObject(comicsList, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
            Console.WriteLine("Дані успішно збережено у JSON файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні у JSON файл: {ex.Message}");
        }
    }

    public void LoadFromJson(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                List<Comic> loadedComics = JsonConvert.DeserializeObject<List<Comic>>(jsonData);
                comicsList.AddRange(loadedComics);
                Console.WriteLine("Дані успішно завантажено з файлу JSON.");
            }
            else
            {
                Console.WriteLine("Файл JSON не знайдено.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завантаженні даних з файлу JSON: {ex.Message}");
        }
    }
    private void SaveToJsonMenu()
    {
        Console.Write("Введіть шлях для збереження файлу *.json: ");
        string filePath = Console.ReadLine();
        SaveToJson(filePath);
    }

    private void LoadFromJsonMenu()
    {
        Console.Write("Введіть шлях для зчитування файлу *.json: ");
        string filePath = Console.ReadLine();
        LoadFromJson(filePath);
    }
}


