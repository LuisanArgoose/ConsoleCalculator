using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleCalculator.Core.Models
{
    // Класс токена
    public class Token(TokenType type, string value)
    {
        public TokenType Type { get; } = type;
        public string Value { get; } = value;
    }
}
