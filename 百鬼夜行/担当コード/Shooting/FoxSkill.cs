using UnityEngine;

public class FoxSkill : BulletSkill
{
    private int counterCount = 0;
    private const int maxCounters = 3; // �J�E���^�[�����񐔂̏��
    private float counterCooldown = 0.1f;
    private float counterCooldownTimer = 0f;
    private bool recentlyCountered = false;
    private bool isInCounterMode = false; // �J�E���^�[���[�h���ǂ���
    private bool eventFlug = false;


    private CharacterHealthManager healthManager;
    private Shooting shooting;

    // �����^�X�L���Ƃ��Đݒ�iDuration�^�C�v�j
    protected override SkillType Type => SkillType.Duration;
    protected override float maxDuration => 5f;

    // ���������Ƀv���C���[�̕K�v�R���|�[�l���g�擾�ƃC�x���g�o�^
    public override void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        base.Init(player, playerStateMachine);

        if (player == null) return;

        healthManager = player.GetComponent<CharacterHealthManager>();
        shooting = player.GetComponentInChildren<Shooting>();

        // �v���C���[�̍U���󂯂��C�x���g���w�ǂ��J�E���^�[���������m
        if (!eventFlug)
        {
            player.OnAttackedByEnemy -= HandleAttackEvent;
            player.OnAttackedByEnemy += HandleAttackEvent;
            eventFlug = true;
        }

    }

    // �X�L���j�����ɃC�x���g�����i���������[�N�h�~�j
    public override void Remove()
    {
        base.Remove();
        if (eventFlug)
        {
            player.OnAttackedByEnemy -= HandleAttackEvent;
            eventFlug = false;
            Debug.Log("[FoxSkill] �C�x���g����");
        }
    }

    // �X�L���J�n���ɃJ�E���^�[��Ԃɓ��薳�G��
    protected override void BeginSkill()
    {
        base.BeginSkill();
        isInCounterMode = true;
        counterCount = 0;

        if (healthManager != null)
            healthManager.SetIsInvincible(true); // �_���[�W������

        Debug.Log("[FoxSkill] �J�E���^�[���[�h�J�n");
    }

    // �X�L���I�����ɃJ�E���^�[��ԉ����A���G������
    protected override void EndSkill()
    {
        base.EndSkill();
        isInCounterMode = false;

        if (healthManager != null)
            healthManager.SetIsInvincible(false);

        Debug.Log("[FoxSkill] �J�E���^�[���[�h�I��");
    }

    // ���t���[�������F�J�E���^�[���ԊǗ�
    public override void Update()
    {
        base.Update();

        if (!isInCounterMode) return;

        if (recentlyCountered)
        {
            counterCooldownTimer += Time.deltaTime;
            if (counterCooldownTimer >= counterCooldown)
            {
                recentlyCountered = false;
                counterCooldownTimer = 0f;
            }
        }

    }

    // �v���C���[���U�����󂯂����ɌĂ΂�鏈��
    private void HandleAttackEvent()
    {
        if (!isInCounterMode || recentlyCountered) return;

        recentlyCountered = true;

        Debug.Log("[FoxSkill] �J�E���^�[�����I");
        ShootInEightDirections();

        counterCount += 1;
        Debug.Log("Count " + counterCount);

        if (counterCount >= maxCounters)
        {
            EndSkill();
        }
    }

    // XY���ʁi���X�N���[����ʂɍ��킹��Z��0�j��8�����֓��Ԋu�ɒe�𔭎�
    private void ShootInEightDirections()
    {
        if (shooting == null) return;

        int bulletCount = 8;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float rad = angle * Mathf.Deg2Rad;

            // �e�̕����iXY���ʁAZ=0�j
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f).normalized;

            shooting.RequestShoot(dir);

            angle += angleStep;
        }
    }
}
