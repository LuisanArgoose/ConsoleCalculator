using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Services;
using System;

namespace ConsoleCalculator.Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData("1 + 2", 3)] // Простое выражение суммы
        [InlineData("2 * 3", 6)] // Простое выражение умножения
        [InlineData("5 - 2", 3)] // Простое выражение вычитания
        [InlineData("10 / 2", 5)] // Простое выражение деления
        [InlineData("1 + 2 * 3 - 4 / 2", 5)] // Комплексное выражение
        [InlineData("10 + (2 * 3) - (4 / 2)", 14)] // Выражение с использованием скобок
        [InlineData("(5.5 + 2 * 3) - (4 / 2) + 7.8 * 2.2", 26.66)] // Выражение с вещественными числами
        public void Calculate_ValidExpression_ReturnsCorrectResult(string expression, double expectedResult)
        {
            // Arrange
            ITokenizer tokenizer = new Tokenizer();
            IParser parser = new Parser();
            IEvaluator evaluator = new Evaluator();
            ICalculator calculator = new Calculator(tokenizer, parser, evaluator);

            // Act
            double result = calculator.Calculate(expression);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}