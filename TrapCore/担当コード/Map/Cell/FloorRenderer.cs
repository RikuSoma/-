using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRenderer : ICellRenderer
{
    [SerializeField] private GameObject floorPrefab;

    public override void RenderCell(Vector2 position)
    {
        GameObject.Instantiate(floorPrefab, position, Quaternion.identity);
    }
}