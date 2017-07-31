using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quake.asset", menuName = "Quake")]
public class Quake : ScriptableObject
{
    public List<Shape> Shapes;
    public Line testLine;
    public Ring testRing;
    public bool line;

    public float Intensity;
    public float Duration;
    public float PropogationDelay = 0.1f;

    public void DoAt(AxialCoordinate origin)
    {
        if (line)
            DoAt(testLine, origin);
        else
            DoAt(testRing, origin);
    }

    private void DoAt(Shape shape, AxialCoordinate origin)
    {
        // nTiers = last coord distance to centre
        // tier = coord distance to centre
        IEnumerable<AxialCoordinate> coords = shape.From(origin);
        int count = coords.Count();
        int firstTier = coords.First().Distance(origin + shape.Offset);
        int lastTier = coords.Last().Distance(origin + shape.Offset);
        int tiers = lastTier - firstTier;

        HexMap map = FindObjectOfType<HexMap>();
        foreach (var coord in coords)
        {
            HexCell cell = map[coord];
            if (!cell)
                break;
            int tier = coord.Distance(origin + shape.Offset);
            float t = (tier - firstTier) / (float)tiers;
            float delay = (tier - firstTier) * PropogationDelay;

            cell.Quake(Intensity * (1 - t), Duration - delay, delay);
        }
    }
}
