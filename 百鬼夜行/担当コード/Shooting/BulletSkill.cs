using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillType
{
    Instant,    // �P���X�L��
    Duration    // �����X�L��
}

public class BulletSkill : IPlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerShooting playerShooting;

    protected float SkillTime = 0;

    private static float gauge = 0f;
    private const float maxGauge = 5f;
    private const float gaugeIncreasePerSecond = 0.1f;

    private bool isEventRegistered = false;

    // �X�L���̎�ނ��`�i�f�t�H���g��Instant�j
    protected virtual SkillType Type => SkillType.Instant;

    // �����X�L���p�ϐ�
    protected bool isActive = false;
    protected virtual float maxDuration => 5f;

    // �e��ƑΉ��X�L���̃}�b�v
    private static readonly Dictionary<Type, Type> shootingToSkillMap = new()
    {
        { typeof(FoxState), typeof(FoxSkill) },
        { typeof(DemonState), typeof(DemonSkill) },
        { typeof(NohMaskState), typeof(NohMaskSkill) }
    };

    public virtual void Init(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
        this.playerShooting = playerStateMachine.GetState<PlayerShooting>();

        if (!isEventRegistered)
        {
            PlayerShooting.OnShootingModeChanged -= OnShootingModeChanged;
            PlayerShooting.OnShootingModeChanged += OnShootingModeChanged;
            isEventRegistered = true;
        }
    }

    public void HandleInput()
    {
        ShootSkill();
    }

    public virtual void Update()
    {
        IncreaseGauge(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
        {
            gauge = maxGauge;
        }
    }

    public virtual void Remove()
    {
        if (isEventRegistered)
        {
            PlayerShooting.OnShootingModeChanged -= OnShootingModeChanged;
            isEventRegistered = false;
            Debug.Log("[BulletSkill] �C�x���g����");
        }
    }

    private void IncreaseGauge(float deltaTime)
    {
        gauge += gaugeIncreasePerSecond * deltaTime;
        if (gauge > maxGauge)
            gauge = maxGauge;
    }

    private void OnShootingModeChanged(Type newShootingType)
    {
        if (shootingToSkillMap.TryGetValue(newShootingType, out Type newSkillType))
        {
            BulletSkill newSkill = (BulletSkill)Activator.CreateInstance(newSkillType);
            newSkill.Init(player, playerStateMachine);

            var currentSkillState = playerStateMachine.GetState<BulletSkill>();
            if (currentSkillState != null)
            {
                currentSkillState.Remove();
                playerStateMachine.DeactivateState(currentSkillState);
            }

            playerStateMachine.ActivateState(newSkill);

            Debug.Log($"[Skill] Switched to: {newSkillType.Name}");
        }
    }

    private void ShootSkill()
    {
        if (playerStateMachine.SkillPressed && gauge == maxGauge)
        {
            gauge -= maxGauge;

            if (Type == SkillType.Instant)
            {
                Skill();
            }
            else if (Type == SkillType.Duration)
            {
                BeginSkill();
            }
        }
    }

    protected virtual void BeginSkill()
    {
        isActive = true;
        Skill(); // ���ʂ̊J�n
        Debug.Log("Duration Skill Started");
    }

    protected virtual void EndSkill()
    {
        isActive = false;
        OnSkillEnd(); // ���ʂ̏I��
        Debug.Log("Duration Skill Ended");
    }

    // �㏑���O��F���� or �J�n���̌���
    protected virtual void Skill() { }

    // �㏑���O��F�����X�L���̏I�����̌�n��
    protected virtual void OnSkillEnd() { }

    // �⏕�X�L���i�C�Ӂj
    protected virtual void SubSkill() { }

    // �Q�[�W�m�F�p
    public float GetGauge() => gauge;

    public void SetGauge(float value) { gauge = value; }
}