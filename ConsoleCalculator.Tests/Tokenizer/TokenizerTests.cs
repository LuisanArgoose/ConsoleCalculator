using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using ConsoleCalculator.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleCalculator.Tests
{
    public class TokenizerTests : TestBase
    {
        [Theory]
        [InlineData("1 + 2", new object[] { "1", "+", "2" })] // Токенизация сложения
        [InlineData("5 - 2", new object[] { "5", "-", "2" })] // Токенизация вычитания
        [InlineData("10 / 2", new object[] { "10", "/", "2" })] // Токенизация деления
        [InlineData("(10 * 2)", new object[] { "(", "10", "*", "2", ")" })] // Токенизация умножения
        [InlineData("2.5 * 3", new object[] { "2.5", "*", "3" })] // Тоекнизация выражения с вещественным числом
        [InlineData("-18 + 21", new object[] { "-18", "+", "21" })] // Тоекнизация выражения с отрицательным числом вначале
        [InlineData("9 + -18 + 21", new object[] { "9", "+", "-18", "+", "21" })] // Тоекнизация выражения с отрицательным числом 
        [InlineData("(7 + 4)", new object[] { "(", "7", "+", "4", ")" })] // Токенизация выражения со скобками 
        [InlineData("((7 + 4) * 2)", new object[] { "(", "(", "7", "+", "4", ")", "*", "2", ")" })] // Токенизация выражения с вложенными скобками 
        [InlineData("(7 + 4) * (2 + 4)", new object[] { "(", "7", "+", "4", ")", "*", "(", "2", "+", "4", ")" })] // Токенизация выражения с несколькими скобками 
        [InlineData("  -     2   *   (   3   -   -   1   )   /   7    +   3   ", new object[] { "-2", "*", "(", "3", "-", "-1", ")", "/", "7", "+", "3" })] // Токенизация выражения с большим количеством пробелов
        [InlineData("2 * (3 - 1) / 7 + 3", new object[] { "2", "*", "(", "3", "-", "1", ")", "/", "7", "+", "3" })] // Токенизация комплексного выражения 

        public void Tokenize_ValidExpressions_ReturnsCorrectTokens(string expression, object[] expectedTokens)
        {
            // Arrange
            var tokenizer = GetService<Tokenizer>();

            // Act
            var tokens = tokenizer.Tokenize(expression);

            // Assert
            AssertTokenValues(tokens, expectedTokens);
        }

        [Theory]
        [InlineData("1", "No operations found")] // Отсутствуют операции
        [InlineData("1 + d ", "Unexpected character 'd' at position 4")] // Неизвестный символ в строке
        [InlineData("", "No token found")] // Отсутствие символов в строке
        [InlineData("      ", "No token found")] // Только пробелы в строке
        [InlineData("2 * 3 /", "Invalid expression: insufficient operands for operator '/'")] // Отсутсвие числа после символа операции
        [InlineData("(2 * 3 -)", "Invalid expression: insufficient operands for operator '-'")] // Скобка после символа операции
        [InlineData("7 / 3)", "Invalid expression: opening parenthesis not found")] // Отсутствует открывающая скобка
        [InlineData("(7 * 12", "Invalid expression: closing parenthesis not found")] // Отсутствует закрывающая скобка
        [InlineData("+ 6 - 9", "Invalid expression: insufficient operands for operator '+'")] // Символ операции вначале
        [InlineData("(6 - (9 + 8)", "Invalid expression: closing parenthesis not found")] // Отсутствует одна закрывающая скобка
        public void Tokenize_InvalidExpressions_ThrowsException(string expression, string expectedErrorMessage)
        {
            // Arrange
            var tokenizer = GetService<Tokenizer>();

            // Act
            Action tokenizeAction = () => tokenizer.Tokenize(expression);

            // Assert
            var exception = Assert.Throws<ArgumentException>(tokenizeAction);
            Assert.Contains(expectedErrorMessage, exception.Message);
        }

        private void AssertTokenValues(IEnumerable<Token> tokens, object[] expectedValues)
        {
            var tokenList = new List<Token>(tokens);
            Assert.Equal(expectedValues.Length, tokenList.Count);

            for (int i = 0; i < expectedValues.Length; i++)
            {
                var expectedValue = expectedValues[i].ToString();
                Assert.Equal(expectedValue, tokenList[i].Value.ToString());
            }
        }
    }
}
