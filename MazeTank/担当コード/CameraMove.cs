// 
// 名前       ：CameraMoveクラス
// 使用用途   ：ゲームカメラの挙動制御
// 作成者     ：相馬理玖
// 作成日     ：9月20日
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
    private GameObject m_GameSceneMng;   // ゲームシーンマネージャー格納用

    [SerializeField] private float m_ShakeStrength;     // 揺れる強さ
    [SerializeField] private float m_ShakeTime; // 揺れの時間
    [SerializeField] private float m_ShakeVibrato; // 揺れの大きさ
    Vector3 m_Position; // カメラの初期位置
    private float m_NowTime;

    private GameObject m_BattleManager;     // battleManager格納用

    private bool m_IsSetting; // 設定が完了したか

    private bool m_Change;

    /// 揺れ情報
    private struct ShakeInfo
    {
        public ShakeInfo(float duration, float strength, float vibrato)
        {
            Duration = duration;
            Strength = strength;
            Vibrato = vibrato;
        }
        public float Duration { get; } // 時間
        public float Strength { get; } // 揺れの強さ
        public float Vibrato { get; }  // どのくらい振動するか
    }

    private ShakeInfo _shakeInfo;

    private Vector3 _initPosition; // 初期位置
    private bool _isDoShake;       // 揺れ実行中か？
    private float _totalShakeTime; // 揺れ経過時間

    // Start is called before the first frame update
    void Start()
    {
        m_GameSceneMng = GameObject.Find("SceneManager");
        m_Position = transform.position;
        m_IsSetting = false; 
        m_NowTime = 0.0f;
        m_Change = true;

        // 初期位置を保持
        _initPosition = gameObject.transform.position;
        _isDoShake = true;
    }

    // Update is called once per frame
    void Update()
    {
        // NULLチェック
        if (m_GameSceneMng == null) return;

        // 現在のゲームシーンを取得
         GameScene gameScene = (GameScene)m_GameSceneMng.GetComponent<GameSceneManager>().GetGameScene();

        if(gameScene == GameScene.Play)
        {
            // BattleManagerの設定
            if(!m_IsSetting)
            {
                m_BattleManager = GameObject.Find(m_GameSceneMng.GetComponent<GameSceneManager>().GetBattleMNGName());
                m_IsSetting = true;
            }

            // BattleManagerのNULLチェック
            if (m_BattleManager == null) return;

            // battleの決着がついた場合
            if(m_BattleManager.GetComponent<BattleManager>().GetIsConclusion() && _isDoShake)
            {
                // 揺れの情報設定
                StartShake(m_ShakeTime, m_ShakeStrength, m_ShakeVibrato);

                // 揺れ位置情報更新
                gameObject.transform.position = UpdateShakePosition(
                    gameObject.transform.position,
                    _shakeInfo,
                    _totalShakeTime,
                    _initPosition);

                // duration分の時間が経過したら揺らすのを止める
                _totalShakeTime += Time.deltaTime;
                if (_totalShakeTime >= _shakeInfo.Duration)
                {
                    _isDoShake = false;
                    _totalShakeTime = 0.0f;
                    // 初期位置に戻す
                    gameObject.transform.position = _initPosition;
                }

            }
        }
    }
    // 更新後の揺れ位置を取得
    private Vector3 UpdateShakePosition(Vector3 currentPosition, ShakeInfo shakeInfo, float totalTime, Vector3 initPosition)
    {
        // 揺れの強さを取得
        var strength = shakeInfo.Strength;
        var randomX = Random.Range(-1.0f * strength, strength);
        var randomY = Random.Range(-1.0f * strength, strength);

        // 現在の位置に加える
        var position = currentPosition;
        position.x += randomX;
        position.y += randomY;

        // 設定した間隔に収める
        var vibrato = shakeInfo.Vibrato;
        var ratio = 1.0f - totalTime / shakeInfo.Duration;
        vibrato *= ratio; // フェードアウトさせるため、経過時間により揺れの量を減衰
        position.x = Mathf.Clamp(position.x, initPosition.x - vibrato, initPosition.x + vibrato);
        position.y = Mathf.Clamp(position.y, initPosition.y - vibrato, initPosition.y + vibrato);
        return position;
    }


    // 揺れ開始
    public void StartShake(float duration, float strength, float vibrato)
    {
        // 揺れ情報を設定して開始
        _shakeInfo = new ShakeInfo(duration, strength, vibrato);
        _isDoShake = true;
        _totalShakeTime = 0.0f;
    }
}

