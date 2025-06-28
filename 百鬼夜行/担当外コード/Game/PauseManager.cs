using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private ButtonManager _buttonManager;

    public GameObject pausePanel;

    public GameObject OptionPanel;

    public bool isPaused = false;

    public SelectManager SelectManager;

    public GameObject[] mainSceneButtons;

    // ESC�L?�܂���Start??����??�Y���J���A�N�V����
    private InputAction pauseOpenAction;

    // ESC�L?�܂���Back??����??�Y�����A�N�V����
    private InputAction pauseCloseAction;

    private void Awake()
    {
        // ??�Y���J�����́i�L???�h�FESC�A�Q??�p�b�h�FStart�j
        pauseOpenAction = new InputAction("PauseOpen", binding: "<Keyboard>/escape");
        pauseOpenAction.AddBinding("<Gamepad>/start");
        pauseOpenAction.Enable();

        // ??�Y�������́i�L???�h�FESC�A�Q??�p�b�h�FBack/Select�j
        pauseCloseAction = new InputAction("PauseClose", binding: "<Keyboard>/escape");
        pauseCloseAction.AddBinding("<Gamepad>/select");
        pauseCloseAction.Enable();
    }

    private void Start()
    {
        _buttonManager = FindAnyObjectByType<ButtonManager>();
    }

    void Update()
    {
        // ??�Y���J���Ă��Ȃ���ԂŁA�J�����͂��󂯂���??�Y��?��
        if (!isPaused && pauseOpenAction.triggered)
        {
            ShowPause();
        }
        // ??�Y���ŁA������͂��󂯂��ꍇ
        else if (isPaused && pauseCloseAction.triggered)
        {
            // �I�v�V������ʂ��J���Ă���ꍇ �� ����??�Y��ʂɖ߂�
            if (OptionPanel.activeSelf)
            {
                OptionPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
            // ����ȊO�̏ꍇ �� �Q??�ĊJ
            else
            {
                _buttonManager.ResumeGame();
                
            }
        }
    }

    public void ShowPause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        isPaused = true;

    }

    //public void BackToTitle()
    //{
    //    Time.timeScale = 1f;
    //    SceneManager.LoadScene("Title");
    //}
}
