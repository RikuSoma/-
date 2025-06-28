using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // クリア時のイメ?ジ画像
    [SerializeField] 
    GameObject ClearUIPrefab;

    // 
    [SerializeField] 
    GameObject UIParent;

    // スコア?ネ?ジャ?のスクリプト
  
    GameObject ScoreManager;

    // ?イ???ネジャ?のスクリプト
    [SerializeField]
	TimerManager timerManager;

    private PlayerHP _playerHP;

    bool IsClearShown = false;

    public static GameManager Instance;


    // シングルトーン
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

        // タイマーマネジャーを探す
        timerManager = FindObjectOfType<TimerManager>();

        // プレイヤーHPのスクリプトを探す
        _playerHP = FindAnyObjectByType<PlayerHP>();
    }

    // Update is called once per frame
    void Update()
    {
        // デバック用ESCを押したらリスタート
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Restart();
        //}
        // クリア判定されてクリアイメージが表示されて時点でキーを押すとリザルト画面に移動
        if (Input.anyKeyDown && IsClearShown)
        {
			LoadResult();
        }
    }

    // 機能   ：プレイシーンに移動する関数
    // 引数   ：無し
    // 戻り値  ：無し
	private void Restart()
	{
        SceneManager.LoadScene("RealScene");
        Time.timeScale = 1.0f;
    }

    // 機能   ：タイトルシーンに移動する関数
    // 引数   ：無し
    // 戻り値  ：無し
    private void LoadTitle()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1.0f;
    }

    // 機能   ：リザルトシーンに移動する関数
    // 引数   ：無し
    // 戻り値  ：無し
    private void LoadResult()
    {
        SceneManager.LoadScene("Result");
    }

    // 機能   ：ゲームオバーシーンに移動する関数
    // 引数   ：無し
    // 戻り値  ：無し
    public void ChangeOverScene()
    {
        SceneManager.LoadScene("Dead");
    }

    // 機能   ：プレイヤーからクリア判定を貰ってタイマーとプレイヤーHP変動(カウント)を停止
    // 引数   ：無し
    // 戻り値  ：無し
    public void Clear()
    {
        
		// ゲ??をクリアしたと判定
		timerManager.VictoryGame = true;

        timerManager.StopTimer();

        _playerHP.StopCountHP();

        // 現在のステージ名を使ってステージ番号を抽出
        int clearedStage = PlayerPrefs.GetInt("SelectedStageNumber", 1);
        int nextStage = clearedStage + 1;

        // テージのアンロック
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
