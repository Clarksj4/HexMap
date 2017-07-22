﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class MathExtension
    {
        public static int Wrap(int number, int min, int max)
        {
            if (number >= max)
                number = min + (number - max);
            else if (number < min)
                number = max + (number - min);

            return number;
        }
    }
}