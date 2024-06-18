using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Core.Models
{
    // Класс узла дерева
    public class Node(Token token, Node left = null, Node right = null)
    {
        public Token Token { get; } = token;
        public Node Left { get; } = left;
        public Node Right { get; } = right;

        public override bool Equals(object obj)
        {
            if (obj is Node other)
            {
                return Token.Equals(other.Token) &&
                       Equals(Left, other.Left) &&
                       Equals(Right, other.Right);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Token, Left, Right);
        }

    }


}
