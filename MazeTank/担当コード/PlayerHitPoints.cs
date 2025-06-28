using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerController;

/// <summary>
///  PlayerHitpointクラス
///  PlayerのHP管理を行うクラス
///  作成日：2024/10/16 作成開始
/// </summary>

public class PlayerHitPoints : MonoBehaviour
{
    [SerializeField] private float m_PlayerHP;  // 体力
    [SerializeField] private readonly float m_InvincibleTime = 2.0f;    // 無敵時間
    [SerializeField] private float m_InvincibleNowTime; // 無敵経過時間

    private PlayerController m_Controller;  // PlayerControllerクラス格納用
    private PlayerStatus m_PlayerStatus;   // Playerの状態を確認用

    [SerializeField] private bool m_IsHitBullet;    // PlayerがBulletに当たったか?
    [SerializeField] private bool m_IsHitLandmine; // PlayerがLandminesに当たったか？

    [SerializeField] private GameObject m_Bullet;   // Playerが当たらない弾

    struct DamageInfo
    {
        public Vector3 vForword;
        public float damage;
    };

    DamageInfo mDamageInfo = new DamageInfo();
    

    // Start is called before the first frame update
    void Start()
    {
        // 体力初期値は3にする
        m_PlayerHP = 3.0f;
        m_InvincibleNowTime = 0.0f;

        // PlayerControllerを持ってくる（PlayerStatusを使用するため）
        m_Controller = this.GetComponent<PlayerController>();
        m_PlayerStatus = PlayerStatus.IDLE;

        m_IsHitBullet = false;
        m_IsHitLandmine = false;
    }

    // Update is called once per frame
    void Update()
    {
        // PlayerControllerから現在のPlayerの状態を取得
        m_PlayerStatus = (PlayerStatus)m_Controller.GetPlayerStatus();

        switch (m_PlayerStatus)
        {
            // 待機状態は無敵
            case PlayerStatus.IDLE:
                break;
            // 通常状態では当たり判定、HPの管理を行う
            case PlayerStatus.FINE:
                UpdateHP();

                // 体力が0以下になれば死亡状態になる
                if(m_PlayerHP <= 0.0f)
                {
                    m_PlayerStatus = PlayerStatus.DEAD;
                }
                break;
            // 死亡状態
            case PlayerStatus.DEAD:
                break;
            case PlayerStatus.RESPAWN:
                break;
            case PlayerStatus.CLEAR:
                break;
            default:
                break;
        }
    }
        
    // HPの更新処理
    private void UpdateHP()
    {
        // プレイヤーが弾に当たっているか
        if (m_IsHitBullet && m_InvincibleNowTime == 0.0f)
        {
            // 前方ベクトル取得
            Vector3 PlayerSide = m_Controller.transform.right;
            Vector3 BulletSide = mDamageInfo.vForword;

            // プレイヤーとBulletの前方ベクトルを比較する
            if (-BulletSide == PlayerSide || BulletSide == -PlayerSide)
            {
                // 対を向いていた場合ダメージは受けない
                m_IsHitBullet = false;
                return;
            }

            if (m_InvincibleNowTime == 0.0f)
            {
                // そのBulletの攻撃力分体力が削れる
                m_PlayerHP -= mDamageInfo.damage;
            }
            // 無敵時間を計測
            m_InvincibleNowTime += Time.deltaTime;
        }

        // プレイヤーが地雷に当たっているか
        if (m_IsHitLandmine && m_InvincibleNowTime == 0.0f)
        {
            // その地雷の攻撃力分体力が削れる
            m_PlayerHP -= mDamageInfo.damage;

            // 無敵時間を計測
            m_InvincibleNowTime += Time.deltaTime;
        }

        // ダメージを食らったら
        if (m_IsHitBullet || m_IsHitLandmine)
        {
            // 無敵時間を計測
            m_InvincibleNowTime += Time.deltaTime;
        }

        // 無敵時間が過ぎた時の後処理
        if (m_InvincibleNowTime > m_InvincibleTime)
        {
            m_IsHitBullet = false;
            m_IsHitLandmine = false;
            m_InvincibleNowTime = 0;
        }
    }

    // 接触した際の当たり判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        // Bulletの当たり判定
        // 自身が撃った弾には当たらない
        if (collision.tag == "Bullet" && collision.name != m_Bullet.name && !m_IsHitBullet)
        {
            m_IsHitBullet = true;

            // そのBulletの情報を取得
            mDamageInfo.vForword = collision.transform.right;
            mDamageInfo.damage = collision.GetComponent<Bullet>().GetBulletPower();

            //Debug.Log("Bulletに当たったよ");
        }

        // Landminesの当たり判定
        if (collision.tag == "LandMine" && !m_IsHitLandmine)
        {
            m_IsHitLandmine = true;

            // そのLandMineの情報を取得
            mDamageInfo.vForword = collision.transform.right;
            mDamageInfo.damage = collision.GetComponent<Mine>().GetLandMinePower();

            //Debug.Log("Landminesに当たったよ");
        }
    }

    // プレイヤーは死亡体力になったか
    public bool IsDead()
    {
        bool ret = false;
        if (m_PlayerHP <= 0)
        {
            ret = true;
        }
        return ret;
    }

    // プレイヤーの体力を取得
    public float GetPlayerHitPoints()
    {
        return m_PlayerHP;
    }

    // ダメージを受けているか
    public bool IsDamage()
    {
        bool ret = false;

        // 弾か地雷が当たっていればtrueを返す
        if(m_IsHitBullet || m_IsHitLandmine)
        {
            ret = true;
        }
        return ret;
    }
}
