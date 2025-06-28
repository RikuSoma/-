using UnityEngine;

// �U�����W�b�N�͕ʂ̃R���|�[�l���g���S�����邽�߁A
// ���̃N���X��EnemyAttack�R���|�[�l���g�������Ƃ�����v������
[RequireComponent(typeof(EnemyAttack))]
public class HomingEnemy : TargetingEnemy
{
    // �U���̐U�镑����S�����郂�W���[��
    private EnemyAttack attackModule;

    [SerializeField] private HomingEnemyConfig config;
    [SerializeField] private float attackInterval = 2.5f;
    private float attackTimer;

    new void Start()
    {
        base.Start();

        if (config == null)
        {
            Debug.LogError("HomingEnemyConfig���ݒ肳��Ă��܂���B", gameObject);
            this.enabled = false;
            return;
        }

        // ���g�����U�����W���[�����擾
        attackModule = GetComponent<HomingEnemyAttack>();
        if (attackModule != null)
        {
            // �U�����W���[����Config�ŏ�����
            attackModule.Initialize(config);
            attackModule.SetTarget(Target);
        }
        else
        {
            // RequireComponent������̂Œʏ�͔������Ȃ�
            Debug.LogError("EnemyAttack�R���|�[�l���g��������܂���B", gameObject);
            this.enabled = false;
            return;
        }

        attackTimer = attackInterval;

        healthManager.OnDeath += Dead;
    }

    void Update()
    {
        if (TargetObject == null) return;

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            // �U���̈ӎv����̂ݍs���A���s�̓��W���[���ɈϏ�����
            attackModule.PerformAttack();
        }
    }

    private void Dead()
    {
        Explode();

        Destroy(gameObject);
    }
}