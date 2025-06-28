using UnityEngine;
using UnityEngine.UI;

public class StageUnLockManager : MonoBehaviour
{
    // �C���X�y�N�^�[�Őݒ肷��X�e�[�W�̃{�^��
    public Button[] StageButtons;

    // �X�e�[�W�̍ő吔
    public int maxStageCount = 4;

    // ���͗p��string�i�V�[�����ɐݒu�j
    private string commandInput = "";

    // �X�e�[�W�A�����b�N�󋵂�\������C���[�W
    public GameObject lock2;
    public GameObject unlocked2;
    public GameObject lock3;
    public GameObject unlocked3;
    public GameObject lock4;
    public GameObject unlocked4;

    private void Start()
    {
        for (int i = 0; i < StageButtons.Length; i++)
        {
            int stageNumber = i + 1;
            if (stageNumber == 1 || PlayerPrefs.GetInt("Stage" + stageNumber + "_Unlocked", 0) == 1)
            {
                StageButtons[i].interactable = true;
            }
            else
            {
                StageButtons[i].interactable = false;
            }
        }

        JudgementofStageUnlocked();
    }

    // �R�}���h������
    void HandleCommand(string input)
    {
        if (input.Equals("U", System.StringComparison.OrdinalIgnoreCase))
        {
            UnlockAllStages();
        }
        else if (input.Equals("L", System.StringComparison.OrdinalIgnoreCase))
        {
            LockAllStagesExceptStage1();
        }

        // ���͗�����ɂ���
        commandInput = "";
    }

    void UnlockAllStages()
    {
        for(int i = 1; i <= maxStageCount; i++)
        {
            PlayerPrefs.SetInt("Stage" + i + "_Unlocked", 1);
        }

        PlayerPrefs.Save();
        Debug.Log("UnLock");
    }

    void LockAllStagesExceptStage1()
    {
        for (int i = 2; i <= maxStageCount; i++)
        {
            PlayerPrefs.SetInt("Stage" + i + "_Unlocked", 0);
        }
        PlayerPrefs.Save();
        Debug.Log("Lock");
    }

    // �X�e�[�W�N���A���Ɏ��̃X�e�[�W���A�����b�N���鏈��
    public static void UnlockNextStage(int CurrentStage)
    {
        int NextStage = CurrentStage + 1;

        //���̃X�e�[�W���A�����b�N��Ԃɕۑ�
        PlayerPrefs.SetInt("RealScene" + NextStage + "_Unlocked", 1);
        PlayerPrefs.Save();
    }

    private void JudgementofStageUnlocked()
    {
        if (PlayerPrefs.GetInt("Stage2_Unlocked", 0) == 1)
        {
            lock2.SetActive(false);
            unlocked2.SetActive(true);
        }
        else
        {
            lock2.SetActive(true);
            unlocked2.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Stage3_Unlocked", 0) == 1)
        {
            lock3.SetActive(false);
            unlocked3.SetActive(true);
        }
        else
        {
            lock3.SetActive(true);
            unlocked3.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Stage4_Unlocked", 0) == 1)
        {
            lock4.SetActive(false);
            unlocked4.SetActive(true);
        }
        else
        {
            lock4.SetActive(true);
            unlocked4.SetActive(false);
        }
    }

    ////�f�o�b�O�p�S�X�e�[�W���b�N�����Z�b�g
    //public static void ResetStage()
    //{
    //    for(int i = 2; i <= 10; i++)
    //    {
    //        PlayerPrefs.DeleteKey("RealScene" + i + "_Unlocked");
    //    }
    //    PlayerPrefs.Save();
    //}
}
