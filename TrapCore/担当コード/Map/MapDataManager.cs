using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapData", menuName = "Map Data")]
public class MapDataManager : ScriptableObject
{
    private List<List<int>> mapData = new List<List<int>>();

    public void LoadMapData(TextAsset mapFile)
    {
        mapData.Clear();
        var lines = mapFile.text.Split('\n');

        foreach (var line in lines)
        {
            List<int> row = new List<int>();
            foreach (var c in line.Trim())
            {
                if (int.TryParse(c.ToString(), out int cellValue))
                {
                    row.Add(cellValue);
                }
            }
            mapData.Add(row);
        }

        Debug.Log($"Map Height: {GetHeight()}");
        Debug.Log($"Map Width: {GetWidth()}");
    }

    public int GetCellData(int x, int y)
    {
        if (y >= 0 && y < mapData.Count && x >= 0 && x < mapData[y].Count)
        {
            return mapData[y][x];
        }
        return -1; // �͈͊O
    }

    public int GetWidth() => mapData.Count > 0 ? mapData[0].Count : 0;
    public int GetHeight() => mapData.Count;
}
