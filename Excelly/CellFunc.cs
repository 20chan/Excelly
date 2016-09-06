using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelly
{
    public static class CellFunc
    {
        #region Random
        private static Random _rand = new Random((int)DateTime.Now.Ticks);
        public static int RandInt()
        {
            return _rand.Next();
        }
        public static int RandInt(int max)
        {
            return _rand.Next(max);
        }
        public static int RandInt(int min, int max)
        {
            return _rand.Next(min, max);
        }

        public static double RandDouble()
        {
            return _rand.NextDouble();
        }
        public static double RandDouble(double max)
        {
            return _rand.NextDouble() * max;
        }
        public static double RandDouble(double min, double max)
        {
            return min + max == 0 ? 2 * _rand.NextDouble() * max - min :
                _rand.NextDouble() * (max + min) - min;
        }
        #endregion

        public static double Sum(double[] values)
        {
            return values.Sum();
        }

        public static double Average(double[] values)
        {
            return values.Average();
        }

        public static double Variance(double[] values)
        {
            double average = values.Average();
            return values.Select(v => Math.Pow(average - v, 2)).Average();
        }

        public static double Deviation(double[] values)
        {
            return Math.Sqrt(Variance(values));
        }
    }
}
