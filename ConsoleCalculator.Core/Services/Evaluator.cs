using ConsoleCalculator.Core.Interfaces;
using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Services
{
    // Реализация вычислителя
    public class Evaluator : IEvaluator
    {
        public double Evaluate(Node node)
        {
            // Если узел - число, вернуть его значение
            if (node.Token.Type == TokenType.Number)
            {
                return double.Parse(node.Token.Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            // Вычисление левого и правого подвыражения
            double left = Evaluate(node.Left);
            double right = Evaluate(node.Right);

            // Выполнение операции, представленной текущим узлом
            return node.Token.Value switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => left / right,
                _ => throw new Exception($"Unexpected operator: {node.Token.Value}")
            };
        }
    }
}
