using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using ConsoleCalculator.Core.Operations;
using ConsoleCalculator.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class EvaluatorTests : TestBase
    {

        [Theory]
        [MemberData(nameof(GetValidExpressions))]
        public void Evaluate_ValidExpression_ReturnsCorrectResult(Node ast, double expectedResult)
        {
            // Arrange
            var evaluator = GetService<Evaluator>();

            // Act 
            var result = evaluator.Evaluate(ast);

            // Assert
            Assert.Equal(expectedResult, Math.Round(result,10));
        }

        public static IEnumerable<object[]> GetValidExpressions()
        {
            // Рассчет результата древа сложения
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "1")), new Node(new Token(TokenType.Number, "2"))),
                3.0
            };

            // Рассчет результата древа вычитания
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "-"), new Node(new Token(TokenType.Number, "5")), new Node(new Token(TokenType.Number, "2"))),
                3.0
            };

            // Рассчет результата древа деления
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "/"), new Node(new Token(TokenType.Number, "10")), new Node(new Token(TokenType.Number, "2"))),
                5.0
            };

            // Рассчет результата древа умножения
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "*"), new Node(new Token(TokenType.Number, "10")), new Node(new Token(TokenType.Number, "2"))),
                20.0
            };

            // Рассчет результата древа выражения с вещественным числом
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "*"), new Node(new Token(TokenType.Number, "2.5")), new Node(new Token(TokenType.Number, "3"))),
                7.5
            };

            // Рассчет результата древа выражения с отрицательным числом вначале
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "-18")), new Node(new Token(TokenType.Number, "21"))),
                3.0
            };

            // Рассчет результата древа выражения с отрицательным числом
            yield return new object[]
            {
                new Node(
                    new Token(TokenType.Operator, "+"),
                    new Node(
                        new Token(TokenType.Operator, "+"),
                        new Node(new Token(TokenType.Number, "9")),
                        new Node(new Token(TokenType.Number, "-18"))
                    ),
                    new Node(new Token(TokenType.Number, "21"))
                ),
                12.0
            };

            // Рассчет результата древа выражения со скобками
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "7")), new Node(new Token(TokenType.Number, "4"))),
                11.0
            };

            // Рассчет результата древа выражения с вложенными скобками
            yield return new object[]
            {
                new Node(
                    new Token(TokenType.Operator, "*"),
                    new Node(
                        new Token(TokenType.Operator, "+"),
                        new Node(new Token(TokenType.Number, "7")),
                        new Node(new Token(TokenType.Number, "4"))
                    ),
                    new Node(new Token(TokenType.Number, "2"))
                ),
                22.0
            };

            // Рассчет результата древа выражения с несколькими скобками
            yield return new object[]
            {
                new Node(
                    new Token(TokenType.Operator, "*"),
                    new Node(
                        new Token(TokenType.Operator, "+"),
                        new Node(new Token(TokenType.Number, "7")),
                        new Node(new Token(TokenType.Number, "4"))
                    ),
                    new Node(
                        new Token(TokenType.Operator, "+"),
                        new Node(new Token(TokenType.Number, "2")),
                        new Node(new Token(TokenType.Number, "4"))
                    )
                ),
                66.0
            };

            // Рассчет результата древа комплексного выражения
            yield return new object[]
            {
                new Node(
                    new Token(TokenType.Operator, "+"),
                    new Node(
                        new Token(TokenType.Operator, "/"),
                        new Node(
                            new Token(TokenType.Operator, "*"),
                            new Node(new Token(TokenType.Number, "2")),
                            new Node(
                                new Token(TokenType.Operator, "-"),
                                new Node(new Token(TokenType.Number, "3")),
                                new Node(new Token(TokenType.Number, "1"))
                            )
                        ),
                        new Node(new Token(TokenType.Number, "7"))
                    ),
                    new Node(new Token(TokenType.Number, "3"))
                ),
                3.5714285714
            };
        }

        [Theory]
        [MemberData(nameof(GetInvalidExpressions))]
        public void Evaluate_InvalidExpression_ThrowsException(Node ast, string expectedErrorMessage)
        {
            // Arrange
            var evaluator = GetService<Evaluator>();

            // Act
            Action evaluateAction = () => evaluator.Evaluate(ast);

            // Assert
            var exception = Assert.Throws<ArgumentException>(evaluateAction);
            Assert.Contains(expectedErrorMessage, exception.Message);
        }

        public static IEnumerable<object[]> GetInvalidExpressions()
        {
            // Дерево с неизвестным оператором
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "?"), new Node(new Token(TokenType.Number, "1")), new Node(new Token(TokenType.Number, "2"))),
                "Unexpected operator '?'"
            };

            // Дерево с отсутствующим левым операндом
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "+"), null, new Node(new Token(TokenType.Number, "2"))),
                "Invalid expression: missing left operand"
            };

            // Дерево с отсутствующим правым операндом
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "1")), null),
                "Invalid expression: missing right operand"
            };

            // Дерево с делением на ноль
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "/"), new Node(new Token(TokenType.Number, "1")), new Node(new Token(TokenType.Number, "0"))),
                "Division by zero"
            };

            // Дерево с некорректным числом
            yield return new object[]
            {
                new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "1.1.1")), new Node(new Token(TokenType.Number, "2"))),
                "Invalid number format '1.1.1'"
            };

        }
    }
}
