using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------
// Bullet�N���X
// �p�r�F�e�N���X�A��Ɉړ��Ȃǂ��s��
// �쐬�ҁF23cu0216�@���n����
// �쐬���F2024/10/04 �쐬�J�n
// ------------------------------------

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerObject; // Player�̖��O�擾�p
    private readonly float MoveSpeed = 0.1f;    // �ړ����x
    Vector2 m_Velocity;    // �ړ���

    SpriteRenderer m_SpriteRenderer; 

    GameObject m_PlayerObj; // Player�i�[�p
    PlayerController m_PlayerController;    // PlayerController�i�[�p

    [SerializeField] private float m_Power; // �e�̍U����

    private bool m_IsSetting;   // Bullet�̐ݒ肪�����������H

    [SerializeField] private GameObject ExpObj; // �����A�j���[�V�����p

    void Start()
    {
        // �e�ϐ��̏�����
        m_PlayerObj = GameObject.Find(m_PlayerObject.name);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_PlayerController = m_PlayerObj.GetComponent<PlayerController>();
        m_Velocity = Vector2.zero;
        m_IsSetting = false;
    }
    
    void Update()
    {
        // Bullet�̐ݒ�����Ă��Ȃ���ΐݒ���s��
        if (!m_IsSetting)
        {
            // �v���C���[�̉�]�p�x���擾
            Vector3 Playerangles = m_PlayerController.GetEulerAnglesZ();
            SettingBullet(Playerangles.z);
            m_IsSetting = true;
        }

        // �ݒ芮��������
        // Bullet��Tag�t�����s��
        if(m_IsSetting)
        {
            tag = "Bullet";
        }

        // �ړ�����
        BulletMove();
    }

    // �e�̈ړ��֐�
    // �����ɃI�C���[�p�̐��l�������
    private void BulletMove() 
    {
        GetComponent<Rigidbody2D>().velocity = m_Velocity / Time.deltaTime;
    }

    // �ˌ��O�̐ݒ�(�p�x��ړ������Ȃ�)
    private void SettingBullet(float angles)
    {
        // �v���C���[�̌����Ă���������m�F����
        // ��F90���@���F180���@���F270���@�E�F0��or 360��
        int anglesnum = (int)((angles / 90.0f) + 0.5f);

        // �����ɍ��킹�Ĉړ��ʂ�ς���
        if (anglesnum == 1)
        {
            m_Velocity.y += MoveSpeed;
        }
        else if (anglesnum == 2)
        {
            m_Velocity.x -= MoveSpeed;
        }
        else if (anglesnum == 3)
        {
            m_Velocity.y -= MoveSpeed;
        }
        else if (anglesnum == 0 || anglesnum == 4)
        {
            m_Velocity.x += MoveSpeed;
        }
    }

    // �e�I�u�W�F�N�g�Ƃ̏Փ˔���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        // ���������̂�Player���ˌ�����Player�łȂ����
        if(collision.tag == "Player" && collision.name != m_PlayerObject.name)
        {
            // �e�𓧖���
            m_SpriteRenderer.color = new Vector4(m_SpriteRenderer.color.r, m_SpriteRenderer.color.g, m_SpriteRenderer.color.b, 0.0f);

            // �����I�u�W�F�N�g���C���X�^���X��
            Instantiate(ExpObj,transform.position,Quaternion.identity);

            // ���g���폜����
            Destroy(gameObject);
        }

        // ���������̂�Wall�ł����
        if (collision.tag == "Wall")
        {
            // �����I�u�W�F�N�g���C���X�^���X��
            Instantiate(ExpObj, transform.position, Quaternion.identity);
            // ���g���폜
            Destroy(gameObject);
        }

    }

    // Bullet�̍U���͂��擾
    public float GetBulletPower()
    {
        return m_Power;
    }
}
