using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class HomingEnemyAttack : EnemyAttack
{
    private Bullet bullet;

    private GameObject TargetObject;

    /// <summary>
    /// HomingEnemyConfig�Ɋ�Â��āA���˂���e��ݒ肵�܂��B
    /// </summary>
    public override void Initialize(ScriptableObject config)
    {
        // �󂯎����config��HomingEnemyConfig�Ƃ��ăL���X�g
        var homingConfig = config as HomingEnemyConfig;
        if (homingConfig == null)
        {
            Debug.LogError("�n���ꂽConfig��HomingEnemyConfig�ł͂���܂���B", this);
            return;
        }

        // Config�Ŏw�肳�ꂽ�e�̃v���n�u��Shooting�R���|�[�l���g�ɐݒ�
        if (homingConfig.BulletObject != null)
        {
            bullet = homingConfig.BulletObject.GetComponent<HomingEnemyBullet>();
            shooting.SetBulletByName(homingConfig.BulletObject.name);
            shooting.SetBulletSpeed(homingConfig.BulletSpeed);
            bullet.SetOwner(gameObject);
            bullet.SetLifeTime(homingConfig.LifeTime);
        }
        else
        {
            Debug.LogWarning("Config�Œe���ݒ肳��Ă��܂���B", this);
        }
    }

    /// <summary>
    /// �ˌ��V�X�e���ɔ��˃��N�G�X�g�𑗂�܂��B
    /// </summary>
    public override void PerformAttack()
    {
        if (shooting == null || TargetObject == null)
        {
            return;
        }

        // �^�[�Q�b�g�ւ̕������v�Z
        Vector3 directionToTarget = (TargetObject.transform.position - transform.position).normalized;

        // �v�Z���������Ŏˌ������N�G�X�g����
        shooting.RequestShoot(directionToTarget);
    }

    public override void SetTarget(GameObject target)
    {
        TargetObject = target;
    }
}