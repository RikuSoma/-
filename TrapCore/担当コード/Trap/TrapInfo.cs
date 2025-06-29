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
    public Vector2 JumpDirection = Vector2.up; // �C�ӕ���
}

[System.Serializable]
public class TrapInfo
{
    public Vector2 TrapPos;
    public TrapKinds TrapKind;

    // FakeNeedle�ɂ����K�v�Ȑݒ��t�^
    public FakeNeedleSettings fakeNeedleSettings;
}

