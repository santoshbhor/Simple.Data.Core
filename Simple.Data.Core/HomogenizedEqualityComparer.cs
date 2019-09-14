﻿using System.Collections.Generic;
using Simple.Data.Extensions;

namespace Simple.Data
{
    public class HomogenizedEqualityComparer : IEqualityComparer<string>
    {
        public static readonly HomogenizedEqualityComparer DefaultInstance = new HomogenizedEqualityComparer();

        public bool Equals(string x, string y)
        {
            return ReferenceEquals(x, y.Homogenize())
                   || x.Homogenize() == y.Homogenize();
        }

        public int GetHashCode(string obj)
        {
            return obj.Homogenize().GetHashCode();
        }
    }
}