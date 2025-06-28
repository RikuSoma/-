using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string defaultSceneName = "RealScene1";

    [SerializeField] 
    private float minDelay = 3f;

    [SerializeField] 
    private float maxDelay = 7f;

    void Start()
    {
        Time.timeScale = 1f;

        Debug.Log("[SceneLoader] Start");

        // 
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            Debug.Log("[SceneLoader] Animator");
            anim.Play(0, 0, 0f);
        }
        else
        {
            Debug.LogWarning("[SceneLoader] Animator");
        }

        float delay = Random.Range(minDelay, maxDelay);
        Debug.Log($"[SceneLoader] Delay set to {delay}");

        Invoke(nameof(LoadNextScene), delay);

    }


    void LoadNextScene()
    {
        // 保存されたステージ情報を取得
        string nextSceneName = PlayerPrefs.GetString("SelectedStage", defaultSceneName);
        int stageNumber = PlayerPrefs.GetInt("SelectedStageNumber", -1);

        Debug.Log($"[SceneLoader] シーン名: {nextSceneName}, ステージ番号: {stageNumber}");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("[SceneLoader] シーン名が設定されていません。");
        }
    }
}
