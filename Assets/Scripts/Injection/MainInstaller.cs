using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller<MainInstaller>
{
    public HexMap HexMapPrefab;
    public HexCell HexCellPrefab;

    public override void InstallBindings()
    {

        // Bind HexMapFactory
        Container.BindFactory<HexMap, HexMap.Factory>()
                 .FromComponentInNewPrefab(HexMapPrefab);

        // Bind HexCellFactory
        Container.BindFactory<HexCell, HexCell.Factory>()
                 .FromComponentInNewPrefab(HexCellPrefab);
    }

}
