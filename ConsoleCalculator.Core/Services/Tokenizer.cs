using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Services
{
    public class Tokenizer : ITokenizer
    {
        private readonly IEnumerable<IOperation> _operations;
        public Tokenizer(IEnumerable<IOperation> operations)
        {
            _operations = operations;
        }
        // Реализация токенизатора
        public IEnumerable<Token> Tokenize(string expression)
        {
            var tokens = new List<Token>();
            var numberBuilder = new StringBuilder();
            var operatorSymbols = new HashSet<string>(_operations.Select(op => op.GetOperator()));
            var parenthesisStack = new Stack<char>();

            for (int i = 0; i < expression.Length; i++)
            {
                var ch = expression[i];

                if (ch == ' ')
                {
                    continue;
                }
                else if (char.IsDigit(ch) || ch == '.')
                {
                    numberBuilder.Append(ch);
                }
                else if (ch == '-' && numberBuilder.Length == 0 && (tokens.Count == 0 || tokens.Last().Type == TokenType.Operator || tokens.Last().Type == TokenType.LeftParenthesis))
                {
                    numberBuilder.Append(ch); // Отрицательное число
                }
                else
                {
                    if (numberBuilder.Length > 0)
                    {
                        tokens.Add(new Token(TokenType.Number, numberBuilder.ToString()));
                        numberBuilder.Clear();
                    }

                    if (operatorSymbols.Contains(ch.ToString()))
                    {
                        if (tokens.Count == 0 || tokens.Last().Type == TokenType.Operator || tokens.Last().Type == TokenType.LeftParenthesis)
                        {
                            throw new ArgumentException($"Invalid expression: insufficient operands for operator '{ch}'");
                        }
                        tokens.Add(new Token(TokenType.Operator, ch.ToString()));
                    }
                    else if (ch == '(')
                    {
                        parenthesisStack.Push(ch);
                        tokens.Add(new Token(TokenType.LeftParenthesis, ch.ToString()));
                    }
                    else if (ch == ')')
                    {
                        if (tokens.Last().Type == TokenType.Operator)
                        {
                            throw new ArgumentException($"Invalid expression: insufficient operands for operator '{tokens.Last().Value}'");
                        }
                        if (parenthesisStack.Count == 0 || parenthesisStack.Pop() != '(')
                        {
                            throw new ArgumentException("Invalid expression: opening parenthesis not found");
                        }

                        tokens.Add(new Token(TokenType.RightParenthesis, ch.ToString()));
                    }
                    else
                    {
                        throw new ArgumentException($"Unexpected character '{ch}' at position {i}");
                    }
                }
            }


            if (numberBuilder.Length > 0)
            {
                tokens.Add(new Token(TokenType.Number,numberBuilder.ToString()));
            }
            if(tokens.Count < 1)
            {
                throw new ArgumentException("No token found");
            }
            if (tokens.Count < 2)
            {
                throw new ArgumentException("No operations found");
            }
            if (tokens.Count > 0 && tokens.Last().Type == TokenType.Operator)
            {
                throw new ArgumentException($"Invalid expression: insufficient operands for operator '{tokens.Last().Value}'");
            }
            if (parenthesisStack.Count > 0)
            {
                throw new ArgumentException("Invalid expression: closing parenthesis not found");
            }

            return tokens;
        }
    }
}
