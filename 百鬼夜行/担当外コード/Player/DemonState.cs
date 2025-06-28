using UnityEngine;

public class DemonState : PlayerShooting
{
    [Header("ŠgUƒVƒ‡ƒbƒgİ’è")]
    public int pelletCount = 4; // ’e‚Ì”
    public float spreadAngle = 30f; // ŠgUŠp“xi}‚Å•ªŠ„j

    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        shooting.SetBulletByName("DemonBullet");
        shooting.SetBulletSpeed(16f);
        SetShootInterval(0.15f);
        Debug.Log("DemonState ‰Šú‰»Š®—¹: DemonBullet g—p");
    }

    protected override void Fire()
    {
        float angleStep = spreadAngle / (pelletCount - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 spreadDir = Quaternion.Euler(0, 0, angle) * shootingDirection.normalized;

            shooting.SetDirection(spreadDir);
            shooting.RequestShoot();
        }
    }
}
