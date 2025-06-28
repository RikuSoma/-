using UnityEngine;

public class GameData : MonoBehaviour
{
    public float PlayTime;

    public float PlayerHP;

    public float PlayerMaxHP = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static GameData instance;
    public static GameData Instance => instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetAll()
    {
        PlayTime = 0f;
        PlayerHP = 0f;
    }

    // 機能   ：タイマーマネージャーからプレイタイムを貰ってこのスクリプトの変数に代入関数
    // 引数   ：float型、変数名time
    // 戻り値  ：無し
    public void saveTime(float time)
    {
        PlayTime = time;
        
    }

    // 機能   ：プレイヤーHPスクリプトからプレイ中に変動されたHP情報を取得し代入関数
    // 引数   ：float型、変数名HP
    // 戻り値  ：無し
    public void savePlayerHP(float HP)
    {
        PlayerHP = HP;
        Debug.Log("KUSO" + PlayerHP);
    }

}
