using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerController;

/// <summary>
///  PlayerHitpoint�N���X
///  Player��HP�Ǘ����s���N���X
///  �쐬���F2024/10/16 �쐬�J�n
/// </summary>

public class PlayerHitPoints : MonoBehaviour
{
    [SerializeField] private float m_PlayerHP;  // �̗�
    [SerializeField] private readonly float m_InvincibleTime = 2.0f;    // ���G����
    [SerializeField] private float m_InvincibleNowTime; // ���G�o�ߎ���

    private PlayerController m_Controller;  // PlayerController�N���X�i�[�p
    private PlayerStatus m_PlayerStatus;   // Player�̏�Ԃ��m�F�p

    [SerializeField] private bool m_IsHitBullet;    // Player��Bullet�ɓ���������?
    [SerializeField] private bool m_IsHitLandmine; // Player��Landmines�ɓ����������H

    [SerializeField] private GameObject m_Bullet;   // Player��������Ȃ��e

    struct DamageInfo
    {
        public Vector3 vForword;
        public float damage;
    };

    DamageInfo mDamageInfo = new DamageInfo();
    

    // Start is called before the first frame update
    void Start()
    {
        // �̗͏����l��3�ɂ���
        m_PlayerHP = 3.0f;
        m_InvincibleNowTime = 0.0f;

        // PlayerController�������Ă���iPlayerStatus���g�p���邽�߁j
        m_Controller = this.GetComponent<PlayerController>();
        m_PlayerStatus = PlayerStatus.IDLE;

        m_IsHitBullet = false;
        m_IsHitLandmine = false;
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerController���猻�݂�Player�̏�Ԃ��擾
        m_PlayerStatus = (PlayerStatus)m_Controller.GetPlayerStatus();

        switch (m_PlayerStatus)
        {
            // �ҋ@��Ԃ͖��G
            case PlayerStatus.IDLE:
                break;
            // �ʏ��Ԃł͓����蔻��AHP�̊Ǘ����s��
            case PlayerStatus.FINE:
                UpdateHP();

                // �̗͂�0�ȉ��ɂȂ�Ύ��S��ԂɂȂ�
                if(m_PlayerHP <= 0.0f)
                {
                    m_PlayerStatus = PlayerStatus.DEAD;
                }
                break;
            // ���S���
            case PlayerStatus.DEAD:
                break;
            case PlayerStatus.RESPAWN:
                break;
            case PlayerStatus.CLEAR:
                break;
            default:
                break;
        }
    }
        
    // HP�̍X�V����
    private void UpdateHP()
    {
        // �v���C���[���e�ɓ������Ă��邩
        if (m_IsHitBullet && m_InvincibleNowTime == 0.0f)
        {
            // �O���x�N�g���擾
            Vector3 PlayerSide = m_Controller.transform.right;
            Vector3 BulletSide = mDamageInfo.vForword;

            // �v���C���[��Bullet�̑O���x�N�g�����r����
            if (-BulletSide == PlayerSide || BulletSide == -PlayerSide)
            {
                // �΂������Ă����ꍇ�_���[�W�͎󂯂Ȃ�
                m_IsHitBullet = false;
                return;
            }

            if (m_InvincibleNowTime == 0.0f)
            {
                // ����Bullet�̍U���͕��̗͂�����
                m_PlayerHP -= mDamageInfo.damage;
            }
            // ���G���Ԃ��v��
            m_InvincibleNowTime += Time.deltaTime;
        }

        // �v���C���[���n���ɓ������Ă��邩
        if (m_IsHitLandmine && m_InvincibleNowTime == 0.0f)
        {
            // ���̒n���̍U���͕��̗͂�����
            m_PlayerHP -= mDamageInfo.damage;

            // ���G���Ԃ��v��
            m_InvincibleNowTime += Time.deltaTime;
        }

        // �_���[�W��H�������
        if (m_IsHitBullet || m_IsHitLandmine)
        {
            // ���G���Ԃ��v��
            m_InvincibleNowTime += Time.deltaTime;
        }

        // ���G���Ԃ��߂������̌㏈��
        if (m_InvincibleNowTime > m_InvincibleTime)
        {
            m_IsHitBullet = false;
            m_IsHitLandmine = false;
            m_InvincibleNowTime = 0;
        }
    }

    // �ڐG�����ۂ̓����蔻��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        // Bullet�̓����蔻��
        // ���g���������e�ɂ͓�����Ȃ�
        if (collision.tag == "Bullet" && collision.name != m_Bullet.name && !m_IsHitBullet)
        {
            m_IsHitBullet = true;

            // ����Bullet�̏����擾
            mDamageInfo.vForword = collision.transform.right;
            mDamageInfo.damage = collision.GetComponent<Bullet>().GetBulletPower();

            //Debug.Log("Bullet�ɓ���������");
        }

        // Landmines�̓����蔻��
        if (collision.tag == "LandMine" && !m_IsHitLandmine)
        {
            m_IsHitLandmine = true;

            // ����LandMine�̏����擾
            mDamageInfo.vForword = collision.transform.right;
            mDamageInfo.damage = collision.GetComponent<Mine>().GetLandMinePower();

            //Debug.Log("Landmines�ɓ���������");
        }
    }

    // �v���C���[�͎��S�̗͂ɂȂ�����
    public bool IsDead()
    {
        bool ret = false;
        if (m_PlayerHP <= 0)
        {
            ret = true;
        }
        return ret;
    }

    // �v���C���[�̗̑͂��擾
    public float GetPlayerHitPoints()
    {
        return m_PlayerHP;
    }

    // �_���[�W���󂯂Ă��邩
    public bool IsDamage()
    {
        bool ret = false;

        // �e���n�����������Ă����true��Ԃ�
        if(m_IsHitBullet || m_IsHitLandmine)
        {
            ret = true;
        }
        return ret;
    }
}
