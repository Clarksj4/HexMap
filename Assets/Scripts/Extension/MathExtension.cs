using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class Maths
    {
        public static T Wrap<T>(int index) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            Array values = Enum.GetValues(typeof(T));

            int count = values.Length;
            int wrappedIndex = Wrap(index, 0, count);

            return values.Cast<T>().ElementAt(wrappedIndex);
        }

        public static int Wrap(int number, int min, int max)
        {
            if (number >= max)
                number = min + (number - max);
            else if (number < min)
                number = max + (number - min);

            return number;
        }

        public static int Wrap0(int number, int max)
        {
            return Wrap(number, 0, max);
        }

        public static int Sign(bool positive)
        {
            return positive ? 1 : -1;
        }

        public static int Max(params int[] values)
        {
            return values.OrderByDescending(v => v).First();
        }
    }
}
