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
        private readonly IEnumerable<IOperation> _operations;

        public Parser(IEnumerable<IOperation> operations)
        {
            _operations = operations;
        }

        public Node Parse(IEnumerable<Token> tokens)
        {
            var tokenList = tokens.ToList();

            // Handle empty input
            if (tokenList.Count == 0)
            {
                throw new ArgumentException("No token found");
            }

            return ParseExpression(tokenList, 0, tokenList.Count - 1);
        }

        private Node ParseExpression(List<Token> tokens, int start, int end)
        {
            Stack<Node> nodes = new Stack<Node>();
            Stack<IOperation> ops = new Stack<IOperation>();

            for (int i = start; i <= end; i++)
            {
                var token = tokens[i];

                if (token.Type == TokenType.Number)
                {
                    if(tokens.Count < 2)
                    {
                        throw new ArgumentException("No operations found");
                    }
                    nodes.Push(new Node(token));
                }
                else if (token.Type == TokenType.Operator || token.Type == TokenType.LeftParenthesis || token.Type == TokenType.RightParenthesis)
                {
                    if (token.Type == TokenType.LeftParenthesis)
                    {
                        int j = FindClosingParenthesis(tokens, i);
                        nodes.Push(ParseExpression(tokens, i + 1, j - 1));
                        i = j;
                    }
                    else if (token.Type == TokenType.RightParenthesis)
                    {
                        throw new ArgumentException("Invalid expression: opening parenthesis not found");
                    }
                    else
                    {
                        var operation = _operations.FirstOrDefault(op => op.GetOperator() == token.Value);
                        if (operation == null)
                        {
                            throw new ArgumentException($"Unknown operator '{token.Value}' at position {i + 1}");
                        }

                        while (ops.Count > 0 && ops.Peek().GetPriority() >= operation.GetPriority())
                        {
                            var opNode = CreateOperationNode(ops.Pop(), nodes);
                            nodes.Push(opNode);
                        }
                        ops.Push(operation);
                    }
                }
                else
                {
                    throw new ArgumentException($"Unexpected character '{token.Value}' at position {i + 1}");
                }
            }

            while (ops.Count > 0)
            {
                var opNode = CreateOperationNode(ops.Pop(), nodes);
                nodes.Push(opNode);
            }

            if (nodes.Count != 1)
            {
                throw new ArgumentException("Invalid expression: insufficient operands");
            }

            return nodes.Pop();
        }

        private Node CreateOperationNode(IOperation operation, Stack<Node> nodes)
        {
            if(nodes.Count < 2)
            {
                throw new ArgumentException("Invalid expression: insufficient operands");
            }
            var rightNode = nodes.Pop();
            var leftNode = nodes.Pop();
            var token = new Token(TokenType.Operator, operation.GetOperator());
            return new Node(token, leftNode, rightNode); 
        }

        private int FindClosingParenthesis(List<Token> tokens, int start)
        {
            int depth = 1;
            for (int i = start + 1; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.LeftParenthesis) depth++;
                if (tokens[i].Type == TokenType.RightParenthesis) depth--;
                if (depth == 0) return i;
            }
            throw new ArgumentException("Invalid expression: closing parenthesis not found");
        }
    }
}
