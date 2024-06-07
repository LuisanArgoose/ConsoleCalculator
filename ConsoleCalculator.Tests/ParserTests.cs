using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using ConsoleCalculator.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class ParserTests
    {
        [Theory]
        [InlineData("1 + 2", 3)] // Простое выражение
        [InlineData("2 * (3 - 1)", 5)] // Выражение с использованием скобок
        [InlineData("10 / 2", 3)] // Выражение с делением
        [InlineData("5 - 2", 3)] // Выражение с вычитанием
        [InlineData("2.5 * 3", 3)] // Выражение с вещественным числом
        [InlineData("1 + 2 * (3 - 4) / 2", 9)] // Комплексное выражение
        public void Parse_ValidExpression_ReturnsCorrectNumberOfNodes(string expression, int expectedNodeCount)
        {
            // Arrange
            ITokenizer tokenizer = new Tokenizer();
            IParser parser = new Parser();

            // Act
            var tokens = tokenizer.Tokenize(expression);
            var ast = parser.Parse(tokens);

            // Assert
            Assert.NotNull(ast);
            Assert.Equal(expectedNodeCount, CountNodes(ast));
        }

        // Вспомогательный метод для подсчета количества узлов в AST
        private int CountNodes(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            // Учитываем текущий узел и рекурсивно обрабатываем его дочерние узлы
            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }
    }
}
