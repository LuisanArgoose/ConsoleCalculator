using ConsoleCalculator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCalculator.Tests
{
    public class NodeComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node x, Node y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Token.Equals(y.Token) &&
                   Equals(x.Left, y.Left) &&
                   Equals(x.Right, y.Right);
        }

        public int GetHashCode(Node obj)
        {
            int hash = obj.Token.GetHashCode();
            if (obj.Left != null)
                hash = hash * 31 + obj.Left.GetHashCode();
            if (obj.Right != null)
                hash = hash * 31 + obj.Right.GetHashCode();
            return hash;
        }
    }
}

