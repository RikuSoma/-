using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------
// Bulletクラス
// 用途：弾クラス、主に移動などを行う
// 作成者：23cu0216　相馬理玖
// 作成日：2024/10/04 作成開始
// ------------------------------------

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerObject; // Playerの名前取得用
    private readonly float MoveSpeed = 0.1f;    // 移動速度
    Vector2 m_Velocity;    // 移動量

    SpriteRenderer m_SpriteRenderer; 

    GameObject m_PlayerObj; // Player格納用
    PlayerController m_PlayerController;    // PlayerController格納用

    [SerializeField] private float m_Power; // 弾の攻撃力

    private bool m_IsSetting;   // Bulletの設定が完了したか？

    [SerializeField] private GameObject ExpObj; // 爆発アニメーション用

    void Start()
    {
        // 各変数の初期化
        m_PlayerObj = GameObject.Find(m_PlayerObject.name);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_PlayerController = m_PlayerObj.GetComponent<PlayerController>();
        m_Velocity = Vector2.zero;
        m_IsSetting = false;
    }
    
    void Update()
    {
        // Bulletの設定をしていなければ設定を行う
        if (!m_IsSetting)
        {
            // プレイヤーの回転角度を取得
            Vector3 Playerangles = m_PlayerController.GetEulerAnglesZ();
            SettingBullet(Playerangles.z);
            m_IsSetting = true;
        }

        // 設定完了したら
        // BulletのTag付けを行う
        if(m_IsSetting)
        {
            tag = "Bullet";
        }

        // 移動処理
        BulletMove();
    }

    // 弾の移動関数
    // 引数にオイラー角の数値をいれる
    private void BulletMove() 
    {
        GetComponent<Rigidbody2D>().velocity = m_Velocity / Time.deltaTime;
    }

    // 射撃前の設定(角度や移動方向など)
    private void SettingBullet(float angles)
    {
        // プレイヤーの向いている方向を確認する
        // 上：90°　左：180°　下：270°　右：0°or 360°
        int anglesnum = (int)((angles / 90.0f) + 0.5f);

        // 方向に合わせて移動量を変える
        if (anglesnum == 1)
        {
            m_Velocity.y += MoveSpeed;
        }
        else if (anglesnum == 2)
        {
            m_Velocity.x -= MoveSpeed;
        }
        else if (anglesnum == 3)
        {
            m_Velocity.y -= MoveSpeed;
        }
        else if (anglesnum == 0 || anglesnum == 4)
        {
            m_Velocity.x += MoveSpeed;
        }
    }

    // 各オブジェクトとの衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        // 当たったのがPlayerかつ射撃したPlayerでなければ
        if(collision.tag == "Player" && collision.name != m_PlayerObject.name)
        {
            // 弾を透明化
            m_SpriteRenderer.color = new Vector4(m_SpriteRenderer.color.r, m_SpriteRenderer.color.g, m_SpriteRenderer.color.b, 0.0f);

            // 爆発オブジェクトをインスタンス化
            Instantiate(ExpObj,transform.position,Quaternion.identity);

            // 自身を削除する
            Destroy(gameObject);
        }

        // 当たったのがWallであれば
        if (collision.tag == "Wall")
        {
            // 爆発オブジェクトをインスタンス化
            Instantiate(ExpObj, transform.position, Quaternion.identity);
            // 自身を削除
            Destroy(gameObject);
        }

    }

    // Bulletの攻撃力を取得
    public float GetBulletPower()
    {
        return m_Power;
    }
}
