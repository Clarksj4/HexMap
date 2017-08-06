using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quake.asset", menuName = "Quake")]
public class Quake : ScriptableObject
{
    //
    // TODO: serialize / deserialize to area. This stuff is here as a hacky way to serialize shapes
    //
    public Line[] Lines;
    public int[] LineIndices;

    public Ring[] Rings;
    public int[] RingIndices;

    // TODO: serialize / deserialize this
    private Area area = new Area();

    public float Intensity;
    public float Duration;
    public float PropogationDelay = 0.1f;

    public void DoAt(AxialCoordinate origin)
    {
        ConstructArea();

        IEnumerable<AxialCoordinate> coords = area.From(origin).OrderBy(c => c.Distance(origin));
        int tiers = coords.Last().Distance(origin) + 1;

        HexMap map = FindObjectOfType<HexMap>();
        foreach (var coord in coords)
        {
            HexCell cell = map[coord];
            if (cell)
            {
                int tier = coord.Distance(origin);
                float t = tier / (float)tiers;
                float delay = tier * PropogationDelay;

                cell.Quake(Intensity * (1 - t), Duration - delay, delay);
            }
        }
    }

    private void ConstructArea()
    {
        for (int i = 0; i < Lines.Length; i++)
            area.Add(LineIndices[i], Lines[i]);

        for (int i = 0; i < Rings.Length; i++)
            area.Add(RingIndices[i], Rings[i]);
    }
}
