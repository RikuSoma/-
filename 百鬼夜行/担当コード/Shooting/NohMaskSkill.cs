using UnityEngine;

public class NohMaskSkill : BulletSkill
{
    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        Debug.Log("NohMaskSkill ����������");
    }

    protected override void Skill()
    {
    }

    protected override void SubSkill()
    {
        // �K�v�ɉ����Ď���
    }

    public override void Remove()
    {
        base.Remove();
        Debug.Log("[NohMaskSkill] �C�x���g����");

    }
}
