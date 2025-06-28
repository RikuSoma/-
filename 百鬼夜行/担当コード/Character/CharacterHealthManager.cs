using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float invincibleDuration = 0f; // 無敵時間 (0に設定)
    private bool isInvincible = false; // 無敵中か？

    private float accumulatedDamage = 0f; // 蓄積されたダメージ
    private bool isProcessingDamage = false; // ダメージ処理中か

    [SerializeField, TagSelector] private List<string> DesignationTag; // 接触時ダメージを食らうタグ

    // 死亡時イベント
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    // 被撃時イベント
    public delegate void DamageTakenEvent();
    public event DamageTakenEvent OnDamageTaken;

    // 回復処理
    public void Recovery(float value)
    {
        Health += value;
    }

    // 被撃処理（ダメージを蓄積する）
    public void ApplyDamage(float value)
    {
        if (isInvincible) return; // 無敵 or 死亡中なら無視
        accumulatedDamage += value;
        if (!isProcessingDamage)
        {
            StartCoroutine(ProcessDamage());
        }
    }

    // 蓄積されたダメージを処理するコルーチン
    private IEnumerator ProcessDamage()
    {
        isProcessingDamage = true;
        yield return null; // 次のフレームまで待つことで、同フレーム内の複数のダメージ蓄積を待つ

        if (accumulatedDamage > 0)
        {
            Health -= accumulatedDamage;
            // ダメージ通知を行う
            OnDamageTaken?.Invoke();
            accumulatedDamage = 0f;

            // 死亡判定と無敵時間処理
            if (Health <= 0)
            {
                Die();
            }
            else if (invincibleDuration > 0)
            {
                StartCoroutine(InvincibleRoutine());
            }
        }
        isProcessingDamage = false;
    }

    // 無敵時間処理
    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        float timer = 0f;
        MeshRenderer rend = GetComponentInChildren<MeshRenderer>();
        bool visible = true;

        while (timer < invincibleDuration)
        {
            visible = !visible;
            if (rend != null) rend.enabled = visible;

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        if (rend != null) rend.enabled = true;
        isInvincible = false;
    }

    // 死亡時のイベント処理
    private void Die()
    {
        OnDeath?.Invoke();
    }

    // 死んでいるか
    public bool IsDead() { return Health <= 0; }

    // ダメージを受けているか (無敵時間中)
    public bool IsTakingDamage() { return isInvincible; }

    public void SetIsInvincible(bool invincible) { isInvincible = invincible; }

    // キャラクターどうしが当たった時用
    private void OnCollisionEnter(Collision collision)
    {
        if (DesignationTag.Contains(collision.gameObject.tag))
        {
            ApplyDamage(1);
        }
    }

    public float GetInvincibleDuration() { return invincibleDuration; }

    public float GetHelth() { return Health; }
}