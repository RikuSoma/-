using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRenderer : ICellRenderer
{
    [SerializeField] private GameObject wallPrefab;

    public override void RenderCell(Vector2 position)
    {
        GameObject.Instantiate(wallPrefab, position, Quaternion.identity);
    }
}