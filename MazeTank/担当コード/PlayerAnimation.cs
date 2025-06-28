using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static PlayerController;

/// <summary>
/// PlayerAnimation�N���X
/// Player�̃A�j���[�V�������s��
/// �쐬�ҁF23cu0216 ���n����
/// �쐬���F2024/10/21 �쐬�J�n
/// </summary>

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus m_PlayerStatus;    // �v���C���[�̏��

    SpriteRenderer m_sprite;    // Sprite�i�[�p
    [SerializeField] private bool m_Istransparent;  // ���������Ă��邩�H
    [SerializeField] private float m_transparentTime; // �������Ԋu
    [SerializeField] private float m_Time;

    [SerializeField] private GameObject m_exObj;    // �����p

	// Start is called before the first frame update
	void Start()
    {
        // PlayerController����Player�����擾
        m_PlayerStatus = (PlayerStatus)GetComponent<PlayerController>().GetPlayerStatus();
        m_sprite = GetComponent<SpriteRenderer>();
        m_Istransparent = false;
        m_Time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerController����Player�����擾
        m_PlayerStatus = (PlayerStatus)GetComponent<PlayerController>().GetPlayerStatus();

        switch (m_PlayerStatus)
        {
            // �ҋ@���
            case PlayerStatus.IDLE:
                break;
            // �ʏ���
            case PlayerStatus.FINE:
                // �_���[�W���󂯂Ă����ꍇ
                // Player��_�ł�����
                if(GetComponent<PlayerHitPoints>().IsDamage())
                {
					m_Time += Time.deltaTime;

					if (!m_Istransparent)
                    {
                        // ���g�𓧖���
                        m_sprite.color = new Vector4(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 0.0f);
                        if (m_Time > m_transparentTime)
                        {
                            m_Istransparent = true;
                            m_Time = 0.0f;
                        }
					}
					else
                    {
						// ���g�̓�����������
						m_sprite.color = new Vector4(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1.0f);
						if (m_Time > m_transparentTime)
						{
							m_Istransparent = false;
							m_Time = 0.0f;
						}
					}
				}
                else
                {
					// ���g�̓�����������
					m_sprite.color = new Vector4(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, 1.0f);
				}
				break;
            // ���S���
            case PlayerStatus.DEAD:
                // ���g�̏ꏊ�ɔ����I�u�W�F�N�g���C���X�^���X����
                Instantiate(m_exObj, transform.position, Quaternion.identity);

                // ���g�͍폜����
                Destroy(gameObject);
                break;
            // ���X�|�[�����
            case PlayerStatus.RESPAWN:
                break;
            // �N���A���
            case PlayerStatus.CLEAR:
                break;
            default:
                break;
        }
    }
}
