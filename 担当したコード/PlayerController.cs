using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// ---------------------------------------------
// プレイヤーコントローラークラス
// 用途：プレイヤーを操作するため
// 作成者：23cu0216 相馬理玖
// 作成日：2024/09/30 作成開始
//       :2024/10/4 作成完了
//       :2024/10/28 修正開始 Playerの壁衝突判定を修正
// ---------------------------------------------

public class PlayerController : MonoBehaviour
{
    // プレイヤーの状態
    public enum PlayerStatus
    {
        IDLE,
        FINE,
        DEAD,
        RESPAWN,
        CLEAR
    }

    private enum KeyDown
    {
        none,
        left,
        right
    }

    private enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] private PlayerStatus m_PlayerStatus;   // Playerの状態
    private Rigidbody2D m_Rigidbody;    // 
    private Transform m_transform;

    private PlayerHitPoints m_PlayerHitPoints;

    [SerializeField] private float MoveSpeed;   // プレイヤーの移動速度
    [SerializeField] private float RotateSpeed;// プレイヤーの回転速度
    private float m_angle = 0.0f; // 回転方向
    private float m_nowangle = 0.0f;  // 回転中の現在回転した角度
    private bool m_Isrotate;  // 回転中か
    KeyDown m_KeyDown;  // 押されたキー
    [SerializeField]Direction m_Direction; // 壁に接触してる際に使用する

    PlayerInput plyaerInput;

    [SerializeField] GameObject m_MapObj;   // Map情報確認用
    private MapEditor m_MapEditor;  // MapEditor格納用
    [SerializeField] private bool m_IsWall; // 壁があるか

    [SerializeField] private GameObject m_BulletMngObj; // BulletManagerオブジェクト格納用
    private BulletManager m_BulletMng;  // BulletManager格納用

    [SerializeField] private float m_SpeedUpTime; // スピードアップの効果時間
    [SerializeField] private float m_SpeedMagnification;    // スピードアップ時の速度アップ倍率
    private float m_NowSpeedUpTime; // スピードアップ効果継続時間
    [SerializeField] private bool m_CanSpeedUp;   // スピードアップできるか？

    private bool m_CanAddMine;  // 地雷増加できるか?

    void Start()
    {
        // 各変数の初期化
        m_transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PlayerHitPoints = GetComponent<PlayerHitPoints>();
        m_MapEditor = m_MapObj.GetComponent<MapEditor>();
        m_PlayerStatus = PlayerStatus.IDLE;
        m_Isrotate = false;
        m_KeyDown = KeyDown.none;
        m_Direction = Direction.None;
        m_IsWall = false;
        m_BulletMng = GameObject.Find(m_BulletMngObj.name).GetComponent<BulletManager>();
        m_NowSpeedUpTime = 0.0f;
        m_CanSpeedUp = false;
        m_CanAddMine = false;
    }

    void Update()
    {
        // Playerの体力がなければ
        if (m_PlayerHitPoints.IsDead())
        {
            // プレイヤーを死亡状態にする
            m_PlayerStatus = PlayerStatus.DEAD;
        }

        switch (m_PlayerStatus)
        {
            // 待機状態
            case PlayerStatus.IDLE:
                break;
            // 通常状態
            case PlayerStatus.FINE:
                // 移動処理や回転処理を行う
                ChangeRotation();
                Move();
                break;
            // 死亡状態
            case PlayerStatus.DEAD:
                break;
            // 復帰状態
            case PlayerStatus.RESPAWN:
                break;
            // クリア状態
            case PlayerStatus.CLEAR:
                break;
            default:
                break;
        }
    }
    // 移動処理
    private void Move()
    {
        // 現在情報と角度を取得
        Vector2 PlayerPos = m_transform.position;
        var angles = m_transform.rotation.eulerAngles;


        // プレイヤーが回転中でなければ移動する
        if (!m_Isrotate)
        {
            //前方ベクトル
            Vector2 vForward = transform.right;
            Vector2 vUp = transform.up;

            Vector2 texsize = vForward / 2.0f;
            Vector2 pointinterval = vUp / 3.0f;
            float Speed = MoveSpeed; // 移動量

            // PlayerがSpeedUp状態であれば
            if (m_CanSpeedUp)
            {
                // 移動速度を上げる
                Speed *= m_SpeedMagnification;
                // 効果時間を計測
                m_NowSpeedUpTime += Time.deltaTime;

                // 効果をリセットする
                if (m_NowSpeedUpTime > m_SpeedUpTime)
                {
                    m_CanSpeedUp = false;
                    m_NowSpeedUpTime = 0.0f;
                }
            }
                Vector2 vMove = vForward * (Speed * Time.deltaTime);

            // Playerの先頭の上下に2点作る
            float fx1 = (PlayerPos.x + texsize.x + vMove.x + pointinterval.x);
            float fy1 = (PlayerPos.y + texsize.y + vMove.y + pointinterval.y);

            float fx2 = (PlayerPos.x + texsize.x +vMove.x - pointinterval.x);
            float fy2 = (PlayerPos.y + texsize.y + vMove.y - pointinterval.y);

            // 移動しようとしている場所に壁がある確認
            m_IsWall = m_MapEditor.IsInsideWall(fx1, fy1) || m_MapEditor.IsInsideWall(fx2,fy2);

            // 壁がなければ
            if (!m_IsWall)
            {
                // 移動する
                m_transform.transform.Translate(vMove.magnitude, 0.0f,0.0f);
            }
            // 壁があった場合
            else
            {
                switch (m_Direction)
                {
                    case Direction.Right:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.x = (float)((int)PlayerPos.x + 1);
                        }
                        break;
                    case Direction.Up:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.y = (float)((int)PlayerPos.y);
                        }
                        break;
                    case Direction.Left:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.x = (float)((int)PlayerPos.x);
                        }
                        break;
                    case Direction.Down:
                        {
                            Vector3 v = m_transform.transform.position;
                            v.y = (float)((int)PlayerPos.y-1);
                        }
                        break;
                }
            }
        }
    }

    // 回転処理
    private void ChangeRotation()
    {
        bool completionRotation = false;

        switch (m_KeyDown)
        {
            case KeyDown.none:
                break;
            case KeyDown.left:
                m_nowangle += RotateSpeed;
                // 現在の角度に指定された角度まで回転する
                if (m_angle > m_nowangle)
                {
                    m_transform.Rotate(0.0f, 0.0f, RotateSpeed);
                }
                else
                {
                    completionRotation = true;
                }
                break;
            case KeyDown.right:
                m_nowangle -= RotateSpeed;
                // 現在の角度に指定された角度まで回転する
                if (m_angle < m_nowangle)
                {
                    m_transform.Rotate(0.0f, 0.0f, -RotateSpeed);
                }
                else
                {
                    completionRotation = true;
                }
                break;
        }

        // 補正を行う
        if (completionRotation)
        {
            // 現在の角度を取得
            var angles = m_transform.rotation.eulerAngles;

            // 現在の角度をint型にし、理想の角度を計算する
            int anglesnum = (int)((angles.z / 90)+0.5f);
            angles.z = anglesnum * 90.0f;

            // 現在の角度から差を引き理想の角度にする
            m_transform.rotation = Quaternion.Euler(0.0f, 0.0f,angles.z);

            // 後処理
            m_nowangle = 0.0f;
            m_Isrotate = false;
            m_KeyDown = KeyDown.none;
        }
    }

    // Playerの衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // NULLチェック
        if (collision == null) return;

        // 衝突したアイテムがSpeedUPアイテムであれば
        if(collision.tag == "SpeedUp")
        {
            // 効果発揮中に取った場合は重ね書きを行う
            if (m_CanSpeedUp) m_NowSpeedUpTime = 0.0f;
            // スピードアップ状態にする
            m_CanSpeedUp = true;
        }

        // 衝突したアイテムがAddMineならば
        if(collision.tag == "AddMine")
        {
            m_CanAddMine = true;
        }
    }

    // 地雷を追加できる状態か？
    public bool CanAddMine()
    {
        return m_CanAddMine;
    }

    // 回転中か？
    public bool IsRotate()
    {
        return m_Isrotate;
    }

    // プレイヤーの状態を設定
    public void SetPlayerStatus(int  statusnum)
    {
        m_PlayerStatus = (PlayerStatus)statusnum;
    }

    // プレイヤーの地雷設置可能状態を設定
    public void SetCanAddMine(bool canaddmine = false)
    {
        m_CanAddMine = canaddmine;
    }

    // 向いている方向を取得（オイラー角）
    public Vector3 GetEulerAnglesZ()
    {
        return m_transform.rotation.eulerAngles;
    }

    // プレイヤーの現在位置を取得
    public Vector3 GetPosition()
    {
        return m_transform.position;
    }

    public int GetPlayerStatus()
    {
        return (int)m_PlayerStatus;
    }

    // 各InputSystemのトリガー
    // 射撃キー
    public void OnShooting(InputValue inputValue)
    {
        // Playerが通常状態出なければ射撃ができない
        if(m_PlayerStatus == PlayerStatus.FINE)
        {
            m_BulletMng.PlayerShooting();
        }
    }

    // 地雷設置キー
    public void OnSetMine(InputValue inputValue)
    {
        // Playerが通常状態出なければ射撃ができない
        if (m_PlayerStatus == PlayerStatus.FINE)
        {
            m_BulletMng.PlayerSetMines();
        }
    }

    // 左回転キー
    public void OnRotateLeft(InputValue inputValue)
    {
        // Playerが通常状態で
        // 回転中出なければ回転できる
        if (m_PlayerStatus == PlayerStatus.FINE && !m_Isrotate)
        {
            m_angle = 90;
            m_KeyDown = KeyDown.left;
            m_Isrotate = true;
        }
    }

    // 右回転キー
    public void OnRotateRight(InputValue inputValue)
    {
        // Playerが通常状態で
        // 回転中出なければ回転できる
        if (m_PlayerStatus == PlayerStatus.FINE && !m_Isrotate)
        {
            m_angle = -90;
            m_KeyDown = KeyDown.right;
            m_Isrotate = true;
        }
    }

    // 強制的にリザルト画面に贈られるキー
    public void OnLoadResultScene(InputValue inputValue)
    {
        SceneManager.LoadScene("Result");
    }
}