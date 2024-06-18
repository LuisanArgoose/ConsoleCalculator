using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Services
{
    // Реализация вычислителя
    public class Evaluator : IEvaluator
    {
        private readonly Dictionary<string, IOperation> _operations;

        public Evaluator(IEnumerable<IOperation> operations)
        {
            _operations = operations.ToDictionary(op => op.GetOperator());
        }

        public double Evaluate(Node node)
        {
            // Если узел - число, вернуть его значение
            if (node.Token.Type == TokenType.Number)
            {
                if (!double.TryParse(node.Token.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double number))
                {
                    throw new ArgumentException($"Invalid number format '{node.Token.Value}'");
                }
                return number;
            }

            // Выполнение операции, представленной текущим узлом
            if (node.Token.Type == TokenType.Operator)
            {
                if (node.Left == null)
                {
                    throw new ArgumentException("Invalid expression: missing left operand");
                }
                if (node.Right == null)
                {
                    throw new ArgumentException("Invalid expression: missing right operand");
                }

                // Вычисление левого и правого подвыражения
                double left = Evaluate(node.Left);
                double right = Evaluate(node.Right);

                if (!_operations.ContainsKey(node.Token.Value))
                {
                    throw new ArgumentException($"Unexpected operator '{node.Token.Value}'");
                }

                // Проверка деления на ноль
                if (node.Token.Value == "/" && right == 0)
                {
                    throw new ArgumentException("Division by zero");
                }

                return _operations[node.Token.Value].Execute(left, right);
            }

            throw new ArgumentException($"Unexpected token '{node.Token.Value}'");
        }
    }

}
