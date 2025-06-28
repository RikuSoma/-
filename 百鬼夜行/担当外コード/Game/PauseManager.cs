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

    // ESCキ?またはStart??ンで??ズを開くアクション
    private InputAction pauseOpenAction;

    // ESCキ?またはBack??ンで??ズを閉じるアクション
    private InputAction pauseCloseAction;

    private void Awake()
    {
        // ??ズを開く入力（キ???ド：ESC、ゲ??パッド：Start）
        pauseOpenAction = new InputAction("PauseOpen", binding: "<Keyboard>/escape");
        pauseOpenAction.AddBinding("<Gamepad>/start");
        pauseOpenAction.Enable();

        // ??ズを閉じる入力（キ???ド：ESC、ゲ??パッド：Back/Select）
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
        // ??ズが開いていない状態で、開く入力を受けたら??ズを?示
        if (!isPaused && pauseOpenAction.triggered)
        {
            ShowPause();
        }
        // ??ズ中で、閉じる入力を受けた場合
        else if (isPaused && pauseCloseAction.triggered)
        {
            // オプション画面が開いている場合 → 閉じて??ズ画面に戻す
            if (OptionPanel.activeSelf)
            {
                OptionPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
            // それ以外の場合 → ゲ??再開
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
