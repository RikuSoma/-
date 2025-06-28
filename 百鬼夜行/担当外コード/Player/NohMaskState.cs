using UnityEngine;

public class NohMaskState : PlayerShooting
{
    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        shooting.SetBulletByName("NohMaskBullet");
        shooting.SetBulletSpeed(10f);
        SetShootInterval(0.2f);
        Debug.Log("NohMaskState èâä˙âªäÆóπ: PlayerBullet égóp");
    }

    protected override void Fire()
    {
        shooting.SetDirection(shootingDirection);
        shooting.RequestShoot();
    }
}
