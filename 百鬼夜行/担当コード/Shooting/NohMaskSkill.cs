using UnityEngine;

public class NohMaskSkill : BulletSkill
{
    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        Debug.Log("NohMaskSkill 初期化完了");
    }

    protected override void Skill()
    {
    }

    protected override void SubSkill()
    {
        // 必要に応じて実装
    }

    public override void Remove()
    {
        base.Remove();
        Debug.Log("[NohMaskSkill] イベント解除");

    }
}
