using ConsoleCalculator.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Operations
{
    public class MultiplyOperation : IOperation
    {
        public string GetOperator() => "*"; 
        public int GetPriority() => 2;
        public double Execute(double left, double right) => left * right;
    }
}
