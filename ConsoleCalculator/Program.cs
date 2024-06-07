using ConsoleCalculator.Core.Services;

// Инициализация компонентов калькулятора
var tokenizer = new Tokenizer();
var parser = new Parser();
var evaluator = new Evaluator();
var calculator = new Calculator(tokenizer, parser, evaluator);

// Основной цикл программы
while (true)
{
    Console.WriteLine("Введите математическое выражение или 'Выход' для завершения:");
    string expression = Console.ReadLine();

    // Выход из программы при вводе 'Выход'
    if (expression.Equals("Выход", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    try
    {
        // Вычисление и вывод результата
        double result = calculator.Calculate(expression);
        Console.WriteLine($"Результат: {result}");
    }
    catch (Exception ex)
    {
        // Вывод ошибки
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

}
