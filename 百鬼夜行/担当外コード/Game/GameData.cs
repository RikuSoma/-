using UnityEngine;

public class GameData : MonoBehaviour
{
    public float PlayTime;

    public float PlayerHP;

    public float PlayerMaxHP = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static GameData instance;
    public static GameData Instance => instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetAll()
    {
        PlayTime = 0f;
        PlayerHP = 0f;
    }

    // �@�\   �F�^�C�}�[�}�l�[�W���[����v���C�^�C�������Ă��̃X�N���v�g�̕ϐ��ɑ���֐�
    // ����   �Ffloat�^�A�ϐ���time
    // �߂�l  �F����
    public void saveTime(float time)
    {
        PlayTime = time;
        
    }

    // �@�\   �F�v���C���[HP�X�N���v�g����v���C���ɕϓ����ꂽHP�����擾������֐�
    // ����   �Ffloat�^�A�ϐ���HP
    // �߂�l  �F����
    public void savePlayerHP(float HP)
    {
        PlayerHP = HP;
        Debug.Log("KUSO" + PlayerHP);
    }

}
