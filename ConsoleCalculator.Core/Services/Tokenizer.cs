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
        // Реализация токенизатора
        public IEnumerable<Token> Tokenize(string expression)
        {
            var tokens = new List<Token>();
            int length = expression.Length;

            for (int i = 0; i < length; i++)
            {
                char c = expression[i];

                if (char.IsWhiteSpace(c))
                    continue;

                if (char.IsDigit(c) || c == '.')
                {
                    string number = string.Empty;
                    while (i < length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        number += expression[i];
                        i++;
                    }
                    i--;
                    tokens.Add(new Token(TokenType.Number, number));
                }
                else if ("+-*/()".Contains(c))
                {
                    tokens.Add(new Token(TokenType.Operator, c.ToString()));
                }
                else
                {
                    throw new Exception($"Unexpected character: {c}");
                }
            }

            return tokens;
        }
    }
}
