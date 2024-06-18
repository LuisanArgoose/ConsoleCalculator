using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Operations;
using ConsoleCalculator.Core.Services;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

// Регистрация операций
services.AddTransient<IOperation, AddOperation>();
services.AddTransient<IOperation, SubtractOperation>();
services.AddTransient<IOperation, MultiplyOperation>();
services.AddTransient<IOperation, DivideOperation>();

// Регистрация новых операций
services.AddTransient<IOperation, PowerOperation>();
services.AddTransient<IOperation, ModulusOperation>();

// Регистрация других сервисов
services.AddTransient<ITokenizer, Tokenizer>();
services.AddTransient<IParser, Parser>();
services.AddTransient<IEvaluator, Evaluator>();
services.AddTransient<Calculator>();

var serviceProvider = services.BuildServiceProvider();

var calculator = serviceProvider.GetService<Calculator>();

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
        Console.WriteLine($"{ex.Message}");
    }

}



