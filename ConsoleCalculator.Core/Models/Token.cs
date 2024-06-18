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
        public override bool Equals(object obj)
        {
            if (obj is Token other)
            {
                return Type == other.Type && Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }
    }
}
