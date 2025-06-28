using UnityEngine;

public class StuckRecoveryState : BaseState
{
    public StuckRecoveryState(BlueBoundEnemyMovement enemy) : base(enemy) { }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("壁が近いのでパニック・バックジャンプ！");
        enemy.Rb.linearVelocity = Vector3.zero;

        // プレイヤーの方向を計算
        Vector3 directionToPlayer = (enemy.TargetObject.transform.position - enemy.transform.position).normalized;
        directionToPlayer.y = 0;

        // プレイヤーとは逆の方向（後ろ）に、通常より少し強い力でジャンプ
        Vector3 backwardDirection = -directionToPlayer;
        Vector3 jumpForce = (backwardDirection * enemy.moveForce * 1.2f) + (Vector3.up * enemy.jumpForce);

        enemy.Rb.AddForce(jumpForce, ForceMode.VelocityChange);
    }

    public override void UpdateState()
    {
        // ジャンプ後は、地面に接地したらすぐにIdle状態に戻る
        if (enemy.Rb.linearVelocity.y < 0 && enemy.IsGrounded())
        {
            enemy.ChangeState(enemy.idleState);
        }
    }
}