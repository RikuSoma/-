using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// トラップ描画用
public class TrapRenderer : ICellRenderer
{
    [SerializeField] private GameObject trapPrefab;  // トラップのプレハブ

    public override void RenderCell(Vector2 position)
    {
        Instantiate(trapPrefab, position, Quaternion.identity);
    }
}
