using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Interfaces
{
    public interface IOperation
    {
        double Execute(double left, double right);
        string GetOperator();
        int GetPriority();
    }
}
