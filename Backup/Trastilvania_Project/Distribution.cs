using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trastilvania_Project
{
    class Distribution : Random
    {
        public int next_Poisson(double Lamda)
        {
            double elambda = Math.Exp(-1 * Lamda);
            double product = 1;
            int count = 0;
            int result = 0;
            while (product >= elambda)
            {
                product *= NextDouble();
                result = count;
                count++; // keep result one behind
            }
            return result;
        }
        public double nextExponential(double b)
        {
            double randx;
            double result;
            randx = NextDouble();
            result = -1 * b * Math.Log(randx);
            return result;
        }
    }
}
