using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using ConsoleCalculator.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class ParserTests : TestBase
    {
        [Theory]
        [MemberData(nameof(GetValidExpressions))]
        public void Parse_ValidExpression_ReturnsCorrectAst(IEnumerable<Token> tokens, Node expectedAst)
        {
            // Arrange
            var parser = GetService<Parser>();

            // Act 
            var ast = parser.Parse(tokens);

            // Assert
            Assert.Equal(expectedAst, ast, new NodeComparer());
        }

        public static IEnumerable<object[]> GetValidExpressions()
        {
            // Парсинг токенов сложения
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "1"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "2")
            },
            new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "1")), new Node(new Token(TokenType.Number, "2")))
            };

            // Парсинг токенов вычитания
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "5"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "2")
            },
            new Node(new Token(TokenType.Operator, "-"), new Node(new Token(TokenType.Number, "5")), new Node(new Token(TokenType.Number, "2")))
            };

            // Парсинг токенов деления
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "10"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Number, "2")
            },
            new Node(new Token(TokenType.Operator, "/"), new Node(new Token(TokenType.Number, "10")), new Node(new Token(TokenType.Number, "2")))
            };

            // Парсинг токенов умножения
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Number, "10"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.RightParenthesis, ")")
            },
            new Node(new Token(TokenType.Operator, "*"), new Node(new Token(TokenType.Number, "10")), new Node(new Token(TokenType.Number, "2")))
            };

            // Парсинг токенов выражения с вещественным числом
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "2.5"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "3")
            },
            new Node(new Token(TokenType.Operator, "*"), new Node(new Token(TokenType.Number, "2.5")), new Node(new Token(TokenType.Number, "3")))
            };

            // Парсинг токенов выражения с отрицательным числом вначале
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "-18"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "21")
            },
            new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "-18")), new Node(new Token(TokenType.Number, "21")))
            };

            // Парсинг токенов выражения с отрицательным числом
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "9"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "-18"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "21")
            },
            new Node(
                new Token(TokenType.Operator, "+"),
                new Node(
                    new Token(TokenType.Operator, "+"),
                    new Node(new Token(TokenType.Number, "9")),
                    new Node(new Token(TokenType.Number, "-18"))
                ),
                new Node(new Token(TokenType.Number, "21"))
            )
            };

            // Парсинг токенов выражения со скобками
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.RightParenthesis, ")")
            },
            new Node(new Token(TokenType.Operator, "+"), new Node(new Token(TokenType.Number, "7")), new Node(new Token(TokenType.Number, "4")))
            };

            // Парсинг токенов выражения с вложенными скобками
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.RightParenthesis, ")")
            },
            new Node(
                new Token(TokenType.Operator, "*"),
                new Node(
                    new Token(TokenType.Operator, "+"),
                    new Node(new Token(TokenType.Number, "7")),
                    new Node(new Token(TokenType.Number, "4"))
                ),
                new Node(new Token(TokenType.Number, "2"))
            )
            };

            // Парсинг токенов выражения с несколькими скобками
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "4"),
                new Token(TokenType.RightParenthesis, ")")
            },
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
            )
            };

            // Парсинг токенов комплексного выражения
            yield return new object[]
            {
            new List<Token>
            {
                new Token(TokenType.Number, "2"),
                new Token(TokenType.Operator, "*"),
                new Token(TokenType.LeftParenthesis, "("),
                new Token(TokenType.Number, "3"),
                new Token(TokenType.Operator, "-"),
                new Token(TokenType.Number, "1"),
                new Token(TokenType.RightParenthesis, ")"),
                new Token(TokenType.Operator, "/"),
                new Token(TokenType.Number, "7"),
                new Token(TokenType.Operator, "+"),
                new Token(TokenType.Number, "3")
            },
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
            )
            };
        }

        [Theory]
        [MemberData(nameof(GetInvalidExpressions))]
        public void Parse_InvalidExpression_ThrowsException(List<Token> tokens, string expectedErrorMessage)
        {
            // Arrange
            var parser = GetService<Parser>();

            // Act
            Action parseAction = () => parser.Parse(tokens);

            // Assert
            var exception = Assert.Throws<ArgumentException>(parseAction);
            Assert.Contains(expectedErrorMessage, exception.Message);
        }

        public static IEnumerable<object[]> GetInvalidExpressions()
        {
            // Отсутствие операций
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.Number, "2"),
                },
                "No operations found"
            };
            // Отсутствие токенов
            yield return new object[]
            {
                new List<Token>
                {
                    
                },
                "No token found"
            };

            // Отсутствие числа после символа операции
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.Number, "2"),
                    new Token(TokenType.Operator, "*"),
                    new Token(TokenType.Number, "3"),
                    new Token(TokenType.Operator, "/")
                },
                "Invalid expression: insufficient operands"
            };

            // Скобка после символа операции
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.LeftParenthesis, "("),
                    new Token(TokenType.Number, "2"),
                    new Token(TokenType.Operator, "*"),
                    new Token(TokenType.Number, "3"),
                    new Token(TokenType.Operator, "-"),
                    new Token(TokenType.RightParenthesis, ")")
                },
                "Invalid expression: insufficient operands"
            };

            // Отсутствует открывающая скобка
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.Number, "7"),
                    new Token(TokenType.Operator, "/"),
                    new Token(TokenType.Number, "3"),
                    new Token(TokenType.RightParenthesis, ")")
                },
                "Invalid expression: opening parenthesis not found"
            };

            // Отсутствует закрывающая скобка
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.LeftParenthesis, "("),
                    new Token(TokenType.Number, "7"),
                    new Token(TokenType.Operator, "*"),
                    new Token(TokenType.Number, "12")
                },
                "Invalid expression: closing parenthesis not found"
            };

            // Символ операции в начале
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.Operator, "+"),
                    new Token(TokenType.Number, "6"),
                    new Token(TokenType.Operator, "-"),
                    new Token(TokenType.Number, "9")
                },
                "Invalid expression: insufficient operands"
            };

            // Отсутствует одна закрывающая скобка
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.LeftParenthesis, "("),
                    new Token(TokenType.Number, "6"),
                    new Token(TokenType.Operator, "-"),
                    new Token(TokenType.LeftParenthesis, "("),
                    new Token(TokenType.Number, "9"),
                    new Token(TokenType.Operator, "+"),
                    new Token(TokenType.Number, "8")
                },
                "Invalid expression: closing parenthesis not found"
            };

            // Незарегистрированный оператор
            yield return new object[]
            {
                new List<Token>
                {
                    new Token(TokenType.Number, "2"),
                    new Token(TokenType.Operator, "#"),
                    new Token(TokenType.Number, "3")
                },
                "Unknown operator '#' at position 2"
            };


        }
    }
}
