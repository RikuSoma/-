using UnityEngine;

public class FoxState : PlayerShooting
{
    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        shooting.SetBulletByName("FoxBullet");
        shooting.SetBulletSpeed(10f);
        SetShootInterval(0.2f);
        Debug.Log("FoxState èâä˙âªäÆóπ: FoxBullet égóp");
    }

    protected override void Fire()
    {
        shooting.SetDirection(shootingDirection);
        shooting.RequestShoot();
    }
}
