using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Interfaces
{
    // Интерфейс для парсера
    public interface IParser
    {
        Node Parse(IEnumerable<Token> tokens);
    }
}
