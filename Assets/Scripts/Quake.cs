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
    public Area Area;

    public void DoAt(AxialCoordinate origin, HexRotation orientation)
    {
        HexMap map = FindObjectOfType<HexMap>();
        map.StartCoroutine(DoQuake(origin, orientation));
    }

    IEnumerator DoQuake(AxialCoordinate origin, HexRotation orientation)
    {
        IEnumerable<AxialCoordinate> coords = Area.From(origin, orientation).OrderBy(c => c.Distance(origin));
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
}
