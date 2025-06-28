using UnityEngine;

public class JumpingState : BaseState
{
    public JumpingState(BlueBoundEnemyMovement enemy) : base(enemy) { }

    public override void UpdateState()
    {

        // 落下中で地面に接地したら待機状態へ
        if (enemy.Rb.linearVelocity.y < 0 && enemy.IsGrounded())
        {
            enemy.ChangeState(enemy.idleState);
        }
    }
}