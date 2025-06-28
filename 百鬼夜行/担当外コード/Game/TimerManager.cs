using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class TimerManager : MonoBehaviour
{

	// ?イ??をカウントする変数
	private float StartTimer;

	// 経過した?イ??を計算する変数
	private float ElapsedTimer;

	//	ゲ??終了を判断する変数
	public bool VictoryGame = false;

	private GameData _gameData;
	void Start()
    {
        _gameData = GameData.Instance;
        // ゲ??がス??トしたら?イ?計測
        StartTimer = Time.time;
        VictoryGame = false;
    }

    // Update is called once per frame
    void Update()
    {
		// 毎フレ??経過時間を計算し代入
		if (!VictoryGame)
		{
			CountTime();
		}
		else
		{
			StopTimer();
		}
	}

    // 機能   ：プレイが始まったらプレイ時間をカウント始める関数
    // 引数   ：無し
    // 戻り値  ：無し
    private void CountTime()
	{
		float CurrentTime = Time.time - StartTimer;
	
	}

    // 機能   ：クリア判定がされたらタイマーのカウントを停止してGameDataに送る関数
    // 引数   ：無し
    // 戻り値  ：無し
    public float StopTimer()
	{
		// ?イ??停?
		ElapsedTimer = Time.time;
		float EndTimer = ElapsedTimer - StartTimer;

        if (_gameData != null)
        {
            _gameData.saveTime(EndTimer);
            Debug.Log("[TimerManager] Time saved: " + FormatTime(EndTimer));
        }

        return EndTimer;
	}

    public static string FormatTime(float timeInSeconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(timeInSeconds);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
