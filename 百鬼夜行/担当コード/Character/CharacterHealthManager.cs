using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float invincibleDuration = 0f; // ���G���� (0�ɐݒ�)
    private bool isInvincible = false; // ���G�����H

    private float accumulatedDamage = 0f; // �~�ς��ꂽ�_���[�W
    private bool isProcessingDamage = false; // �_���[�W��������

    [SerializeField, TagSelector] private List<string> DesignationTag; // �ڐG���_���[�W��H�炤�^�O

    // ���S���C�x���g
    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    // �팂���C�x���g
    public delegate void DamageTakenEvent();
    public event DamageTakenEvent OnDamageTaken;

    // �񕜏���
    public void Recovery(float value)
    {
        Health += value;
    }

    // �팂�����i�_���[�W��~�ς���j
    public void ApplyDamage(float value)
    {
        if (isInvincible) return; // ���G or ���S���Ȃ疳��
        accumulatedDamage += value;
        if (!isProcessingDamage)
        {
            StartCoroutine(ProcessDamage());
        }
    }

    // �~�ς��ꂽ�_���[�W����������R���[�`��
    private IEnumerator ProcessDamage()
    {
        isProcessingDamage = true;
        yield return null; // ���̃t���[���܂ő҂��ƂŁA���t���[�����̕����̃_���[�W�~�ς�҂�

        if (accumulatedDamage > 0)
        {
            Health -= accumulatedDamage;
            // �_���[�W�ʒm���s��
            OnDamageTaken?.Invoke();
            accumulatedDamage = 0f;

            // ���S����Ɩ��G���ԏ���
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

    // ���G���ԏ���
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

    // ���S���̃C�x���g����
    private void Die()
    {
        OnDeath?.Invoke();
    }

    // ����ł��邩
    public bool IsDead() { return Health <= 0; }

    // �_���[�W���󂯂Ă��邩 (���G���Ԓ�)
    public bool IsTakingDamage() { return isInvincible; }

    public void SetIsInvincible(bool invincible) { isInvincible = invincible; }

    // �L�����N�^�[�ǂ����������������p
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