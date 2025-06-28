using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Dont Destoroy �N���X
/// �p�r�F�V�[���Ԃ̔j���������Ȃ����̂�o�^����
/// �쐬�ҁF23Cu0216���n����
/// �쐬���F2024/10/07 �쐬�J�n
/// </summary>


public class DontDestoroyGameObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_prefabs;

    public Scene Destoroy { get; private set; }

    void Start()
    {
        // ���M�������Ȃ��悤�ɂ���
        DontDestroyOnLoad(this);

        // �v�f���擾
        int Index = m_prefabs.Count;

        // DontDestroyOnLoad�ɓo�^
        for (int i = 0; i < Index; ++i)
        {
            DontDestroyOnLoad(m_prefabs[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���X�g�̒ǉ�
    public void AddList(GameObject gameobject)
    {
        int num = m_prefabs.Count;
        bool ret = true;
        for (int i = 0; i < num; ++i)
        {
            // �ǉ����悤�Ƃ��Ă���GameObject�ɓ������̂����邩����
            if (m_prefabs[i] == gameobject) ret = false;
        }
        // �������̂��Ȃ����
        if (!ret)
        {
            // ���X�g�ɒǉ�
            m_prefabs.Add(gameObject);
        }

        // DontDestroyOnLoad�ɓo�^
        DontDestroyOnLoad(gameobject);
    }

    // ���X�g�̍폜
    public void RemoveList(GameObject gameobject)
    {
        int num = m_prefabs.Count;
        bool ret = false;
        for (int i = 0; i < num; ++i)
        {
            // �폜���悤�Ƃ��Ă���GameObject�ɓ������̂����邩����
            if (m_prefabs[i] == gameobject) ret = true;
        }
        // �������̂������
        if (ret)
        {
            // Dontdestoroy����폜����
            SceneManager.MoveGameObjectToScene(gameobject,Destoroy);
        }

        // ���X�g����폜����
        int Index = m_prefabs.Count;

        for (int i = 0; i < Index; ++i)
        {
            // �����������������̂��o���
            if (m_prefabs[i] == gameobject)
            {
                //�@�폜����
                m_prefabs.Remove(gameobject);
            }
        }
    }
}
