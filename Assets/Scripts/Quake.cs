using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quake.asset", menuName = "Quake")]
public class Quake : ScriptableObject
{
    // TODO: figure out a way to serialize this, expose it in the inspector etc
    public List<Shape> Shapes = new List<Shape>();
    public Line testLine;
    public Ring testRing;

    public float Intensity;
    public float Duration;
    public float PropogationDelay = 0.1f;

    public void DoAt(AxialCoordinate origin)
    {
        Shapes.Add(testRing);
        Shapes.Add(testLine);

        foreach (var shape in Shapes)
            DoAt(shape, origin);
    }

    private void DoAt(Shape shape, AxialCoordinate origin)
    {
        IEnumerable<AxialCoordinate> coords = shape.From(origin);
        int count = coords.Count();
        int firstTier = coords.First().Distance(origin + shape.Offset);
        int lastTier = coords.Last().Distance(origin + shape.Offset);
        int tiers = lastTier - firstTier;

        HexMap map = FindObjectOfType<HexMap>();
        foreach (var coord in coords)
        {
            HexCell cell = map[coord];
            if (cell)
            {
                int tier = coord.Distance(origin + shape.Offset);
                float t = (tier - firstTier) / (float)tiers;
                float delay = (tier - firstTier) * PropogationDelay;

                cell.Quake(Intensity * (1 - t), Duration - delay, delay);
            }
        }
    }
}
