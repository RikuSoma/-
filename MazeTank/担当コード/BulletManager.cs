using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ------------------------------------
// BulletManagerクラス
// 用途：弾のインスタンス等を行う
// 作成者：23cu0216　相馬理玖
// 作成日：2024/10/04 作成開始
// ------------------------------------


public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObj;   // プレイヤー格納用
    private Transform PlayerTransform;
    private PlayerController PlayerController;

    [SerializeField] private GameObject BulletObj;   // プレイヤー用弾オブジェクト
    [SerializeField] private GameObject LandMines;    // プレイヤー用地雷オブジェクト

    [SerializeField] private int m_LandMineNum;   // 地雷の残りの数
    [SerializeField] private int m_MineUpperLimit;    // 地雷の所持上限

    private bool IsShooting;    // 射撃を行ったか
    private readonly float ShootingCoolTime = 3.0f; // 発射のクールタイム 
    private float ShootingTime; // 発射した時間

    private bool IsSetLandmines;   // 地雷が設置されたか



    void Start()
    {
        // 各データを初期化
        PlayerTransform = GameObject.Find(PlayerObj.name).GetComponent<Transform>();
        PlayerController = GameObject.Find(PlayerObj.name).GetComponent<PlayerController>();

        IsShooting = false;
        IsSetLandmines = false;
        ShootingTime = 0.0f;
    }

    void Update()
    {
        // クールタイムを計測
        if (IsShooting || IsSetLandmines)
        {
            ShootingTime += Time.deltaTime;

            if (ShootingTime > ShootingCoolTime)
            {
                // 後処理
                IsShooting = false;
                IsSetLandmines = false;
                ShootingTime = 0.0f;
            }
        }

        // Playerが設置可能な状態なら
        if(PlayerController.CanAddMine())
        {
            // 所持上限を増やす
            AddMine();
            PlayerController.SetCanAddMine(false);
        }
    }

    // 所持地雷増加処理
    private void AddMine()
    {
        // 地雷を所持上限持っていた場合無視する
        if (m_LandMineNum >= m_MineUpperLimit) return;

        ++m_LandMineNum;
    }

    public void PlayerShooting()
    {
        // プレイヤーが回転中であれば射撃はできない
        if (!PlayerController.IsRotate())
        {
            // 射撃キーが押されていて
            // 射撃クールタイムではなかった場合
            if (!IsShooting)
            {
                Vector3 BulletPos = Vector3.zero;
                Vector3 TexSize = PlayerTransform.right / 1.5f;
                BulletPos = PlayerTransform.position + TexSize;
                // 弾をプレイヤー情報に合わせてインスタンス化する
                var bullet = Instantiate(BulletObj,BulletPos, PlayerTransform.rotation);
                bullet.name = BulletObj.name;
                IsShooting = true;
            }
        }
    }

    // プレイヤーの地雷設置処理
    public void PlayerSetMines()
    {
        // プレイヤーが回転中であれば設置はできない
        if (!PlayerController.IsRotate())
        {
            // 設置ボタンが押されていた場合
            // 設置数は３つ
            if (!IsSetLandmines && m_LandMineNum > 0)
            {
                // 地雷をプレイヤー情報に合わせてインスタンス化する
                --m_LandMineNum;
                var landmine = Instantiate(LandMines, PlayerTransform.position, PlayerTransform.rotation);
                landmine.name = LandMines.name;
                IsSetLandmines = true;
            }
        }
    }
    // 地雷の設置可能数を取得
    public int GetLandMineNum()
    {
        return m_LandMineNum;
    }
}
