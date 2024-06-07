using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class EvaluatorTests
    {
        [Theory]
        [InlineData("1 + 2", 3)] // Простое выражение суммы
        [InlineData("2 * (3 - 1)", 4)] // Выражение с использованием скобок
        [InlineData("10 / 2", 5)] // Выражение с делением
        [InlineData("5 - 2", 3)] // Выражение с вычитанием
        [InlineData("2.5 * 3", 7.5)] // Выражение с вещественным числом
        [InlineData("1 + 2 * 3 - 4 / 2", 5)] // Комплексное выражение
        public void Evaluate_ValidExpression_ReturnsCorrectResult(string expression, double expectedResult)
        {
            // Arrange
            IEvaluator evaluator = new Evaluator();
            ITokenizer tokenizer = new Tokenizer();
            IParser parser = new Parser();
            var tokens = tokenizer.Tokenize(expression);
            var ast = parser.Parse(tokens);

            // Act
            double result = evaluator.Evaluate(ast);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
