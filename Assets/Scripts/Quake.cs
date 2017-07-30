using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quake.asset", menuName = "Quake")]
public class Quake : ScriptableObject
{
    // type: line, circle
    // foreach 
    public List<Shape> Shapes;
    public Line testLine;
    public Ring testRing;

    public float Intensity;
    public float Duration;
    public float PropogationDelay = 0.1f;

    public void Do(AxialCoordinate origin)
    {
        //if (testLine != null)
        //    DoLine(testLine, origin);
        if (testRing != null)
            DoRing(testRing, origin);
    }

    public void DoLine(Line line, AxialCoordinate origin)
    {
        int i = 0;
        int n = line.From(origin).Count();
        float durationIncrement = Duration / n;

        HexMap map = FindObjectOfType<HexMap>();
        foreach (var coord in line.From(origin))
        {
            HexCell cell = map[coord];
            if (!cell)
                break;

            float delay = i * PropogationDelay;
            float t = i / n;

            cell.Quake(Intensity * (1 - t), Duration - delay, delay);
            i++;
        }
    }

    public void DoRing(Ring ring, AxialCoordinate origin)
    {
        // nTiers = last coord distance to centre
        // tier = coord distance to centre

        int i = 0;
        int n = ring.From(origin).Count();
        int nTiers = ((n - 1) % 6) + 1;
        float durationIncrement = Duration / n;
        int previousTiersCount = 0;
        int tierSize = 1;
        int tier = 0;

        // 1, 6, 12, 18, 24, 30, 36
        // 1, 7, 19, 37
        // 0, 6, 18, 36, 60, 90, 126 ->  n
        // 0, 1,  3,  6, 10, 15,  21
        // 0, 1,  2,  3,  4,  5,   6
        // 0, 1,  2,  3,  4,  5,   6 ->  tier

        HexMap map = FindObjectOfType<HexMap>();
        foreach (var coord in ring.From(origin))
        {
            HexCell cell = map[coord];
            if (!cell)
                break;

            if (i == 0)
                cell.Quake(1, 3);
            else if (i < 7)
            {
                tier++;
                tierSize = tier * 6;

                float t = tier / nTiers;
                cell.Quake(Intensity * (1 - t), 3, ((i - 1) / 6) * durationIncrement);
            }

            else
            {


                // 6 - 17 / ? = 1
                // 1 = 0 - 5, 2 = 6 - 17
                // i - (n - i) / tierSize
                int tier = (i - (n - 1) / tierSize) + 1;
                cell.Quake(1, 3, ((i - 1) / 6) * durationIncrement);
            }

            i++;
        }
    }
}
