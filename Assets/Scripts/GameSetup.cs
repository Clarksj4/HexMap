using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSetup : MonoBehaviour
{
    public int CellsWide;
    public int CellsDeep;
    public float OuterRadius;

    [Inject]
    private HexMap.Factory hexMapFactory;
    private HexMap hexMap;

    private void Start()
    {
        hexMap = hexMapFactory.Create(CellsWide, CellsDeep, OuterRadius);
    }
}
