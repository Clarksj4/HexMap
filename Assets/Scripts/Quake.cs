using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quake.asset", menuName = "Quake")]
public class Quake : ScriptableObject
{
    public float Intensity;
    public float Duration;
    public float PropogationDelay = 0.1f;
    public QuakeCell QuakeCellPrefab;

    //
    // TODO: serialize / deserialize to area. This stuff is here as a hacky way to serialize shapes
    //
    public Line[] Lines;
    public int[] LineIndices;

    public Ring[] Rings;
    public int[] RingIndices;

    // TODO: serialize / deserialize this
    private Area area = new Area();

    public void DoAt(AxialCoordinate origin, HexRotation orientation)
    {
        ConstructArea();

        HexMap map = FindObjectOfType<HexMap>();
        map.StartCoroutine(DoQuake(origin, orientation));

        //IEnumerable<AxialCoordinate> coords = area.From(origin, orientation).OrderBy(c => c.Distance(origin));
        //int tiers = coords.Last().Distance(origin) + 1;

        //HexMap map = FindObjectOfType<HexMap>();
        //foreach (var coord in coords)
        //{
        //    HexCell cell = map[coord];
        //    if (cell)
        //    {
        //        int tier = coord.Distance(origin);
        //        float t = tier / (float)tiers;
        //        float delay = tier * PropogationDelay;

        //        QuakeCellPrefab.DoAt(cell, Intensity * (1 - t), Duration - delay, delay);
        //    }
        //}
    }

    IEnumerator DoQuake(AxialCoordinate origin, HexRotation orientation)
    {
        IEnumerable<AxialCoordinate> coords = area.From(origin, orientation).OrderBy(c => c.Distance(origin));
        int tiers = coords.Last().Distance(origin) + 1;
        int previousTier = 0;

        HexMap map = FindObjectOfType<HexMap>();
        foreach (var coord in coords)
        {
            HexCell cell = map[coord];
            if (cell)
            {
                int tier = coord.Distance(origin);
                float t = tier / (float)tiers;
                float delay = tier * PropogationDelay;

                if (previousTier != tier)
                    yield return new WaitForSeconds(PropogationDelay);

                previousTier = tier;

                QuakeCellPrefab.DoAt(cell, Intensity * (1 - t), Duration - delay);
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
