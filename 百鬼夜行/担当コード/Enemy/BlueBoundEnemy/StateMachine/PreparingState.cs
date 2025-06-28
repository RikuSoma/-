using UnityEngine;

public class PreparingState : BaseState
{
    public PreparingState(BlueBoundEnemyMovement enemy) : base(enemy) { }

    public override void EnterState()
    {
        base.EnterState();
        // ���Ⴊ�ރR���[�`�����J�n����
        enemy.StartCoroutine(enemy.CrouchCoroutine());
    }

    public override void UpdateState()
    {
        // �����̓R���[�`�����S�����邽�߁A�����ł͉������Ȃ�
    }
}