// 
// ���O       �FCameraMove�N���X
// �g�p�p�r   �F�Q�[���J�����̋�������
// �쐬��     �F���n����
// �쐬��     �F9��20��
//

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using static GameSceneManager;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Random = UnityEngine.Random;

public class CameraMove : MonoBehaviour
{
    private GameObject m_GameSceneMng;   // �Q�[���V�[���}�l�[�W���[�i�[�p

    [SerializeField] private float m_ShakeStrength;     // �h��鋭��
    [SerializeField] private float m_ShakeTime; // �h��̎���
    [SerializeField] private float m_ShakeVibrato; // �h��̑傫��
    Vector3 m_Position; // �J�����̏����ʒu
    private float m_NowTime;

    private GameObject m_BattleManager;     // battleManager�i�[�p

    private bool m_IsSetting; // �ݒ肪����������

    private bool m_Change;

    /// �h����
    private struct ShakeInfo
    {
        public ShakeInfo(float duration, float strength, float vibrato)
        {
            Duration = duration;
            Strength = strength;
            Vibrato = vibrato;
        }
        public float Duration { get; } // ����
        public float Strength { get; } // �h��̋���
        public float Vibrato { get; }  // �ǂ̂��炢�U�����邩
    }

    private ShakeInfo _shakeInfo;

    private Vector3 _initPosition; // �����ʒu
    private bool _isDoShake;       // �h����s�����H
    private float _totalShakeTime; // �h��o�ߎ���

    // Start is called before the first frame update
    void Start()
    {
        m_GameSceneMng = GameObject.Find("SceneManager");
        m_Position = transform.position;
        m_IsSetting = false; 
        m_NowTime = 0.0f;
        m_Change = true;

        // �����ʒu��ێ�
        _initPosition = gameObject.transform.position;
        _isDoShake = true;
    }

    // Update is called once per frame
    void Update()
    {
        // NULL�`�F�b�N
        if (m_GameSceneMng == null) return;

        // ���݂̃Q�[���V�[�����擾
         GameScene gameScene = (GameScene)m_GameSceneMng.GetComponent<GameSceneManager>().GetGameScene();

        if(gameScene == GameScene.Play)
        {
            // BattleManager�̐ݒ�
            if(!m_IsSetting)
            {
                m_BattleManager = GameObject.Find(m_GameSceneMng.GetComponent<GameSceneManager>().GetBattleMNGName());
                m_IsSetting = true;
            }

            // BattleManager��NULL�`�F�b�N
            if (m_BattleManager == null) return;

            // battle�̌����������ꍇ
            if(m_BattleManager.GetComponent<BattleManager>().GetIsConclusion() && _isDoShake)
            {
                // �h��̏��ݒ�
                StartShake(m_ShakeTime, m_ShakeStrength, m_ShakeVibrato);

                // �h��ʒu���X�V
                gameObject.transform.position = UpdateShakePosition(
                    gameObject.transform.position,
                    _shakeInfo,
                    _totalShakeTime,
                    _initPosition);

                // duration���̎��Ԃ��o�߂�����h�炷�̂��~�߂�
                _totalShakeTime += Time.deltaTime;
                if (_totalShakeTime >= _shakeInfo.Duration)
                {
                    _isDoShake = false;
                    _totalShakeTime = 0.0f;
                    // �����ʒu�ɖ߂�
                    gameObject.transform.position = _initPosition;
                }

            }
        }
    }
    // �X�V��̗h��ʒu���擾
    private Vector3 UpdateShakePosition(Vector3 currentPosition, ShakeInfo shakeInfo, float totalTime, Vector3 initPosition)
    {
        // �h��̋������擾
        var strength = shakeInfo.Strength;
        var randomX = Random.Range(-1.0f * strength, strength);
        var randomY = Random.Range(-1.0f * strength, strength);

        // ���݂̈ʒu�ɉ�����
        var position = currentPosition;
        position.x += randomX;
        position.y += randomY;

        // �ݒ肵���Ԋu�Ɏ��߂�
        var vibrato = shakeInfo.Vibrato;
        var ratio = 1.0f - totalTime / shakeInfo.Duration;
        vibrato *= ratio; // �t�F�[�h�A�E�g�����邽�߁A�o�ߎ��Ԃɂ��h��̗ʂ�����
        position.x = Mathf.Clamp(position.x, initPosition.x - vibrato, initPosition.x + vibrato);
        position.y = Mathf.Clamp(position.y, initPosition.y - vibrato, initPosition.y + vibrato);
        return position;
    }


    // �h��J�n
    public void StartShake(float duration, float strength, float vibrato)
    {
        // �h�����ݒ肵�ĊJ�n
        _shakeInfo = new ShakeInfo(duration, strength, vibrato);
        _isDoShake = true;
        _totalShakeTime = 0.0f;
    }
}

