using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Operations;
using ConsoleCalculator.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public abstract class TestBase
    {
        protected ServiceProvider ServiceProvider { get; private set; }

        protected TestBase()
        {
            var services = new ServiceCollection();

            // Регистрация базовых операций
            services.AddTransient<IOperation, AddOperation>();
            services.AddTransient<IOperation, SubtractOperation>();
            services.AddTransient<IOperation, MultiplyOperation>();
            services.AddTransient<IOperation, DivideOperation>();

            // Регистрация новых операций
            services.AddTransient<IOperation, PowerOperation>();
            services.AddTransient<IOperation, ModulusOperation>();

            // Регистрация других сервисов
            services.AddTransient<ITokenizer, Tokenizer>();
            services.AddTransient<Tokenizer>();
            services.AddTransient<IParser, Parser>();
            services.AddTransient<Parser>();
            services.AddTransient<IEvaluator, Evaluator>();
            services.AddTransient<Evaluator>();
            services.AddTransient<Calculator>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected T GetService<T>() => ServiceProvider.GetService<T>();
    }
}
