using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapKinds
{
    Cloud,
    DeathCloud,
    Needle,
    UpNeedle,
    FakeNeedle,
    FallingCelling,
    FallingWall,
    Door
}

[System.Serializable]
public class FakeNeedleSettings
{
    public List<FakeNeedle.DetectionCondition> Conditions;
    public Vector2 JumpDirection = Vector2.up; // 任意方向
}

[System.Serializable]
public class TrapInfo
{
    public Vector2 TrapPos;
    public TrapKinds TrapKind;

    // FakeNeedleにだけ必要な設定を付与
    public FakeNeedleSettings fakeNeedleSettings;
}

