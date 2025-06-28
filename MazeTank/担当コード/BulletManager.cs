using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ------------------------------------
// BulletManager�N���X
// �p�r�F�e�̃C���X�^���X�����s��
// �쐬�ҁF23cu0216�@���n����
// �쐬���F2024/10/04 �쐬�J�n
// ------------------------------------


public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObj;   // �v���C���[�i�[�p
    private Transform PlayerTransform;
    private PlayerController PlayerController;

    [SerializeField] private GameObject BulletObj;   // �v���C���[�p�e�I�u�W�F�N�g
    [SerializeField] private GameObject LandMines;    // �v���C���[�p�n���I�u�W�F�N�g

    [SerializeField] private int m_LandMineNum;   // �n���̎c��̐�
    [SerializeField] private int m_MineUpperLimit;    // �n���̏������

    private bool IsShooting;    // �ˌ����s������
    private readonly float ShootingCoolTime = 3.0f; // ���˂̃N�[���^�C�� 
    private float ShootingTime; // ���˂�������

    private bool IsSetLandmines;   // �n�����ݒu���ꂽ��



    void Start()
    {
        // �e�f�[�^��������
        PlayerTransform = GameObject.Find(PlayerObj.name).GetComponent<Transform>();
        PlayerController = GameObject.Find(PlayerObj.name).GetComponent<PlayerController>();

        IsShooting = false;
        IsSetLandmines = false;
        ShootingTime = 0.0f;
    }

    void Update()
    {
        // �N�[���^�C�����v��
        if (IsShooting || IsSetLandmines)
        {
            ShootingTime += Time.deltaTime;

            if (ShootingTime > ShootingCoolTime)
            {
                // �㏈��
                IsShooting = false;
                IsSetLandmines = false;
                ShootingTime = 0.0f;
            }
        }

        // Player���ݒu�\�ȏ�ԂȂ�
        if(PlayerController.CanAddMine())
        {
            // ��������𑝂₷
            AddMine();
            PlayerController.SetCanAddMine(false);
        }
    }

    // �����n����������
    private void AddMine()
    {
        // �n����������������Ă����ꍇ��������
        if (m_LandMineNum >= m_MineUpperLimit) return;

        ++m_LandMineNum;
    }

    public void PlayerShooting()
    {
        // �v���C���[����]���ł���Ύˌ��͂ł��Ȃ�
        if (!PlayerController.IsRotate())
        {
            // �ˌ��L�[��������Ă���
            // �ˌ��N�[���^�C���ł͂Ȃ������ꍇ
            if (!IsShooting)
            {
                Vector3 BulletPos = Vector3.zero;
                Vector3 TexSize = PlayerTransform.right / 1.5f;
                BulletPos = PlayerTransform.position + TexSize;
                // �e���v���C���[���ɍ��킹�ăC���X�^���X������
                var bullet = Instantiate(BulletObj,BulletPos, PlayerTransform.rotation);
                bullet.name = BulletObj.name;
                IsShooting = true;
            }
        }
    }

    // �v���C���[�̒n���ݒu����
    public void PlayerSetMines()
    {
        // �v���C���[����]���ł���ΐݒu�͂ł��Ȃ�
        if (!PlayerController.IsRotate())
        {
            // �ݒu�{�^����������Ă����ꍇ
            // �ݒu���͂R��
            if (!IsSetLandmines && m_LandMineNum > 0)
            {
                // �n�����v���C���[���ɍ��킹�ăC���X�^���X������
                --m_LandMineNum;
                var landmine = Instantiate(LandMines, PlayerTransform.position, PlayerTransform.rotation);
                landmine.name = LandMines.name;
                IsSetLandmines = true;
            }
        }
    }
    // �n���̐ݒu�\�����擾
    public int GetLandMineNum()
    {
        return m_LandMineNum;
    }
}
