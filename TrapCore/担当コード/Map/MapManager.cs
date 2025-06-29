// MapManager�i�}�b�v�̊Ǘ��j
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    [SerializeField] private TextAsset mapFile;
    [SerializeField] private TrapManager trapManager = new TrapManager();
    [SerializeField] private MapDataManager mapDataManager;
    [SerializeField] private MapRenderer mapRenderer;

    private Dictionary<Vector2, IEventTrigger> eventTriggers = new Dictionary<Vector2, IEventTrigger>();

    void Start()
    {
        // �}�b�v�f�[�^�̓ǂݍ���
        mapDataManager.LoadMapData(mapFile);
        mapRenderer.RenderMap();

    }

    public void CheckPlayerPosition(Vector2 playerPos)
    {
        if (eventTriggers.ContainsKey(playerPos))
        {
            eventTriggers[playerPos].Trigger();
        }
    }
}