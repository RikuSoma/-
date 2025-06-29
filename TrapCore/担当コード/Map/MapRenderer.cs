using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [SerializeField] private MapDataManager mapDataManager;
    [SerializeField] private ICellRenderer wallRenderer;
    [SerializeField] private ICellRenderer floorRenderer;
    [SerializeField] private float cellSize = 1.0f;

    public void RenderMap()
    {
        for (int y = 0; y < mapDataManager.GetHeight(); y++)
        {
            for (int x = 0; x < mapDataManager.GetWidth(); x++)
            {
                int cellType = mapDataManager.GetCellData(x, y);
                Vector2 position = new Vector2(x * cellSize, -y * cellSize); // オフセット削除で左下基準

                switch (cellType)
                {
                    case 0:
                        floorRenderer.RenderCell(position);
                        break;
                    case 1:
                        wallRenderer.RenderCell(position);
                        break;
                }
            }
        }
    }
}
