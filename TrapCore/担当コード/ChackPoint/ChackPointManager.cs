using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChackPointManager : MonoBehaviour
{
    [SerializeField] private List<ChackPoint> checkpoints;
    private ChackPoint activeCheckpoint;

    void Start()
    {
        foreach (var cp in checkpoints)
        {
            cp.Initialize(this);
        }
    }

    public void SetActiveCheckpoint(ChackPoint newCheckpoint)
    {
        foreach (var cp in checkpoints)
        {
            cp.SetFlag(false);
        }

        newCheckpoint.SetFlag(true);
        activeCheckpoint = newCheckpoint;

        GameData.Instance.SetPlayerInitPos(activeCheckpoint.transform.position);
    }

    public Vector2 GetCheckpointPosition()
    {
        return activeCheckpoint != null
            ? (Vector2)activeCheckpoint.transform.position
            : new Vector2(3.0f, -7.0f); // デフォルト
    }
}