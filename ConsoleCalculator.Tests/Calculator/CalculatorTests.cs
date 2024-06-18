using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Operations;
using ConsoleCalculator.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleCalculator.Tests
{
    public class CalculatorTests : TestBase
    {
        [Theory]
        [InlineData("1 + 2", 3)] // Результат сложения
        [InlineData("5 - 2", 3)] // Результат вычитания
        [InlineData("10 / 2", 5)] // Результат деления
        [InlineData("(10 * 2)", 20)] // Результат умножения
        [InlineData("2.5 * 3", 7.5)] // Результат выражения с вещественным числом
        [InlineData("-18 + 21", 3)] // Результат выражения с отрицательным числом вначале
        [InlineData("9 + -18 + 21", 12)] // Результат выражения с отрицательным числом 
        [InlineData("(7 + 4)", 11)] // Результат выражения со скобками 
        [InlineData("((7 + 4) * 2)", 22)] // Результат выражения с вложенными скобками 
        [InlineData("(7 + 4) * (2 + 4)", 66)] // Результат выражения с несколькими скобками 
        [InlineData("   -    2   * (  3   -   -1  )   / 7   +    3", 1.8571428571)] // Результат выражения с большим количеством пробелов и отрицательным числом
        [InlineData("2 * (3 - 1) / 7 + 3", 3.5714285714)] // Результат комплексного выражения 
        public void Calculate_ValidExpression_ReturnsCorrectResult(string expression, double expectedResult)
        {
            // Arrange
            var calculator = GetService<Calculator>();

            // Act
            double result = calculator.Calculate(expression);

            // Assert
            Assert.Equal(expectedResult, Math.Round(result, 10));

        }
        [Theory]
        [InlineData("1", "No operations found")] // Отсутствуют операции
        [InlineData("1 + d ", "Unexpected character 'd' at position 4")] // Неизвестный символ в строке
        [InlineData("", "No token found")] // Отсутствие символов в строке
        [InlineData("      ", "No token found")] // Только пробелы в строке
        [InlineData("5 / 0", "Division by zero")] // Деление на ноль
        [InlineData("2 * 3 /", "Invalid expression: insufficient operands for operator '/'")] // Отсутсвие числа после символа операции
        [InlineData("(2 * 3 -)", "Invalid expression: insufficient operands for operator '-'")] // Скобка после символа операции
        [InlineData("7 / 3)", "Invalid expression: opening parenthesis not found")] // Отсутствует открывающая скобка
        [InlineData("(7 * 12", "Invalid expression: closing parenthesis not found")] // Отсутствует закрывающая скобка
        [InlineData("+ 6 - 9", "Invalid expression: insufficient operands for operator '+'")] // Символ операции вначале
        [InlineData("(6 - (9 + 8)", "Invalid expression: closing parenthesis not found")] // Отсутствует одна закрывающая скобка
        public void Calculate_InvalidExpressions_ThrowsException(string expression, string expectedErrorMessage)
        {
            // Arrange
            var calculator = GetService<Calculator>();

            // Act
            Action calculateAction = () => calculator.Calculate(expression);

            // Assert
            var exception = Assert.Throws<ArgumentException>(calculateAction);
            Assert.Contains(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [InlineData("5^3", 125)] // Использование добавленной операции возведения в степень
        [InlineData("20 % 3", 2)] // Использование добавленной операции остатка от деления
        public void Calculate_NewOperation_ValidExpression_ReturnsCorrectResult(string expression, double expectedResult)
        {
            
            // Arrange
            var calculator = GetService<Calculator>();

            // Act
            double result = calculator.Calculate(expression);

            // Assert
            Assert.Equal(expectedResult, Math.Round(result, 10));

        }
        

    }
}