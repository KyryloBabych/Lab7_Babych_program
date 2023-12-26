using System;
using System.Globalization;
using System.Xml;
using Newtonsoft.Json;


public class Comic
{
    private string name;
    private Genre genre;
    private DateTime yearReleased;
    private string author;
    private double price;
    //
    private static int numberOfObjects;
    private static string domainCharacteristic;

    // Статичні публічні властивості для доступу до статичних полів
    public static int NumberOfObjects
    {
        get { return numberOfObjects; }
    }

    public static string DomainCharacteristic
    {
        get { return domainCharacteristic; }
        set { domainCharacteristic = value; }
    }
    public static void PrintComicStatistics()
    {
        Console.WriteLine($"Загальна кількість створених коміксів: {NumberOfObjects}");
        Console.WriteLine($"Характеристика предметної області: {DomainCharacteristic}");
    }
    public static Dictionary<Genre, double> CalculateGenreStatistics(List<Comic> comicsList)
    {
        if (comicsList == null || comicsList.Count == 0)
        {
            Console.WriteLine("Список коміксів порожній або не існує.");
            return null;
        }

        Dictionary<Genre, int> genreCounts = new Dictionary<Genre, int>();

        // Підрахунок кількості коміксів для кожного жанру
        foreach (Comic comic in comicsList)
        {
            if (genreCounts.ContainsKey(comic.Genre))
            {
                genreCounts[comic.Genre]++;
            }
            else
            {
                genreCounts[comic.Genre] = 1;
            }
        }

        // Обчислення відсоткової частки кожного жанру в загальній кількості коміксів
        int totalComics = comicsList.Count;
        Dictionary<Genre, double> genrePercentages = new Dictionary<Genre, double>();

        foreach (var genreCount in genreCounts)
        {
            double percentage = (genreCount.Value / (double)totalComics) * 100;
            genrePercentages.Add(genreCount.Key, percentage);
        }

        return genrePercentages;
    }
    public static Comic Parse(string s)
    {
        string[] parts = s.Split(',');

        if (parts.Length != 5)
        {
            throw new FormatException("Невірний формат рядка.");
        }

        // Розпаковуємо частини рядка
        string name = parts[0].Trim();
        if (name.Length < 3 || !name.All(c => char.IsLetter(c) || c == ' '))
        {
            throw new FormatException("Назва занадто коротка або містить неправильні символи.");
        }

        if (!Enum.TryParse(parts[1].Trim(), out Genre genre) || !Enum.IsDefined(typeof(Genre), genre))
        {
            throw new FormatException("Невірний формат жанру.");
        }

        if (!DateTime.TryParseExact(parts[2].Trim(), "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime yearReleased)
            || yearReleased < new DateTime(1960, 1, 1) || yearReleased > DateTime.Now)
        {
            throw new FormatException("Невірний формат або діапазон року випуску.");
        }

        string author = parts[3].Trim();

        if (!double.TryParse(parts[4].Trim(), out double price) || price < 2 || price > 100)
        {
            throw new FormatException("Невірний формат або діапазон ціни.");
        }

        // Створюємо та повертаємо об'єкт Comic
        return new Comic(name, genre, yearReleased, author, price);
    }
    public static bool TryParse(string s, out Comic obj)
    {
        obj = null;
        try
        {
            obj = Parse(s);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public override string ToString()
    {
        return $"{Name};{Genre};{YearReleased:dd.MM.yyyy};{Author};{Price}";
    }

    ///
    // Властивості для доступу до private-полів

    public string Name
    {
        get { return name; }
        set
        {
            if (value != null && value.Length > 0 && value.All(c => char.IsLetter(c) || c == ' '))
                name = value;
            else
                Console.WriteLine("Назва містить неправильні символи. Спробуйте ще раз.");
        }
    }

    public Genre Genre
    {
        get { return genre; }
        set { genre = value; }
    }

    public DateTime YearReleased
    {
        get { return yearReleased; }
        set
        {
            if (value >= new DateTime(1960, 1, 1) && value <= DateTime.Now)
                yearReleased = value;
            else
                Console.WriteLine("Невірний формат або діапазон року.");
        }
    }

    public string Author
    {
        get { return author; }
        set { author = value; }
    }

    public double Price
    {
        get { return price; }
        set
        {
            if (value >= 2 && value <= 100)
                price = value;
            else
                Console.WriteLine("Невірний діапазон ціни.");
        }
    }

    // Автовластивість зі значенням за замовчуванням
    public string DefaultProperty { get; set; } = "Default";

    // Обчислювальна властивість
    public string CombinedInfo
    {
        get { return $"{Name} - {Author}"; }
    }

    // Властивість із різним рівнем доступу для секцій get і set
    public DateTime ReleaseYear
    {
        get { return yearReleased; }
        private set { yearReleased = value; }
    }


    public Comic()
    {
        // Конструктор без параметрів
    }

    public Comic(string name, Genre genre, DateTime yearReleased, string author, double price)
    {
        numberOfObjects++;
        Name = name;
        Genre = genre;
        YearReleased = yearReleased;
        Author = author;
        Price = price;
        // Конструктор з параметрами
    }

    public Comic(string name, Genre genre, DateTime yearReleased, string author)
        : this(name, genre, yearReleased, author, 0)
    {

        // Конструктор, який викликає інший конструктор
    }

    // Додатковий перевантажений конструктор
    public Comic(string name, Genre genre, string author)
        : this(name, genre, DateTime.Now, author, 0)
    {
        // Ініціалізація за замовчуванням для року та ціни
    }

    public void DisplayInfo()
    {
        CultureInfo uaCulture = new CultureInfo("uk-UA");
        CultureInfo.DefaultThreadCurrentCulture = uaCulture;
        CultureInfo.DefaultThreadCurrentUICulture = uaCulture;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"Назва: {Name}");
        Console.WriteLine($"Жанр: {Genre}");
        Console.WriteLine($"Рік випуску: {YearReleased:dd.MM.yyyy}");
        Console.WriteLine($"Автор: {Author}");
        Console.WriteLine($"Ціна: ${Price}");
        Console.WriteLine($"Назва - Автор: {CombinedInfo}");
    }
    // Перевантажений метод з іншою сигнатурою
    public void DisplayInfo(bool includePrice)
    {
        CultureInfo uaCulture = new CultureInfo("uk-UA");
        CultureInfo.DefaultThreadCurrentCulture = uaCulture;
        CultureInfo.DefaultThreadCurrentUICulture = uaCulture;

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"Назва: {Name}");
        Console.WriteLine($"Жанр: {Genre}");
        Console.WriteLine($"Рік випуску: {YearReleased:dd.MM.yyyy}");
        Console.WriteLine($"Автор: {Author}");

        if (includePrice)
        {
            Console.WriteLine($"Ціна: ${Price}");
        }

        Console.WriteLine($"Обчислювальна властивість: {CombinedInfo}");
    }
    private int GetGenreNumericValue(Genre genre)
    {
        return (int)genre;
    }

    // Метод для получения Genre из числового представления
    private Genre GetGenreFromNumericValue(int numericValue)
    {
        return (Genre)numericValue;
    }

    // Метод сохранения в CSV
    public void SaveToCsv(List<Comic> comicsList, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Comic comic in comicsList)
            {
                string line = $"{comic.Name},{GetGenreNumericValue(comic.Genre)},{comic.YearReleased},{comic.Author},{comic.Price}";
                writer.WriteLine(line);
            }
        }
    }

    // Метод загрузки из CSV
    public static Comic FromCsv(string csvLine)
    {
        string[] values = csvLine.Split(',');

        // Проверка наличия 6 элементов в массиве
        if (values.Length != 6)
        {
            // Обработка ошибки, например, возврат null или генерация исключения
            return null;
        }

        // Распаковка значений из массива
        string name = values[0];
        Genre genre;
        DateTime yearReleased;
        string author = values[3];
        double price;

        // Пример преобразования строки в перечисление Genre
        if (Enum.TryParse(values[1], out genre) &&
            DateTime.TryParse(values[2], out yearReleased) &&
            double.TryParse(values[4], out price))
        {
            // Создание объекта Comic с полученными значениями
            return new Comic(name, genre, yearReleased, author, price);
        }
        else
        {
            // Обработка ошибки в преобразовании, например, возврат null или генерация исключения
            return null;
        }
    }
}