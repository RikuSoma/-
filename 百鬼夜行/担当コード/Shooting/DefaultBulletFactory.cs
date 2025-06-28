using UnityEngine;

public class DefaultBulletFactory : IBulletFactory
{
    public GameObject CreateBullet(GameObject prefab, Vector3 position, Vector3 direction, Shooting shooter)
    {
        var bullet = Object.Instantiate(prefab, position, Quaternion.LookRotation(direction));
        if (bullet.TryGetComponent(out Bullet bulletScript))
        {
            bulletScript.SetOwner(shooter.gameObject);
            bulletScript.SetIgnoreTags(shooter.GetIgnoreTags());
        }

        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = direction * shooter.GetBulletSpeed();
        }

        // �����e���z�[�~���O�e�Ȃ�A���ˎ傩��^�[�Q�b�g�����擾���Đݒ肷��
        if (bullet.TryGetComponent(out HomingEnemyBullet homingBullet))
        {
            // ���ˎ�ishooter�j���^�[�Q�b�g�������Ă��邩�m�F (TargetingEnemy���p�����Ă��邩)
            if (shooter.TryGetComponent(out TargetingEnemy targetingEnemy))
            {
                homingBullet.SetTarget(targetingEnemy.Target);
            }
        }

        return bullet;
    }
}