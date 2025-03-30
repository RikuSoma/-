using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// ---------------------------------------------
// �v���C���[�R���g���[���[�N���X
// �p�r�F�v���C���[�𑀍삷�邽��
// �쐬�ҁF23cu0216 ���n����
// �쐬���F2024/09/30 �쐬�J�n
//       :2024/10/4 �쐬����
//       :2024/10/28 �C���J�n Player�̕ǏՓ˔�����C��
// ---------------------------------------------

public class PlayerController : MonoBehaviour
{
    // �v���C���[�̏��
    public enum PlayerStatus
    {
        IDLE,
        FINE,
        DEAD,
        RESPAWN,
        CLEAR
    }

    private enum KeyDown
    {
        none,
        left,
        right
    }

    private enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private PlayerStatus m_PlayerStatus;   // Player�̏��
    private Rigidbody2D m_Rigidbody;    // 
    private Transform m_transform;

    private PlayerHitPoints m_PlayerHitPoints;

    [SerializeField] private float MoveSpeed;   // �v���C���[�̈ړ����x
    [SerializeField] private float RotateSpeed;// �v���C���[�̉�]���x
    private float m_angle = 0.0f; // ��]����
    private float m_nowangle = 0.0f;  // ��]���̌��݉�]�����p�x
    private bool m_Isrotate;  // ��]����
    KeyDown m_KeyDown;  // �����ꂽ�L�[
    [SerializeField]Direction m_Direction; // �ǂɐڐG���Ă�ۂɎg�p����

    PlayerInput plyaerInput;

    [SerializeField] GameObject m_MapObj;   // Map���m�F�p
    private MapEditor m_MapEditor;  // MapEditor�i�[�p
    [SerializeField] private bool m_IsWall; // �ǂ����邩

    [SerializeField] private GameObject m_BulletMngObj; // BulletManager�I�u�W�F�N�g�i�[�p
    private BulletManager m_BulletMng;  // BulletManager�i�[�p

    [SerializeField] private float m_SpeedUpTime; // �X�s�[�h�A�b�v�̌��ʎ���
    [SerializeField] private float m_SpeedMagnification;    // �X�s�[�h�A�b�v���̑��x�A�b�v�{��
    private float m_NowSpeedUpTime; // �X�s�[�h�A�b�v���ʌp������
    [SerializeField] private bool m_CanSpeedUp;   // �X�s�[�h�A�b�v�ł��邩�H

    private bool m_CanAddMine;  // �n�������ł��邩?

    void Start()
    {
        // �e�ϐ��̏�����
        m_transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PlayerHitPoints = GetComponent<PlayerHitPoints>();
        m_MapEditor = m_MapObj.GetComponent<MapEditor>();
        m_PlayerStatus = PlayerStatus.IDLE;
        m_Isrotate = false;
        m_KeyDown = KeyDown.none;
        m_Direction = Direction.None;
        m_IsWall = false;
        m_BulletMng = GameObject.Find(m_BulletMngObj.name).GetComponent<BulletManager>();
        m_NowSpeedUpTime = 0.0f;
        m_CanSpeedUp = false;
        m_CanAddMine = false;
    }

