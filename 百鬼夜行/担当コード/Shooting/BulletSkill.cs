using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillType
{
    Instant,    // 単発スキル
    Duration    // 持続スキル
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

    // スキルの種類を定義（デフォルトはInstant）
    protected virtual SkillType Type => SkillType.Instant;

    // 持続スキル用変数
    protected bool isActive = false;
    protected virtual float maxDuration => 5f;

    // 弾種と対応スキルのマップ
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
            Debug.Log("[BulletSkill] イベント解除");
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
        Skill(); // 効果の開始
        Debug.Log("Duration Skill Started");
    }

    protected virtual void EndSkill()
    {
        isActive = false;
        OnSkillEnd(); // 効果の終了
        Debug.Log("Duration Skill Ended");
    }

    // 上書き前提：即時 or 開始時の効果
    protected virtual void Skill() { }

    // 上書き前提：持続スキルの終了時の後始末
    protected virtual void OnSkillEnd() { }

    // 補助スキル（任意）
    protected virtual void SubSkill() { }

    // ゲージ確認用
    public float GetGauge() => gauge;

    public void SetGauge(float value) { gauge = value; }
}