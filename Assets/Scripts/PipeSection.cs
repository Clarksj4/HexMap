using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PipeSection : MonoBehaviour
{
    public virtual void Set(HexCell cell, HexDirection direction)
    {
        // Size
        Vector3 scale = new Vector3(transform.localScale.x, transform.localScale.y, cell.HexMesh.InnerRadius / 2);
        transform.localScale = scale;

        // Orientation
        transform.LookAt(direction);

        // Position
        transform.position = cell.Position + (direction.ToNormalizedVector() * (scale.z));
    }
}