    void Update()
    {
        // Player�̗̑͂��Ȃ����
        if (m_PlayerHitPoints.IsDead())
        {
            // �v���C���[�����S��Ԃɂ���
            m_PlayerStatus = PlayerStatus.DEAD;
        }

        switch (m_PlayerStatus)
        {
            // �ҋ@���
            case PlayerStatus.IDLE:
                break;
            // �ʏ���
            case PlayerStatus.FINE:
                // �ړ��������]�������s��
                ChangeRotation();
                Move();
                break;
            // ���S���
            case PlayerStatus.DEAD:
                break;
            // ���A���
            case PlayerStatus.RESPAWN:
                break;
            // �N���A���
            case PlayerStatus.CLEAR:
                break;
            default:
                break;
        }
    }
    // �ړ�����
    private void Move()
    {
        // ���ݏ��Ɗp�x���擾
        Vector2 PlayerPos = m_transform.position;
        var angles = m_transform.rotation.eulerAngles;


        // �v���C���[����]���łȂ���Έړ�����
        if (!m_Isrotate)
        {
            //�O���x�N�g��
            Vector2 vForward = transform.right;
            Vector2 vUp = transform.up;

            Vector2 texsize = vForward / 2.0f;
            Vector2 pointinterval = vUp / 3.0f;
            float Speed = MoveSpeed; // �ړ���

            // Player��SpeedUp��Ԃł����
            if (m_CanSpeedUp)
            {
                // �ړ����x���グ��
                Speed *= m_SpeedMagnification;
                // ���ʎ��Ԃ��v��
                m_NowSpeedUpTime += Time.deltaTime;

                // ���ʂ����Z�b�g����
                if (m_NowSpeedUpTime > m_SpeedUpTime)
                {
                    m_CanSpeedUp = false;
                    m_NowSpeedUpTime = 0.0f;
                }
            }
                Vector2 vMove = vForward * (Speed * Time.deltaTime);

            // Player�̐擪�̏㉺��2�_���
            float fx1 = (PlayerPos.x + texsize.x + vMove.x + pointinterval.x);
            float fy1 = (PlayerPos.y + texsize.y + vMove.y + pointinterval.y);

            float fx2 = (PlayerPos.x + texsize.x +vMove.x - pointinterval.x);
            float fy2 = (PlayerPos.y + texsize.y + vMove.y - pointinterval.y);

            // �ړ����悤�Ƃ��Ă���ꏊ�ɕǂ�����m�F
            m_IsWall = m_MapEditor.IsInsideWall(fx1, fy1) || m_MapEditor.IsInsideWall(fx2,fy2);

            // �ǂ��Ȃ����
            if (!m_IsWall)
            {
                // �ړ�����
                m_transform.transform.Translate(vMove.magnitude, 0.0f,0.0f);
            }
            // �ǂ��������ꍇ
            else
            {
                switch (m_Direction)
                {
                    case Direction.Right:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.x = (float)((int)PlayerPos.x + 1);
                        }
                        break;
                    case Direction.Up:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.y = (float)((int)PlayerPos.y);
                        }
                        break;
                    case Direction.Left:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.x = (float)((int)PlayerPos.x);
                        }
                        break;
                    case Direction.Down:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.y = (float)((int)PlayerPos.y-1);
                        }
                        break;
                }
            }
        }
    }

    // ��]����
    private void ChangeRotation()
    {
        bool completionRotation = false;

        switch (m_KeyDown)
        {
            case KeyDown.none:
                break;
            case KeyDown.left:
                m_nowangle += RotateSpeed;
                // ���݂̊p�x�Ɏw�肳�ꂽ�p�x�܂ŉ�]����
                if (m_angle > m_nowangle)
                {
                    m_transform.Rotate(0.0f, 0.0f, RotateSpeed);
                }
                else
                {
                    completionRotation = true;
                }
                break;
            case KeyDown.right:
                m_nowangle -= RotateSpeed;
                // ���݂̊p�x�Ɏw�肳�ꂽ�p�x�܂ŉ�]����
                if (m_angle < m_nowangle)
                {
                    m_transform.Rotate(0.0f, 0.0f, -RotateSpeed);
                }
                else
                {
                    completionRotation = true;
                }
                break;
        }

        // �␳���s��
        if (completionRotation)
        {
            // ���݂̊p�x���擾
            var angles = m_transform.rotation.eulerAngles;

            // ���݂̊p�x��int�^�ɂ��A���z�̊p�x���v�Z����
            int anglesnum = (int)((angles.z / 90)+0.5f);
            angles.z = anglesnum * 90.0f;

            // ���݂̊p�x���獷���������z�̊p�x�ɂ���
            m_transform.rotation = Quaternion.Euler(0.0f, 0.0f,angles.z);

            // �㏈��
            m_nowangle = 0.0f;
            m_Isrotate = false;
            m_KeyDown = KeyDown.none;
        }
    }

    // Player�̏Փ˔���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // NULL�`�F�b�N
        if (collision == null) return;

        // �Փ˂����A�C�e����SpeedUP�A�C�e���ł����
        if(collision.tag == "SpeedUp")
        {
            // ���ʔ������Ɏ�����ꍇ�͏d�ˏ������s��
            if (m_CanSpeedUp) m_NowSpeedUpTime = 0.0f;
            // �X�s�[�h�A�b�v��Ԃɂ���
            m_CanSpeedUp = true;
        }

        // �Փ˂����A�C�e����AddMine�Ȃ��
        if(collision.tag == "AddMine")
        {
            m_CanAddMine = true;
        }
    }

    // �n����ǉ��ł����Ԃ��H
    public bool CanAddMine()
    {
        return m_CanAddMine;
    }

    // ��]�����H
    public bool IsRotate()
    {
        return m_Isrotate;
    }

    // �v���C���[�̏�Ԃ�ݒ�
    public void SetPlayerStatus(int  statusnum)
    {
        m_PlayerStatus = (PlayerStatus)statusnum;
    }

    // �v���C���[�̒n���ݒu�\��Ԃ�ݒ�
    public void SetCanAddMine(bool canaddmine = false)
    {
        m_CanAddMine = canaddmine;
    }

    // �����Ă���������擾�i�I�C���[�p�j
    public Vector3 GetEulerAnglesZ()
    {
        return m_transform.rotation.eulerAngles;
    }

    // �v���C���[�̌��݈ʒu���擾
    public Vector3 GetPosition()
    {
        return m_transform.position;
    }

    public int GetPlayerStatus()
    {
        return (int)m_PlayerStatus;
    }

    // �eInputSystem�̃g���K�[
    // �ˌ��L�[
    public void OnShooting(InputValue inputValue)
    {
        // Player���ʏ��ԏo�Ȃ���Ύˌ����ł��Ȃ�
        if(m_PlayerStatus == PlayerStatus.FINE)
        {
            m_BulletMng.PlayerShooting();
        }
    }

    // �n���ݒu�L�[
    public void OnSetMine(InputValue inputValue)
    {
        // Player���ʏ��ԏo�Ȃ���Ύˌ����ł��Ȃ�
        if (m_PlayerStatus == PlayerStatus.FINE)
        {
            m_BulletMng.PlayerSetMines();
        }
    }

    // ����]�L�[
    public void OnRotateLeft(InputValue inputValue)
    {
        // Player���ʏ��Ԃ�
        // ��]���o�Ȃ���Ή�]�ł���
        if (m_PlayerStatus == PlayerStatus.FINE && !m_Isrotate)
        {
            m_angle = 90;
            m_KeyDown = KeyDown.left;
            m_Isrotate = true;
        }
    }

    // �E��]�L�[
    public void OnRotateRight(InputValue inputValue)
    {
        // Player���ʏ��Ԃ�
        // ��]���o�Ȃ���Ή�]�ł���
        if (m_PlayerStatus == PlayerStatus.FINE && !m_Isrotate)
        {
            m_angle = -90;
            m_KeyDown = KeyDown.right;
            m_Isrotate = true;
        }
    }

    // �����I�Ƀ��U���g��ʂɑ�����L�[
    public void OnLoadResultScene(InputValue inputValue)
    {
        SceneManager.LoadScene("Result");
    }
}