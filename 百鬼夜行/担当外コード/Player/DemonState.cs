using UnityEngine;

public class DemonState : PlayerShooting
{
    [Header("�g�U�V���b�g�ݒ�")]
    public int pelletCount = 4; // �e�̐�
    public float spreadAngle = 30f; // �g�U�p�x�i�}�ŕ����j

    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        shooting.SetBulletByName("DemonBullet");
        shooting.SetBulletSpeed(16f);
        SetShootInterval(0.15f);
        Debug.Log("DemonState ����������: DemonBullet �g�p");
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
