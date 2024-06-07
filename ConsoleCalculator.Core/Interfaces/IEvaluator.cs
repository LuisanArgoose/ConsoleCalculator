using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Interfaces
{
    // Интерфейс для вычислителя
    public interface IEvaluator
    {
        double Evaluate(Node expression);
    }
}
