using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityEngine
{
    public static class TransformExtension
    {
        /// <summary>
        /// Sets the transforms rotation so it is looking in the given direction
        /// </summary>
        public static void LookAt(this Transform transform, HexDirection direction)
        {
            // Set rotation
            transform.rotation = direction.ToRotation();
        }
    }
}
