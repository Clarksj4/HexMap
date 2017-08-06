using System;
using System.Collections.Generic;
using System.Linq;

public class Area
{
    public List<KeyValuePair<int, Shape>> Shapes { get; private set; }

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

    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin)
    {
        return Shapes.Select(kvp => kvp.Value).SelectMany(s => s.From(origin));
    }
}
