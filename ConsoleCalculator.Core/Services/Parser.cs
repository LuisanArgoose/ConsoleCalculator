using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Services
{
    // Реализация парсера
    public class Parser : IParser
    {
        private IEnumerator<Token> _tokens;
        private Token _currentToken;

        public Node Parse(IEnumerable<Token> tokens)
        {
            _tokens = tokens.GetEnumerator();
            Advance();
            return ParseExpression();
        }

        private void Advance()
        {
            if (_tokens.MoveNext())
            {
                _currentToken = _tokens.Current;
            }
            else
            {
                _currentToken = null;
            }
        }

        private Node ParseExpression()
        {
            var left = ParseTerm();

            // Обработка операторов + и -
            while (_currentToken != null && (_currentToken.Value == "+" || _currentToken.Value == "-"))
            {
                var op = _currentToken;
                Advance();
                var right = ParseTerm();
                left = new Node(op, left, right);
            }

            return left;
        }

        private Node ParseTerm()
        {
            var left = ParseFactor();

            // Обработка операторов * и /
            while (_currentToken != null && (_currentToken.Value == "*" || _currentToken.Value == "/"))
            {
                var op = _currentToken;
                Advance();
                var right = ParseFactor();
                left = new Node(op, left, right);
            }

            return left;
        }

        private Node ParseFactor()
        {
            if (_currentToken.Type == TokenType.Number)
            {
                var number = _currentToken;
                Advance();
                return new Node(number);
            }

            if (_currentToken.Value == "(")
            {
                Advance();
                var expression = ParseExpression();
                if (_currentToken.Value != ")")
                {
                    throw new Exception("Missing closing parenthesis");
                }
                Advance();
                return expression;
            }

            throw new Exception("Unexpected token");
        }
    }
}
