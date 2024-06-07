using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Interfaces
{
    public interface ICalculator
    {
        // Интерфейс для калькулятора
        double Calculate(string expression);
    }
}
