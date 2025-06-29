using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private List<TrapInfo> trapPositions;      // 㩂̈ʒu�Ǝ�ނƐݒ�
    [SerializeField] private List<GameObject> trapPrefabs;      // TrapKinds����Prefab��o�^����

    private Dictionary<Vector2, ITrap> placedTraps = new();

    void Start()
    {
        foreach (var info in trapPositions)
        {
            PlaceTrap(info.TrapPos, info.TrapKind, info);
        }
    }

    public void PlaceTrap(Vector2 position, TrapKinds kind, TrapInfo info = null)
    {
        int index = (int)kind;
        if (index >= trapPrefabs.Count) return;

        GameObject trapObj = Instantiate(trapPrefabs[index], position, Quaternion.identity);
        ITrap trap = trapObj.GetComponent<ITrap>();

        if (trap != null)
        {
            placedTraps[position] = trap;
        }

        // FakeNeedle�����ɐݒ��n��
        if (trap is FakeNeedle fakeNeedle && info?.fakeNeedleSettings != null)
        {
            fakeNeedle.SetConditions(info.fakeNeedleSettings.Conditions);
            fakeNeedle.SetJumpDirection(info.fakeNeedleSettings.JumpDirection);
        }
    }

    public void TriggerTrap(Vector2 position)
    {
        if (placedTraps.TryGetValue(position, out var trap))
        {
            trap.Activate();
        }
    }
}
