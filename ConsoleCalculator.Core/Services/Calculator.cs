using ConsoleCalculator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Services
{
    // Реализация калькулятора, использующая токенизатор, парсер и вычислитель
    public class Calculator : ICalculator
    {
        private readonly ITokenizer _tokenizer;
        private readonly IParser _parser;
        private readonly IEvaluator _evaluator;

        public Calculator(ITokenizer tokenizer, IParser parser, IEvaluator evaluator)
        {
            _tokenizer = tokenizer;
            _parser = parser;
            _evaluator = evaluator;
        }

        public double Calculate(string expression)
        {
            // Токенизация строки выражения
            var tokens = _tokenizer.Tokenize(expression);
            // Парсинг токенов в AST
            var parsedExpression = _parser.Parse(tokens);
            // Вычисление значения выражения
            return _evaluator.Evaluate(parsedExpression);
        }
    }
}
