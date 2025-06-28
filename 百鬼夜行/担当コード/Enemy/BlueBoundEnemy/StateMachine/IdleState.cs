using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(BlueBoundEnemyMovement enemy) : base(enemy) { }

    public override void UpdateState()
    {
        // 一定時間経過し、かつ地面にいたら準備状態へ
        stateTimer += Time.deltaTime;
        if (stateTimer > enemy.idleDuration && enemy.IsGrounded())
        {
            enemy.ChangeState(enemy.preparingState);
        }
    }
}