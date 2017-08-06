using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Area
{
    public List<KeyValuePair<int, Shape>> Shapes { get; private set; }

    //
    // TODO: serialize / deserialize to area. This stuff is here as a hacky way to serialize shapes
    //
    public Line[] Lines;
    public int[] LineIndices;

    public Ring[] Rings;
    public int[] RingIndices;

    public Area()
    {
        Shapes = new List<KeyValuePair<int, Shape>>();
    }

    public void Add(int priority, Shape value)
    {
        Shapes.Add(new KeyValuePair<int, Shape>(priority, value));
        Shapes.OrderBy(kvp => kvp.Key);
    }

    public bool Remove(Shape shape)
    {
        int index = Shapes.FindIndex(kvp => kvp.Value == shape);
        if (index >= 0)
        {
            Shapes.RemoveAt(index);
            return true;
        }

        return false;
    }

    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin, HexRotation orientation)
    {
        ConstructArea();
        return Shapes.Select(kvp => kvp.Value).SelectMany(s => s.From(origin, orientation));
    }

    private void ConstructArea()
    {
        for (int i = 0; i < Lines.Length; i++)
            Add(LineIndices[i], Lines[i]);

        for (int i = 0; i < Rings.Length; i++)
            Add(RingIndices[i], Rings[i]);
    }
}
