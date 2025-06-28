using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �N���A���̃C��?�W�摜
    [SerializeField] 
    GameObject ClearUIPrefab;

    // 
    [SerializeField] 
    GameObject UIParent;

    // �X�R�A?�l?�W��?�̃X�N���v�g
  
    GameObject ScoreManager;

    // ?�C???�l�W��?�̃X�N���v�g
    [SerializeField]
	TimerManager timerManager;

    private PlayerHP _playerHP;

    bool IsClearShown = false;

    public static GameManager Instance;


    // �V���O���g�[��
	private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        Time.timeScale = 1.0f;

        // �^�C�}�[�}�l�W���[��T��
        timerManager = FindObjectOfType<TimerManager>();

        // �v���C���[HP�̃X�N���v�g��T��
        _playerHP = FindAnyObjectByType<PlayerHP>();
    }

    // Update is called once per frame
    void Update()
    {
        // �f�o�b�N�pESC���������烊�X�^�[�g
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Restart();
        //}
        // �N���A���肳��ăN���A�C���[�W���\������Ď��_�ŃL�[�������ƃ��U���g��ʂɈړ�
        if (Input.anyKeyDown && IsClearShown)
        {
			LoadResult();
        }
    }

    // �@�\   �F�v���C�V�[���Ɉړ�����֐�
    // ����   �F����
    // �߂�l  �F����
	private void Restart()
	{
        SceneManager.LoadScene("RealScene");
        Time.timeScale = 1.0f;
    }

    // �@�\   �F�^�C�g���V�[���Ɉړ�����֐�
    // ����   �F����
    // �߂�l  �F����
    private void LoadTitle()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1.0f;
    }

    // �@�\   �F���U���g�V�[���Ɉړ�����֐�
    // ����   �F����
    // �߂�l  �F����
    private void LoadResult()
    {
        SceneManager.LoadScene("Result");
    }

    // �@�\   �F�Q�[���I�o�[�V�[���Ɉړ�����֐�
    // ����   �F����
    // �߂�l  �F����
    public void ChangeOverScene()
    {
        SceneManager.LoadScene("Dead");
    }

    // �@�\   �F�v���C���[����N���A��������ă^�C�}�[�ƃv���C���[HP�ϓ�(�J�E���g)���~
    // ����   �F����
    // �߂�l  �F����
    public void Clear()
    {
        
		// �Q??���N���A�����Ɣ���
		timerManager.VictoryGame = true;

        timerManager.StopTimer();

        _playerHP.StopCountHP();

        // ���݂̃X�e�[�W�����g���ăX�e�[�W�ԍ��𒊏o
        int clearedStage = PlayerPrefs.GetInt("SelectedStageNumber", 1);
        int nextStage = clearedStage + 1;

        // �e�[�W�̃A�����b�N
        if (!PlayerPrefs.HasKey("Stage" + nextStage + "_Unlocked"))
        {
            PlayerPrefs.SetInt("Stage" + nextStage + "_Unlocked", 1);
            PlayerPrefs.Save();
        }


        Instantiate(ClearUIPrefab, UIParent.transform);
        Time.timeScale = 0.0f;
        IsClearShown = true;

    }
}
