using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class ButtonManager : MonoBehaviour
{
    private BulletSkill bulletskill;
    private InputSystem_PlayerActions inputAction;
    private PauseManager _pauseManager;

    // ローディングシーンの名前
    private string loadingSceneName = "Loading";

    void Start()
    {
        inputAction = new InputSystem_PlayerActions();
        _pauseManager = FindAnyObjectByType<PauseManager>();
        bulletskill = new BulletSkill();
    }

    void Update()
    {
    }

    public void OnOptionWindow()
    {
        _pauseManager.ShowPause();
    }

    public void OnStage1Button()
    {
        OnStageButtonClicked("RealScene1", 1);
    }

    public void OnStage2Button()
    {
        OnStageButtonClicked("RealScene2", 2);
    }

    public void OnStage3Button()
    {
        OnStageButtonClicked("RealScene3", 3);
    }

    public void OnStage4Button()
    {
        OnStageButtonClicked("RealScene4", 4);
    }

    // ステージボタンが押されたときに呼び出される関数
    public void OnStageButtonClicked(string stageSceneName, int stageNumber)
    {
        PlayerPrefs.SetString("SelectedStage", stageSceneName);
        PlayerPrefs.SetInt("SelectedStageNumber", stageNumber);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Loading");
    }

    // ゲームを終了する
    public void ExitGame()
    {
        Application.Quit();
    }

    // ローディングシーンに移動
    public void GoToLoadingSecene()
    {
        SceneManager.LoadScene("Loading");
    }

    // タイトル画面に戻る
    public void BackToTitle()
    {
        if (GameData.Instance != null)
            GameData.Instance.ResetAll();
        bulletskill.SetGauge(0f);
        SceneManager.LoadScene("Title");
    }

    // ステージ選択画面に移動
    public void GoToSceneSelecter()
    {
        SceneManager.LoadScene("SceneSelecter");
    }

    // ゲームを再開する
    public void ResumeGame()
    {
        StartCoroutine(ResumeWithDelay());
    }

    private IEnumerator ResumeWithDelay()
    {
        yield return null; // 1フレーム待機
        Time.timeScale = 1f;
        _pauseManager.pausePanel.SetActive(false);
        _pauseManager.isPaused = false;
    }

    // オプション画面を開く
    public void OpenOptionPanel()
    {
        _pauseManager.pausePanel.SetActive(false);
        _pauseManager.OptionPanel.SetActive(true);
    }

    // ゲーム画面に戻る
    public void BackToStage()
    {
        // 選択されたステージ名を取得
        string stageName = PlayerPrefs.GetString("SelectedStage", "Scene");
        SceneManager.LoadScene(stageName);
        bulletskill.SetGauge(0f);
        Time.timeScale = 1.0f;
    }
}
