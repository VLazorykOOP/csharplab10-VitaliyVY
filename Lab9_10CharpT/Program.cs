using System;

class StringReverseException : Exception
{
    public StringReverseException() : base("Помилка при знаходженні зворотнього рядка") { }

    public StringReverseException(string message) : base(message) { }

    public StringReverseException(string message, Exception innerException) : base(message, innerException) { }
}

class Horse
{
    public event EventHandler Born;
    public event EventHandler Injured;
    public event EventHandler Fed;
    public event EventHandler Died;

    private int health;
    private int hunger;
    private Random random;

    public Horse()
    {
        health = 100;
        hunger = 0;
        random = new Random();
    }

    protected virtual void OnBorn(EventArgs e)
    {
        Born?.Invoke(this, e);
    }

    protected virtual void OnInjured(EventArgs e)
    {
        Injured?.Invoke(this, e);
        hunger += random.Next(10, 30); // Збільшення голоду при травмі
    }

    protected virtual void OnFed(EventArgs e)
    {
        Fed?.Invoke(this, e);
        hunger -= random.Next(10, 30); // Віднімання від голоду при годуванні
        if (hunger < 0) hunger = 0; // Голод не може бути менше нуля
    }

    protected virtual void OnDied(EventArgs e)
    {
        Died?.Invoke(this, e);
    }

    public void SimulateLife(int days)
    {
        for (int day = 1; day <= days; day++)
        {
            Console.WriteLine($"День {day}:");
            if (day == 1)
            {
                OnBorn(EventArgs.Empty);
            }
            else
            {
                if (random.NextDouble() < 0.2)
                {
                    health -= random.Next(10, 30);
                    OnInjured(EventArgs.Empty);
                }
                if (random.NextDouble() < 0.5)
                {
                    OnFed(EventArgs.Empty);
                }
                Console.WriteLine($"Здоров'я коня: {health}, Голод коня: {hunger}");

                if (hunger > 30)
                {
                    OnDied(EventArgs.Empty);
                    Console.WriteLine("Кінь помер :(");
                    return;
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1./ Перевірити чи є рядок зворотнім до іншого");
            Console.WriteLine("2.. Моделювання життя коня");
            Console.WriteLine("0. Вихід\n");
            Console.Write("Виберіть опцію: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Некоректний ввід. Спробуйте ще раз.");
                continue;
            }

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Програма завершила роботу.");
                    return;
                case 1:
                    CheckStringReverse();
                    break;
                case 2:
                    SimulateHorseLife();
                    break;
                default:
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void CheckStringReverse()
    {
        try
        {
            Console.Write("Введіть перший рядок: ");
            string s1 = Console.ReadLine();
            Console.Write("Введіть другий рядок: ");
            string s2 = Console.ReadLine();

            if (IsReverse(s1, s2))
            {
                Console.WriteLine($"{s2} є зворотнім до {s1}.");
            }
            else
            {
                Console.WriteLine($"{s2} не є зворотнім до {s1}.");
            }
        }
        catch (StringReverseException ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Сталася помилка при приведенні типів: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася невідома помилка: {ex.Message}");
        }
    }

    static void SimulateHorseLife()
    {
        try
        {
            Console.Write("Введіть тривалість моделювання (кількість днів): ");
            int days = int.Parse(Console.ReadLine());

            Horse horse = new Horse();
            horse.Born += (sender, e) => Console.WriteLine("Народився новий конь!");
            horse.Injured += (sender, e) => Console.WriteLine("Коня травмувався.");
            horse.Fed += (sender, e) => Console.WriteLine("Коня погодовано.");
            horse.Died += (sender, e) => Console.WriteLine("Коня помер.");

            horse.SimulateLife(days);
        }
        catch (FormatException)
        {
            Console.WriteLine("Некоректний формат числа днів.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася невідома помилка: {ex.Message}");
        }
    }

    static bool IsReverse(string s1, string s2)
    {
        if (s1.Length != s2.Length)
        {
            throw new StringReverseException("Рядки мають різну довжину");
        }

        try
        {
            char[] arr1 = s1.ToCharArray();
            Array.Reverse(arr1);
            string reversed = new string(arr1);
            return reversed.Equals(s2);
        }
        catch (Exception ex)
        {
            throw new StringReverseException("Помилка при обробці рядків", ex);
        }
    }
}
