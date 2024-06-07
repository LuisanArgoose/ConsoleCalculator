using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class TokenizerTests
    {
        [Theory]
        [InlineData("1 + 2", 3)] // Простое выражение
        [InlineData("2 * (3 - 1)", 7)] // Выражение с использованием скобок
        [InlineData("10 / 2", 3)] // Выражение с делением
        [InlineData("5 - 2", 3)] // Выражение с вычитанием
        [InlineData("2.5 * 3", 3)] // Выражение с вещественным числом
        [InlineData("1 + 2 * 3 - 4 / 2", 9)] // Комплексное выражение
        public void Tokenize_ValidExpression_ReturnsCorrectNumberOfTokens(string expression, int expectedTokenCount)
        {
            // Arrange
            ITokenizer tokenizer = new Tokenizer();

            // Act
            var tokens = tokenizer.Tokenize(expression);

            // Assert
            Assert.NotNull(tokens);
            Assert.Equal(expectedTokenCount, tokens.Count());
        }
    }
}
